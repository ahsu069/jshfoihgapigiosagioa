using api.Data;
namespace api.Models.Mappers
{
    public static class ItemMappers
    {
         public static ItemDto MapToDto(this Item? dto, HttpRequest request)
        {
            
            if (dto == null) return new ItemDto();
            var baseUrl = $"{request.Scheme}://{request.Host}";

            ApplicationDbContext _context;
            return new ItemDto
            {
                barang_id = dto.barang_id,
                nama_barang = dto.nama_barang,
                msl_barang = dto.msl_barang.GetValueOrDefault(0),
                jumlah_barang = dto.jumlah_barang,
                booked_qty = dto.booked_qty,
                satuanbar_id = dto.satuanbar_id,
                uomDto = dto.uomDto.MapToDto(),
                kategoribar_id = dto.kategoribar_id,
                categoryDto = dto.categoryDto.MapToDto(),
                link_gambar_bar = (!string.IsNullOrEmpty(dto.link_gambar_bar)) ? $"{baseUrl}/{dto.link_gambar_bar}" : null,
                status_bar = dto.status_bar,
                is_deleted = dto.is_deleted,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
                // readiness_item = (dto.msl_barang.GetValueOrDefault(0) == 0) ? Math.Round((decimal)0, 2) : Math.Round((decimal)(dto.jumlah_barang / dto.msl_barang * 100), 2)
            };
        }
        public static Item MapToDtoFromCreate(this ItemRequest? dto)
        {
            if (dto == null) return new Item();

            return new Item
            {
                // namakategoribar = dto.namakategoribar,
                // is_deleted = dto.is_deleted,
            };
        }
    }
}