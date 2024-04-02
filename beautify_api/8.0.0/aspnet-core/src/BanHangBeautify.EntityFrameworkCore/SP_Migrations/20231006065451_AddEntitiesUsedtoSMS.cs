using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddEntitiesUsedtoSMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeThong_SMS_DM_KhachHang_IdKhachHang",
                table: "HeThong_SMS");

            migrationBuilder.DropForeignKey(
                name: "FK_HeThong_SMS_NS_NhanVien_IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.DropIndex(
                name: "IX_HeThong_SMS_IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.DropColumn(
                name: "IdNguoiGui",
                table: "HeThong_SMS");

            migrationBuilder.AddColumn<Guid>(
                name: "IdBrandname",
                table: "QuyHoaDon",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IdKhachHang",
                table: "HeThong_SMS",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "HeThong_SMS",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HT_SMSBrandname",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Brandname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SDTCuaHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrangThai = table.Column<byte>(type: "tinyint", nullable: true),
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
                    table.PrimaryKey("PK_HT_SMSBrandname", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SMS_Template",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdLoaiTin = table.Column<byte>(type: "tinyint", nullable: true),
                    TenMauTin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoiDungTinMau = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaMacDinh = table.Column<bool>(type: "bit", nullable: true),
                    TrangThai = table.Column<byte>(type: "tinyint", nullable: true),
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
                    table.PrimaryKey("PK_SMS_Template", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SMS_CaiDat_NhacNho",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdLoaiTin = table.Column<byte>(type: "tinyint", nullable: true),
                    NhacTruocKhoangThoiGian = table.Column<float>(type: "real", nullable: true),
                    LoaiThoiGian = table.Column<byte>(type: "tinyint", nullable: true),
                    IdMauTin = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NoiDungTin = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_SMS_CaiDat_NhacNho", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMS_CaiDat_NhacNho_SMS_Template_IdMauTin",
                        column: x => x.IdMauTin,
                        principalTable: "SMS_Template",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_IdBrandname",
                table: "QuyHoaDon",
                column: "IdBrandname");

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdHoaDon",
                table: "HeThong_SMS",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_SMS_CaiDat_NhacNho_IdMauTin",
                table: "SMS_CaiDat_NhacNho",
                column: "IdMauTin");

            migrationBuilder.AddForeignKey(
                name: "FK_HeThong_SMS_BH_HoaDon_IdHoaDon",
                table: "HeThong_SMS",
                column: "IdHoaDon",
                principalTable: "BH_HoaDon",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeThong_SMS_DM_KhachHang_IdKhachHang",
                table: "HeThong_SMS",
                column: "IdKhachHang",
                principalTable: "DM_KhachHang",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuyHoaDon_HT_SMSBrandname_IdBrandname",
                table: "QuyHoaDon",
                column: "IdBrandname",
                principalTable: "HT_SMSBrandname",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeThong_SMS_BH_HoaDon_IdHoaDon",
                table: "HeThong_SMS");

            migrationBuilder.DropForeignKey(
                name: "FK_HeThong_SMS_DM_KhachHang_IdKhachHang",
                table: "HeThong_SMS");

            migrationBuilder.DropForeignKey(
                name: "FK_QuyHoaDon_HT_SMSBrandname_IdBrandname",
                table: "QuyHoaDon");

            migrationBuilder.DropTable(
                name: "HT_SMSBrandname");

            migrationBuilder.DropTable(
                name: "SMS_CaiDat_NhacNho");

            migrationBuilder.DropTable(
                name: "SMS_Template");

            migrationBuilder.DropIndex(
                name: "IX_QuyHoaDon_IdBrandname",
                table: "QuyHoaDon");

            migrationBuilder.DropIndex(
                name: "IX_HeThong_SMS_IdHoaDon",
                table: "HeThong_SMS");

            migrationBuilder.DropColumn(
                name: "IdBrandname",
                table: "QuyHoaDon");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "HeThong_SMS");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdKhachHang",
                table: "HeThong_SMS",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdNguoiGui",
                table: "HeThong_SMS",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdNguoiGui",
                table: "HeThong_SMS",
                column: "IdNguoiGui");

            migrationBuilder.AddForeignKey(
                name: "FK_HeThong_SMS_DM_KhachHang_IdKhachHang",
                table: "HeThong_SMS",
                column: "IdKhachHang",
                principalTable: "DM_KhachHang",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeThong_SMS_NS_NhanVien_IdNguoiGui",
                table: "HeThong_SMS",
                column: "IdNguoiGui",
                principalTable: "NS_NhanVien",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
