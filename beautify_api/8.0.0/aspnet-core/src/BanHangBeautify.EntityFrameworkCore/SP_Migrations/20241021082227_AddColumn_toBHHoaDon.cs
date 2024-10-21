using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddColumntoBHHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LaHoaDonDauKy",
                table: "BH_HoaDon",
                type: "bit",
                nullable: true);

            migrationBuilder.Sql(@"update BH_HoaDon set LaHoaDonDauKy='0' where LaHoaDonDauKy is null");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LaHoaDonDauKy",
                table: "BH_HoaDon");
        }
    }
}
