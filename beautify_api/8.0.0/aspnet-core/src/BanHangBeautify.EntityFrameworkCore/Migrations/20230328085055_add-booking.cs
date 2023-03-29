using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class addbooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tennhom",
                table: "DM_NhomKhachHangs",
                newName: "TenNhom");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiBooking = table.Column<byte>(type: "tinyint", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserXuLy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayXuLy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_DM_ChiNhanhs_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_NgayNghiLes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TenNgayLe = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongSoNgay = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DM_NgayNghiLes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NS_LichLamViecs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    LapLai = table.Column<bool>(type: "bit", nullable: false),
                    KieuLapLai = table.Column<int>(type: "int", nullable: false),
                    GiaTriLap = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_NS_LichLamViecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_LichLamViecs_DM_ChiNhanhs_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_LichLamViecs_NS_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_NhanVien_TimeOffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongNgayNghi = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_NS_NhanVien_TimeOffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingNhanViens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdBooking = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_BookingNhanViens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingNhanViens_Bookings_IdBooking",
                        column: x => x.IdBooking,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingNhanViens_NS_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdDonViQuiDoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdBooking = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_BookingServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingServices_Bookings_IdBooking",
                        column: x => x.IdBooking,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingServices_DM_DonViQuiDois_IdDonViQuiDoi",
                        column: x => x.IdDonViQuiDoi,
                        principalTable: "DM_DonViQuiDois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_LichLamViec_Cas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdLichLam = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCaLamViec = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdLichLamViec = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GiaTri = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_NS_LichLamViec_Cas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_LichLamViec_Cas_NS_CaLamViecs_IdLichLamViec",
                        column: x => x.IdLichLamViec,
                        principalTable: "NS_CaLamViecs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NS_LichLamViec_Cas_NS_LichLamViecs_IdCaLamViec",
                        column: x => x.IdCaLamViec,
                        principalTable: "NS_LichLamViecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingNhanViens_IdBooking",
                table: "BookingNhanViens",
                column: "IdBooking");

            migrationBuilder.CreateIndex(
                name: "IX_BookingNhanViens_IdNhanVien",
                table: "BookingNhanViens",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_IdChiNhanh",
                table: "Bookings",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServices_IdBooking",
                table: "BookingServices",
                column: "IdBooking");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServices_IdDonViQuiDoi",
                table: "BookingServices",
                column: "IdDonViQuiDoi");

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViec_Cas_IdCaLamViec",
                table: "NS_LichLamViec_Cas",
                column: "IdCaLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViec_Cas_IdLichLamViec",
                table: "NS_LichLamViec_Cas",
                column: "IdLichLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViecs_IdChiNhanh",
                table: "NS_LichLamViecs",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViecs_IdNhanVien",
                table: "NS_LichLamViecs",
                column: "IdNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingNhanViens");

            migrationBuilder.DropTable(
                name: "BookingServices");

            migrationBuilder.DropTable(
                name: "DM_NgayNghiLes");

            migrationBuilder.DropTable(
                name: "NS_LichLamViec_Cas");

            migrationBuilder.DropTable(
                name: "NS_NhanVien_TimeOffs");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "NS_LichLamViecs");

            migrationBuilder.RenameColumn(
                name: "TenNhom",
                table: "DM_NhomKhachHangs",
                newName: "Tennhom");
        }
    }
}
