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
    public class SharedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public SharedController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetBagianUserSelect2")]
        public IActionResult GetBagianUserSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.BagianUsers.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.nama.ToLower().Contains(keyword));
            }

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.nama)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.bagian_id,
                    text = u.nama
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        [HttpGet("GetCategorySelect2")]
        public IActionResult GetKategoriSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.Categorys.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.namakategoribar.ToLower().Contains(keyword));
            }
            query = query.Where(u => !u.is_deleted);

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.namakategoribar)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.kategoribar_id,
                    text = u.namakategoribar
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        [HttpGet("GetCategoryEmployeeSelect2")]
        public IActionResult GetCategoryEmployeeSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.CategoryEmployees.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.nama_kategori.ToLower().Contains(keyword));
            }
            query = query.Where(u => !u.is_deleted);

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.nama_kategori)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.kategori_pekerja_id,
                    text = u.nama_kategori
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        [HttpGet("GetCategoryTransactionsSelect2")]
        public IActionResult GetCategoryTransactionsSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.CategoryTransactions.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.nama_kategori_transact.ToLower().Contains(keyword));
            }

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.nama_kategori_transact)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.kategori_transact_id,
                    text = u.nama_kategori_transact
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        [HttpGet("GetEmployeeSelect2")]
        public IActionResult GetEmployeeSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? bagian_id = null
        )
        {
            var query = _context.Employees
            .Include(e => e.BagianUserDto)
            .AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.nama_pekerja.ToLower().Contains(keyword) || u.id_finger.ToLower().Contains(keyword));

            }
            if (bagian_id.HasValue)
            {
                query = query.Where(u => u.bagian_id == bagian_id.Value);
            }

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.nama_pekerja)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.pekerja_temp_id,
                    text = u.id_finger,
                    u.nama_pekerja,
                    u.fungsi_pekerja,
                    u.id_finger,
                    u.perusahaan_pekerja,
                    u.link_file_pendukung,
                    u.synced_at,
                    u.bagian_id,
                    // 👉 ADD nama_bagian from BagianUser
                    bagian_nm = u.BagianUserDto != null 
                        ? u.BagianUserDto.nama 
                        : null,
                    u.created_at,
                    u.updated_at
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        [HttpGet("GetFungsiUserSelect2")]
        public IActionResult GetFungsiUserSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.FungsiUsers.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.nama.ToLower().Contains(keyword));
            }

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.nama)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.fungsi_id,
                    text = u.nama
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        [HttpGet("GetItemSelect2")]
        public IActionResult GetItemSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.Items.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.nama_barang.ToLower().Contains(keyword));
            }
                query = query.Where(u => !u.is_deleted);

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.nama_barang)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.barang_id,
                    text = u.nama_barang,
                    jumlah_barang = u.jumlah_barang,
                    booked_qty = (
                        from d in _context.TransactionDetails
                        join h in _context.TransactionHistories
                            on d.transact_id equals h.transact_id
                        where d.barang_id == u.barang_id
                            && (h.status.ToLower() != "done" && !h.status.ToLower().Contains("rejected"))
                        select (int?)d.jumlah_bar
                    ).Sum() ?? 0
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        [HttpGet("GetPermissionSelect2")]
        public IActionResult GetPermissionSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.Permissions.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.name.ToLower().Contains(keyword));
            }

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.permission_id,
                    text = u.name,
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        [HttpGet("GetRoleSelect2")]
        public IActionResult GetRoleSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.Roles.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.name.ToLower().Contains(keyword));
            }
            query = query.Where(u => u.is_active);

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.role_id,
                    text = u.name,
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        // Uom
        [HttpGet("GetUomSelect2")]
        public IActionResult GetUomSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.Uoms.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.nama_satuanbar.ToLower().Contains(keyword));
            }
            query = query.Where(u => !u.is_deleted);

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.nama_satuanbar)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.satuanbar_id,
                    text = u.nama_satuanbar
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
        [HttpGet("GetUserSelect2")]
        public IActionResult GetUserSelect2(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.SigapUsers.AsQueryable();

            // 🔍 Filter by search term
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                query = query.Where(u => u.nama.ToLower().Contains(keyword));
            }

            var totalCount = query.Count();

            // 📄 Pagination
            var items = query
                .OrderBy(u => u.nama)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    id = u.user_id,
                    text = u.nama,
                })
                .ToList();

            // ✅ Tell Select2 if more data available
            var more = totalCount > page * pageSize;

            return Ok(new
            {
                results = items,
                pagination = new { more }
            });
        }
    }
}