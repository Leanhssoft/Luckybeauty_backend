using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddColumnIdChiNhanhtoTblUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdChiNhanhMacDinh",
                table: "AbpUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
              name: "IX_AbpUsers_IdChiNhanhMacDinh",
              table: "AbpUsers",
              column: "IdChiNhanhMacDinh");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_DM_ChiNhanh_IdChiNhanhMacDinh",
                table: "AbpUsers",
                column: "IdChiNhanhMacDinh",
                principalTable: "DM_ChiNhanh",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_AbpUsers_DM_ChiNhanh_IdChiNhanhMacDinh",
               table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_IdChiNhanhMacDinh",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
              name: "IdChiNhanhMacDinh",
              table: "AbpUsers");
        }
    }
}
