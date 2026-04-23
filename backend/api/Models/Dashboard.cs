namespace api.Models
{
    public class DashboardDto
    {
        public int transact_in_cnt { get; set; }
        public int transact_out_cnt { get; set; }
        public int transact_pending_cnt { get; set; }
        public int item_low_stock_cnt { get; set; }
        public int item_ready_stock_cnt { get; set; }
        public List<TransactionHistoryDto> LatestTransactionInDto { get; set; } = new List<TransactionHistoryDto>();
        public List<TransactionHistoryDto> LatestTransactionOutDto { get; set; } = new List<TransactionHistoryDto>();
    }
    public class DashboardReadinessDto
    {
        public Guid kategoribar_id { get; set; } = Guid.NewGuid();
        public string namakategoribar { get; set; } = string.Empty;
        public bool is_deleted { get; set; } = false;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public int item_low_stock_cnt { get; set; }
        public int item_ready_stock_cnt { get; set; }
        public List<ItemDto> ItemDto { get; set; } = new List<ItemDto>();

        // public decimal readiness_item { get; set; }
        
    }
}