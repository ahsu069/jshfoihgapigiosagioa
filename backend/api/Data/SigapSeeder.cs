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
            SeedSatuanBarang(db);
        }

        private static void SeedRoles(ApplicationDbContext db)
        {
            if (db.Roles.Any()) return;

            db.Roles.AddRange(
                new Role { role_id = Guid.NewGuid(), name = "Admin Gudang",        code = "ADMIN_GUDANG",  is_active = true, created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Role { role_id = Guid.NewGuid(), name = "Section Head",        code = "SH_NON_SAFETY", is_active = true, created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Role { role_id = Guid.NewGuid(), name = "Safety Section Head", code = "SH_SAFETY",     is_active = true, created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Role { role_id = Guid.NewGuid(), name = "Manajemen",           code = "MANAJEMEN",     is_active = true, created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Role { role_id = Guid.NewGuid(), name = "Staff",               code = "STAFF",         is_active = true, created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow }
            );
            db.SaveChanges();
        }

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

        private static void SeedSatuanBarang(ApplicationDbContext db)
        {
            if (db.Uoms.Any()) return;

            db.Uoms.AddRange(
                new Uom { satuanbar_id = "pcs", nama_satuanbar = "Pcs", is_deleted = false, created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Uom { satuanbar_id = "box", nama_satuanbar = "Box", is_deleted = false, created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new Uom { satuanbar_id = "unit", nama_satuanbar = "Unit", is_deleted = false, created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow }
            );
            db.SaveChanges();
        }

        private static void SeedCategoryEmployee(ApplicationDbContext db)
        {
            if (db.CategoryEmployees.Any()) return;

            db.CategoryEmployees.AddRange(
                new CategoryEmployee { kategori_pekerja_id = "OWN", nama_kategori = "Pekerja Sendiri", created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow },
                new CategoryEmployee { kategori_pekerja_id = "KON", nama_kategori = "Kontraktor",      created_at = DateTime.UtcNow, updated_at = DateTime.UtcNow }
            );
            db.SaveChanges();
        }

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

        private static void SeedUsers(ApplicationDbContext db)
        {
            if (db.SigapUsers.Any()) return;

            var adminUser = new SigapUser
            {
                user_id = Guid.NewGuid(),
                nama = "Admin Gudang",
                username = "admin",
                password = "$2a$12$rWVXVFI6l.a5JxExzj4K8O/m5fyTj7FRmI8o1HVxspcnkZYEtv6e2",
                bagian_id = null,
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

        private static void SeedSuperAdminRole(ApplicationDbContext db)
        {
            if (db.Roles.Any(r => r.code == "SUPER_ADMIN")) return;

            db.Roles.Add(new Role
            {
                role_id = Guid.NewGuid(),
                code = "SUPER_ADMIN",
                name = "Super Admin",
                description = "Full system access including permission management",
                is_active = true,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
            });
            db.SaveChanges();
        }

        private static void SeedPermissions(ApplicationDbContext db)
        {
            if (db.Permissions.Any()) return;

            var now = DateTime.UtcNow;
            var permissions = new List<Permission>
            {
                new() { permission_id = Guid.NewGuid(), code = "approval:read",                 name = "Approval Read",              description = "Access approval pages", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "barang:create",                 name = "Barang Create",              description = "Create barang/master stock", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "barang:delete",                 name = "Barang Delete",              description = "Delete barang/master stock", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "barang:read",                   name = "Barang Read",                description = "View barang/master stock", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "barang:update",                 name = "Barang Update",              description = "Edit barang/master stock", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "kategori_barang:create",        name = "Kategori Barang Create",     description = "Create kategori barang", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "kategori_barang:delete",        name = "Kategori Barang Delete",     description = "Delete kategori barang", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "kategori_barang:read",          name = "Kategori Barang Read",       description = "View kategori barang", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "kategori_barang:update",        name = "Kategori Barang Update",     description = "Edit kategori barang", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "permission:create",             name = "Permission Create",          description = "Create permission", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "permission:delete",             name = "Permission Delete",          description = "Delete permission", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "permission:read",               name = "Permission Read",            description = "View permission", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "permission:update",             name = "Permission Update",          description = "Edit permission", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "rbac:permission:manage",       name = "RBAC Permission Manage",     description = "Manage role-permission mapping", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "riwayat:stock:read",            name = "Riwayat Stock Read",         description = "View stock history", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "riwayat:transaksi:read",        name = "Riwayat Transaksi Read",     description = "View transaction history", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "role:create",                   name = "Role Create",                description = "Create role", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "role:delete",                   name = "Role Delete",                description = "Delete role", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "role:read",                     name = "Role Read",                  description = "View role", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "role:update",                   name = "Role Update",                description = "Edit role", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "transaksi:addtransaksi",        name = "Transaksi Add",              description = "Create transaksi pemasukan/pengeluaran", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "transaksi:pemasukan",           name = "Transaksi Pemasukan",        description = "Access transaksi pemasukan page", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "transaksi:permintaan",          name = "Transaksi Permintaan",       description = "Access transaksi permintaan page", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "transaksi:riwayat:read",        name = "Transaksi Riwayat Read",     description = "Access transaksi riwayat page", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "user:create",                   name = "User Create",                description = "Create user", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "user:delete",                   name = "User Delete",                description = "Delete user", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "user:read",                     name = "User Read",                  description = "View user", created_at = now, updated_at = now },
                new() { permission_id = Guid.NewGuid(), code = "user:update",                   name = "User Update",                description = "Edit user", created_at = now, updated_at = now }
            };

            db.Permissions.AddRange(permissions);
            db.SaveChanges();
        }

        private static void SeedRolePermissions(ApplicationDbContext db)
        {
            if (db.RolePermissions.Any()) return;

            var now = DateTime.UtcNow;

            Guid RoleId(string code) => db.Roles.First(r => r.code == code).role_id;
            Guid PermId(string code) => db.Permissions.FirstOrDefault(p => p.code == code)?.permission_id
                ?? throw new Exception($"Permission not seeded: {code}");

            void Grant(string roleCode, IEnumerable<string> permCodes)
            {
                var roleId = RoleId(roleCode);
                foreach (var pc in permCodes)
                {
                    db.RolePermissions.Add(new RolePermission
                    {
                        role_permission_id = Guid.NewGuid(),
                        role_id = roleId,
                        permission_id = PermId(pc),
                        created_at = now,
                        updated_at = now,
                    });
                }
            }

            Grant("STAFF", new[] {
                "barang:read",
                "transaksi:permintaan",
                "riwayat:transaksi:read"
            });

            Grant("SH_NON_SAFETY", new[] {
                "barang:read",
                "transaksi:permintaan",
                "riwayat:transaksi:read",
                "approval:read"
            });

            Grant("SH_SAFETY", new[] {
                "approval:read",
                "barang:read",
                "kategori_barang:read",
                "riwayat:stock:read",
                "riwayat:transaksi:read",
                "transaksi:pemasukan",
                "transaksi:permintaan"
            });

            Grant("MANAJEMEN", new[] {
                "approval:read",
                "barang:read",
                "riwayat:stock:read",
                "riwayat:transaksi:read",
                "transaksi:permintaan"
            });

            Grant("ADMIN_GUDANG", new[] {
                "approval:read",
                "barang:create",
                "barang:delete",
                "barang:read",
                "barang:update",
                "kategori_barang:create",
                "kategori_barang:delete",
                "kategori_barang:read",
                "kategori_barang:update",
                "permission:create",
                "permission:delete",
                "permission:read",
                "permission:update",
                "rbac:permission:manage",
                "riwayat:stock:read",
                "riwayat:transaksi:read",
                "role:create",
                "role:delete",
                "role:read",
                "role:update",
                "transaksi:addtransaksi",
                "transaksi:pemasukan",
                "transaksi:permintaan",
                "transaksi:riwayat:read",
                "user:create",
                "user:delete",
                "user:read",
                "user:update"
            });

            Grant("SUPER_ADMIN", new[] {
                "approval:read",
                "barang:create",
                "barang:delete",
                "barang:read",
                "barang:update",
                "kategori_barang:create",
                "kategori_barang:delete",
                "kategori_barang:read",
                "kategori_barang:update",
                "permission:create",
                "permission:delete",
                "permission:read",
                "permission:update",
                "rbac:permission:manage",
                "riwayat:stock:read",
                "riwayat:transaksi:read",
                "role:create",
                "role:delete",
                "role:read",
                "role:update",
                "transaksi:addtransaksi",
                "transaksi:pemasukan",
                "transaksi:permintaan",
                "transaksi:riwayat:read",
                "user:create",
                "user:delete",
                "user:read",
                "user:update"
            });

            db.SaveChanges();
        }
    }
}
