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
    public class ProfileController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetProfile()
        {
            try
            {
                var tokenUserid = User.Identity?.Name;
                if (tokenUserid == null)
                {
                    Errors?.Add("token", new[] { "The bearer 'token' is invalid." });
                    return NotFound(ApiResponse<object>.Fail("Get profile detail failed", Errors));
                }

                Guid user_id = Guid.Empty;
                Guid.TryParse(tokenUserid.ToString(), out user_id);
                
                var data = _context.SigapUsers.Include(o => o.BagianUserDto).FirstOrDefault(u => u.user_id == user_id);
                if (data == null)
                {
                    Errors?.Add("user_id", new[] { "The field 'user_id' value is not found." });
                    return NotFound(ApiResponse<object>.Fail("Get user detail failed", Errors));
                }

                int? fungsiId = data.BagianUserDto?.fungsi_id;
                FungsiUser fungsiUser;
                if (fungsiId.HasValue)
                {
                    fungsiUser = _context.FungsiUsers
                        .FirstOrDefault(f => f.fungsi_id == fungsiId.Value) ?? new FungsiUser();
                }
                else
                {
                    fungsiUser = new FungsiUser();
                }
                UserRole userRole = _context.UserRoles.FirstOrDefault(ur => ur.user_id == user_id) ?? new UserRole();
                Role role = (from r in _context.Roles
                             join ur in _context.UserRoles on r.role_id equals ur.role_id
                             where ur.user_id == user_id
                             select new Role
                             {
                                 role_id = r.role_id,
                                 code = r.code,
                                 name = r.name,
                                 description = r.description,
                                 is_active = r.is_active,
                                 created_at = r.created_at,
                                 updated_at = r.updated_at,
                             })
                .FirstOrDefault() ?? new Role();
                List<RolePermission> rolePermissions = _context.RolePermissions.Where(rp => rp.role_id == role.role_id).ToList() ?? new List<RolePermission>();
                List<PermissionDto> permissions = (from p in _context.Permissions
                                   join rp in _context.RolePermissions on p.permission_id equals rp.permission_id
                                   where rp.role_id == role.role_id
                                   select new PermissionDto
                                   {
                                       permission_id = p.permission_id,
                                       code = p.code,
                                       name = p.name,
                                       description = p.description,
                                       created_at = p.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                                       updated_at = p.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
                                   })
                .ToList();
                
                return Ok(ApiResponse<ProfileDto>.Ok("Profile retrieved successfully", data.ProfileMapToDto(fungsiUser, role, permissions)));
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
    }
}