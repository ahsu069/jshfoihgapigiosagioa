namespace api.Models.Mappers
{
    public static class UserRoleMappers
    {
        public static UserRoleDto MapToDto(this UserRole? dto)
        {
            if (dto == null) return new UserRoleDto();

            return new UserRoleDto
            {
                user_role_id = dto.user_role_id,
                user_id = dto.user_id,
                role_id = dto.role_id,
                effective_from = dto.effective_from.ToString("dd/MM/yyyy HH:mm:ss"),
                effective_to = dto.effective_to?.ToString("dd/MM/yyyy HH:mm:ss"),
                is_primary = dto.is_primary,
                RoleDto = dto.RoleDto.MapToDto()
            };
        }
    }
}