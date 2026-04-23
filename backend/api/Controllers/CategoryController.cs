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
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll(string? namakategoribar, bool? is_deleted, string? orderColumn, string? orderDir)
        {
            try
            {
                var query = _context.Categorys.AsQueryable();
                query = query.Include(o => o.ItemDto);
                query = query.Where(r =>
                    (r.namakategoribar!.Contains(namakategoribar ?? string.Empty) || string.IsNullOrEmpty(namakategoribar))
                    && (r.is_deleted == is_deleted || is_deleted == null)
                );
                if (!string.IsNullOrEmpty(orderColumn))
                    query = orderDir == "asc"
                        ? query.OrderByDynamic(orderColumn, true)
                        : query.OrderByDynamic(orderColumn, false);
                else
                    query = query.OrderBy(r => r.namakategoribar);
                var data = query.ToList().Select(r => r.MapToDto());
                return Ok(ApiResponse<IEnumerable<CategoryDto>>.Ok("Category retrieved successfully", data));
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
                var data = _context.Categorys.FirstOrDefault(u => u.kategoribar_id == id);
                if (data == null)
                {
                    Errors?.Add("kategoribar_id", new[] { "The field 'kategoribar_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Get category detail failed", Errors));
                }
                return Ok(ApiResponse<CategoryDto>.Ok("Category detail retrieved successfully", data.MapToDto()));
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
        public IActionResult Create([FromBody] CategoryRequest request)
        {
            try
            {
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Create category failed", ValidationHelper.GetErrorDictionary(errors)));

                Category data = request.MapToDtoFromCreate();
                data.kategoribar_id = Guid.NewGuid();
                data.namakategoribar = request.namakategoribar;
                data.is_deleted = request.is_deleted;
                data.created_at = DateTime.Now;
                data.updated_at = DateTime.Now;
                
                _context.Categorys.Add(data);
                _context.SaveChanges();
                return Ok(ApiResponse<CategoryDto>.Ok("Create category successfully", data.MapToDto()));
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
        public IActionResult Update(Guid id, [FromBody] CategoryRequest request)
        {
            try
            {
                var data = _context.Categorys.FirstOrDefault(u => u.kategoribar_id == id);
                if (data == null)
                {
                    Errors?.Add("kategoribar_id", new[] { "The field 'kategoribar_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Update category failed", Errors));
                }
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Update category failed", ValidationHelper.GetErrorDictionary(errors)));

                data.namakategoribar = request.namakategoribar;
                data.is_deleted = request.is_deleted;
                data.updated_at = DateTime.Now;

                _context.Categorys.Update(data);
                _context.SaveChanges();
                return Ok(ApiResponse<CategoryDto>.Ok("Update category successfully", data.MapToDto()));
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
                var data = _context.Categorys.FirstOrDefault(u => u.kategoribar_id == id && !u.is_deleted);
                if (data == null)
                {
                    Errors?.Add("kategoribar_id", new[] { "The field 'kategoribar_id' value is not found." });
                    return NotFound(ApiResponse<object>.Fail("Delete category failed", Errors));
                }

                data.is_deleted = true;
                data.updated_at = DateTime.Now;
                _context.Categorys.Update(data);
                _context.SaveChanges();
                return Ok(ApiResponse<object>.Ok("Delete category successfully"));
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
            var query = _context.Categorys.AsQueryable();

            // ✅ Global search
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();

                // hanya cari di kolom yang searchable
                var searchableColumns = request.Columns?.Where(c => c.Searchable).ToList();
                if (searchableColumns?.Any() == true)
                {
                    // bangun query dinamis pakai OR
                    var predicate = PredicateBuilder.False<Category>();
                    foreach (var col in searchableColumns)
                    {
                        if (col.Data == "namakategoribar")
                            predicate = predicate.Or(u => u.namakategoribar.ToLower().Contains(searchValue));
                        else if (col.Data == "is_deleted")
                            predicate = predicate.Or(u => u.is_deleted == (searchValue.ToLower() == "true"));
                        // tambahkan sesuai field
                    }

                    query = query.Where(predicate);
                }
            }

            // ✅ Filter per kolom (per field)
            if (request.Columns != null)
            {
                foreach (var column in request.Columns)
                {
                    if (column.Searchable && !string.IsNullOrEmpty(column.Search?.Value))
                    {
                        var val = column.Search.Value.ToLower();
                        switch (column.Data)
                        {
                            case "namakategoribar":
                                query = query.Where(u => u.namakategoribar.ToLower().Contains(val));
                                break;
                            case "is_deleted":
                                query = query.Where(u => u.is_deleted == (val.ToLower() == "true"));
                                break;
                        }
                    }
                }
            }

            var recordsTotal = _context.Categorys.Count();
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
                data = data.Select(r => r.MapToDto())
            });
        }
    }
}