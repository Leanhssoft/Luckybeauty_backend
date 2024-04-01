using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddEntitiesSMSLichSuNapChuyenTien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SMS_LichSuNap_ChuyenTien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdPhieuNapTien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThoiGianNapChuyenTien = table.Column<DateTime>(name: "ThoiGianNap_ChuyenTien", type: "datetime2", nullable: false),
                    IdNguoiChuyenTien = table.Column<long>(type: "bigint", nullable: true),
                    IdNguoiNhanTien = table.Column<long>(type: "bigint", nullable: true),
                    SoTienChuyenNhan = table.Column<double>(name: "SoTienChuyen_Nhan", type: "float", nullable: true),
                    NoiDungChuyenNhan = table.Column<string>(name: "NoiDungChuyen_Nhan", type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_SMS_LichSuNap_ChuyenTien", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMS_LichSuNap_ChuyenTien");
        }
    }
}
