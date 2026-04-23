using api.Commons;
using api.Data;
using api.Models;
using api.Models.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Services;
namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll(string? nama, int? bagian_id, string? username, string? orderColumn, string? orderDir)
        {
            try
            {
                var query = _context.SigapUsers.AsQueryable();
                query = query.Include(o => o.BagianUserDto);
                query = query.Include(o => o.UserRoleDto!).ThenInclude(o => o.RoleDto);
                query = query.Where(d =>
                    (d.nama.Contains(nama ?? string.Empty) || String.IsNullOrEmpty(nama))
                    && (d.bagian_id == bagian_id || bagian_id == 0 || bagian_id == null)
                    && (d.username!.Contains(username ?? string.Empty) || String.IsNullOrEmpty(username))
                );
                if (!string.IsNullOrEmpty(orderColumn))
                    query = orderDir == "asc"
                        ? query.OrderByDynamic(orderColumn, true)
                        : query.OrderByDynamic(orderColumn, false);
                else
                    query = query.OrderBy(r => r.nama);
                var data = query.ToList().Select(r => r.MapToDto());
                return Ok(ApiResponse<IEnumerable<SigapUserDto>>.Ok("User retrieved successfully", data));
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
                var data = _context.SigapUsers.Include(o => o.BagianUserDto)
                .Include(o => o.UserRoleDto!).ThenInclude(o => o.RoleDto)
                .FirstOrDefault(u => u.user_id == id);
                if (data == null)
                {
                    Errors?.Add("user_id", new[] { "The field 'user_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Get user detail failed", Errors));
                }

                return Ok(ApiResponse<SigapUserDto>.Ok("User detail retrieved successfully", data.MapToDto()));
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
        public IActionResult Create([FromBody] SigapUserRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Create user failed", ValidationHelper.GetErrorDictionary(errors)));

                if (request.bagian_id != null)
                {
                    if (!ValidationHelper.TryValidateForeignKey(
                        request.bagian_id,
                        id => _context.BagianUsers.Any(d => d.bagian_id == id),
                        out var BagianUsersErrors))
                    Errors?.Add("bagian_id", new[] { BagianUsersErrors });
                }
                if (request?.role_id != null)
                {
                    if (!ValidationHelper.TryValidateForeignKey(
                        request.role_id,
                        id => _context.Roles.Any(d => d.role_id == id),
                        out var RolesErrors))
                    Errors?.Add("role_id", new[] { RolesErrors });
                }
                
                SigapUser? sigapUser = _context.SigapUsers.FirstOrDefault(o => o.username == request!.username);
                if(sigapUser != null)
                    Errors?.Add("username", new[] { "The field 'username' value is already exist."} );

                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Create user failed", Errors));
                    
                SigapUser data = request.MapToDtoFromCreate();
                data.user_id = Guid.NewGuid();
                data.nama = request!.nama;
                data.bagian_id = request.bagian_id;
                data.username = request.username;
                if(!String.IsNullOrEmpty(request.password))
                    data.password = AuthService.HashPassword(request.password);
                data.created_at = DateTime.Now;
                data.updated_at = DateTime.Now;

                _context.SigapUsers.Add(data);
                _context.SaveChanges();

                FungsiUser? fungsiUser = new FungsiUser();
                if (data.bagian_id != null)
                {
                    data.BagianUserDto = _context.BagianUsers.FirstOrDefault(b => b.bagian_id == data.bagian_id);
                    fungsiUser = _context.FungsiUsers.FirstOrDefault(f => data.BagianUserDto != null && f.fungsi_id == data.BagianUserDto.fungsi_id);
                }

                UsersCache usersCache = new UsersCache();
                usersCache.user_id = data.user_id.ToString();
                usersCache.nama_pekerja = data.nama;
                usersCache.fungsi_pekerja = fungsiUser!.nama;
                usersCache.bagian_pekerja = data?.BagianUserDto?.nama ?? "";
                usersCache.created_at = DateTime.Now;
                usersCache.updated_at = DateTime.Now;
                _context.UsersCaches.Add(usersCache);

                if (request?.role_id != null)
                {
                    Role? role = _context.Roles.FirstOrDefault(o => o.role_id == request.role_id);
                    UserRole userRole = new UserRole();
                    userRole.user_role_id = Guid.NewGuid();
                    userRole.user_id = data.user_id;
                    userRole.role_id = request.role_id;
                    userRole.effective_from = DateTime.Now;
                    userRole.RoleDto = role ?? new Role();
                    userRole.is_primary = true;
                    userRole.created_at = DateTime.Now;
                    userRole.updated_at = DateTime.Now;
                    userRole.SigapUsers = data;
                    _context.UserRoles.Add(userRole);

                    data.UserRoleDto = userRole;
                }

                _context.SaveChanges();
                transaction.Commit();
                return Ok(ApiResponse<SigapUserDto>.Ok("Create user successfully", data.MapToDto()));
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
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] SigapUserRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var data = _context.SigapUsers.FirstOrDefault(u => u.user_id == id);
                if (data == null)
                {
                    Errors?.Add("user_id", new[] { "The field 'user_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Update user failed", Errors));
                }
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Update user failed", ValidationHelper.GetErrorDictionary(errors)));

                if (request.bagian_id != null)
                {
                    if (!ValidationHelper.TryValidateForeignKey(
                        request.bagian_id,
                        id => _context.BagianUsers.Any(d => d.bagian_id == id),
                        out var BagianUsersErrors))
                    Errors?.Add("bagian_id", new[] { BagianUsersErrors });
                }
                if (request?.role_id != null)
                {
                    if (!ValidationHelper.TryValidateForeignKey(
                        request.role_id,
                        id => _context.Roles.Any(d => d.role_id == id),
                        out var RolesErrors))
                    Errors?.Add("role_id", new[] { RolesErrors });
                }
                SigapUser? sigapUser = _context.SigapUsers.FirstOrDefault(o => o.username == request!.username && o.user_id != id);
                if(sigapUser != null)
                    Errors?.Add("username", new[] { "The field 'username' value is already exist."} );
                if (Errors?.Any() == true)
                    return StatusCode(400, ApiResponse<object>.Fail("Update user failed", Errors));

                data.nama = request!.nama;
                data.bagian_id = request.bagian_id;
                data.username = request.username;
                if(!String.IsNullOrEmpty(request.password))
                    data.password = AuthService.HashPassword(request.password);
                data.updated_at = DateTime.Now;
                
                _context.SigapUsers.Update(data);
                _context.SaveChanges();

                FungsiUser? fungsiUser = new FungsiUser();
                if (data.bagian_id != null)
                {
                    data.BagianUserDto = _context.BagianUsers.FirstOrDefault(b => b.bagian_id == data.bagian_id);
                    fungsiUser = _context.FungsiUsers.FirstOrDefault(f => data.BagianUserDto != null && f.fungsi_id == data.BagianUserDto.fungsi_id);
                }

                UsersCache? checkUsersCache = _context.UsersCaches.FirstOrDefault(u => u.user_id == data.user_id.ToString());
                UsersCache usersCache = checkUsersCache != null ? checkUsersCache : new UsersCache();

                usersCache.user_id = data.user_id.ToString();
                usersCache.nama_pekerja = data.nama;
                usersCache.fungsi_pekerja = fungsiUser!.nama;
                usersCache.bagian_pekerja = data?.BagianUserDto?.nama ?? "";
                usersCache.updated_at = DateTime.Now;

                if(checkUsersCache == null)
                {
                    usersCache.created_at = DateTime.Now;
                    _context.UsersCaches.Add(usersCache);
                }
                else
                    _context.UsersCaches.Update(usersCache);

                UserRole? checkUserRole = _context.UserRoles.FirstOrDefault(u => u.user_id == data.user_id);
                UserRole userRole = checkUserRole != null ? checkUserRole : new UserRole();
                
                if (request?.role_id != null)
                {
                    Role? role = _context.Roles.FirstOrDefault(o => o.role_id == request.role_id);
                    userRole.role_id = request.role_id;
                    userRole.effective_from = DateTime.Now;
                    userRole.updated_at = DateTime.Now;
                    userRole.SigapUsers = data;
                    userRole.RoleDto = role ?? new Role();
                        
                    if(checkUserRole == null)
                    {
                        userRole.user_role_id = Guid.NewGuid();
                        userRole.user_id = data.user_id;
                        userRole.is_primary = true;
                        userRole.created_at = DateTime.Now;
                        _context.UserRoles.Add(userRole);
                    }
                    else
                        _context.UserRoles.Update(userRole);
                }
                else if (checkUserRole != null)
                    _context.UserRoles.Remove(checkUserRole);

                _context.SaveChanges();

                transaction.Commit();
                return Ok(ApiResponse<SigapUserDto>.Ok("Update user successfully", data.MapToDto()));
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
                var data = _context.SigapUsers.Include(o => o.UserRoleDto).FirstOrDefault(u => u.user_id == id);
                if (data == null)
                {
                    Errors?.Add("user_id", new[] { "The field 'user_id' value is not found."} );
                    return NotFound(ApiResponse<object>.Fail("Delete user failed", Errors));
                }
                if (data.UserRoleDto != null)
                {
                    _context.UserRoles.Remove(data.UserRoleDto);
                }
                _context.SigapUsers.Remove(data);
                _context.SaveChanges();

                return Ok(ApiResponse<object>.Ok("Delete user successfully"));
            }
            catch (DbUpdateConcurrencyException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [HttpPost("datatable")]
        public IActionResult GetDataTable([FromBody] DataTableRequest request)
        {
            var query = _context.SigapUsers.AsQueryable();
                query = query.Include(o => o.BagianUserDto);
                query = query.Include(o => o.UserRoleDto!).ThenInclude(o => o.RoleDto);

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

            var recordsTotal = _context.SigapUsers.Count();
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