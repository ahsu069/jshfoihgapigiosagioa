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
    public class PermissionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public PermissionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(string? code, string? name, string? description, string? orderColumn, string? orderDir)
        {
            try
            {
                var query = _context.Permissions.AsQueryable();
                query = query.Where(r =>
                    (r.code.Contains(code ?? string.Empty) || String.IsNullOrEmpty(code))
                    && (r.name.Contains(name ?? string.Empty) || String.IsNullOrEmpty(name))
                    && (r.description.Contains(description ?? string.Empty) || String.IsNullOrEmpty(description))
                );
                if (!string.IsNullOrEmpty(orderColumn))
                    query = orderDir == "asc"
                        ? query.OrderByDynamic(orderColumn, true)
                        : query.OrderByDynamic(orderColumn, false);
                else
                    query = query.OrderBy(r => r.name);
                var data = query.ToList().Select(r => r.MapToDto());
                return Ok(ApiResponse<IEnumerable<PermissionDto>>.Ok("Permission retrieved successfully", data));
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
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var data = _context.Permissions.FirstOrDefault(u => u.permission_id == id);
                if (data == null)
                {
                    Errors?.Add("permission_id", new[] { "The field 'permission_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Get permission detail failed", Errors));
                }
                return Ok(ApiResponse<PermissionDto>.Ok("Permission detail retrieved successfully", data.MapToDto()));
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
        public IActionResult Create([FromBody] PermissionRequest request)
        {
            try
            {
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Create permission failed", ValidationHelper.GetErrorDictionary(errors)));

                Permission? permission = _context.Permissions.FirstOrDefault(o => o.code == request.code);
                if(permission != null)
                    Errors?.Add("code", new[] { "The field 'code' value is already exist."} );

                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Create permission failed", Errors));
                    
                Permission data = request.MapToDtoFromCreate();
                data.permission_id = Guid.NewGuid();
                data.code = request.code;
                data.name = request.name;
                data.description = request.description;
                data.created_at = DateTime.Now;
                data.updated_at = DateTime.Now;

                _context.Permissions.Add(data);
                _context.SaveChanges();
                return Ok(ApiResponse<PermissionDto>.Ok("Create permission successfully", data.MapToDto()));
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
        public IActionResult Update(Guid id, [FromBody] PermissionRequest request)
        {
            try
            {
                var data = _context.Permissions.FirstOrDefault(u => u.permission_id == id);
                if (data == null)
                {
                    Errors?.Add("permission_id", new[] { "The field 'permission_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Update permission failed", Errors));
                }
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Update permission failed", ValidationHelper.GetErrorDictionary(errors)));

                Permission? permission = _context.Permissions.FirstOrDefault(o => o.code == request.code && o.permission_id != id);
                if(permission != null)
                    Errors?.Add("code", new[] { "The field 'code' value is already exist."} );

                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Create permission failed", Errors));
                    
                data.code = request.code;
                data.name = request.name;
                data.description = request.description;
                data.updated_at = DateTime.Now;

                _context.Permissions.Update(data);
                _context.SaveChanges();
                return Ok(ApiResponse<PermissionDto>.Ok("Update permission successfully", data.MapToDto()));
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
                var data = _context.Permissions.FirstOrDefault(u => u.permission_id == id);
                if (data == null)
                {
                    Errors?.Add("permission_id", new[] { "The field 'permission_id' value is not found." });
                    return NotFound(ApiResponse<object>.Fail("Delete permission failed", Errors));
                }
                var dataList = _context.RolePermissions.Where(o => o.permission_id == data.permission_id).ToList();
                if(dataList.Count() > 0)
                {
                    Errors?.Add("permission_id", new[] { "The field 'permission_id' is used in Role Permission." });
                    return NotFound(ApiResponse<object>.Fail("Delete permission failed", Errors));
                }
                _context.Permissions.Remove(data);
                _context.SaveChanges();
                return Ok(ApiResponse<object>.Ok("Delete permission successfully"));
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
            var query = _context.Permissions.AsQueryable();

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


            var recordsTotal = _context.Permissions.Count();
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