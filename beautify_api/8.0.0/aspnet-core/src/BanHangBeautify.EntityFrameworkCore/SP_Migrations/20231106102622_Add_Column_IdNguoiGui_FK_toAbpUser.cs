using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddColumnIdNguoiGuiFKtoAbpUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IdNguoiGui",
                table: "HeThong_SMS",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdNguoiGui",
                table: "HeThong_SMS",
                column: "IdNguoiGui");

            migrationBuilder.AddForeignKey(
                name: "FK_HeThong_SMS_AbpUsers_IdNguoiGui",
                table: "HeThong_SMS",
                column: "IdNguoiGui",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeThong_SMS_AbpUsers_IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.DropIndex(
                name: "IX_HeThong_SMS_IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.DropColumn(
                name: "IdNguoiGui",
                table: "HeThong_SMS");
        }
    }
}
