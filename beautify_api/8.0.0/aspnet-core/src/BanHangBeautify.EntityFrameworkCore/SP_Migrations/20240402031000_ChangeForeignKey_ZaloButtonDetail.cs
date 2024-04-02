using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class ChangeForeignKeyZaloButtonDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zalo_ButtonDetail_Zalo_Element_IdElement",
                table: "Zalo_ButtonDetail");

            migrationBuilder.RenameColumn(
                name: "IdElement",
                table: "Zalo_ButtonDetail",
                newName: "IdTemplate");

            migrationBuilder.RenameIndex(
                name: "IX_Zalo_ButtonDetail_IdElement",
                table: "Zalo_ButtonDetail",
                newName: "IX_Zalo_ButtonDetail_IdTemplate");

            migrationBuilder.AddColumn<string>(
                name: "TenMauTin",
                table: "Zalo_Template",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);


            migrationBuilder.AddForeignKey(
                name: "FK_Zalo_ButtonDetail_Zalo_Template_IdTemplate",
                table: "Zalo_ButtonDetail",
                column: "IdTemplate",
                principalTable: "Zalo_Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zalo_ButtonDetail_Zalo_Template_IdTemplate",
                table: "Zalo_ButtonDetail");

            migrationBuilder.DropColumn(
                name: "TenMauTin",
                table: "Zalo_Template");

            migrationBuilder.RenameColumn(
                name: "IdTemplate",
                table: "Zalo_ButtonDetail",
                newName: "IdElement");

            migrationBuilder.RenameIndex(
                name: "IX_Zalo_ButtonDetail_IdTemplate",
                table: "Zalo_ButtonDetail",
                newName: "IX_Zalo_ButtonDetail_IdElement");

            migrationBuilder.AddForeignKey(
                name: "FK_Zalo_ButtonDetail_Zalo_Element_IdElement",
                table: "Zalo_ButtonDetail",
                column: "IdElement",
                principalTable: "Zalo_Element",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
