namespace api.Models.Mappers
{
    public static class BagianUserMappers
    {
        public static BagianUserDto MapToDto(this BagianUser? dto)
        {
            if (dto == null) return new BagianUserDto();

            return new BagianUserDto
            {
                bagian_id = dto.bagian_id,
                nama = dto.nama,
                fungsi_id = dto.fungsi_id,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
    }
}