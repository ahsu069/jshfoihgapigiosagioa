using System.Security.Claims;
using System.Security.Cryptography;
using api.Commons;
namespace api.Models.Mappers
{
    public static class TransactionHistoryMappers
    {
        
        public static TransactionHistoryDto MapToDto(this TransactionHistory? dto, HttpRequest request, ClaimsPrincipal User)
        {
            if (dto == null) return new TransactionHistoryDto();
            
            var tokenUserid = User.Identity?.Name;

            return new TransactionHistoryDto
            {
                transact_id = dto.transact_id,
                kategori_transact_id = dto.kategori_transact_id,
                CategoryTransactionsDto = dto.CategoryTransactionsDto.CategoryTransactionMapToDto(),
                kategori_pekerja = dto.kategori_pekerja,
                CategoryEmployeeDto = dto.CategoryEmployeeDto.CategoryEmployeeMapToDto(),
                no_miv_safety = dto.no_miv_safety,
                no_miv_custom = dto.no_miv_custom,
                users_cache_id = dto.users_cache_id,
                UsersCacheDto = dto.UsersCacheDto!.UsersCacheMapToDto(),
                pekerja_temp_id = dto.pekerja_temp_id,
                EmployeeDto = dto.EmployeeDto?.EmployeeMapToDto(),
                approval_manajemen_pekerja_id = dto.approval_manajemen_pekerja_id,
                ApprovalManajemenPekerjaIdDto = dto.ApprovalManajemenPekerjaIdDto.MapToDto(),
                approval_gudang_id = dto.approval_gudang_id,
                ApprovalGudangIdDto = dto.ApprovalGudangIdDto.MapToDto(),
                approval_sectionhead_id = dto.approval_sectionhead_id,
                ApprovalSectionheadIdDto = dto.ApprovalSectionheadIdDto.MapToDto(),
                status = dto.status,
                keterangan = dto.keterangan,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
                TransactionDetailDto = dto.TransactionDetails.Select(o => new TransactionDetailDto
                {
                    transact_detail_id = o.transact_detail_id,
                    transact_id = o.transact_id,
                    barang_id = o.barang_id,
                    itemDto = o.itemDto.MapToDto(request),
                    jumlah_bar = o.jumlah_bar,
                    created_at = o.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                    updated_at = o.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
                }).ToList(),
                is_allow_to_approve =
                // Supervisor's turn
                (dto?.status == TransactionStatus.PENDING_SUPERVISOR &&
                dto?.ApprovalManajemenPekerjaIdDto?.user_id == tokenUserid &&
                dto?.ApprovalManajemenPekerjaIdDto?.is_approved == null) ? true :
                // Admin Gudang normal processing turn
                (dto?.status == TransactionStatus.DIPROSES_GUDANG &&
                dto?.ApprovalGudangIdDto?.user_id == tokenUserid &&
                dto?.ApprovalGudangIdDto?.is_approved == null) ? true :
                false,

                is_allow_to_reject =
                // Supervisor can reject on pending supervisor
                (dto?.status == TransactionStatus.PENDING_SUPERVISOR &&
                dto?.ApprovalManajemenPekerjaIdDto?.user_id == tokenUserid &&
                dto?.ApprovalManajemenPekerjaIdDto?.is_approved == null) ? true :
                // Admin Gudang can reject on diproses gudang
                (dto?.status == TransactionStatus.DIPROSES_GUDANG &&
                dto?.ApprovalGudangIdDto?.user_id == tokenUserid &&
                dto?.ApprovalGudangIdDto?.is_approved == null) ? true :
                // Admin Gudang early cancel / reject while still pending supervisor
                (dto?.status == TransactionStatus.PENDING_SUPERVISOR &&
                dto?.ApprovalGudangIdDto?.user_id == tokenUserid &&
                dto?.ApprovalGudangIdDto?.is_approved == null) ? true :
                false

            };
        }
        public static TransactionHistory MapToDtoFromCreate(this TransactionHistoryRequest? dto)
        {
            if (dto == null) return new TransactionHistory();

            return new TransactionHistory
            {
                kategori_transact_id = dto.kategori_transact_id,
                kategori_pekerja = dto.kategori_pekerja,
                no_miv_safety = dto.no_miv_safety,
                no_miv_custom = dto.no_miv_custom,
                users_cache_id = dto.users_cache_id,
                keterangan = dto.keterangan,
                // pekerja_temp_id = dto.pekerja_temp_id,
            };
        }
    }
}