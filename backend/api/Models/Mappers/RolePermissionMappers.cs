namespace api.Models.Mappers
{
    public static class RolePermissionMappers
    {
        
         public static RolePermissionDto MapToDto(this RolePermission? dto)
        {
            if (dto == null) return new RolePermissionDto();

            return new RolePermissionDto
            {
                role_permission_id = dto.role_permission_id,
                role_id = dto.role_id,
                RoleDto = dto.RoleDto.MapToDto(),
                permission_id = dto.permission_id,
                PermissionDto = dto.PermissionDto.MapToDto(),
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
        public static RolePermission MapToDtoFromCreate(this RolePermissionRequest? dto)
        {
            if (dto == null) return new RolePermission();

            return new RolePermission
            {
                role_id = dto.role_id,
                permission_id = dto.permission_id,
            };
        }
    }
}