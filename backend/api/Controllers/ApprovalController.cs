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
    public class ApprovalController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public ApprovalController(ApplicationDbContext context)
        {
            _context = context;
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
                    return NotFound(ApiResponse<object>.Fail("Get approval detail failed", Errors));
                }
                return Ok(ApiResponse<TransactionHistoryDto>.Ok("Approval detail retrieved successfully", data.MapToDto(Request, User)));
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
        [HttpPost]
        public IActionResult Process([FromBody] ApprovalRequest request)
        {
            List<string> CategoryTransOut = new() {"OUT"};
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Approval process failed", ValidationHelper.GetErrorDictionary(errors)));
                if (request.transact_id == null || request.transact_id.Count == 0)
                    Errors?.Add("transact_id", new[] { "The item 'transact_id' must be greater than 0." });
                if (!(request.is_approved == "A" || request.is_approved == "R"))
                    Errors?.Add("is_approved", new[] { "The field 'is_approved' value must 'A' or 'R'." });
                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Approval process failed", Errors));

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

                var tokenUserid = User.Identity?.Name;
                var roleCode    = User.FindFirst("RoleCode")?.Value;
                var isSuperAdmin = string.Equals(roleCode, "SUPER_ADMIN", StringComparison.OrdinalIgnoreCase);

                foreach (var c in User.Claims)
                {
                    Console.WriteLine($"[CLAIM] {c.Type} = {c.Value}");
                }

                if (!isSuperAdmin)
                {
                    // Non-superadmin: only their own pending slot, with status matching new 2-level flow
                    query = query.Where(o =>
                        request.transact_id!.Contains(o.transact_id) &&
                        (
                            (
                                o.ApprovalManajemenPekerjaIdDto!.user_id == tokenUserid &&
                                o.ApprovalManajemenPekerjaIdDto.is_approved == null &&
                                o.status == TransactionStatus.PENDING_SUPERVISOR
                            )
                            ||
                            (
                                o.ApprovalGudangIdDto!.user_id == tokenUserid &&
                                o.ApprovalGudangIdDto.is_approved == null &&
                                o.status == TransactionStatus.DIPROSES_GUDANG
                            )
                        )
                    );
                }

                // query = query.Where(o => request.transact_id!.Contains(o.transact_id) && (o.status.ToLower().Contains("pending") || o.status.ToLower().Contains("menunggu")))
                // .Where(o => (
                //     (o.ApprovalManajemenPekerjaIdDto!.user_id == tokenUserid && o.ApprovalManajemenPekerjaIdDto.is_approved == null) ||
                //     (o.ApprovalGudangIdDto!.user_id == tokenUserid && o.ApprovalGudangIdDto.is_approved == null) ||
                //     (o.ApprovalSectionheadIdDto!.user_id == tokenUserid && o.ApprovalSectionheadIdDto.is_approved == null)
                // ))
                // ;
                
                List<TransactionHistory> transactionHistories = query.ToList();
                Console.WriteLine($"[APPROVAL] isSuperAdmin={isSuperAdmin}, tokenUserId={tokenUserid}");
                Console.WriteLine($"[APPROVAL] request.transact_id: {string.Join(",", request.transact_id)}");
                Console.WriteLine($"[APPROVAL] transactionHistories.Count={transactionHistories.Count}");
                foreach (var th in transactionHistories)
                {
                    Console.WriteLine($"[APPROVAL] DB transact_id={th.transact_id}, status={th.status}");
                }

                if(request?.transact_id?.Count != transactionHistories.Count)
                    Errors?.Add("transact_id", new[] { "One or more 'transact_id' is invalid." });
                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Approval process failed", Errors));

                List<ApprovalStatus> approvalStatuses = new List<ApprovalStatus>();
                string role_type = String.Empty;
                foreach (var (d, index) in transactionHistories.Select((value, index) => (value, index)))
                {
                    ApprovalStatus? target = null;

                    if (isSuperAdmin)
                    {
                        // SUPER_ADMIN: pick the first pending slot in order 1 → 2 → 3
                        if (d.ApprovalManajemenPekerjaIdDto?.is_approved == null)
                            target = d.ApprovalManajemenPekerjaIdDto;
                        else if (d.ApprovalSectionheadIdDto?.is_approved == null)
                            target = d.ApprovalSectionheadIdDto;
                        else if (d.ApprovalGudangIdDto?.is_approved == null)
                            target = d.ApprovalGudangIdDto;
                    }
                    else
                    {
                        // Normal approver: only their own pending slot
                        if (d.ApprovalManajemenPekerjaIdDto!.user_id == tokenUserid && d.ApprovalManajemenPekerjaIdDto.is_approved == null)
                            target = d.ApprovalManajemenPekerjaIdDto;
                        else if (d.ApprovalGudangIdDto!.user_id == tokenUserid && d.ApprovalGudangIdDto.is_approved == null)
                            target = d.ApprovalGudangIdDto;
                        else if (d.ApprovalSectionheadIdDto!.user_id == tokenUserid && d.ApprovalSectionheadIdDto.is_approved == null)
                            target = d.ApprovalSectionheadIdDto;
                    }

                    if (target == null)
                        continue;

                    // Apply decision
                    target.is_approved = request?.is_approved;
                    target.remark     = request?.remark;
                    target.updated_at = DateTime.Now;
                    approvalStatuses.Add(target);
                    role_type = target.role_type;

                    // 2‑level approval flow
                    if (request?.is_approved == "A")
                    {
                        if (role_type == "1")
                        {
                            // Supervisor approved → warehouse will process
                            d.status = TransactionStatus.DIPROSES_GUDANG;
                            d.updated_at = DateTime.Now;
                        }
                        else if (role_type == "2" || role_type == "3")
                        {
                            // Warehouse finished processing → request is complete
                            d.status = TransactionStatus.DONE;
                            d.updated_at = DateTime.Now;

                            // Stock deduction on final approval (similar to old role_type == "3" logic)
                            List<Item> items = new List<Item>();
                            foreach (var (d2, index2) in d.TransactionDetails.Select((value, index) => (value, index)))
                            {
                                Item? item = _context.Items.FirstOrDefault(o => o.barang_id == d2.barang_id);
                                if (item != null)
                                {
                                    item.updated_at = DateTime.Now;
                                    if (CategoryTransOut.Contains(d.kategori_transact_id))
                                    {
                                        item.jumlah_barang = item.jumlah_barang - d2.jumlah_bar;
                                    }
                                    items.Add(item);
                                }
                            }
                            _context.Items.UpdateRange(items);
                            _context.SaveChanges();
                        }
                    }
                    else if (request?.is_approved == "R")
                    {
                        if (role_type == "1")
                        {
                            d.status = TransactionStatus.DITOLAK_SUPERVISOR;
                        }
                        else if (role_type == "2" || role_type == "3")
                        {
                            d.status = TransactionStatus.DITOLAK_GUDANG;
                        }
                        d.updated_at = DateTime.Now;
                    }
                }
                // foreach (var (d, index) in transactionHistories.Select((value, index) => (value, index)))
                // {
                //     if (d.ApprovalManajemenPekerjaIdDto!.user_id == tokenUserid && d.ApprovalManajemenPekerjaIdDto.is_approved == null)
                //     {
                //         d.ApprovalManajemenPekerjaIdDto.is_approved = request?.is_approved;
                //         d.ApprovalManajemenPekerjaIdDto.remark = request?.remark;
                //         d.ApprovalManajemenPekerjaIdDto.updated_at = DateTime.Now;
                //         approvalStatuses.Add(d.ApprovalManajemenPekerjaIdDto);
                //         role_type = d.ApprovalManajemenPekerjaIdDto.role_type;
                //     }
                //     else if (d.ApprovalGudangIdDto!.user_id == tokenUserid && d.ApprovalGudangIdDto.is_approved == null)
                //     {
                //         d.ApprovalGudangIdDto.is_approved = request?.is_approved;
                //         d.ApprovalGudangIdDto.remark = request?.remark;
                //         d.ApprovalGudangIdDto.updated_at = DateTime.Now;
                //         approvalStatuses.Add(d.ApprovalGudangIdDto);
                //         role_type = d.ApprovalGudangIdDto.role_type;
                //     }
                //     else if (d.ApprovalSectionheadIdDto!.user_id == tokenUserid && d.ApprovalSectionheadIdDto.is_approved == null)
                //     {
                //         d.ApprovalSectionheadIdDto.is_approved = request?.is_approved;
                //         d.ApprovalSectionheadIdDto.remark = request?.remark;
                //         d.ApprovalSectionheadIdDto.updated_at = DateTime.Now;
                //         approvalStatuses.Add(d.ApprovalSectionheadIdDto);
                //         role_type = d.ApprovalSectionheadIdDto.role_type;
                //     }
                //     if (request?.is_approved == "A" && role_type == "3")
                //     {
                //         d.status = "Done";
                //         d.updated_at = DateTime.Now;
                //         List<Item> items = new List<Item>();
                //         foreach (var (d2, index2) in d.TransactionDetails.Select((value, index) => (value, index)))
                //         {
                //             Item? item = _context.Items.FirstOrDefault(o => o.barang_id == d2.barang_id);
                //             if (item != null)
                //             {
                //                 item.updated_at = DateTime.Now;
                //                 if (CategoryTransOut.Contains(d.kategori_transact_id))
                //                     item.jumlah_barang = item.jumlah_barang - d2.jumlah_bar;
                //                 // else
                //                 //     item.jumlah_barang = item.jumlah_barang + d2.jumlah_bar;
                //                 items.Add(item);
                //             }
                //         }
                //         _context.Items.UpdateRange(items);
                //         _context.SaveChanges();
                //     }
                //     else if (request?.is_approved == "A")
                //     {
                //         // d.status = String.Format("Approval {0} Pending", int.TryParse(role_type, out var i) ? i + 1 : role_type);
                //         if(role_type == "1")
                //         {
                //             d.status = "Waiting Safety Approval";
                //         }
                //         else if(role_type == "2")
                //         {
                //             d.status = "Menunggu Konfirmasi Gudang";
                //         }
                //         d.updated_at = DateTime.Now;
                //     }
                //     else if (request?.is_approved == "R")
                //     {
                //         // d.status = String.Format("Approval {0} Rejected", role_type);

                //         if(role_type == "1")
                //         {
                //             d.status = "Approval Section Head Rejected";
                //         }
                //         else if(role_type == "2")
                //         {
                //             d.status = "Approval Section Head Safety Rejected";
                //         }else if(role_type == "3")
                //         {
                //             d.status = "Approval Gudang Rejected";
                //         }
                //         d.updated_at = DateTime.Now;
                //     }
                // }

                _context.ApprovalStatuses.UpdateRange(approvalStatuses);
                _context.TransactionHistories.UpdateRange(transactionHistories);

                _context.SaveChanges();
                
                transaction.Commit();

                return Ok(ApiResponse<TransactionHistoryDto>.Ok("Approval process successfully"));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                transaction.Rollback();
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex)
            {
                transaction.Rollback();
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
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

            var tokenUserid = User.Identity?.Name;

            query = query.Where(o => (
                o.ApprovalManajemenPekerjaIdDto!.user_id == tokenUserid ||
                o.ApprovalGudangIdDto!.user_id == tokenUserid ||
                o.ApprovalSectionheadIdDto!.user_id == tokenUserid
            ));

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

            var recordsTotal = _context.TransactionHistories.Where(o => (
                o.ApprovalManajemenPekerjaIdDto!.user_id == tokenUserid ||
                o.ApprovalGudangIdDto!.user_id == tokenUserid ||
                o.ApprovalSectionheadIdDto!.user_id == tokenUserid
            )).Count();
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