using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTblKHCheckIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCheckIn",
                table: "KH_CheckIn",
                newName: "DateTimeCheckIn");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdChiNhanh",
                table: "KH_CheckIn",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "KH_CheckIn",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "KH_CheckIn");

            migrationBuilder.RenameColumn(
                name: "DateTimeCheckIn",
                table: "KH_CheckIn",
                newName: "DateCheckIn");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdChiNhanh",
                table: "KH_CheckIn",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
