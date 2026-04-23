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
    public class RolePermissionController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public RolePermissionController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll(Guid? role_id, Guid? permission_id, string? orderColumn, string? orderDir)
        {
            try
            {
                var query = _context.RolePermissions.AsQueryable();
                query = query.Include(r => r.RoleDto).Include(r => r.PermissionDto);
                query = query.Where(r =>
                    (r.role_id == role_id || role_id == null)
                    && (r.permission_id == permission_id || permission_id == null)
                );
                if (!string.IsNullOrEmpty(orderColumn))
                    query = orderDir == "asc"
                        ? query.OrderByDynamic(orderColumn, true)
                        : query.OrderByDynamic(orderColumn, false);
                else
                    query = query.OrderBy(r => r.RoleDto!.name);
                var data = query.ToList().Select(r => r.MapToDto());
                return Ok(ApiResponse<IEnumerable<RolePermissionDto>>.Ok("Role Permission retrieved successfully", data));
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
                var data = _context.RolePermissions.Include(r => r.RoleDto).Include(r => r.PermissionDto).FirstOrDefault(u => u.role_permission_id == id);
                if (data == null)
                {
                    Errors?.Add("role_permission_id", new[] { "The field 'role_permission_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Get role permission detail failed", Errors));
                }
                return Ok(ApiResponse<RolePermissionDto>.Ok("Permission detail retrieved successfully", data.MapToDto()));
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
        public IActionResult Create([FromBody] RolePermissionRequest request)
        {
            try
            {
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Create role permission failed", ValidationHelper.GetErrorDictionary(errors)));

                if (!ValidationHelper.TryValidateForeignKey(
                        request.role_id,
                        id => _context.Roles.Any(d => d.role_id == id),
                        out var RolesErrors))
                    Errors?.Add("role_id", new[] { RolesErrors });

                if (!ValidationHelper.TryValidateForeignKey(
                        request.permission_id,
                        id => _context.Permissions.Any(d => d.permission_id == id),
                        out var PermissionsErrors))
                    Errors?.Add("permission_id", new[] { PermissionsErrors });

                RolePermission? rolePermission = _context.RolePermissions.FirstOrDefault(o => o.role_id == request.role_id && o.permission_id == request.permission_id);
                if(rolePermission != null)
                {
                    Errors?.Add("role_id", new[] { "The field 'role_id' value is already exist."} );
                    Errors?.Add("permission_id", new[] { "The field 'permission_id' value is already exist."} );
                }

                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Create role permission failed", Errors));

                RolePermission data = request.MapToDtoFromCreate();
                data.permission_id = Guid.NewGuid();
                data.role_id = request.role_id;
                data.permission_id = request.permission_id;
                data.created_at = DateTime.Now;
                data.updated_at = DateTime.Now;

                _context.RolePermissions.Add(data);
                _context.SaveChanges();

                if (data?.role_id != null)
                    data.RoleDto = _context.Roles.FirstOrDefault(b => b.role_id == data.role_id);
                if (data?.permission_id != null)
                    data.PermissionDto = _context.Permissions.FirstOrDefault(b => b.permission_id == data.permission_id);

                return Ok(ApiResponse<RolePermissionDto>.Ok("Create role permission successfully", data.MapToDto()));
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
        public IActionResult Update(Guid id, [FromBody] RolePermissionRequest request)
        {
            try
            {
                var data = _context.RolePermissions.FirstOrDefault(u => u.role_permission_id == id);
                if (data == null)
                {
                    Errors?.Add("role_permission_id", new[] { "The field 'role_permission_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Update role permission failed", Errors));
                }
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Update permission failed", ValidationHelper.GetErrorDictionary(errors)));

                if (!ValidationHelper.TryValidateForeignKey(
                        request.role_id,
                        id => _context.Roles.Any(d => d.role_id == id),
                        out var RolesErrors))
                    Errors?.Add("role_id", new[] { RolesErrors });

                if (!ValidationHelper.TryValidateForeignKey(
                        request.permission_id,
                        id => _context.Permissions.Any(d => d.permission_id == id),
                        out var PermissionsErrors))
                    Errors?.Add("permission_id", new[] { PermissionsErrors });

                RolePermission? rolePermission = _context.RolePermissions.FirstOrDefault(o => o.role_id == request.role_id && o.permission_id == request.permission_id && o.role_permission_id != id);
                if(rolePermission != null)
                {
                    Errors?.Add("role_id", new[] { "The field 'role_id' value is already exist."} );
                    Errors?.Add("permission_id", new[] { "The field 'permission_id' value is already exist."} );
                }
                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Update role permission failed", Errors));
                    
                data.role_id = request.role_id;
                data.permission_id = request.permission_id;
                data.updated_at = DateTime.Now;

                _context.RolePermissions.Update(data);
                _context.SaveChanges();

                if (data?.role_id != null)
                    data.RoleDto = _context.Roles.FirstOrDefault(b => b.role_id == data.role_id);
                if (data?.permission_id != null)
                    data.PermissionDto = _context.Permissions.FirstOrDefault(b => b.permission_id == data.permission_id);

                return Ok(ApiResponse<RolePermissionDto>.Ok("Update role permission successfully", data.MapToDto()));
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
                var data = _context.RolePermissions.FirstOrDefault(u => u.role_permission_id == id);
                if (data == null)
                {
                    Errors?.Add("role_permission_id", new[] { "The field 'role_permission_id' value is not found." });
                    return NotFound(ApiResponse<object>.Fail("Delete role permission failed", Errors));
                }
                
                _context.RolePermissions.Remove(data);
                _context.SaveChanges();
                return Ok(ApiResponse<object>.Ok("Delete role permission successfully"));
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
            var query = _context.RolePermissions.AsQueryable();
            query = query.Include(r => r.RoleDto).Include(r => r.PermissionDto);

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

            var recordsTotal = _context.RolePermissions.Count();
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