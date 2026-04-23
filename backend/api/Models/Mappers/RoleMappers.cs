namespace api.Models.Mappers
{
    public static class RoleMappers
    {
         public static RoleDto MapToDto(this Role? dto)
        {
            if (dto == null) return new RoleDto();

            return new RoleDto
            {
                role_id = dto.role_id,
                code = dto.code,
                name = dto.name,
                description = dto.description!,
                is_active = dto.is_active,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
        public static Role MapToDtoFromCreate(this RoleRequest? dto)
        {
            if (dto == null) return new Role();

            return new Role
            {
                code = dto.code,
                name = dto.name,
                description = dto.description,
                is_active = dto.is_active,
            };
        }
    }
}
