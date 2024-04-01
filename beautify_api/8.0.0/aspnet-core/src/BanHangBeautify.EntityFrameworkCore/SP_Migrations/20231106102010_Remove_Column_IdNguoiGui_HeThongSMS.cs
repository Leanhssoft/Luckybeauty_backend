using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class RemoveColumnIdNguoiGuiHeThongSMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeThong_SMS_NS_NhanVien_IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.DropIndex(
                name: "IX_HeThong_SMS_IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.DropColumn(
                name: "IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.AlterColumn<string>(
                name: "IdTinNhan",
                table: "HeThong_SMS",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SMS_LichSuNap_ChuyenTien_IdNguoiChuyenTien",
                table: "SMS_LichSuNap_ChuyenTien",
                column: "IdNguoiChuyenTien");

            migrationBuilder.CreateIndex(
                name: "IX_SMS_LichSuNap_ChuyenTien_IdNguoiNhanTien",
                table: "SMS_LichSuNap_ChuyenTien",
                column: "IdNguoiNhanTien");

            migrationBuilder.AddForeignKey(
                name: "FK_SMS_LichSuNap_ChuyenTien_AbpUsers_IdNguoiChuyenTien",
                table: "SMS_LichSuNap_ChuyenTien",
                column: "IdNguoiChuyenTien",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SMS_LichSuNap_ChuyenTien_AbpUsers_IdNguoiNhanTien",
                table: "SMS_LichSuNap_ChuyenTien",
                column: "IdNguoiNhanTien",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SMS_LichSuNap_ChuyenTien_AbpUsers_IdNguoiChuyenTien",
                table: "SMS_LichSuNap_ChuyenTien");

            migrationBuilder.DropForeignKey(
                name: "FK_SMS_LichSuNap_ChuyenTien_AbpUsers_IdNguoiNhanTien",
                table: "SMS_LichSuNap_ChuyenTien");

            migrationBuilder.DropIndex(
                name: "IX_SMS_LichSuNap_ChuyenTien_IdNguoiChuyenTien",
                table: "SMS_LichSuNap_ChuyenTien");

            migrationBuilder.DropIndex(
                name: "IX_SMS_LichSuNap_ChuyenTien_IdNguoiNhanTien",
                table: "SMS_LichSuNap_ChuyenTien");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdTinNhan",
                table: "HeThong_SMS",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdNguoiGui",
                table: "HeThong_SMS",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdNguoiGui",
                table: "HeThong_SMS",
                column: "IdNguoiGui");

            migrationBuilder.AddForeignKey(
                name: "FK_HeThong_SMS_NS_NhanVien_IdNguoiGui",
                table: "HeThong_SMS",
                column: "IdNguoiGui",
                principalTable: "NS_NhanVien",
                principalColumn: "Id");
        }
    }
}
