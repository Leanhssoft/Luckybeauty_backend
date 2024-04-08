using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class RemoveTblCaiDatNhacNhoChiTiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeThong_SMS_BH_HoaDon_IdHoaDon",
                table: "HeThong_SMS");

            migrationBuilder.DropForeignKey(
                name: "FK_SMS_CaiDat_NhacNho_SMS_Template_IdMauTin",
                table: "SMS_CaiDat_NhacNho");

            migrationBuilder.DropTable(
                name: "CaiDat_NhacNho_ChiTiet");

            migrationBuilder.DropIndex(
                name: "IX_SMS_CaiDat_NhacNho_IdMauTin",
                table: "SMS_CaiDat_NhacNho");

            migrationBuilder.DropIndex(
                name: "IX_HeThong_SMS_IdHoaDon",
                table: "HeThong_SMS");

            migrationBuilder.DropColumn(
                name: "NoiDungTin",
                table: "SMS_CaiDat_NhacNho");

            migrationBuilder.DropColumn(
                name: "IdHoaDon",
                table: "HeThong_SMS");

            migrationBuilder.AlterColumn<string>(
                name: "IdMauTin",
                table: "SMS_CaiDat_NhacNho",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "HinhThucGui",
                table: "SMS_CaiDat_NhacNho",
                type: "tinyint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhThucGui",
                table: "SMS_CaiDat_NhacNho");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdMauTin",
                table: "SMS_CaiDat_NhacNho",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDungTin",
                table: "SMS_CaiDat_NhacNho",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdHoaDon",
                table: "HeThong_SMS",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaiDat_NhacNho_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCaiDatNhacNho = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HinhThucGui = table.Column<byte>(type: "tinyint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaiDat_NhacNho_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaiDat_NhacNho_ChiTiet_SMS_CaiDat_NhacNho_IdCaiDatNhacNho",
                        column: x => x.IdCaiDatNhacNho,
                        principalTable: "SMS_CaiDat_NhacNho",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SMS_CaiDat_NhacNho_IdMauTin",
                table: "SMS_CaiDat_NhacNho",
                column: "IdMauTin");

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdHoaDon",
                table: "HeThong_SMS",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_CaiDat_NhacNho_ChiTiet_IdCaiDatNhacNho",
                table: "CaiDat_NhacNho_ChiTiet",
                column: "IdCaiDatNhacNho");

            migrationBuilder.AddForeignKey(
                name: "FK_HeThong_SMS_BH_HoaDon_IdHoaDon",
                table: "HeThong_SMS",
                column: "IdHoaDon",
                principalTable: "BH_HoaDon",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SMS_CaiDat_NhacNho_SMS_Template_IdMauTin",
                table: "SMS_CaiDat_NhacNho",
                column: "IdMauTin",
                principalTable: "SMS_Template",
                principalColumn: "Id");
        }
    }
}
