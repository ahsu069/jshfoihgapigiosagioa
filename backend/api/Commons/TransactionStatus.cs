namespace api.Commons
{
    public static class TransactionStatus
    {
        public const string PENDING_SUPERVISOR   = "Menunggu Approval Supervisor";
        public const string DIPROSES_GUDANG      = "Diproses Gudang";
        public const string DONE                 = "Done";
        public const string DITOLAK_SUPERVISOR   = "Ditolak Supervisor";
        public const string DITOLAK_GUDANG       = "Ditolak Gudang";
        public const string CANCELLED            = "Cancelled";
    }
}