using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddEntitiesSMSNhatKyGuiTin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SMS_NhatKy_GuiTin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdHeThongSMS = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdBooking = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThoiGianTu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianDen = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_SMS_NhatKy_GuiTin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMS_NhatKy_GuiTin_BH_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SMS_NhatKy_GuiTin_Booking_IdBooking",
                        column: x => x.IdBooking,
                        principalTable: "Booking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SMS_NhatKy_GuiTin_HeThong_SMS_IdHeThongSMS",
                        column: x => x.IdHeThongSMS,
                        principalTable: "HeThong_SMS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SMS_NhatKy_GuiTin_IdBooking",
                table: "SMS_NhatKy_GuiTin",
                column: "IdBooking");

            migrationBuilder.CreateIndex(
                name: "IX_SMS_NhatKy_GuiTin_IdHeThongSMS",
                table: "SMS_NhatKy_GuiTin",
                column: "IdHeThongSMS");

            migrationBuilder.CreateIndex(
                name: "IX_SMS_NhatKy_GuiTin_IdHoaDon",
                table: "SMS_NhatKy_GuiTin",
                column: "IdHoaDon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMS_NhatKy_GuiTin");
        }
    }
}
