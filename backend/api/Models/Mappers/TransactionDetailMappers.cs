namespace api.Models.Mappers
{
    public static class TransactionDetailMappers
    {
        public static TransactionDetailDto MapToDto(this TransactionDetail? dto, HttpRequest request)
        {
            if (dto == null) return new TransactionDetailDto();

            return new TransactionDetailDto
            {
                transact_detail_id = dto.transact_detail_id,
                transact_id = dto.transact_id,
                barang_id = dto.barang_id,
                itemDto = dto.itemDto.MapToDto(request),
                jumlah_bar = dto.jumlah_bar,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
            };
        }
    }
}