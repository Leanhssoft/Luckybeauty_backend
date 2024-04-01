using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class EditSomeTable20240221 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update DM_KhachHang set IdLoaiKhach=1 where IdLoaiKhach= 0");
            migrationBuilder.DropForeignKey(
                name: "FK_Zalo_KhachHangThanhVien_DM_KhachHang_IdKhachHang",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropIndex(
                name: "IX_Zalo_KhachHangThanhVien_IdKhachHang",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
                name: "IdKhachHang",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
                name: "NguoiSua",
                table: "DM_KhachHang");

            migrationBuilder.DropColumn(
                name: "NguoiTao",
                table: "DM_KhachHang");

            migrationBuilder.RenameColumn(
                name: "NguoiXoa",
                table: "DM_KhachHang",
                newName: "IdKhachHangZOA");

            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "Zalo_KhachHangThanhVien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenQuanHuyen",
                table: "Zalo_KhachHangThanhVien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenTinhThanh",
                table: "Zalo_KhachHangThanhVien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DM_TinhThanh",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    MaTinhThanh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenTinhThanh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM_TinhThanh", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_QuanHuyen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    MaQuanHuyen = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenQuanHuyen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdTinhThanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM_QuanHuyen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_QuanHuyen_DM_TinhThanh_IdTinhThanh",
                        column: x => x.IdTinhThanh,
                        principalTable: "DM_TinhThanh",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdKhachHangZOA",
                table: "DM_KhachHang",
                column: "IdKhachHangZOA",
                unique: true,
                filter: "[IdKhachHangZOA] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdLoaiKhach",
                table: "DM_KhachHang",
                column: "IdLoaiKhach");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdNguonKhach",
                table: "DM_KhachHang",
                column: "IdNguonKhach");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdNhomKhach",
                table: "DM_KhachHang",
                column: "IdNhomKhach");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdQuanHuyen",
                table: "DM_KhachHang",
                column: "IdQuanHuyen");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdTinhThanh",
                table: "DM_KhachHang",
                column: "IdTinhThanh");

            migrationBuilder.CreateIndex(
                name: "IX_DM_QuanHuyen_IdTinhThanh",
                table: "DM_QuanHuyen",
                column: "IdTinhThanh");

            migrationBuilder.AddForeignKey(
                name: "FK_DM_KhachHang_DM_LoaiKhach_IdLoaiKhach",
                table: "DM_KhachHang",
                column: "IdLoaiKhach",
                principalTable: "DM_LoaiKhach",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DM_KhachHang_DM_NguonKhach_IdNguonKhach",
                table: "DM_KhachHang",
                column: "IdNguonKhach",
                principalTable: "DM_NguonKhach",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DM_KhachHang_DM_NhomKhachHang_IdNhomKhach",
                table: "DM_KhachHang",
                column: "IdNhomKhach",
                principalTable: "DM_NhomKhachHang",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DM_KhachHang_DM_QuanHuyen_IdQuanHuyen",
                table: "DM_KhachHang",
                column: "IdQuanHuyen",
                principalTable: "DM_QuanHuyen",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DM_KhachHang_DM_TinhThanh_IdTinhThanh",
                table: "DM_KhachHang",
                column: "IdTinhThanh",
                principalTable: "DM_TinhThanh",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DM_KhachHang_Zalo_KhachHangThanhVien_IdKhachHangZOA",
                table: "DM_KhachHang",
                column: "IdKhachHangZOA",
                principalTable: "Zalo_KhachHangThanhVien",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DM_KhachHang_DM_LoaiKhach_IdLoaiKhach",
                table: "DM_KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DM_KhachHang_DM_NguonKhach_IdNguonKhach",
                table: "DM_KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DM_KhachHang_DM_NhomKhachHang_IdNhomKhach",
                table: "DM_KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DM_KhachHang_DM_QuanHuyen_IdQuanHuyen",
                table: "DM_KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DM_KhachHang_DM_TinhThanh_IdTinhThanh",
                table: "DM_KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DM_KhachHang_Zalo_KhachHangThanhVien_IdKhachHangZOA",
                table: "DM_KhachHang");

            migrationBuilder.DropTable(
                name: "DM_QuanHuyen");

            migrationBuilder.DropTable(
                name: "DM_TinhThanh");

            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdKhachHangZOA",
                table: "DM_KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdLoaiKhach",
                table: "DM_KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdNguonKhach",
                table: "DM_KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdNhomKhach",
                table: "DM_KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdQuanHuyen",
                table: "DM_KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdTinhThanh",
                table: "DM_KhachHang");

            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
                name: "TenQuanHuyen",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
                name: "TenTinhThanh",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.RenameColumn(
                name: "IdKhachHangZOA",
                table: "DM_KhachHang",
                newName: "NguoiXoa");

            migrationBuilder.AddColumn<Guid>(
                name: "IdKhachHang",
                table: "Zalo_KhachHangThanhVien",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiSua",
                table: "DM_KhachHang",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiTao",
                table: "DM_KhachHang",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2080), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2085), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2088), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2090), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2092), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2094), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2101), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2103), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2105), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2107), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2109), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2111), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2113), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2115), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(1543), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(1568), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(1570), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2019), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2044), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Zalo_KhachHangThanhVien_IdKhachHang",
                table: "Zalo_KhachHangThanhVien",
                column: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_Zalo_KhachHangThanhVien_DM_KhachHang_IdKhachHang",
                table: "Zalo_KhachHangThanhVien",
                column: "IdKhachHang",
                principalTable: "DM_KhachHang",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
