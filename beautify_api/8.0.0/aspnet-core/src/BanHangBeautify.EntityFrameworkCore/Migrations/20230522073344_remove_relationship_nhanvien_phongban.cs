using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class removerelationshipnhanvienphongban : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NS_NhanVien_DM_PhongBan_IdPhongBan",
                table: "NS_NhanVien");

            migrationBuilder.DropForeignKey(
                name: "FK_NS_QuaTrinh_CongTac_DM_PhongBan_IdPhongBan",
                table: "NS_QuaTrinh_CongTac");

            migrationBuilder.DropIndex(
                name: "IX_NS_QuaTrinh_CongTac_IdPhongBan",
                table: "NS_QuaTrinh_CongTac");

            migrationBuilder.DropIndex(
                name: "IX_NS_NhanVien_IdPhongBan",
                table: "NS_NhanVien");

            migrationBuilder.DropColumn(
                name: "IdPhongBan",
                table: "NS_NhanVien");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TuNgay",
                table: "NS_QuaTrinh_CongTac",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IdPhongBan",
                table: "NS_QuaTrinh_CongTac",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "IdChiNhanh",
                table: "NS_QuaTrinh_CongTac",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 14, 33, 41, 894, DateTimeKind.Local).AddTicks(5253), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 14, 33, 41, 894, DateTimeKind.Local).AddTicks(5281), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 14, 33, 41, 894, DateTimeKind.Local).AddTicks(5284), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 14, 33, 41, 894, DateTimeKind.Local).AddTicks(5551), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 14, 33, 41, 894, DateTimeKind.Local).AddTicks(5557), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdChiNhanh",
                table: "NS_QuaTrinh_CongTac");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TuNgay",
                table: "NS_QuaTrinh_CongTac",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdPhongBan",
                table: "NS_QuaTrinh_CongTac",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdPhongBan",
                table: "NS_NhanVien",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 9, 1, 23, 273, DateTimeKind.Local).AddTicks(5047), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 9, 1, 23, 273, DateTimeKind.Local).AddTicks(5066), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 9, 1, 23, 273, DateTimeKind.Local).AddTicks(5068), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 9, 1, 23, 273, DateTimeKind.Local).AddTicks(5360), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 22, 9, 1, 23, 273, DateTimeKind.Local).AddTicks(5365), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_NS_QuaTrinh_CongTac_IdPhongBan",
                table: "NS_QuaTrinh_CongTac",
                column: "IdPhongBan");

            migrationBuilder.CreateIndex(
                name: "IX_NS_NhanVien_IdPhongBan",
                table: "NS_NhanVien",
                column: "IdPhongBan");

            migrationBuilder.AddForeignKey(
                name: "FK_NS_NhanVien_DM_PhongBan_IdPhongBan",
                table: "NS_NhanVien",
                column: "IdPhongBan",
                principalTable: "DM_PhongBan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NS_QuaTrinh_CongTac_DM_PhongBan_IdPhongBan",
                table: "NS_QuaTrinh_CongTac",
                column: "IdPhongBan",
                principalTable: "DM_PhongBan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
