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
    public class ItemController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private string uploadPath;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public ItemController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            uploadPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads/items");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
        }
        [HttpGet]
        public IActionResult GetAll(string? nama_barang, int? msl_barang, int? jumlah_barang, string? satuanbar_id
            , Guid? kategoribar_id, string? status_bar, bool? is_deleted, string? orderColumn, string? orderDir
        )
        {
            try
            {
                var query = _context.Items.AsQueryable();
                query = query.Include(o => o.categoryDto).Include(o => o.uomDto);
                query = query.Where(i =>
                    (i.nama_barang.Contains(nama_barang ?? string.Empty) || String.IsNullOrEmpty(nama_barang))
                    // && (!msl_barang.HasValue || i.msl_barang.ToString().Contains(msl_barang.Value.ToString()))
                    && (!msl_barang.HasValue || (i.msl_barang.HasValue && i.msl_barang.Value.ToString().Contains(msl_barang.Value.ToString())))
                    && (!jumlah_barang.HasValue || i.jumlah_barang.ToString().Contains(jumlah_barang.Value.ToString()))
                    && (i.satuanbar_id == satuanbar_id || String.IsNullOrEmpty(satuanbar_id))
                    && (i.kategoribar_id == kategoribar_id || kategoribar_id == null)
                    && (i.status_bar == status_bar || String.IsNullOrEmpty(status_bar))
                    && (i.is_deleted == is_deleted || is_deleted == null)
                );
                if (!string.IsNullOrEmpty(orderColumn))
                    query = orderDir == "asc"
                        ? query.OrderByDynamic(orderColumn, true)
                        : query.OrderByDynamic(orderColumn, false);
                else
                    query = query.OrderBy(r => r.nama_barang);
                var data = query
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
                                && (h.status.ToLower() != "done" && !h.status.ToLower().Contains("rejected"))
                            select (int?)d.jumlah_bar
                        ).Sum() ?? 0
                    }).ToList().Select(r => r.MapToDto(Request));
                return Ok(ApiResponse<IEnumerable<ItemDto>>.Ok("Item retrieved successfully", data));
            }
            catch (DbUpdateConcurrencyException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var data = _context.Items.Include(o => o.categoryDto).Include(o => o.uomDto)
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
                                && (h.status.ToLower() != "done" && !h.status.ToLower().Contains("rejected"))
                            select (int?)d.jumlah_bar
                        ).Sum() ?? 0
                        })
                        .FirstOrDefault(u => u.barang_id == id);
                if (data == null)
                {
                    Errors?.Add("barang_id", new[] { "The field 'barang_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Get item detail failed", Errors));
                }
                return Ok(ApiResponse<ItemDto>.Ok("Item detail retrieved successfully", data.MapToDto(Request)));
            }
            catch (DbUpdateConcurrencyException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [HttpPost]
        public IActionResult Create([FromForm] ItemRequest request)
        {
            try
            {
                string? link_gambar_bar = null;
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Create item failed", ValidationHelper.GetErrorDictionary(errors)));

                if (!string.IsNullOrEmpty(request.satuanbar_id))
                {
                    if (!ValidationHelper.TryValidateForeignKey(
                        request.satuanbar_id,
                        id => _context.Uoms.Any(d => d.satuanbar_id == id),
                        out var UomsErrors))
                        Errors?.Add("satuanbar_id", new[] { UomsErrors });
                }
                if (request?.kategoribar_id != null)
                {
                    if (!ValidationHelper.TryValidateForeignKey(
                        request.kategoribar_id,
                        id => _context.Categorys.Any(d => d.kategoribar_id == id),
                        out var UomsErrors))
                        Errors?.Add("kategoribar_id", new[] { UomsErrors });
                }
                if (request?.link_gambar_bar != null && request.link_gambar_bar.Length > 0)
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(request.link_gambar_bar.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                        Errors?.Add("link_gambar_bar", new[] { "The field 'link_gambar_bar' is invalid file type." });
                        
                    if (Errors?.Any() == false)
                    {
                        var fileName = $"{Guid.NewGuid()}{extension}";
                        var fileUrl = $"uploads/items/{fileName}";

                        // var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/items");
                        var uploadsFolder = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            $"wwwroot{Path.DirectorySeparatorChar}uploads{Path.DirectorySeparatorChar}items"
                        );
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            request.link_gambar_bar.CopyTo(stream);
                        }
                        link_gambar_bar = fileUrl;
                    }
                }

                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Create item failed", Errors));

                Item data = request.MapToDtoFromCreate();
                data.barang_id = Guid.NewGuid();
                data.nama_barang = request!.nama_barang;
                data.msl_barang = request.msl_barang.GetValueOrDefault(0);
                data.jumlah_barang = request.jumlah_barang;
                data.satuanbar_id = request.satuanbar_id;
                data.kategoribar_id = request.kategoribar_id;
                data.link_gambar_bar = link_gambar_bar;
                data.status_bar = request.status_bar;
                data.is_deleted = request.is_deleted;
                data.created_at = DateTime.Now;
                data.updated_at = DateTime.Now;

                _context.Items.Add(data);
                _context.SaveChanges();

                if (!string.IsNullOrEmpty(data.satuanbar_id))
                    data.uomDto = _context.Uoms.FirstOrDefault(b => b.satuanbar_id == data.satuanbar_id);
                if (data?.kategoribar_id != null)
                    data.categoryDto = _context.Categorys.FirstOrDefault(b => b.kategoribar_id == data.kategoribar_id);

                return Ok(ApiResponse<ItemDto>.Ok("Created item successfully", data.MapToDto(Request)));
            }
            catch (DbUpdateConcurrencyException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromForm] ItemRequest request)
        {
            try
            {
                string? link_gambar_bar = null;
                string? link_gambar_bar_before = null;
                var data = _context.Items.FirstOrDefault(u => u.barang_id == id);
                if (data == null)
                {
                    Errors?.Add("barang_id", new[] { "The field 'barang_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Update item failed", Errors));   
                }

                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Create item failed", ValidationHelper.GetErrorDictionary(errors)));

                if (!string.IsNullOrEmpty(request.satuanbar_id))
                {
                    if (!ValidationHelper.TryValidateForeignKey(
                        request.satuanbar_id,
                        id => _context.Uoms.Any(d => d.satuanbar_id == id),
                        out var UomsErrors))
                        Errors?.Add("satuanbar_id", new[] { UomsErrors });
                }
                if (request?.kategoribar_id != null)
                {
                    if (!ValidationHelper.TryValidateForeignKey(
                        request.kategoribar_id,
                        id => _context.Categorys.Any(d => d.kategoribar_id == id),
                        out var UomsErrors))
                        Errors?.Add("kategoribar_id", new[] { UomsErrors });
                }

                if (request?.link_gambar_bar != null && request.link_gambar_bar.Length > 0)
                {

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(request.link_gambar_bar.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                        Errors?.Add("link_gambar_bar", new[] { "The field 'link_gambar_bar' is invalid file type." });

                    if (Errors?.Any() == false)
                    {
                        var fileName = $"{Guid.NewGuid()}{extension}";
                        var fileUrl = $"uploads/items/{fileName}";

                        // var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/items");
                        var uploadsFolder = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            $"wwwroot{Path.DirectorySeparatorChar}uploads{Path.DirectorySeparatorChar}items"
                        );
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            request.link_gambar_bar.CopyTo(stream);
                        }
                        link_gambar_bar = fileUrl;
                        link_gambar_bar_before = data.link_gambar_bar;
                    }
                }
                else
                {
                    link_gambar_bar = data.link_gambar_bar;
                }

                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Update item failed", Errors));

                data.nama_barang = request!.nama_barang;
                data.msl_barang = request.msl_barang.GetValueOrDefault(0);
                data.jumlah_barang = request.jumlah_barang;
                data.satuanbar_id = request.satuanbar_id;
                data.kategoribar_id = request.kategoribar_id;
                data.link_gambar_bar = link_gambar_bar;
                data.status_bar = request.status_bar;
                data.is_deleted = request.is_deleted;
                data.updated_at = DateTime.Now;

                if (!String.IsNullOrEmpty(link_gambar_bar_before))
                {
                    try
                    {
                        string fileName = Path.GetFileName(link_gambar_bar_before);
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads\\items");
                        var filePath = Path.Combine(uploadsFolder, fileName);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
                _context.Items.Update(data);
                _context.SaveChanges();

                if (!string.IsNullOrEmpty(data.satuanbar_id))
                    data.uomDto = _context.Uoms.FirstOrDefault(b => b.satuanbar_id == data.satuanbar_id);
                if (data?.kategoribar_id != null)
                    data.categoryDto = _context.Categorys.FirstOrDefault(b => b.kategoribar_id == data.kategoribar_id);
                return Ok(ApiResponse<ItemDto>.Ok("Update item successfully", data.MapToDto(Request)));
            }
            catch (DbUpdateConcurrencyException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var data = _context.Items.FirstOrDefault(u => u.barang_id == id && !u.is_deleted);
                if (data == null)
                {
                    Errors?.Add("barang_id", new[] { "The field 'barang_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Delete item failed", Errors));   
                }
                data.is_deleted = true;
                data.updated_at = DateTime.Now;
                _context.Items.Update(data);
                _context.SaveChanges();

                return Ok(ApiResponse<object>.Ok("Delete item successfully"));
            }
            catch (DbUpdateConcurrencyException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [HttpPost("datatable")]
        public IActionResult GetDataTable([FromBody] DataTableRequest request)
        {
            var query = _context.Items
                .Include(o => o.categoryDto)
                .Include(o => o.uomDto)
                .AsQueryable();

            // 🔍 Global search
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();
                var searchableColumns = IQueryableExtensions.GetSearchableColumns(request);

                if (searchableColumns?.Any() == true)
                {
                    query = query.WhereDynamicSearch(searchValue, searchableColumns.ToArray());
                }
            }

            // 🔍 Column filter
            if (request.Columns != null)
                query = query.WhereDynamicColumnFilter(request);

            var recordsTotal = _context.Items.Count();
            var recordsFiltered = query.Count();

            // -----------------------------------------
            // 1️⃣ Projection first (compute booked_qty)
            // -----------------------------------------
            var projected = query.Select(x => new Item
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
                        && (h.status.ToLower() != "done"
                            && !h.status.ToLower().Contains("rejected"))
                    select (int?)d.jumlah_bar
                ).Sum() ?? 0
            });

            // -----------------------------------------
            // 2️⃣ Ordering (NOW IT CAN SORT booked_qty)
            // -----------------------------------------
            if (request.Order?.Any() == true)
            {
                foreach (var order in request.Order)
                {
                    var col = request.Columns?[order.Column];
                    if (col != null && col.Orderable)
                    {
                        projected = order.Dir == "asc"
                            ? projected.OrderByDynamic(col.Data, true)
                            : projected.OrderByDynamic(col.Data, false);
                    }
                }
            }

            // -----------------------------------------
            // 3️⃣ Paging
            // -----------------------------------------
            var data = projected
                .Skip(request.Start)
                .Take(request.Length)
                .ToList();

            return Ok(new
            {
                draw = request.Draw,
                recordsTotal,
                recordsFiltered,
                data = data.Select(r => r.MapToDto(Request))
            });
        }

    }
}