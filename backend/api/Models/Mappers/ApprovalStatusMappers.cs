namespace api.Models.Mappers
{
    public static class ApprovalStatusMappers
    {
        
        public static ApprovalStatusDto MapToDto(this ApprovalStatus? dto)
        {
            if (dto == null) return null;

            return new ApprovalStatusDto
            {
                approval_id = dto.approval_id,
                user_id = dto.user_id,
                role_type = dto.role_type,
                approval_role_id = dto.approval_role_id,
                is_approved = dto?.is_approved,
                remark = dto?.remark,
                created_at = dto!.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
                usersCacheDto = dto.usersCacheDto!.UsersCacheMapToDto()
            };
        }
    }
}