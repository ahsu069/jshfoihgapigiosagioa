using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Commons;
namespace api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SigapUser> SigapUsers { get; set; }
        public DbSet<BagianUser> BagianUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<CategoryEmployee> CategoryEmployees { get; set; }
        public DbSet<CategoryTransaction> CategoryTransactions { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Uom> Uoms { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<FungsiUser> FungsiUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UsersCache> UsersCaches { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<ApprovalRoleMap> ApprovalRoleMaps { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BagianUser>().ToTable("BagianUser");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<CategoryEmployee>().ToTable("kategori_pekerja");
            modelBuilder.Entity<CategoryTransaction>().ToTable("kategori_transaksi");
            modelBuilder.Entity<Uom>().ToTable("satuanBarang");
            modelBuilder.Entity<api.Models.Permission>().ToTable("permission");
            modelBuilder.Entity<FungsiUser>().ToTable("FungsiUser");
            // modelBuilder.Entity<Employee>().ToTable("pekerjaTemp");
            modelBuilder.Entity<UsersCache>().ToTable("usersCache");
            modelBuilder.Entity<ApprovalRoleMap>().ToTable("approval_role_map");
            // modelBuilder.Entity<ApprovalStatus>().ToTable("approval_status");

            // modelBuilder.Entity<ApprovalManajemenPekerjaId>().ToTable("approval_status");
            // modelBuilder.Entity<ApprovalGudangId>().ToTable("approval_status");
            // modelBuilder.Entity<ApprovalSectionheadId>().ToTable("approval_status");

            // modelBuilder.Entity<UserRole>().ToTable("user_role");

            modelBuilder.Entity<Category>().ToTable("kategori_barang");
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("kategori_barang");
                // entity.HasOne(o => o.Item)
                // .WithMany()
                // .HasForeignKey(o => o.kategoribar_id)
                // .HasConstraintName(null);
                entity.HasMany(c => c.ItemDto)
                .WithOne(i => i.categoryDto)
                .HasForeignKey(i => i.kategoribar_id)
                // .OnDelete(DeleteBehavior.Restrict)
                ;
            });
            modelBuilder.Entity<ApprovalStatus>(entity =>
            {
                entity.ToTable("approval_status");
                entity.Ignore(o => o.user_id);
                entity.HasOne(rp => rp.usersCacheDto)
                .WithMany()
                .HasForeignKey(rp => rp.user_id);
            });
            modelBuilder.Entity<SigapUser>(entity =>
            {
                entity.ToTable("SIGAPUser");
                entity.HasOne(o => o.BagianUserDto)
                .WithMany(c => c.SigapUsers)
                .HasForeignKey(o => o.bagian_id);
                entity.HasOne(o => o.UserRoleDto)
                .WithOne(c => c.SigapUsers)
                // .HasPrincipalKey(u => u.user_id)
                // .HasForeignKey(o => o.user_id)
                .HasForeignKey<UserRole>(r => r.user_id)
                .OnDelete(DeleteBehavior.Cascade)
                ;
            });
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("pekerjaTemp");
                entity.HasOne(o => o.BagianUserDto)
                .WithMany(c => c.Employees)
                .HasForeignKey(o => o.bagian_id);
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role");
                // entity.Ignore(o => o.role_id);
                entity.HasOne(rp => rp.RoleDto)
                .WithMany()
                .HasPrincipalKey(u => u.role_id)
                .HasForeignKey(rp => rp.role_id);
                entity.HasOne(rp => rp.SigapUsers)
                .WithOne(c => c.UserRoleDto)
                .HasForeignKey<UserRole>(r => r.user_id)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("barang");
                entity.HasOne(o => o.uomDto)
                .WithMany(c => c.Item)
                .HasForeignKey(o => o.satuanbar_id)
                .HasConstraintName(null);
                entity.HasOne(o => o.categoryDto)
                .WithMany(c => c.ItemDto)
                .HasForeignKey(o => o.kategoribar_id)
                .HasConstraintName(null);
                entity.Ignore(o => o.satuanbar_id).Ignore(o => o.kategoribar_id);
            });
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permission");
                entity.HasOne(rp => rp.RoleDto)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.role_id);
                entity.HasOne(rp => rp.PermissionDto)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.permission_id);
            });
            modelBuilder.Entity<TransactionHistory>(entity =>
            {
                entity.ToTable("transaksi_history");
                entity.HasOne(rp => rp.CategoryTransactionsDto)
                .WithMany(r => r.TransactionHistorys)
                .HasForeignKey(rp => rp.kategori_transact_id);
                entity.HasOne(rp => rp.CategoryEmployeeDto)
                .WithMany(r => r.TransactionHistorys)
                .HasForeignKey(rp => rp.kategori_pekerja);
                entity.HasOne(o => o.UsersCacheDto)
                .WithMany(c => c.TransactionHistorys)
                .HasForeignKey(o => o.users_cache_id);
                entity.HasOne(o => o.EmployeeDto)
                .WithMany(c => c.TransactionHistorys)
                .HasForeignKey(o => o.pekerja_temp_id);
                entity
                    .HasOne(t => t.ApprovalManajemenPekerjaIdDto)
                    .WithMany()
                    .HasForeignKey(t => t.approval_manajemen_pekerja_id)
                    .HasPrincipalKey(a => a.approval_id)
                    .IsRequired(false);
                entity
                    .HasOne(t => t.ApprovalGudangIdDto)
                    .WithMany()
                    .HasForeignKey(t => t.approval_gudang_id)
                    .HasPrincipalKey(a => a.approval_id)
                    .IsRequired(false);
                entity
                    .HasOne(t => t.ApprovalSectionheadIdDto)
                    .WithMany()
                    .HasForeignKey(t => t.approval_sectionhead_id)
                    .HasPrincipalKey(a => a.approval_id)
                    .IsRequired(false);
            });
            modelBuilder.Entity<TransactionDetail>(entity =>
            {
                entity.ToTable("transaksi_detail");
                entity.Ignore(o => o.barang_id);
                entity.HasOne(rp => rp.itemDto)
                .WithMany()
                .HasForeignKey(rp => rp.barang_id);
                // entity.HasOne(rp => rp.Item)
                // .WithMany()
                // .HasForeignKey(rp => rp.barang_id)
                // .OnDelete(DeleteBehavior.NoAction)
                // .HasPrincipalKey(th => th.barang_id)
                // .IsRequired(false)
                // .HasConstraintName(null);
            });
            // Logger.Log("DeviceContext: Database & Tables SET.");
        }
    }
}