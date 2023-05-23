using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class removeforeignkeyloaikhachnhomkhachnguonkhach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdLoaiKhach",
                table: "DM_KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdNguonKhach",
                table: "DM_KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdNhomKhach",
                table: "DM_KhachHang");

            migrationBuilder.AlterColumn<int>(
                name: "IdLoaiKhach",
                table: "DM_KhachHang",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdLoaiKhach",
                table: "DM_KhachHang",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7653), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7677), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7678), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7910), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7914), null, null });

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

            migrationBuilder.AddForeignKey(
                name: "FK_DM_KhachHang_DM_LoaiKhach_IdLoaiKhach",
                table: "DM_KhachHang",
                column: "IdLoaiKhach",
                principalTable: "DM_LoaiKhach",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
        }
    }
}
