using System.Linq.Expressions;
using api.Commons;
using api.Data;
using api.Models;
using api.Models.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                List<string> CategoryTransOut = new() { "OUT" };

                DashboardDto dashboardDto = new DashboardDto();
                dashboardDto.transact_in_cnt = _context.TransactionHistories.Count(o => !CategoryTransOut.Contains(o.kategori_transact_id)
                    && o.status.ToLower() == "done"
                );
                dashboardDto.LatestTransactionInDto = _context.TransactionHistories
                .Include(o => o.CategoryTransactionsDto)
                        .Include(o => o.CategoryEmployeeDto)
                        .Include(o => o.UsersCacheDto)
                        .Include(o => o.EmployeeDto)
                            .ThenInclude(d => d.BagianUserDto)
                        .Include(o => o.ApprovalManajemenPekerjaIdDto!)
                                .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.ApprovalGudangIdDto!)
                                .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.ApprovalSectionheadIdDto!)
                                .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.TransactionDetails)
                            .ThenInclude(d => d.itemDto)
                .Where(o => !CategoryTransOut.Contains(o.kategori_transact_id))
                .OrderByDescending(o => o.created_at)
                .Select(o => o.MapToDto(Request, User))
                .Skip(0).Take(10)
                .ToList();

                dashboardDto.transact_out_cnt = _context.TransactionHistories.Count(o => CategoryTransOut.Contains(o.kategori_transact_id)
                    && o.status.ToLower() == "done"
                );
                dashboardDto.LatestTransactionOutDto = _context.TransactionHistories
                .Include(o => o.CategoryTransactionsDto)
                        .Include(o => o.CategoryEmployeeDto)
                        .Include(o => o.UsersCacheDto)
                        .Include(o => o.EmployeeDto)
                            .ThenInclude(d => d.BagianUserDto)
                        .Include(o => o.ApprovalManajemenPekerjaIdDto!)
                                .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.ApprovalGudangIdDto!)
                                .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.ApprovalSectionheadIdDto!)
                                .ThenInclude(d => d.usersCacheDto)
                        .Include(o => o.TransactionDetails)
                            .ThenInclude(d => d.itemDto)
                .Where(o => CategoryTransOut.Contains(o.kategori_transact_id))
                .OrderByDescending(o => o.created_at)
                .Select(o => o.MapToDto(Request, User))
                .Skip(0).Take(10)
                .ToList();

                dashboardDto.transact_pending_cnt = _context.TransactionHistories.Count(o => true
                    && o.status.ToLower() != "done" && !o.status.ToLower().Contains("rejected")
                );
                dashboardDto.item_low_stock_cnt = _context.Items.Count(o => o.jumlah_barang <= o.msl_barang.GetValueOrDefault(0) && !o.is_deleted);
                dashboardDto.item_ready_stock_cnt = _context.Items.Count(o => o.jumlah_barang > o.msl_barang.GetValueOrDefault(0) && !o.is_deleted);
                // dashboardDto.readiness_item = (dashboardDto.total_item_ready_stock + dashboardDto.total_item_low_stock) == 0 ? Math.Round((decimal)0,2) : Math.Round((decimal)(dashboardDto.total_item_ready_stock / (dashboardDto.total_item_ready_stock + dashboardDto.total_item_low_stock)) * 100, 2);

                return Ok(ApiResponse<DashboardDto>.Ok("Dashboard retrieved successfully", dashboardDto));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [HttpGet("GetReadiness")]
        public IActionResult GetReadiness()
        {
            try
            {
                var data = _context.Categorys
                    .Where(c => !c.is_deleted)
                    .Select(cat => new Category
                    {
                        kategoribar_id = cat.kategoribar_id,
                        namakategoribar = cat.namakategoribar,
                        is_deleted = cat.is_deleted,
                        created_at = cat.created_at,
                        updated_at = cat.updated_at,    
                        // 🟩 Project ItemDto manually so we can add booking_qty
                        ItemDto = cat.ItemDto
                            .Where(i => !i.is_deleted)
                            .Select(i => new Item
                            {
                                barang_id = i.barang_id,
                                nama_barang = i.nama_barang,
                                jumlah_barang = i.jumlah_barang,
                                msl_barang = i.msl_barang,
                                satuanbar_id = i.satuanbar_id,
                                kategoribar_id = i.kategoribar_id,
                                status_bar = i.status_bar,
                                link_gambar_bar = i.link_gambar_bar,
                                is_deleted = i.is_deleted,
                                created_at = i.created_at,
                                updated_at = i.updated_at,

                                // 🟢 ADD SUBQUERY FOR EACH ITEM HERE
                                booked_qty = (
                                    from d in _context.TransactionDetails
                                    join h in _context.TransactionHistories
                                        on d.transact_id equals h.transact_id
                                    where d.barang_id == i.barang_id
                                        && h.status.ToLower() != "done"
                                        && !h.status.ToLower().Contains("rejected")
                                    select (int?)d.jumlah_bar
                                ).Sum() ?? 0
                            })
                            .ToList()
                    })
                    .OrderBy(cat => cat.namakategoribar)
                    .ToList();

                var result = data.Select(r => r.DashboardReadinessMapToDto(Request));

                return Ok(ApiResponse<IEnumerable<DashboardReadinessDto>>.Ok(
                    "Dashboard readiness retrieved successfully",
                    result
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
    }
}