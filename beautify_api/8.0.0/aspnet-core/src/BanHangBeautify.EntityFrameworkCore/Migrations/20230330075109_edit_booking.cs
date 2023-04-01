using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class editbooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_DM_ChiNhanh_IdChiNhanh",
                table: "Booking");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdKhachHang",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdChiNhanh",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_IdKhachHang",
                table: "Booking",
                column: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_DM_ChiNhanh_IdChiNhanh",
                table: "Booking",
                column: "IdChiNhanh",
                principalTable: "DM_ChiNhanh",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_DM_KhachHang_IdKhachHang",
                table: "Booking",
                column: "IdKhachHang",
                principalTable: "DM_KhachHang",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_DM_ChiNhanh_IdChiNhanh",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_DM_KhachHang_IdKhachHang",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_IdKhachHang",
                table: "Booking");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdKhachHang",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IdChiNhanh",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_DM_ChiNhanh_IdChiNhanh",
                table: "Booking",
                column: "IdChiNhanh",
                principalTable: "DM_ChiNhanh",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
