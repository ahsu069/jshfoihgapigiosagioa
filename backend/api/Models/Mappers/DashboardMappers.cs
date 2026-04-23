namespace api.Models.Mappers
{
    public static class DashboardMappers
    {
        public static DashboardReadinessDto DashboardReadinessMapToDto(this Category? dto, HttpRequest request)
        {
            if (dto == null) return new DashboardReadinessDto();

            return new DashboardReadinessDto
            {
                kategoribar_id = dto.kategoribar_id,
                namakategoribar = dto.namakategoribar,
                is_deleted = dto.is_deleted,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
                ItemDto = dto.ItemDto.Select(o => o.MapToDto(request)).ToList(),
                item_low_stock_cnt = dto.ItemDto.Where(o => o.kategoribar_id == dto.kategoribar_id && (o.jumlah_barang - o.booked_qty) <= o.msl_barang).Count(),
                item_ready_stock_cnt = dto.ItemDto.Where(o => o.kategoribar_id == dto.kategoribar_id && (o.jumlah_barang - o.booked_qty) > o.msl_barang).Count()
                // readiness_item = (dto.Item.Sum(o => o.msl_barang) == 0) ? Math.Round((decimal)0,2) : Math.Round((decimal)(dto.Item.Sum(o => o.jumlah_barang) / dto.Item.Sum(o => o.msl_barang) * 100), 2)
            };
        }
    }
}