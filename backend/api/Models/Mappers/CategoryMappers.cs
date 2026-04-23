namespace api.Models.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDto MapToDto(this Category? dto)
        {
            if (dto == null) return new CategoryDto();

            return new CategoryDto
            {
                kategoribar_id = dto.kategoribar_id,
                namakategoribar = dto.namakategoribar,
                is_deleted = dto.is_deleted,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
                // ItemDto = dto.Item.Select(o => new ItemDto
                // {
                //     // transact_detail_id = o.transact_detail_id,
                //     // transact_id = o.transact_id,
                //     // barang_id = o.barang_id,
                //     // itemDto = o.Item.MapToDto(request),
                //     // jumlah_bar = o.jumlah_bar,
                //     created_at = o.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                //     updated_at = o.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
                // }).ToList(),
                // readiness_item = (dto.Item.Sum(o => o.msl_barang) == 0) ? Math.Round((decimal)0,2) : Math.Round((decimal)(dto.Item.Sum(o => o.jumlah_barang) / dto.Item.Sum(o => o.msl_barang) * 100), 2)
            };
        }
        public static Category MapToDtoFromCreate(this CategoryRequest? dto)
        {
            if (dto == null) return new Category();

            return new Category
            {
                namakategoribar = dto.namakategoribar,
                is_deleted = dto.is_deleted,
            };
        }
        public static CategoryEmployeeDto CategoryEmployeeMapToDto(this CategoryEmployee? dto)
        {
            if (dto == null) return new CategoryEmployeeDto();

            return new CategoryEmployeeDto
            {
                kategori_pekerja_id = dto.kategori_pekerja_id,
                nama_kategori = dto.nama_kategori,
                is_deleted = dto.is_deleted,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
        public static CategoryTransactionDto CategoryTransactionMapToDto(this CategoryTransaction? dto)
        {
            if (dto == null) return new CategoryTransactionDto();

            return new CategoryTransactionDto
            {
                kategori_transact_id = dto.kategori_transact_id,
                nama_kategori_transact = dto.nama_kategori_transact,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
    }
}