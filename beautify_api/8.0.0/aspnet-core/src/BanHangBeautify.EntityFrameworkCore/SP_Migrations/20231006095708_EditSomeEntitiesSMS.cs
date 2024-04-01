using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class EditSomeEntitiesSMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoaiTin",
                table: "HeThong_SMS",
                newName: "IdLoaiTin");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKichHoat",
                table: "HT_SMSBrandname",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GiaTienMoiTinNhan",
                table: "HeThong_SMS",
                type: "float",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeThong_SMS_NS_NhanVien_IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.DropIndex(
                name: "IX_HeThong_SMS_IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.DropColumn(
                name: "NgayKichHoat",
                table: "HT_SMSBrandname");

            migrationBuilder.DropColumn(
                name: "GiaTienMoiTinNhan",
                table: "HeThong_SMS");

            migrationBuilder.DropColumn(
                name: "IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.RenameColumn(
                name: "IdLoaiTin",
                table: "HeThong_SMS",
                newName: "LoaiTin");
        }
    }
}
