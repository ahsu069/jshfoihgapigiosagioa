namespace api.Models.Mappers
{
    public static class UomMappers
    {
         public static UomDto MapToDto(this Uom? dto)
        {
            if (dto == null) return new UomDto();

            return new UomDto
            {
                satuanbar_id = dto.satuanbar_id,
                nama_satuanbar = dto.nama_satuanbar,
                is_deleted = dto.is_deleted,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
    }
}