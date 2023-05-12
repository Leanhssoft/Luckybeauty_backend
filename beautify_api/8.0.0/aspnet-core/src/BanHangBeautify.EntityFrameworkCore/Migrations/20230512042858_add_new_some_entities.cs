using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class addnewsomeentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booking_Color",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaMau = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
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
                    table.PrimaryKey("PK_Booking_Color", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_KhuyenMai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiKhuyenMai = table.Column<byte>(type: "tinyint", nullable: false),
                    HinhThucKM = table.Column<byte>(type: "tinyint", nullable: false),
                    ThoiGianApDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianKeThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TatCaKhachHang = table.Column<bool>(type: "bit", nullable: false),
                    TatCaChiNhanh = table.Column<bool>(type: "bit", nullable: false),
                    TatCaNhanVien = table.Column<bool>(type: "bit", nullable: false),
                    NgayApDung = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ThangApDung = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ThuApDung = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    GioApDung = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
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
                    table.PrimaryKey("PK_DM_KhuyenMai", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_MauIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiChungTu = table.Column<int>(type: "int", nullable: false),
                    TenMauIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaMacDinh = table.Column<bool>(type: "bit", nullable: false),
                    NoiDungMauIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_DM_MauIn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_MauIn_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DM_MauIn_DM_LoaiChungTu_LoaiChungTu",
                        column: x => x.LoaiChungTu,
                        principalTable: "DM_LoaiChungTu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_NhomKhach_DieuKien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    STT = table.Column<byte>(type: "tinyint", nullable: false),
                    IdNhomKhach = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiDieuKien = table.Column<byte>(type: "tinyint", nullable: false),
                    LoaiSoSanh = table.Column<byte>(type: "tinyint", nullable: false),
                    GiaTriSo = table.Column<float>(type: "real", nullable: false),
                    GiaTriBool = table.Column<bool>(type: "bit", nullable: false),
                    GiaTriThoiGian = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GiaTriKhuVuc = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_NhomKhach_DieuKien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_NhomKhach_DieuKien_DM_NhomKhachHang_IdNhomKhach",
                        column: x => x.IdNhomKhach,
                        principalTable: "DM_NhomKhachHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeThong_SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNguoiGui = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdTinNhan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoTinGui = table.Column<int>(type: "int", nullable: false),
                    NoiDungTin = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ThoiGianGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiTin = table.Column<byte>(type: "tinyint", nullable: false),
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
                    table.PrimaryKey("PK_HeThong_SMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeThong_SMS_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeThong_SMS_DM_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "DM_KhachHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeThong_SMS_NS_NhanViens_IdNguoiGui",
                        column: x => x.IdNguoiGui,
                        principalTable: "NS_NhanViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HT_CauHinh_ChungTu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdLoaiChungTu = table.Column<int>(type: "int", nullable: false),
                    MaLoaiChungTu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuDungMaChiNhanh = table.Column<bool>(type: "bit", nullable: false),
                    KiTuNganCach1 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    KiTuNganCach2 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    NgayThangNam = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    KiTuNganCach3 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    DoDaiSTT = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_HT_CauHinh_ChungTu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HT_CauHinh_ChungTu_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HT_CauHinh_ChungTu_DM_LoaiChungTu_IdLoaiChungTu",
                        column: x => x.IdLoaiChungTu,
                        principalTable: "DM_LoaiChungTu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HT_CauHinhPhanMem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TichDiem = table.Column<bool>(type: "bit", nullable: false),
                    KhuyenMai = table.Column<bool>(type: "bit", nullable: false),
                    MauInMacDinh = table.Column<bool>(type: "bit", nullable: false),
                    SuDungMaChungTu = table.Column<bool>(type: "bit", nullable: false),
                    QLKhachHangTheoChiNhanh = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_HT_CauHinhPhanMem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HT_NhatKyThaoTac",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChucNang = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LoaiNhatKy = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoiDungChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_HT_NhatKyThaoTac", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HT_NhatKyThaoTac_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_ChietKhauDichVu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDonViQuiDoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiChietKhau = table.Column<byte>(type: "tinyint", nullable: false),
                    GiaTri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LaPhanTram = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_NS_ChietKhauDichVu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauDichVu_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauDichVu_DM_DonViQuiDoi_IdDonViQuiDoi",
                        column: x => x.IdDonViQuiDoi,
                        principalTable: "DM_DonViQuiDoi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauDichVu_NS_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_ChietKhauHoaDon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DMChiNhanhId = table.Column<Guid>(name: "DM_ChiNhanhId", type: "uniqueidentifier", nullable: true),
                    LoaiChietKhau = table.Column<byte>(type: "tinyint", nullable: false),
                    GiaTriChietKhau = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChungTuApDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_NS_ChietKhauHoaDon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauHoaDon_DM_ChiNhanh_DM_ChiNhanhId",
                        column: x => x.DMChiNhanhId,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DM_KhuyenMai_ApDung",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhomKhach = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_KhuyenMai_ApDung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_KhuyenMai_ApDung_DM_KhuyenMai_IdKhuyenMai",
                        column: x => x.IdKhuyenMai,
                        principalTable: "DM_KhuyenMai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_KhuyenMai_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    STT = table.Column<byte>(type: "tinyint", nullable: false),
                    TongTienHang = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiamGiaTheoPhanTram = table.Column<bool>(type: "bit", nullable: false),
                    IdNhomHangMua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdDonViQuiDoiMua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhomHangTang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdDonViQuiDoiTang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoLuongMua = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuongTang = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaKhuyenMai = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_DM_KhuyenMai_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_KhuyenMai_ChiTiet_DM_KhuyenMai_IdKhuyenMai",
                        column: x => x.IdKhuyenMai,
                        principalTable: "DM_KhuyenMai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HT_CauHinh_TichDiem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdCauHinh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HTCauHinhPhanMemId = table.Column<Guid>(name: "HT_CauHinhPhanMemId", type: "uniqueidentifier", nullable: true),
                    TyLeDoiDiem = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChoPhepThanhToanBangDiem = table.Column<bool>(type: "bit", nullable: false),
                    DiemThanhToan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienThanhToan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KhongTichDiemHDGiamGia = table.Column<bool>(type: "bit", nullable: false),
                    TichDiemHoaDonGiamGia = table.Column<bool>(type: "bit", nullable: false),
                    KhongTichDiemSPGiamGia = table.Column<bool>(type: "bit", nullable: false),
                    TatCaKhachHang = table.Column<bool>(type: "bit", nullable: false),
                    SoLanMua = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_HT_CauHinh_TichDiem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HT_CauHinh_TichDiem_HT_CauHinhPhanMem_HT_CauHinhPhanMemId",
                        column: x => x.HTCauHinhPhanMemId,
                        principalTable: "HT_CauHinhPhanMem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NS_ChietKhauHoaDon_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChietKhauHD = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_NS_ChietKhauHoaDon_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauHoaDon_ChiTiet_NS_ChietKhauHoaDon_IdChietKhauHD",
                        column: x => x.IdChietKhauHD,
                        principalTable: "NS_ChietKhauHoaDon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauHoaDon_ChiTiet_NS_NhanViens_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HT_CauHinh_TichDiemChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdTichDiem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNhomKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_HT_CauHinh_TichDiemChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HT_CauHinh_TichDiemChiTiet_HT_CauHinh_TichDiem_IdTichDiem",
                        column: x => x.IdTichDiem,
                        principalTable: "HT_CauHinh_TichDiem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhuyenMai_ApDung_IdKhuyenMai",
                table: "DM_KhuyenMai_ApDung",
                column: "IdKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhuyenMai_ChiTiet_IdKhuyenMai",
                table: "DM_KhuyenMai_ChiTiet",
                column: "IdKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_DM_MauIn_IdChiNhanh",
                table: "DM_MauIn",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_DM_MauIn_LoaiChungTu",
                table: "DM_MauIn",
                column: "LoaiChungTu");

            migrationBuilder.CreateIndex(
                name: "IX_DM_NhomKhach_DieuKien_IdNhomKhach",
                table: "DM_NhomKhach_DieuKien",
                column: "IdNhomKhach");

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdChiNhanh",
                table: "HeThong_SMS",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdKhachHang",
                table: "HeThong_SMS",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdNguoiGui",
                table: "HeThong_SMS",
                column: "IdNguoiGui");

            migrationBuilder.CreateIndex(
                name: "IX_HT_CauHinh_ChungTu_IdChiNhanh",
                table: "HT_CauHinh_ChungTu",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_HT_CauHinh_ChungTu_IdLoaiChungTu",
                table: "HT_CauHinh_ChungTu",
                column: "IdLoaiChungTu");

            migrationBuilder.CreateIndex(
                name: "IX_HT_CauHinh_TichDiem_HT_CauHinhPhanMemId",
                table: "HT_CauHinh_TichDiem",
                column: "HT_CauHinhPhanMemId");

            migrationBuilder.CreateIndex(
                name: "IX_HT_CauHinh_TichDiemChiTiet_IdTichDiem",
                table: "HT_CauHinh_TichDiemChiTiet",
                column: "IdTichDiem");

            migrationBuilder.CreateIndex(
                name: "IX_HT_NhatKyThaoTac_IdChiNhanh",
                table: "HT_NhatKyThaoTac",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauDichVu_IdChiNhanh",
                table: "NS_ChietKhauDichVu",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauDichVu_IdDonViQuiDoi",
                table: "NS_ChietKhauDichVu",
                column: "IdDonViQuiDoi");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauDichVu_IdNhanVien",
                table: "NS_ChietKhauDichVu",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauHoaDon_DM_ChiNhanhId",
                table: "NS_ChietKhauHoaDon",
                column: "DM_ChiNhanhId");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauHoaDon_ChiTiet_IdChietKhauHD",
                table: "NS_ChietKhauHoaDon_ChiTiet",
                column: "IdChietKhauHD");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauHoaDon_ChiTiet_IdNhanVien",
                table: "NS_ChietKhauHoaDon_ChiTiet",
                column: "IdNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking_Color");

            migrationBuilder.DropTable(
                name: "DM_KhuyenMai_ApDung");

            migrationBuilder.DropTable(
                name: "DM_KhuyenMai_ChiTiet");

            migrationBuilder.DropTable(
                name: "DM_MauIn");

            migrationBuilder.DropTable(
                name: "DM_NhomKhach_DieuKien");

            migrationBuilder.DropTable(
                name: "HeThong_SMS");

            migrationBuilder.DropTable(
                name: "HT_CauHinh_ChungTu");

            migrationBuilder.DropTable(
                name: "HT_CauHinh_TichDiemChiTiet");

            migrationBuilder.DropTable(
                name: "HT_NhatKyThaoTac");

            migrationBuilder.DropTable(
                name: "NS_ChietKhauDichVu");

            migrationBuilder.DropTable(
                name: "NS_ChietKhauHoaDon_ChiTiet");

            migrationBuilder.DropTable(
                name: "DM_KhuyenMai");

            migrationBuilder.DropTable(
                name: "HT_CauHinh_TichDiem");

            migrationBuilder.DropTable(
                name: "NS_ChietKhauHoaDon");

            migrationBuilder.DropTable(
                name: "HT_CauHinhPhanMem");
        }
    }
}
