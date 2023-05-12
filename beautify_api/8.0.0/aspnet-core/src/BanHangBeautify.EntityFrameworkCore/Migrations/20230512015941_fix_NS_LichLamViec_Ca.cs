using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class fixNSLichLamViecCa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_CaLamViec_IdLichLamViec",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.DropForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_LichLamViec_IdCaLamViec",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.DropColumn(
                name: "IdLichLam",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.AddForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_CaLamViec_IdCaLamViec",
                table: "NS_LichLamViec_Ca",
                column: "IdCaLamViec",
                principalTable: "NS_CaLamViec",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_LichLamViec_IdLichLamViec",
                table: "NS_LichLamViec_Ca",
                column: "IdLichLamViec",
                principalTable: "NS_LichLamViec",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_CaLamViec_IdCaLamViec",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.DropForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_LichLamViec_IdLichLamViec",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.AddColumn<Guid>(
                name: "IdLichLam",
                table: "NS_LichLamViec_Ca",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_CaLamViec_IdLichLamViec",
                table: "NS_LichLamViec_Ca",
                column: "IdLichLamViec",
                principalTable: "NS_CaLamViec",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_LichLamViec_IdCaLamViec",
                table: "NS_LichLamViec_Ca",
                column: "IdCaLamViec",
                principalTable: "NS_LichLamViec",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
