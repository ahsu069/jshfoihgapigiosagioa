using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "approval_role_map",
                columns: table => new
                {
                    approval_role_map_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    legacy_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval_role_map", x => x.approval_role_map_id);
                });

            migrationBuilder.CreateTable(
                name: "BagianUser",
                columns: table => new
                {
                    bagian_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nama = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    fungsi_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagianUser", x => x.bagian_id);
                });

            migrationBuilder.CreateTable(
                name: "FungsiUser",
                columns: table => new
                {
                    fungsi_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FungsiUser", x => x.fungsi_id);
                });

            migrationBuilder.CreateTable(
                name: "kategori_barang",
                columns: table => new
                {
                    kategoribar_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    namakategoribar = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kategori_barang", x => x.kategoribar_id);
                });

            migrationBuilder.CreateTable(
                name: "kategori_pekerja",
                columns: table => new
                {
                    kategori_pekerja_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    nama_kategori = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kategori_pekerja", x => x.kategori_pekerja_id);
                });

            migrationBuilder.CreateTable(
                name: "kategori_transaksi",
                columns: table => new
                {
                    kategori_transact_id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    nama_kategori_transact = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kategori_transaksi", x => x.kategori_transact_id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    permission_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.permission_id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "satuanBarang",
                columns: table => new
                {
                    satuanbar_id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    nama_satuanbar = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_satuanBarang", x => x.satuanbar_id);
                });

            migrationBuilder.CreateTable(
                name: "usersCache",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    nama_pekerja = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    fungsi_pekerja = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    bagian_pekerja = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersCache", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "pekerjaTemp",
                columns: table => new
                {
                    pekerja_temp_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nama_pekerja = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    fungsi_pekerja = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    id_finger = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    perusahaan_pekerja = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    link_file_pendukung = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    bagian_id = table.Column<int>(type: "int", nullable: true),
                    synced_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pekerjaTemp", x => x.pekerja_temp_id);
                    table.ForeignKey(
                        name: "FK_pekerjaTemp_BagianUser_bagian_id",
                        column: x => x.bagian_id,
                        principalTable: "BagianUser",
                        principalColumn: "bagian_id");
                });

            migrationBuilder.CreateTable(
                name: "SIGAPUser",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nama = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    bagian_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    refresh_token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIGAPUser", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_SIGAPUser_BagianUser_bagian_id",
                        column: x => x.bagian_id,
                        principalTable: "BagianUser",
                        principalColumn: "bagian_id");
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                columns: table => new
                {
                    role_permission_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    permission_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permission", x => x.role_permission_id);
                    table.ForeignKey(
                        name: "FK_role_permission_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permission_permission_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permission",
                        principalColumn: "permission_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "barang",
                columns: table => new
                {
                    barang_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nama_barang = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    msl_barang = table.Column<int>(type: "int", nullable: true),
                    jumlah_barang = table.Column<int>(type: "int", nullable: false),
                    satuanbar_id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    kategoribar_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    link_gambar_bar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    status_bar = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_barang", x => x.barang_id);
                    table.ForeignKey(
                        name: "FK_barang_kategori_barang_kategoribar_id",
                        column: x => x.kategoribar_id,
                        principalTable: "kategori_barang",
                        principalColumn: "kategoribar_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_barang_satuanBarang_satuanbar_id",
                        column: x => x.satuanbar_id,
                        principalTable: "satuanBarang",
                        principalColumn: "satuanbar_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "approval_status",
                columns: table => new
                {
                    approval_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(120)", nullable: false),
                    role_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    approval_role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_approved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval_status", x => x.approval_id);
                    table.ForeignKey(
                        name: "FK_approval_status_usersCache_user_id",
                        column: x => x.user_id,
                        principalTable: "usersCache",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    user_role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    effective_from = table.Column<DateTime>(type: "datetime2", nullable: false),
                    effective_to = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_primary = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => x.user_role_id);
                    table.ForeignKey(
                        name: "FK_user_role_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_SIGAPUser_user_id",
                        column: x => x.user_id,
                        principalTable: "SIGAPUser",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaksi_history",
                columns: table => new
                {
                    transact_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    kategori_transact_id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    kategori_pekerja = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    no_miv_safety = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    no_miv_custom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    users_cache_id = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    pekerja_temp_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    approval_manajemen_pekerja_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    approval_gudang_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    approval_sectionhead_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaksi_history", x => x.transact_id);
                    table.ForeignKey(
                        name: "FK_transaksi_history_approval_status_approval_gudang_id",
                        column: x => x.approval_gudang_id,
                        principalTable: "approval_status",
                        principalColumn: "approval_id");
                    table.ForeignKey(
                        name: "FK_transaksi_history_approval_status_approval_manajemen_pekerja_id",
                        column: x => x.approval_manajemen_pekerja_id,
                        principalTable: "approval_status",
                        principalColumn: "approval_id");
                    table.ForeignKey(
                        name: "FK_transaksi_history_approval_status_approval_sectionhead_id",
                        column: x => x.approval_sectionhead_id,
                        principalTable: "approval_status",
                        principalColumn: "approval_id");
                    table.ForeignKey(
                        name: "FK_transaksi_history_kategori_pekerja_kategori_pekerja",
                        column: x => x.kategori_pekerja,
                        principalTable: "kategori_pekerja",
                        principalColumn: "kategori_pekerja_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaksi_history_kategori_transaksi_kategori_transact_id",
                        column: x => x.kategori_transact_id,
                        principalTable: "kategori_transaksi",
                        principalColumn: "kategori_transact_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaksi_history_pekerjaTemp_pekerja_temp_id",
                        column: x => x.pekerja_temp_id,
                        principalTable: "pekerjaTemp",
                        principalColumn: "pekerja_temp_id");
                    table.ForeignKey(
                        name: "FK_transaksi_history_usersCache_users_cache_id",
                        column: x => x.users_cache_id,
                        principalTable: "usersCache",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaksi_detail",
                columns: table => new
                {
                    transact_detail_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    transact_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    barang_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    jumlah_bar = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaksi_detail", x => x.transact_detail_id);
                    table.ForeignKey(
                        name: "FK_transaksi_detail_barang_barang_id",
                        column: x => x.barang_id,
                        principalTable: "barang",
                        principalColumn: "barang_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaksi_detail_transaksi_history_transact_id",
                        column: x => x.transact_id,
                        principalTable: "transaksi_history",
                        principalColumn: "transact_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_approval_status_user_id",
                table: "approval_status",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_barang_kategoribar_id",
                table: "barang",
                column: "kategoribar_id");

            migrationBuilder.CreateIndex(
                name: "IX_barang_satuanbar_id",
                table: "barang",
                column: "satuanbar_id");

            migrationBuilder.CreateIndex(
                name: "IX_pekerjaTemp_bagian_id",
                table: "pekerjaTemp",
                column: "bagian_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_permission_id",
                table: "role_permission",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_role_id",
                table: "role_permission",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_SIGAPUser_bagian_id",
                table: "SIGAPUser",
                column: "bagian_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaksi_detail_barang_id",
                table: "transaksi_detail",
                column: "barang_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaksi_detail_transact_id",
                table: "transaksi_detail",
                column: "transact_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaksi_history_approval_gudang_id",
                table: "transaksi_history",
                column: "approval_gudang_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaksi_history_approval_manajemen_pekerja_id",
                table: "transaksi_history",
                column: "approval_manajemen_pekerja_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaksi_history_approval_sectionhead_id",
                table: "transaksi_history",
                column: "approval_sectionhead_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaksi_history_kategori_pekerja",
                table: "transaksi_history",
                column: "kategori_pekerja");

            migrationBuilder.CreateIndex(
                name: "IX_transaksi_history_kategori_transact_id",
                table: "transaksi_history",
                column: "kategori_transact_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaksi_history_pekerja_temp_id",
                table: "transaksi_history",
                column: "pekerja_temp_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaksi_history_users_cache_id",
                table: "transaksi_history",
                column: "users_cache_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_role_id",
                table: "user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_user_id",
                table: "user_role",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "approval_role_map");

            migrationBuilder.DropTable(
                name: "FungsiUser");

            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "transaksi_detail");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "barang");

            migrationBuilder.DropTable(
                name: "transaksi_history");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "SIGAPUser");

            migrationBuilder.DropTable(
                name: "kategori_barang");

            migrationBuilder.DropTable(
                name: "satuanBarang");

            migrationBuilder.DropTable(
                name: "approval_status");

            migrationBuilder.DropTable(
                name: "kategori_pekerja");

            migrationBuilder.DropTable(
                name: "kategori_transaksi");

            migrationBuilder.DropTable(
                name: "pekerjaTemp");

            migrationBuilder.DropTable(
                name: "usersCache");

            migrationBuilder.DropTable(
                name: "BagianUser");
        }
    }
}
