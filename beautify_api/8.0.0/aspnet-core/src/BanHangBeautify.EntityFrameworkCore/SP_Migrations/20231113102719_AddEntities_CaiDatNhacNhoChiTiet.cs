using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddEntitiesCaiDatNhacNhoChiTiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "TrangThai",
                table: "SMS_CaiDat_NhacNho",
                type: "tinyint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaiDat_NhacNho_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdCaiDatNhacNho = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HinhThucGui = table.Column<byte>(type: "tinyint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "IX_CaiDat_NhacNho_ChiTiet_IdCaiDatNhacNho",
                table: "CaiDat_NhacNho_ChiTiet",
                column: "IdCaiDatNhacNho");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaiDat_NhacNho_ChiTiet");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "SMS_CaiDat_NhacNho");
        }
    }
}
