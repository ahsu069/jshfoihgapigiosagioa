using api.Commons;
using api.Data;
using api.Models;
using api.Models.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll(string? code, string? name, string? description, bool? is_active, string? orderColumn, string? orderDir)
        {
            try
            {
                var query = _context.Roles.AsQueryable();
                query = query.Where(r =>
                    (r.code.Contains(code ?? string.Empty) || String.IsNullOrEmpty(code))
                    && (r.name.Contains(name ?? string.Empty) || String.IsNullOrEmpty(name))
                    && (r.description!.Contains(description ?? string.Empty) || String.IsNullOrEmpty(description))
                    && (r.is_active == is_active || is_active == null)
                );
                if (!string.IsNullOrEmpty(orderColumn))
                    query = orderDir == "asc"
                        ? query.OrderByDynamic(orderColumn, true)
                        : query.OrderByDynamic(orderColumn, false);
                else
                    query = query.OrderBy(r => r.name);
                var data = query.ToList().Select(r => r.MapToDto());
                return Ok(ApiResponse<IEnumerable<RoleDto>>.Ok("Role retrieved successfully", data));
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
                var data = _context.Roles.FirstOrDefault(u => u.role_id == id);
                if (data == null)
                {
                    Errors?.Add("role_id", new[] { "The field 'role_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Get role detail failed", Errors));
                }
                return Ok(ApiResponse<RoleDto>.Ok("Role detail retrieved successfully", data.MapToDto()));
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
        public IActionResult Create([FromBody] RoleRequest request)
        {
            try
            {
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Create role failed", ValidationHelper.GetErrorDictionary(errors)));

                Role data = request.MapToDtoFromCreate();
                data.role_id = Guid.NewGuid();
                data.code = request.code;
                data.name = request.name;
                data.description = request.description;
                data.is_active = request.is_active;
                data.created_at = DateTime.Now;
                data.updated_at = DateTime.Now;

                _context.Roles.Add(data);
                _context.SaveChanges();
                return Ok(ApiResponse<RoleDto>.Ok("Create role successfully", data.MapToDto()));
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
        public IActionResult Update(Guid id, [FromBody] RoleRequest request)
        {
            try
            {
                var data = _context.Roles.FirstOrDefault(u => u.role_id == id);
                if (data == null)
                {
                    Errors?.Add("role_id", new[] { "The field 'role_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Update role failed", Errors));
                }
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Update role failed", ValidationHelper.GetErrorDictionary(errors)));

                data.code = request.code;
                data.name = request.name;
                data.description = request.description;
                data.is_active = request.is_active;
                data.updated_at = DateTime.Now;

                _context.Roles.Update(data);
                _context.SaveChanges();
                return Ok(ApiResponse<RoleDto>.Ok("Update role successfully", data.MapToDto()));
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
                var data = _context.Roles.FirstOrDefault(u => u.role_id == id);
                if (data == null)
                {
                    Errors?.Add("role_id", new[] { "The field 'role_id' value is not found." });
                    return NotFound(ApiResponse<object>.Fail("Delete role failed", Errors));
                }
                var dataList = _context.RolePermissions.Where(o => o.role_id == data.role_id).ToList();
                if(dataList.Count() > 0)
                {
                    Errors?.Add("role_id", new[] { "The field 'role_id' is used in Role Permission." });
                    return NotFound(ApiResponse<object>.Fail("Delete role failed", Errors));
                }var dataList2 = _context.UserRoles.Where(o => o.role_id == data.role_id).ToList();
                if(dataList.Count() > 0)
                {
                    Errors?.Add("role_id", new[] { "The field 'role_id' is used in User Role." });
                    return NotFound(ApiResponse<object>.Fail("Delete role failed", Errors));
                }

                _context.Roles.Remove(data);
                _context.SaveChanges();

                return Ok(ApiResponse<object>.Ok("Delete role successfully"));
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
            var query = _context.Roles.AsQueryable();

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

            var recordsTotal = _context.Roles.Count();
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
