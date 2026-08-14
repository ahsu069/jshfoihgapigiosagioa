using api.Commons;
using api.Data;
using api.Models;
using api.Models.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll(string? orderColumn, string? orderDir)
        {
            try
            {
                var query = _context.TransactionHistories.AsQueryable();
                query = query
                        .Include(o => o.CategoryTransactionsDto)
                        .Include(o => o.CategoryEmployeeDto)
                        .Include(o => o.UsersCacheDto)
                        .Include(o => o.EmployeeDto)
                            .ThenInclude(d => d.BagianUserDto)
                        .Include(o => o.ApprovalManajemenPekerjaIdDto!)
                            .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.ApprovalGudangIdDto!)
                            .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.ApprovalSectionheadIdDto!)
                            .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.TransactionDetails)
                            .ThenInclude(d => d.itemDto);

                // Sorting
                if (!string.IsNullOrEmpty(orderColumn))
                    query = orderDir == "asc"
                        ? query.OrderByDynamic(orderColumn, true)
                        : query.OrderByDynamic(orderColumn, false);
                else
                    query = query.OrderByDescending(r => r.created_at);

                // Load full result
                var list = query.ToList();

                // ============================================
                // 🔥 Add booked_qty for each Item inside details
                // ============================================
                foreach (var trx in list)
                {
                    foreach (var d in trx.TransactionDetails)
                    {
                        var itemId = d.barang_id;

                        d.itemDto.booked_qty =
                            _context.TransactionDetails
                                .Where(x => x.barang_id == itemId)
                                .Join(_context.TransactionHistories,
                                    td => td.transact_id,
                                    th => th.transact_id,
                                    (td, th) => new { td, th })
                                .Where(joined =>
                                    joined.th.status != TransactionStatus.DONE &&
                                    joined.th.status != TransactionStatus.CANCELLED &&
                                    joined.th.status != TransactionStatus.DITOLAK_SUPERVISOR &&
                                    !joined.th.status.ToLower().Contains("rejected"))
                                .Sum(joined => (int?)joined.td.jumlah_bar) ?? 0;
                    }
                }

                // Map to DTO AFTER booked_qty injected
                var data = list.Select(r => r.MapToDto(Request, User));

                return Ok(ApiResponse<IEnumerable<TransactionHistoryDto>>.Ok("Transaction retrieved successfully", data));
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException || ex is DbUpdateException)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var data = _context.TransactionHistories
                        .Include(o => o.CategoryTransactionsDto)
                        .Include(o => o.CategoryEmployeeDto)
                        .Include(o => o.UsersCacheDto)
                        .Include(o => o.EmployeeDto)
                            .ThenInclude(d => d.BagianUserDto)
                        .Include(o => o.ApprovalManajemenPekerjaIdDto!)
                            .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.ApprovalGudangIdDto!)
                            .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.ApprovalSectionheadIdDto!)
                            .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.TransactionDetails)
                            .ThenInclude(d => d.itemDto)
                        .FirstOrDefault(u => u.transact_id == id);
                if (data == null)
                {
                    Errors?.Add("transact_id", new[] { "The field 'transact_id' value is not found." });
                    return NotFound(ApiResponse<object>.Fail("Get transaction detail failed", Errors));
                }

                // 🔥 ADD booked_qty for each transaction detail item
                foreach (var d in data.TransactionDetails)
                {
                    var itemId = d.barang_id;

                    d.itemDto.booked_qty =
                        _context.TransactionDetails
                            .Where(x => x.barang_id == itemId)
                            .Join(_context.TransactionHistories,
                                td => td.transact_id,
                                th => th.transact_id,
                                (td, th) => new { td, th })
                            .Where(joined =>
                                joined.th.status != TransactionStatus.DONE &&
                                joined.th.status != TransactionStatus.CANCELLED &&
                                joined.th.status != TransactionStatus.DITOLAK_SUPERVISOR &&
                                !joined.th.status.ToLower().Contains("rejected"))
                            .Sum(joined => (int?)joined.td.jumlah_bar) ?? 0;
                }
                return Ok(ApiResponse<TransactionHistoryDto>.Ok("Transaction detail retrieved successfully", data.MapToDto(Request, User)));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        public class ValidationRequest
        {
            public Dictionary<string, string[]> errValidate { get; set; } = new();
            public List<Guid> legacyRoleId { get; set; } = new();
            public List<Guid> legacyUserId { get; set; } = new();
            public List<UsersCache> usersCaches { get; set; } = new();
        }
        private ValidationRequest validate(TransactionRequest transactionRequest, List<ApprovalRoleMap> approvalRoleMaps)
        {
            ValidationRequest validationRequest = new ValidationRequest();
            TransactionHistoryRequest request = transactionRequest.transactionHistory;
            EmployeeRequest? employeeRequest = transactionRequest.employeeRequest;
            List<TransactionDetailRequest> requestDetail = transactionRequest.transactionDetail;
            List<TransactionDetail> transactionDetail = new List<TransactionDetail>();
            if (requestDetail == null || requestDetail.Count == 0)
                validationRequest.errValidate.Add("transactionDetail", new[] { "The item 'transactionDetail' must be greater than 0." });
            else
            {
                foreach (var (d, index) in requestDetail.Select((value, index) => (value, index)))
                {
                                        
                    if (int.TryParse(d.jumlah_bar, out int jumlah_bar))
                    {
                        if (d?.jumlah_bar == null || jumlah_bar == 0)
                            validationRequest.errValidate.Add(string.Format("jumlah_bar_{0}", (index + 1)), new[] { "The field 'jumlah_bar' must be greater than 0." });
                    }
                    else
                    {
                        validationRequest.errValidate.Add(string.Format("jumlah_bar_{0}", (index + 1)), new[] { "The field 'jumlah_bar' not an int." });
                    }
                    if (Guid.TryParse(d?.barang_id, out Guid result))
                    {
                        if (!ValidationHelper.TryValidateForeignKey(
                            d.barang_id,
                            id => _context.Items.Any(d => d.barang_id == Guid.Parse(id)),
                            out var ItemsErrors))
                            validationRequest.errValidate.Add(string.Format("barang_id_{0}", (index + 1)), new[] { ItemsErrors });

                        Item? item = _context.Items
                            .Select(x => new Item
                            {
                                barang_id = x.barang_id,
                                nama_barang = x.nama_barang,
                                msl_barang = x.msl_barang,
                                jumlah_barang = x.jumlah_barang,
                                satuanbar_id = x.satuanbar_id,
                                uomDto = x.uomDto,
                                kategoribar_id = x.kategoribar_id,
                                categoryDto = x.categoryDto,
                                link_gambar_bar = x.link_gambar_bar,
                                status_bar = x.status_bar,
                                is_deleted = x.is_deleted,
                                created_at = x.created_at,
                                updated_at = x.updated_at,
                                booked_qty = (
                                    from d in _context.TransactionDetails
                                    join h in _context.TransactionHistories
                                        on d.transact_id equals h.transact_id
                                    where d.barang_id == x.barang_id
                                        && h.status != TransactionStatus.DONE
                                        && h.status != TransactionStatus.CANCELLED
                                        && h.status != TransactionStatus.DITOLAK_SUPERVISOR
                                        && !h.status.ToLower().Contains("rejected")
                                    select (int?)d.jumlah_bar
                                ).Sum() ?? 0
                            })
                            .FirstOrDefault(o => o.barang_id == Guid.Parse(d.barang_id));
                        if (item != null)
                        {
                            // validasi satuan dan barang
                            if (!_context.Uoms.Any(s => s.satuanbar_id == item.satuanbar_id))
                                validationRequest.errValidate.Add(string.Format("satuanbar_id_{0}", (index + 1)), new[] { "The field 'satuanbar_id' value is not found." });

                            if (!_context.Categorys.Any(b => b.kategoribar_id == item.kategoribar_id))
                                validationRequest.errValidate.Add(string.Format("kategoribar_id_{0}", (index + 1)), new[] { "The field 'kategoribar_id' value is not found." });
                            if(item.jumlah_barang - item.booked_qty < jumlah_bar && request.kategori_transact_id == "OUT")
                                validationRequest.errValidate.Add(string.Format("jumlah_bar_{0}", (index + 1)), new[] { "The field 'jumlah_bar' exceeds available stock." });
                        }
                        transactionDetail.Add(new TransactionDetail
                        {
                            barang_id = Guid.Parse(d.barang_id),
                        });
                    }
                    else
                    {
                        validationRequest.errValidate.Add(string.Format("barang_id_{0}", (index + 1)), new[] { "The field 'barang_id' value is invalid Guid." });
                        transactionDetail.Add(new TransactionDetail
                        {
                            barang_id = Guid.NewGuid(),
                        });
                    }
                }
                                
                var duplicateIds = transactionDetail
                    .GroupBy(d => d.barang_id)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateIds.Any())
                    validationRequest.errValidate.Add("transactionDetail", new[] { $"Duplicate item(s) detected for barang_id: {string.Join(", ", duplicateIds)}" });
            }

            if (!string.IsNullOrEmpty(request.kategori_transact_id))
            {
                if (!ValidationHelper.TryValidateForeignKey(
                    request.kategori_transact_id,
                    id => _context.CategoryTransactions.Any(d => d.kategori_transact_id == id),
                    out var CategoryTransactionsErrors))
                    validationRequest.errValidate.Add("kategori_transact_id", new[] { CategoryTransactionsErrors });
            }
            if (!string.IsNullOrEmpty(request.kategori_pekerja))
            {
                if (!ValidationHelper.TryValidateForeignKey(
                    request.kategori_pekerja,
                    id => _context.CategoryEmployees.Any(d => d.kategori_pekerja_id == id),
                    out var CategoryEmployeesErrors))
                    validationRequest.errValidate.Add("kategori_pekerja", new[] { CategoryEmployeesErrors });
            }
            if (!string.IsNullOrEmpty(request.users_cache_id))
            {
                if (!ValidationHelper.TryValidateForeignKey(
                    request.users_cache_id,
                    id => _context.SigapUsers.Any(d => d.user_id.ToString() == id),
                    out var UsersCachesErrors))
                    validationRequest.errValidate.Add("users_cache_id", new[] { UsersCachesErrors });
            }
            if (employeeRequest?.bagian_id != null)
            {
                var bagianIdInt = employeeRequest.bagian_id.Value;

                var bagianExists = _context.BagianUsers.Any(d => d.bagian_id == bagianIdInt);

                if (!bagianExists)
                {
                    validationRequest.errValidate
                        .Add("employeeRequest.bagian_id", new[] { "The field 'bagian_id' value is not found." });
                }
            }
            // if (request.pekerja_temp_id != null)
            // {
            //     if (!ValidationHelper.TryValidateForeignKey(
            //         request.pekerja_temp_id,
            //         id => _context.Employees.Any(d => d.pekerja_temp_id == id),
            //         out var EmployeesErrors))
            //         validationRequest.errValidate.Add("pekerja_temp_id", new[] { EmployeesErrors });
            // }

            if (request.kategori_transact_id == "OUT")
            {
                var requiredCodes = new[] { "1", "3" };
                var optionalCodes = new[] { "2" };

                foreach (var code in requiredCodes)
                {
                    ApprovalRoleMap? legacy = approvalRoleMaps.FirstOrDefault(x => x.legacy_code == code);
                    if (legacy == null)
                    {
                        validationRequest.errValidate.Add("approval_legacy" + code, new[] { "The field 'approval_legacy' value is not found!" });
                        continue;
                    }

                    UserRole? userLegacy = _context.UserRoles
                        .FromSqlRaw("SELECT * FROM user_role WHERE role_id = {0}", legacy.role_id)
                        .FirstOrDefault();

                    if (userLegacy == null)
                    {
                        validationRequest.errValidate.Add("user_legacy" + code, new[] { "The field 'user_legacy' value is not found." });
                        continue;
                    }

                    string targetUserId = userLegacy.user_id.ToString().ToLower();
                    UsersCache? usersCache = _context.UsersCaches
                        .FirstOrDefault(o => o.user_id.ToLower() == targetUserId);

                    if (usersCache == null)
                    {
                        validationRequest.errValidate.Add("user_legacy" + code, new[] { "The field 'user_legacy' value is not found." });
                        continue;
                    }

                    validationRequest.legacyRoleId.Add(legacy.role_id);
                    validationRequest.legacyUserId.Add(userLegacy.user_id);
                    validationRequest.usersCaches.Add(usersCache);
                }

                foreach (var code in optionalCodes)
                {
                    ApprovalRoleMap? legacy = approvalRoleMaps.FirstOrDefault(x => x.legacy_code == code);
                    if (legacy == null) continue;

                    UserRole? userLegacy = _context.UserRoles
                        .FromSqlRaw("SELECT * FROM user_role WHERE role_id = {0}", legacy.role_id)
                        .FirstOrDefault();

                    if (userLegacy == null) continue;

                    string targetUserId = userLegacy.user_id.ToString().ToLower();
                    UsersCache? usersCache = _context.UsersCaches
                        .FirstOrDefault(o => o.user_id.ToLower() == targetUserId);

                    if (usersCache == null) continue;

                    validationRequest.legacyRoleId.Add(legacy.role_id);
                    validationRequest.legacyUserId.Add(userLegacy.user_id);
                    validationRequest.usersCaches.Add(usersCache);
                }
            }
            return validationRequest;
        }
        [HttpPost]
        public IActionResult Create([FromForm] TransactionRequest transactionRequest)
        {
            TransactionHistoryRequest request = transactionRequest.transactionHistory;
            List<TransactionDetailRequest> requestDetail = transactionRequest.transactionDetail;
            EmployeeRequest? employeeRequest = transactionRequest.employeeRequest;
            List<TransactionDetail> transactionDetail = new List<TransactionDetail>();
            string? link_file_pendukung = null;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Create transaction failed", ValidationHelper.GetErrorDictionary(errors)));
                
                List<ApprovalRoleMap> approvalRoleMaps = _context.ApprovalRoleMaps.ToList();
                ValidationRequest validationRequest = validate(transactionRequest, approvalRoleMaps);
                Errors = validationRequest.errValidate;

                if (employeeRequest?.link_file_pendukung != null && employeeRequest?.link_file_pendukung.Length > 0)
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(employeeRequest.link_file_pendukung.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                        Errors?.Add("link_file_pendukung", new[] { "The field 'link_file_pendukung' is invalid file type." });
                        
                    if (!Errors?.Any() == true)
                    {
                        var fileName = $"{Guid.NewGuid()}{extension}";
                        var fileUrl = $"uploads/employees/{fileName}";

                        // var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/employees");
                        var uploadsFolder = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            $"wwwroot{Path.DirectorySeparatorChar}uploads{Path.DirectorySeparatorChar}employees"
                        );
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            employeeRequest.link_file_pendukung.CopyTo(stream);
                        }
                        link_file_pendukung = fileUrl;
                    }
                }
                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Create transaction failed", Errors));
                else if (request.kategori_transact_id == "OUT" &&
                    (validationRequest.legacyRoleId.Count() < 2 || validationRequest.legacyUserId.Count() < 2))
                return StatusCode(400, ApiResponse.Fail("Create transaction failed", Errors));

                if (employeeRequest != null)
                {
                    if(employeeRequest.id_finger != null)
                    {
                        Employee employee = _context.Employees.FirstOrDefault(o => o.id_finger == employeeRequest.id_finger) ?? new Employee();

                        employee.nama_pekerja = employeeRequest.nama_pekerja;
                        employee.fungsi_pekerja = employeeRequest.fungsi_pekerja;
                        employee.id_finger = employeeRequest.id_finger;
                        // employee.perusahaan_pekerja = employeeRequest.perusahaan_pekerja;
                        // Default perusahaan_pekerja for internal (OWN) if empty/null
                        if (string.IsNullOrWhiteSpace(employeeRequest.perusahaan_pekerja)
                            && request.kategori_pekerja == "OWN")
                        {
                            employee.perusahaan_pekerja = "Internal Pertamina";
                        }
                        else
                        {
                            employee.perusahaan_pekerja = employeeRequest.perusahaan_pekerja;
                        }
                        employee.link_file_pendukung = link_file_pendukung ?? employee.link_file_pendukung;
                        employee.bagian_id = employeeRequest.bagian_id;
                        // employee.synced_at = DateTime.Now;
                        // employee.updated_at = DateTime.Now;
                        // if (employee.pekerja_temp_id == Guid.Empty)
                        // {
                        //     employee.pekerja_temp_id = Guid.NewGuid();
                        //     employee.created_at = DateTime.Now;
                        //     _context.Employees.Add(employee);
                        // }
                        // else
                        // {
                        //     _context.Employees.Update(employee);
                        // }
                        // _context.SaveChanges();
                        // employee.BagianUserDto = _context.BagianUsers.FirstOrDefault(b => b.bagian_id == employeeRequest.bagian_id);

                        // NEW: resolve fungsi from bagian_id
                        if (employeeRequest.bagian_id != null && request.kategori_pekerja == "OWN")
                        {
                            var bagianIdInt = employeeRequest.bagian_id.Value;

                            var bagian = _context.BagianUsers.FirstOrDefault(b => b.bagian_id == bagianIdInt);
                            if (bagian != null)
                            {
                                var fungsi = _context.FungsiUsers.FirstOrDefault(f => f.fungsi_id == bagian.fungsi_id);
                                if (fungsi != null)
                                {
                                    employee.fungsi_pekerja = fungsi.nama; // or another property as needed
                                }
                            }
                        }

                        employee.synced_at = DateTime.Now;
                        employee.updated_at = DateTime.Now;
                        if (employee.pekerja_temp_id == Guid.Empty)
                        {
                            employee.pekerja_temp_id = Guid.NewGuid();
                            employee.created_at = DateTime.Now;
                            _context.Employees.Add(employee);
                        }
                        else
                        {
                            _context.Employees.Update(employee);
                        }
                        _context.SaveChanges();

                        if (employeeRequest.bagian_id != null)
                        {
                            var bagianIdInt2 = employeeRequest.bagian_id.Value;
                            employee.BagianUserDto = _context.BagianUsers.FirstOrDefault(b => b.bagian_id == bagianIdInt2);
                        }
                    }
                }
                // SigapUser sigapUser = _context.SigapUsers
                //     .Include(o => o.BagianUserDto)
                //     .Include(o => o.UserRoleDto!).ThenInclude(o => o.RoleDto)
                //     .FirstOrDefault(o => o.user_id.ToString() == request.users_cache_id)!;
                // FungsiUser? fungsiUser = _context.FungsiUsers.FirstOrDefault(o => o.fungsi_id == sigapUser.BagianUserDto!.fungsi_id);

                UsersCache? usersCache = _context.UsersCaches.FirstOrDefault(o => o.user_id == request.users_cache_id);
                List<ApprovalStatus> approvalLegacy = new List<ApprovalStatus>();
                if(request.kategori_transact_id == "OUT")
                {
                    data.approval_manajemen_pekerja_id = approvalLegacy.FirstOrDefault(x => x.role_type == "1")!.approval_id;
                    data.ApprovalManajemenPekerjaIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "1");
                    data.approval_sectionhead_id = approvalLegacy.FirstOrDefault(x => x.role_type == "2")!.approval_id;
                    data.ApprovalSectionheadIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "2");
                    data.approval_gudang_id = approvalLegacy.FirstOrDefault(x => x.role_type == "3")!.approval_id;
                    data.ApprovalGudangIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "3");
                }
                // if(request.kategori_transact_id == "OUT")
                // {
                //     for (int i = 1; i <= 3; i++)
                //     {
                //         approvalLegacy.Add(new ApprovalStatus
                //         {
                //             approval_id = Guid.NewGuid()
                //             , user_id = validationRequest.legacyUserId[i - 1].ToString()
                //             , role_type = i.ToString()
                //             , approval_role_id = validationRequest.legacyRoleId[i - 1]
                //             , is_approved = null
                //             , created_at = DateTime.Now
                //             , updated_at = DateTime.Now
                //             , usersCacheDto = _context.UsersCaches.FirstOrDefault(o => o.user_id == validationRequest.legacyUserId[i - 1].ToString())
                //         });
                //     }   
                // }

                TransactionHistory data = request.MapToDtoFromCreate();
                string status = "";

                if (request.kategori_transact_id == "OUT")
                {
                    status = TransactionStatus.PENDING_SUPERVISOR;
                }
                else
                {
                    status = TransactionStatus.DONE;
                }
                data.transact_id = Guid.NewGuid();
                data.CategoryTransactionsDto = _context.CategoryTransactions.FirstOrDefault(b => b.kategori_transact_id == data.kategori_transact_id);
                data.CategoryEmployeeDto = _context.CategoryEmployees.FirstOrDefault(b => b.kategori_pekerja_id == data.kategori_pekerja);
                data.UsersCacheDto = _context.UsersCaches.FirstOrDefault(b => b.user_id == data.users_cache_id);
                data.pekerja_temp_id = employeeRequest?.id_finger != null ? _context.Employees.FirstOrDefault(o => o.id_finger == employeeRequest.id_finger)!.pekerja_temp_id : null;
                data.EmployeeDto = _context.Employees.FirstOrDefault(b => b.pekerja_temp_id == data.pekerja_temp_id);
                if(request.kategori_transact_id == "OUT")
                {
                    data.approval_manajemen_pekerja_id = approvalLegacy.FirstOrDefault(x => x.role_type == "1")!.approval_id;
                    data.ApprovalManajemenPekerjaIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "1");
                    data.approval_sectionhead_id = approvalLegacy.FirstOrDefault(x => x.role_type == "2")!.approval_id;
                    data.ApprovalSectionheadIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "2");
                    data.approval_gudang_id = approvalLegacy.FirstOrDefault(x => x.role_type == "3")!.approval_id;
                    data.ApprovalGudangIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "3");
                }
                data.status = status;
                data.created_at = DateTime.Now;
                data.updated_at = DateTime.Now;

                _context.ApprovalStatuses.AddRange(approvalLegacy);
                _context.TransactionHistories.Add(data);

                _context.SaveChanges();

                List<Item> items = new List<Item>();
                foreach (var (d, index) in requestDetail.Select((value, index) => (value, index)))
                {
                    transactionDetail.Add(new TransactionDetail
                    {
                        transact_detail_id = Guid.NewGuid()
                        , transact_id = data.transact_id
                        , TransactionHistory = data
                        , barang_id = Guid.Parse(d.barang_id)
                        , itemDto = _context.Items.FirstOrDefault(o => o.barang_id == Guid.Parse(d.barang_id))!
                        , jumlah_bar = int.Parse(d.jumlah_bar)
                        , created_at = DateTime.Now
                        , updated_at = DateTime.Now
                    });
                    if(request.kategori_transact_id == "IN")
                    {
                        Item? item = _context.Items.FirstOrDefault(o => o.barang_id == Guid.Parse(d.barang_id));
                        if (item != null)
                        {
                            item.updated_at = DateTime.Now;
                            item.jumlah_barang = item.jumlah_barang + int.Parse(d.jumlah_bar);
                            items.Add(item);
                        }
                    }
                }
                _context.TransactionDetails.AddRange(transactionDetail);
                if(items.Count() > 0)
                    _context.Items.UpdateRange(items);

                _context.SaveChanges();

                List<TransactionDetailDto> transactionDetailDto = transactionDetail.Select(r => r.MapToDto(Request)).ToList();

                transaction.Commit();

                return Ok(ApiResponse<TransactionHistoryDto>.Ok("Created transaction successfully", data.MapToDto(Request, User)));
            }
            // catch (DbUpdateConcurrencyException ex)
            // {
            //     transaction.Rollback();
            //     return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            // }
            // catch (DbUpdateException ex)
            // {
            //     transaction.Rollback();
            //     return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            // }
            catch (DbUpdateConcurrencyException ex)
            {
                transaction.Rollback();
                Console.WriteLine("[DB CONCURRENCY ERROR] " + ex.Message);
                Console.WriteLine("[DB CONCURRENCY INNER] " + ex.InnerException?.Message);
                return StatusCode(500, ApiResponse<object>.Fail(
                    "Concurrency error: " + (ex.InnerException?.Message ?? ex.Message)
                ));
            }
            catch (DbUpdateException ex)
            {
                transaction.Rollback();
                Console.WriteLine("[DB UPDATE ERROR] " + ex.Message);
                Console.WriteLine("[DB UPDATE INNER] " + ex.InnerException?.Message);
                return StatusCode(500, ApiResponse<object>.Fail(
                    "Database error: " + (ex.InnerException?.Message ?? ex.Message)
                ));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] TransactionRequest transactionRequest)
        {
            TransactionHistoryRequest request = transactionRequest.transactionHistory;
            List<TransactionDetailRequest> requestDetail = transactionRequest.transactionDetail;
            EmployeeRequest? employeeRequest = transactionRequest.employeeRequest;
            List<TransactionDetail> transactionDetail = new List<TransactionDetail>();
            string? link_file_pendukung = null;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var data = _context.TransactionHistories.FirstOrDefault(u => u.transact_id == id);
                if (data == null)
                {
                    Errors?.Add("transact_id", new[] { "The field 'transact_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Update transaction failed", Errors));
                }else if(data.status != TransactionStatus.PENDING_SUPERVISOR)
                {
                    Errors?.Add("transact_id", new[] { "The transaction approval is on processing or done."} );
                    return StatusCode(400, ApiResponse<object>.Fail("Can't update this transaction.", Errors));
                }

                List<ApprovalRoleMap> approvalRoleMaps = _context.ApprovalRoleMaps.ToList();
                ValidationRequest validationRequest = validate(transactionRequest, approvalRoleMaps);
                Errors = validationRequest.errValidate;

                if (employeeRequest?.link_file_pendukung != null && employeeRequest?.link_file_pendukung.Length > 0)
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(employeeRequest.link_file_pendukung.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                        Errors?.Add("link_file_pendukung", new[] { "The field 'link_file_pendukung' is invalid file type." });
                        
                    if (!Errors?.Any() == true)
                    {
                        var fileName = $"{Guid.NewGuid()}{extension}";
                        var fileUrl = $"uploads/employees/{fileName}";

                        // var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/employees");                        
                        var uploadsFolder = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            $"wwwroot{Path.DirectorySeparatorChar}uploads{Path.DirectorySeparatorChar}employees"
                        );
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            employeeRequest.link_file_pendukung.CopyTo(stream);
                        }
                        link_file_pendukung = fileUrl;
                    }
                }
                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Update transaction failed", Errors));
                else if (request.kategori_transact_id == "OUT" && (validationRequest.legacyRoleId.Count() < 2 || validationRequest.legacyUserId.Count() < 2))
                    return StatusCode(400, ApiResponse<object>.Fail("Update transaction failed", Errors));

                // reset data
                List<ApprovalStatus> approvalLegacy = new List<ApprovalStatus>();
                approvalLegacy = _context.ApprovalStatuses.Where(
                    o => o.approval_id == data.approval_manajemen_pekerja_id
                    || o.approval_id == data.approval_gudang_id
                    || o.approval_id == data.approval_sectionhead_id
                ).ToList();
                _context.ApprovalStatuses.RemoveRange(approvalLegacy);

                if (employeeRequest != null)
                {
                    if (employeeRequest.id_finger != null)
                    {
                        Employee employee = _context.Employees.FirstOrDefault(o => o.id_finger == employeeRequest.id_finger) ?? new Employee();

                        employee.nama_pekerja = employeeRequest.nama_pekerja;
                        employee.fungsi_pekerja = employeeRequest.fungsi_pekerja;
                        employee.id_finger = employeeRequest.id_finger;
                        employee.perusahaan_pekerja = employeeRequest.perusahaan_pekerja;
                        employee.link_file_pendukung = link_file_pendukung ?? employee.link_file_pendukung;
                        employee.synced_at = DateTime.Now;
                        employee.updated_at = DateTime.Now;
                        if (employee.pekerja_temp_id == Guid.Empty)
                        {
                            employee.pekerja_temp_id = Guid.NewGuid();
                            employee.created_at = DateTime.Now;
                            _context.Employees.Add(employee);
                        }
                        else
                        {
                            _context.Employees.Update(employee);
                        }
                        _context.SaveChanges();
                    }
                }
                // SigapUser sigapUser = _context.SigapUsers
                // .Include(o => o.BagianUserDto)
                // .Include(o => o.UserRoleDto!).ThenInclude(o => o.RoleDto)
                // .FirstOrDefault(o => o.user_id.ToString() == request.users_cache_id)!;
                // FungsiUser? fungsiUser = _context.FungsiUsers.FirstOrDefault(o => o.fungsi_id == sigapUser.BagianUserDto!.fungsi_id);

                UsersCache? usersCache = _context.UsersCaches.FirstOrDefault(o => o.user_id == request.users_cache_id);
                approvalLegacy = new List<ApprovalStatus>();

                transactionDetail = _context.TransactionDetails.Where(o => o.transact_id == data.transact_id).ToList();

                List<Item> items = new List<Item>();
                foreach (var (d, index) in transactionDetail.Select((value, index) => (value, index)))
                {
                    if(request.kategori_transact_id == "IN")
                    {
                        Item? item = _context.Items.FirstOrDefault(o => o.barang_id == d.barang_id);
                        if (item != null)
                        {
                            item.updated_at = DateTime.Now;
                            item.jumlah_barang = item.jumlah_barang - d.jumlah_bar;
                            items.Add(item);
                        }
                    }
                }
                _context.TransactionDetails.RemoveRange(transactionDetail);
                if(items.Count() > 0)
                    _context.Items.UpdateRange(items);
                
                _context.SaveChanges();

                transactionDetail = new List<TransactionDetail>();

                if(request.kategori_transact_id == "OUT")
                {
                    data.approval_manajemen_pekerja_id = approvalLegacy.FirstOrDefault(x => x.role_type == "1")!.approval_id;
                    data.ApprovalManajemenPekerjaIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "1");
                    data.approval_sectionhead_id = approvalLegacy.FirstOrDefault(x => x.role_type == "2")!.approval_id;
                    data.ApprovalSectionheadIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "2");
                    data.approval_gudang_id = approvalLegacy.FirstOrDefault(x => x.role_type == "3")!.approval_id;
                    data.ApprovalGudangIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "3");
                }
                
                string status = data.status; // default keep current

                if (request.kategori_transact_id == "OUT")
                {
                    if (data.status == TransactionStatus.PENDING_SUPERVISOR)
                    {
                        status = TransactionStatus.PENDING_SUPERVISOR;
                    }
                }
                else
                {
                    status = TransactionStatus.DONE;
                }
                data.kategori_transact_id = request.kategori_transact_id;
                data.CategoryTransactionsDto = _context.CategoryTransactions.FirstOrDefault(b => b.kategori_transact_id == data.kategori_transact_id);
                data.kategori_pekerja = request.kategori_pekerja;
                data.CategoryEmployeeDto = _context.CategoryEmployees.FirstOrDefault(b => b.kategori_pekerja_id == data.kategori_pekerja);
                data.pekerja_temp_id = employeeRequest?.id_finger != null ? _context.Employees.FirstOrDefault(o => o.id_finger == employeeRequest.id_finger)!.pekerja_temp_id : null;
                data.EmployeeDto = _context.Employees.FirstOrDefault(b => b.pekerja_temp_id == data.pekerja_temp_id);
                data.users_cache_id = request.users_cache_id;
                data.UsersCacheDto = _context.UsersCaches.FirstOrDefault(b => b.user_id == data.users_cache_id);
                data.no_miv_safety = request.no_miv_safety;
                data.no_miv_custom = request.no_miv_custom;
                if(request.kategori_transact_id == "OUT")
                {
                    data.approval_manajemen_pekerja_id = approvalLegacy.FirstOrDefault(x => x.role_type == "1")!.approval_id;
                    data.ApprovalManajemenPekerjaIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "1");
                    data.approval_sectionhead_id = approvalLegacy.FirstOrDefault(x => x.role_type == "2")!.approval_id;
                    data.ApprovalSectionheadIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "2");
                    data.approval_gudang_id = approvalLegacy.FirstOrDefault(x => x.role_type == "3")!.approval_id;
                    data.ApprovalGudangIdDto = approvalLegacy.FirstOrDefault(x => x.role_type == "3");
                }
                data.status = status;
                data.updated_at = DateTime.Now;

                _context.ApprovalStatuses.AddRange(approvalLegacy);
                _context.TransactionHistories.Update(data);

                _context.SaveChanges();
                
                items = new List<Item>();
                foreach (var (d, index) in requestDetail.Select((value, index) => (value, index)))
                {
                    transactionDetail.Add(new TransactionDetail
                    {
                        transact_detail_id = Guid.NewGuid()
                        , transact_id = data.transact_id
                        , TransactionHistory = data
                        , barang_id = Guid.Parse(d.barang_id)
                        , itemDto = _context.Items.FirstOrDefault(o => o.barang_id == Guid.Parse(d.barang_id))!
                        , jumlah_bar = int.Parse(d.jumlah_bar)
                        , created_at = DateTime.Now
                        , updated_at = DateTime.Now
                    });
                    if(request.kategori_transact_id == "IN")
                    {
                        Item? item = _context.Items.FirstOrDefault(o => o.barang_id == Guid.Parse(d.barang_id));
                        if (item != null)
                        {
                            item.updated_at = DateTime.Now;
                            item.jumlah_barang = item.jumlah_barang + int.Parse(d.jumlah_bar);
                            items.Add(item);
                        }
                    }
                }
                _context.TransactionDetails.AddRange(transactionDetail);
                if(items.Count() > 0)
                    _context.Items.UpdateRange(items);

                _context.SaveChanges();

                List<TransactionDetailDto> transactionDetailDto = transactionDetail.Select(r => r.MapToDto(Request)).ToList();
                
                transaction.Commit();
                return Ok(ApiResponse<TransactionHistoryDto>.Ok("Update transaction successfully", data.MapToDto(Request, User)));
            }
            catch (DbUpdateConcurrencyException ex){
                transaction.Rollback();
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                transaction.Rollback();
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                transaction.Rollback();
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var data = _context.TransactionHistories.FirstOrDefault(u => u.transact_id == id);
                if (data == null)
                {
                    Errors?.Add("transact_id", new[] { "The field 'transact_id' value is not found." });
                    return NotFound(ApiResponse<object>.Fail("Delete transaction failed", Errors));
                }else if(data.status != TransactionStatus.PENDING_SUPERVISOR)
                {
                    Errors?.Add("transact_id", new[] { "The transaction approval is on progress or done."} );
                    return StatusCode(400, ApiResponse<object>.Fail("Can't delete this transaction.", Errors));
                }

                List<ApprovalStatus> approvalLegacy = new List<ApprovalStatus>();
                approvalLegacy = _context.ApprovalStatuses.Where(
                    o => o.approval_id == data.approval_manajemen_pekerja_id
                    || o.approval_id == data.approval_gudang_id
                    || o.approval_id == data.approval_sectionhead_id
                ).ToList();
                _context.ApprovalStatuses.RemoveRange(approvalLegacy);

                List<TransactionDetail> transactionDetail = new List<TransactionDetail>();
                transactionDetail = _context.TransactionDetails.Where(o => o.transact_id == data.transact_id).ToList();
                _context.TransactionDetails.RemoveRange(transactionDetail);
                transactionDetail = new List<TransactionDetail>();

                _context.TransactionHistories.Remove(data);

                _context.SaveChanges();

                return Ok(ApiResponse<object>.Ok("Delete transaction successfully"));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        
        [HttpPost("datatable")]
        public IActionResult GetDataTable([FromBody] DataTableRequest request)
        {
            var query = _context.TransactionHistories.AsQueryable();
            query = query.Include(o => o.CategoryTransactionsDto)
                    .Include(o => o.CategoryEmployeeDto)
                    .Include(o => o.UsersCacheDto)
                    .Include(o => o.EmployeeDto)
                            .ThenInclude(d => d.BagianUserDto)
                    .Include(o => o.ApprovalManajemenPekerjaIdDto!)
                            .ThenInclude(d => d.usersCacheDto)
                    .Include(o => o.ApprovalGudangIdDto!)
                            .ThenInclude(d => d.usersCacheDto)
                    .Include(o => o.ApprovalSectionheadIdDto!)
                            .ThenInclude(d => d.usersCacheDto)
                    .Include(o => o.TransactionDetails)
                        .ThenInclude(d => d.itemDto);
            
            // ✅ Global search
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();

                var searchableColumns = IQueryableExtensions.GetSearchableColumns(request);

                if (searchableColumns?.Any() == true)
                {
                    query = query.WhereDynamicSearch(searchValue, searchableColumns.ToArray());
                }
            }

            // ✅ Filter per kolom (per field)
            if (request.Columns != null)
            {
                query = query.WhereDynamicColumnFilter(request);
            }

            var recordsTotal = _context.TransactionHistories.Count();
            var recordsFiltered = query.Count();

            // ✅ Ordering (hanya kolom yang orderable)
            if (request.Order?.Any() == true)
            {
                foreach (var order in request.Order)
                {
                    var col = request.Columns?[order.Column];
                    if (col != null && col.Orderable)
                    {
                        query = order.Dir == "asc"
                            ? query.OrderByDynamic(col.Data, true)
                            : query.OrderByDynamic(col.Data, false);
                    }
                }
            }

            // ✅ Paging
            var data = query.Skip(request.Start).Take(request.Length).ToList();
            // 🔥 ADD booked_qty for each transaction detail item
            foreach (var trx in data)
            {
                foreach (var d in trx.TransactionDetails)
                {
                    var itemId = d.barang_id;

                    d.itemDto.booked_qty =
                        _context.TransactionDetails
                            .Where(x => x.barang_id == itemId)
                            .Join(_context.TransactionHistories,
                                td => td.transact_id,
                                th => th.transact_id,
                                (td, th) => new { td, th })
                            .Where(joined =>
                                joined.th.status != TransactionStatus.DONE &&
                                joined.th.status != TransactionStatus.CANCELLED &&
                                joined.th.status != TransactionStatus.DITOLAK_SUPERVISOR &&
                                !joined.th.status.ToLower().Contains("rejected"))
                            .Sum(joined => (int?)joined.td.jumlah_bar) ?? 0;
                }
            }
            return Ok(new
            {
                draw = request.Draw,
                recordsTotal,
                recordsFiltered,
                data = data.Select(r => r.MapToDto(Request, User))
            });
        }
    }
}