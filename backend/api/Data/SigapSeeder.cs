using api.Models;

namespace api.Data
{
    public static class SigapSeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            SeedRoles(db);
            SeedCategoryTransaction(db);
            SeedCategoryEmployee(db);
            SeedApprovalRoleMap(db);
            SeedUsers(db);
            SeedSuperAdminRole(db);   
            SeedPermissions(db);      
            SeedRolePermissions(db);  
        }

        // Seeder Jenis User
        private static void SeedRoles(ApplicationDbContext db)
        {
            if (db.Roles.Any()) return;

            db.Roles.AddRange(
                new Role { role_id = Guid.NewGuid(), name = "Admin Gudang",        code = "ADMIN_GUDANG",  created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Role { role_id = Guid.NewGuid(), name = "Section Head",        code = "SH_NON_SAFETY", created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Role { role_id = Guid.NewGuid(), name = "Safety Section Head", code = "SH_SAFETY",     created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Role { role_id = Guid.NewGuid(), name = "Manajemen",           code = "MANAJEMEN",     created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Role { role_id = Guid.NewGuid(), name = "Staff",               code = "STAFF",         created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow }
            );
            db.SaveChanges();
        }

        // Seeder Jenis Transaksi
        private static void SeedCategoryTransaction(ApplicationDbContext db)
        {
            if (db.CategoryTransactions.Any()) return;

            db.CategoryTransactions.AddRange(
                new CategoryTransaction { kategori_transact_id = "IN",     nama_kategori_transact = "IN",     created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new CategoryTransaction { kategori_transact_id = "OUT",    nama_kategori_transact = "OUT",    created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new CategoryTransaction { kategori_transact_id = "BORROW", nama_kategori_transact = "BORROW", created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow }
            );
            db.SaveChanges();
        }

        // Seeder Tipe User Transaksi
        private static void SeedCategoryEmployee(ApplicationDbContext db)
        {
            if (db.CategoryEmployees.Any()) return;

            db.CategoryEmployees.AddRange(
                new CategoryEmployee { kategori_pekerja_id = "OWN", nama_kategori = "Pekerja Sendiri", created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new CategoryEmployee { kategori_pekerja_id = "KON", nama_kategori = "Kontraktor",      created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow }
            );
            db.SaveChanges();
        }

        // Seeder Approval Role Map
        private static void SeedApprovalRoleMap(ApplicationDbContext db)
        {
            if (db.ApprovalRoleMaps.Any()) return;

            var shNonSafety = db.Roles.FirstOrDefault(r => r.code == "SH_NON_SAFETY");
            var shSafety    = db.Roles.FirstOrDefault(r => r.code == "SH_SAFETY");
            var adminGudang = db.Roles.FirstOrDefault(r => r.code == "ADMIN_GUDANG");

            if (shNonSafety == null || shSafety == null || adminGudang == null) return;

            db.ApprovalRoleMaps.AddRange(
                new ApprovalRoleMap { approval_role_map_id = Guid.NewGuid(), legacy_code = "1", role_id = shNonSafety.role_id, note = $"Legacy 1 -> {shNonSafety.name}", created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new ApprovalRoleMap { approval_role_map_id = Guid.NewGuid(), legacy_code = "2", role_id = shSafety.role_id,    note = $"Legacy 2 -> {shSafety.name}",    created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new ApprovalRoleMap { approval_role_map_id = Guid.NewGuid(), legacy_code = "3", role_id = adminGudang.role_id, note = $"Legacy 3 -> {adminGudang.name}", created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow }
            );
            db.SaveChanges();
        }

        // Seeder Default Admin User
        private static void SeedUsers(ApplicationDbContext db)
        {
            if (db.SigapUsers.Any()) return;

            var adminUser = new SigapUser
            {
                user_id    = Guid.NewGuid(),
                nama       = "Admin Gudang",
                username   = "admin",
                password   = "$2a$12$rWVXVFI6l.a5JxExzj4K8O/m5fyTj7FRmI8o1HVxspcnkZYEtv6e2",
                bagian_id  = null,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            db.SigapUsers.Add(adminUser);
            db.SaveChanges();
            
            var adminRole = db.Roles.FirstOrDefault(r => r.code == "ADMIN_GUDANG");
            if (adminRole != null)
            {
                db.UserRoles.Add(new UserRole
                {
                    user_id = adminUser.user_id,
                    role_id = adminRole.role_id
                });
                db.SaveChanges();
            }
        }

        // Seeder Super Admin Role
        private static void SeedSuperAdminRole(ApplicationDbContext db)
        {
            if (db.Roles.Any(r => r.code == "SUPER_ADMIN")) return;

            db.Roles.Add(new Role
            {
                role_id     = Guid.NewGuid(),
                code        = "SUPER_ADMIN",
                name        = "Super Admin",
                description = "Full system access including permission management",
                is_active   = true,
                created_at  = DateTime.UtcNow,
                updated_at  = DateTime.UtcNow,
            });
            db.SaveChanges();
        }

        //  Seeder Permissions
        private static void SeedPermissions(ApplicationDbContext db)
        {
            if (db.Permissions.Any()) return;

            var now = DateTime.UtcNow;
            db.Permissions.AddRange(
                new Permission { permission_id = Guid.NewGuid(), code = "VIEW_DASHBOARD",    name = "View Dashboard",               description = "Access the main dashboard page",                created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "VIEW_STOCK",        name = "View Stock Gudang",            description = "View warehouse stock / item list",              created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "ADD_ITEM",          name = "Add Item",                     description = "Add new item to warehouse stock",               created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "EDIT_ITEM",         name = "Edit Item",                    description = "Edit existing warehouse item",                  created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "DELETE_ITEM",       name = "Delete Item",                  description = "Delete item from warehouse stock",              created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "VIEW_TRANSAKSI",    name = "View Transaksi Barang",        description = "View goods in/out transaction page",            created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "CREATE_PERMINTAAN", name = "Submit Permintaan",            description = "Submit a new item request",                    created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "VIEW_RIWAYAT",      name = "View Riwayat Transaksi (Own)", description = "View own transaction history",                 created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "VIEW_RIWAYAT_ALL",  name = "View Riwayat Transaksi (All)", description = "View all users transaction history",           created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "APPROVE_GUDANG",    name = "Approve as Gudang",            description = "Approve/reject requests at Gudang step",       created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "APPROVE_SH",        name = "Approve as Section Head",      description = "Approve/reject requests at Section Head step", created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "APPROVE_SAFETY",    name = "Approve as Safety",            description = "Approve/reject requests at Safety step",       created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "APPROVE_MANAJEMEN", name = "Approve as Manajemen",         description = "Approve/reject requests at Management step",   created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "VIEW_LAPORAN",      name = "View Laporan",                 description = "Access transaction and stock report pages",    created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "EXPORT_LAPORAN",    name = "Export Laporan",               description = "Export reports to Excel or PDF",               created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "MANAGE_USERS",      name = "Manage Users",                 description = "View, create, edit, assign roles to users",    created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "MANAGE_ROLE",       name = "Manage Roles",                 description = "Create and manage roles",                      created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "MANAGE_PERMISSION", name = "Manage Permissions",           description = "Create, edit, and assign permissions to roles", created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "MANAGE_CATEGORY",   name = "Manage Categories",            description = "Manage item and transaction categories",        created_at = now, updated_at = now },
                new Permission { permission_id = Guid.NewGuid(), code = "VIEW_NOTIFIKASI",   name = "View Notifikasi",              description = "View in-app notifications",                    created_at = now, updated_at = now }
            );
            db.SaveChanges();
        }

        // Seeder Role2Permission Mapping
        private static void SeedRolePermissions(ApplicationDbContext db)
        {
            if (db.RolePermissions.Any()) return;

            var now = DateTime.UtcNow;

            Guid RoleId(string code) => db.Roles.First(r => r.code == code).role_id;
            Guid PermId(string code) => db.Permissions.First(p => p.code == code).permission_id;

            void Grant(string roleCode, string[] permCodes)
            {
                var roleId = RoleId(roleCode);
                foreach (var pc in permCodes)
                    db.RolePermissions.Add(new RolePermission
                    {
                        role_permission_id = Guid.NewGuid(),
                        role_id            = roleId,
                        permission_id      = PermId(pc),
                        created_at         = now,
                        updated_at         = now,
                    });
            }

            Grant("STAFF", new[] {
                "VIEW_DASHBOARD", "VIEW_STOCK", "CREATE_PERMINTAAN",
                "VIEW_RIWAYAT", "VIEW_NOTIFIKASI"
            });

            Grant("SH_NON_SAFETY", new[] {
                "VIEW_DASHBOARD", "VIEW_STOCK", "CREATE_PERMINTAAN",
                "VIEW_RIWAYAT", "APPROVE_SH", "VIEW_NOTIFIKASI"
            });

            Grant("MANAJEMEN", new[] {
                "VIEW_DASHBOARD", "VIEW_STOCK", "CREATE_PERMINTAAN",
                "VIEW_RIWAYAT", "VIEW_RIWAYAT_ALL",
                "APPROVE_MANAJEMEN", "VIEW_LAPORAN", "EXPORT_LAPORAN",
                "VIEW_NOTIFIKASI"
            });

            Grant("SH_SAFETY", new[] {
                "VIEW_DASHBOARD", "VIEW_STOCK", "ADD_ITEM", "EDIT_ITEM",
                "VIEW_TRANSAKSI", "CREATE_PERMINTAAN",
                "VIEW_RIWAYAT", "VIEW_RIWAYAT_ALL",
                "APPROVE_SAFETY", "VIEW_LAPORAN", "EXPORT_LAPORAN",
                "VIEW_NOTIFIKASI"
            });

            Grant("ADMIN_GUDANG", new[] {
                "VIEW_DASHBOARD", "VIEW_STOCK", "ADD_ITEM", "EDIT_ITEM", "DELETE_ITEM",
                "VIEW_TRANSAKSI", "CREATE_PERMINTAAN",
                "VIEW_RIWAYAT", "VIEW_RIWAYAT_ALL",
                "APPROVE_GUDANG", "VIEW_LAPORAN", "EXPORT_LAPORAN",
                "MANAGE_USERS", "MANAGE_ROLE", "MANAGE_CATEGORY",
                "VIEW_NOTIFIKASI"
            });

            Grant("SUPER_ADMIN", new[] {
                "VIEW_DASHBOARD", "VIEW_STOCK", "ADD_ITEM", "EDIT_ITEM", "DELETE_ITEM",
                "VIEW_TRANSAKSI", "CREATE_PERMINTAAN",
                "VIEW_RIWAYAT", "VIEW_RIWAYAT_ALL",
                "APPROVE_GUDANG", "APPROVE_SH", "APPROVE_SAFETY", "APPROVE_MANAJEMEN",
                "VIEW_LAPORAN", "EXPORT_LAPORAN",
                "MANAGE_USERS", "MANAGE_ROLE", "MANAGE_PERMISSION", "MANAGE_CATEGORY",
                "VIEW_NOTIFIKASI"
            });

            db.SaveChanges();
        }
    }
}