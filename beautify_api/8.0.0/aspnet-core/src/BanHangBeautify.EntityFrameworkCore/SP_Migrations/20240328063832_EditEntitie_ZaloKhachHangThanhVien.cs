using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class EditEntitieZaloKhachHangThanhVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdKhachHangZOA",
                table: "DM_KhachHang");

            migrationBuilder.DropColumn(
                name: "TenDangKy",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
              name: "SoDienThoaiDK",
              table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
              name: "DiaChi",
              table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
              name: "TenTinhThanh",
              table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
                name: "TenQuanHuyen",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.AlterColumn<string>(
                name: "ZOAUserId",
                table: "Zalo_KhachHangThanhVien",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdByApp",
                table: "Zalo_KhachHangThanhVien",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserIsFollower",
                table: "Zalo_KhachHangThanhVien",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisplayName",
                table: "Zalo_KhachHangThanhVien",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
             name: "Avatar",
             table: "Zalo_KhachHangThanhVien",
             type: "nvarchar(max)",
             nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdKhachHangZOA",
                table: "DM_KhachHang",
                column: "IdKhachHangZOA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DM_KhachHang_IdKhachHangZOA",
                table: "DM_KhachHang");

            migrationBuilder.DropColumn(
                name: "UserIdByApp",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
                name: "UserIsFollower",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Zalo_KhachHangThanhVien");

            migrationBuilder.DropColumn(
            name: "Avatar",
            table: "Zalo_KhachHangThanhVien");

            migrationBuilder.AlterColumn<string>(
                name: "ZOAUserId",
                table: "Zalo_KhachHangThanhVien",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenDangKy",
                table: "Zalo_KhachHangThanhVien",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
             name: "SoDienThoaiDK",
             table: "Zalo_KhachHangThanhVien",
             type: "nvarchar(100)",
             maxLength: 100,
             nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdKhachHangZOA",
                table: "DM_KhachHang",
                column: "IdKhachHangZOA",
                unique: true,
                filter: "[IdKhachHangZOA] IS NOT NULL");
        }
    }
}
