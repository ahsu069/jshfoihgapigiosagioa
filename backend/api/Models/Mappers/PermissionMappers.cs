namespace api.Models.Mappers
{
    public static class PermissionMappers
    {
         public static PermissionDto MapToDto(this Permission? dto)
        {
            if (dto == null) return new PermissionDto();

            return new PermissionDto
            {
                permission_id = dto.permission_id,
                code = dto.code,
                name = dto.name,
                description = dto.description,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
        public static Permission MapToDtoFromCreate(this PermissionRequest? dto)
        {
            if (dto == null) return new Permission();

            return new Permission
            {
                code = dto.code,
                name = dto.name,
                description = dto.description,
            };
        }
    }
}