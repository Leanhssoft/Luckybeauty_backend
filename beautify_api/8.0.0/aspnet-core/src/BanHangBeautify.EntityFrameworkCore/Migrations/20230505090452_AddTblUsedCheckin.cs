using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class AddTblUsedCheckin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DM_KhoanThuChi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaKhoanThuChi = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenKhoanThuChi = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    LaKhoanThu = table.Column<bool>(type: "bit", nullable: false),
                    ChungTuApDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_DM_KhoanThuChi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_NganHang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaNganHang = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenNganHang = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ChiPhiThanhToan = table.Column<float>(type: "real", nullable: true),
                    TheoPhanTram = table.Column<bool>(type: "bit", nullable: true),
                    ThuPhiThanhToan = table.Column<bool>(type: "bit", nullable: true),
                    ChungTuApDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_DM_NganHang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_ViTriPhong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaViTriPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenViTriPhong = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
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
                    table.PrimaryKey("PK_DM_ViTriPhong", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KH_CheckIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdBooking = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCheckIn = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_KH_CheckIn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuyHoaDon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdLoaiChungTu = table.Column<int>(type: "int", nullable: false),
                    MaHoaDon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NgayLapHoaDon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TongTienThu = table.Column<float>(type: "real", nullable: true),
                    NoiDungThu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    HachToanKinhDoanh = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_QuyHoaDon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_DM_LoaiChungTu_IdLoaiChungTu",
                        column: x => x.IdLoaiChungTu,
                        principalTable: "DM_LoaiChungTu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_TaiKhoanNganHang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdNganHang = table.Column<Guid>(type: "uniqueidentifier", maxLength: 256, nullable: false),
                    SoTaiKhoan = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TenChuThe = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_TaiKhoanNganHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_TaiKhoanNganHang_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DM_TaiKhoanNganHang_DM_NganHang_IdNganHang",
                        column: x => x.IdNganHang,
                        principalTable: "DM_NganHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_Phong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenPhong = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IdViTri = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_Phong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_Phong_DM_ViTriPhong_IdViTri",
                        column: x => x.IdViTri,
                        principalTable: "DM_ViTriPhong",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BH_HoaDon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdLoaiChungTu = table.Column<int>(type: "int", nullable: false),
                    MaHoaDon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NgayLapHoaDon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayApDung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdPhong = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TongTienHangChuaChietKhau = table.Column<float>(type: "real", nullable: true),
                    PTChietKhauHang = table.Column<float>(type: "real", nullable: true),
                    TongChietKhauHangHoa = table.Column<float>(type: "real", nullable: true),
                    TongTienHang = table.Column<float>(type: "real", nullable: true),
                    PTThueHD = table.Column<float>(type: "real", nullable: true),
                    TongTienThue = table.Column<float>(type: "real", nullable: true),
                    TongTienHDSauVAT = table.Column<float>(type: "real", nullable: true),
                    PTGiamGiaHD = table.Column<float>(type: "real", nullable: true),
                    TongGiamGiaHD = table.Column<float>(type: "real", nullable: true),
                    ChiPhiTraHang = table.Column<float>(type: "real", nullable: true),
                    TongThanhToan = table.Column<float>(type: "real", nullable: true),
                    ChiPhiHD = table.Column<float>(type: "real", nullable: true),
                    ChiPhiGhiChu = table.Column<string>(name: "ChiPhi_GhiChu", type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DiemGiaoDich = table.Column<float>(type: "real", nullable: true),
                    GhiChuHD = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_BH_HoaDon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_BH_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_DM_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "DM_KhachHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_DM_LoaiChungTu_IdLoaiChungTu",
                        column: x => x.IdLoaiChungTu,
                        principalTable: "DM_LoaiChungTu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_DM_Phong_IdPhong",
                        column: x => x.IdPhong,
                        principalTable: "DM_Phong",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_NS_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanViens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BH_HoaDon_Anh",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    URLAnh = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_BH_HoaDon_Anh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_Anh_BH_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BH_HoaDon_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    STT = table.Column<int>(type: "int", nullable: false),
                    IdDonViQuyDoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiTietHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoLuong = table.Column<float>(type: "real", nullable: true),
                    DonGiaTruocCK = table.Column<float>(type: "real", nullable: true),
                    ThanhTienTruocCK = table.Column<float>(type: "real", nullable: true),
                    PTChietKhau = table.Column<float>(type: "real", nullable: true),
                    TienChietKhau = table.Column<float>(type: "real", nullable: true),
                    DonGiaSauCK = table.Column<float>(type: "real", nullable: true),
                    ThanhTienSauCK = table.Column<float>(type: "real", nullable: true),
                    PTThue = table.Column<float>(type: "real", nullable: true),
                    TienThue = table.Column<float>(type: "real", nullable: true),
                    DonGiaSauVAT = table.Column<float>(type: "real", nullable: true),
                    ThanhTienSauVAT = table.Column<float>(type: "real", nullable: true),
                    TonLuyKe = table.Column<float>(type: "real", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_BH_HoaDon_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_ChiTiet_BH_HoaDon_ChiTiet_IdChiTietHoaDon",
                        column: x => x.IdChiTietHoaDon,
                        principalTable: "BH_HoaDon_ChiTiet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_ChiTiet_BH_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_ChiTiet_DM_DonViQuiDoi_IdDonViQuyDoi",
                        column: x => x.IdDonViQuyDoi,
                        principalTable: "DM_DonViQuiDoi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuyHoaDon_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdQuyHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHoaDonLienQuan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdTaiKhoanNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdKhoanThuChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LaPTChiPhiNganHang = table.Column<float>(type: "real", nullable: true),
                    ChiPhiNganHang = table.Column<float>(type: "real", nullable: true),
                    ThuPhiTienGui = table.Column<float>(type: "real", nullable: true),
                    DiemThanhToan = table.Column<float>(type: "real", nullable: true),
                    HinhThucThanhToan = table.Column<byte>(type: "tinyint", nullable: false),
                    TienThu = table.Column<float>(type: "real", nullable: true),
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
                    table.PrimaryKey("PK_QuyHoaDon_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_BH_HoaDon_IdHoaDonLienQuan",
                        column: x => x.IdHoaDonLienQuan,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_DM_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "DM_KhachHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_DM_KhoanThuChi_IdKhoanThuChi",
                        column: x => x.IdKhoanThuChi,
                        principalTable: "DM_KhoanThuChi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_DM_TaiKhoanNganHang_IdTaiKhoanNganHang",
                        column: x => x.IdTaiKhoanNganHang,
                        principalTable: "DM_TaiKhoanNganHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_NS_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanViens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_QuyHoaDon_IdQuyHoaDon",
                        column: x => x.IdQuyHoaDon,
                        principalTable: "QuyHoaDon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BH_NhanVienThucHien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdHoaDonChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdQuyHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PTChietKhau = table.Column<float>(type: "real", nullable: true),
                    TienChietKhau = table.Column<float>(type: "real", nullable: true),
                    HeSo = table.Column<float>(type: "real", nullable: true),
                    ChiaDeuChietKhau = table.Column<bool>(type: "bit", nullable: true),
                    TinhHoaHongTruocCK = table.Column<bool>(type: "bit", nullable: true),
                    LoaiChietKhau = table.Column<byte>(type: "tinyint", nullable: true),
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
                    table.PrimaryKey("PK_BH_NhanVienThucHien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BH_NhanVienThucHien_BH_HoaDon_ChiTiet_IdHoaDonChiTiet",
                        column: x => x.IdHoaDonChiTiet,
                        principalTable: "BH_HoaDon_ChiTiet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_NhanVienThucHien_BH_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_NhanVienThucHien_NS_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BH_NhanVienThucHien_QuyHoaDon_IdQuyHoaDon",
                        column: x => x.IdQuyHoaDon,
                        principalTable: "QuyHoaDon",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdChiNhanh",
                table: "BH_HoaDon",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdHoaDon",
                table: "BH_HoaDon",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdKhachHang",
                table: "BH_HoaDon",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdLoaiChungTu",
                table: "BH_HoaDon",
                column: "IdLoaiChungTu");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdNhanVien",
                table: "BH_HoaDon",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdPhong",
                table: "BH_HoaDon",
                column: "IdPhong");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_Anh_IdHoaDon",
                table: "BH_HoaDon_Anh",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_ChiTiet_IdChiTietHoaDon",
                table: "BH_HoaDon_ChiTiet",
                column: "IdChiTietHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_ChiTiet_IdDonViQuyDoi",
                table: "BH_HoaDon_ChiTiet",
                column: "IdDonViQuyDoi");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_ChiTiet_IdHoaDon",
                table: "BH_HoaDon_ChiTiet",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_NhanVienThucHien_IdHoaDon",
                table: "BH_NhanVienThucHien",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_NhanVienThucHien_IdHoaDonChiTiet",
                table: "BH_NhanVienThucHien",
                column: "IdHoaDonChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_BH_NhanVienThucHien_IdNhanVien",
                table: "BH_NhanVienThucHien",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_BH_NhanVienThucHien_IdQuyHoaDon",
                table: "BH_NhanVienThucHien",
                column: "IdQuyHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_DM_Phong_IdViTri",
                table: "DM_Phong",
                column: "IdViTri");

            migrationBuilder.CreateIndex(
                name: "IX_DM_TaiKhoanNganHang_IdChiNhanh",
                table: "DM_TaiKhoanNganHang",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_DM_TaiKhoanNganHang_IdNganHang",
                table: "DM_TaiKhoanNganHang",
                column: "IdNganHang");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_IdChiNhanh",
                table: "QuyHoaDon",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_IdLoaiChungTu",
                table: "QuyHoaDon",
                column: "IdLoaiChungTu");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdHoaDonLienQuan",
                table: "QuyHoaDon_ChiTiet",
                column: "IdHoaDonLienQuan");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdKhachHang",
                table: "QuyHoaDon_ChiTiet",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdKhoanThuChi",
                table: "QuyHoaDon_ChiTiet",
                column: "IdKhoanThuChi");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdNhanVien",
                table: "QuyHoaDon_ChiTiet",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdQuyHoaDon",
                table: "QuyHoaDon_ChiTiet",
                column: "IdQuyHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdTaiKhoanNganHang",
                table: "QuyHoaDon_ChiTiet",
                column: "IdTaiKhoanNganHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BH_HoaDon_Anh");

            migrationBuilder.DropTable(
                name: "BH_NhanVienThucHien");

            migrationBuilder.DropTable(
                name: "KH_CheckIn");

            migrationBuilder.DropTable(
                name: "QuyHoaDon_ChiTiet");

            migrationBuilder.DropTable(
                name: "BH_HoaDon_ChiTiet");

            migrationBuilder.DropTable(
                name: "DM_KhoanThuChi");

            migrationBuilder.DropTable(
                name: "DM_TaiKhoanNganHang");

            migrationBuilder.DropTable(
                name: "QuyHoaDon");

            migrationBuilder.DropTable(
                name: "BH_HoaDon");

            migrationBuilder.DropTable(
                name: "DM_NganHang");

            migrationBuilder.DropTable(
                name: "DM_Phong");

            migrationBuilder.DropTable(
                name: "DM_ViTriPhong");
        }
    }
}
