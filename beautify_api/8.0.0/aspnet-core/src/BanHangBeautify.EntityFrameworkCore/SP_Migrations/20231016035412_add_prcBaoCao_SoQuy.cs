using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcBaoCaoSoQuy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROC prc_baoCao_BaoCaoSoQuy_TienMat
	@TenantId INT,
	@Filter NVARCHAR(50),
	@TimeFrom DATETIME2,
	@TimeTo DATETIME2,
	@IdChiNhanh UNIQUEIDENTIFIER = null,
	@SkipCount INT = 0,
	@MaxResultCount INT = 10,
	@SortBy NVARCHAR(50) = '',
	@SortType NVARCHAR(4) = ''
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @DataTable TABLE(
		Id UNIQUEIDENTIFIER,
		MaHoaDon NVARCHAR(50),
		NgayLapHoaDon DATETIME2,
		TienThu DECIMAL(18,2),
		TienChi DECIMAL(18,2),
		TonLuyKe DECIMAL(18,2),
		NguoiNop NVARCHAR(100),
		GhiChu NVARCHAR(200)
	)
	INSERT INTO @DataTable
		SELECT 
		qhd.Id,
		qhd.MaHoaDon,
		qhd.NgayLapHoaDon,
		CASE WHEN qhd.IdLoaiChungTu = 11 THEN ISNULL(qhd.TongTienThu, 0) ELSE 0 END AS TienThu,
		CASE WHEN qhd.IdLoaiChungTu = 12 THEN ISNULL(qhd.TongTienThu, 0) ELSE 0 END AS TienChi,
		0 as TonLuyKe,
		CASE WHEN qhdct.IdKhachHang is null and qhdct.IdNhanVien is not null THEN nv.TenNhanVien
		WHEN qhdct.IdNhanVien is null and qhdct.IdKhachHang is not null THEN kh.TenKhachHang
		ELSE N'Khach vãng lai' END AS NguoiNop,
		qhd.NoiDungThu
		FROM QuyHoaDon qhd 
		JOIN QuyHoaDon_ChiTiet qhdct ON qhd.id = qhdct.IdQuyHoaDon AND qhdct.IsDeleted = 0
		LEFT JOIN DM_KhoanThuChi ktc ON qhdct.IdKhoanThuChi = ktc.Id
		LEFT JOIN DM_TaiKhoanNganHang tknh ON qhdct.IdTaiKhoanNganHang = tknh.id
		LEFT JOIN DM_KhachHang kh ON kh.Id = qhdct.IdKhachHang
		LEFT JOIN NS_NhanVien nv ON nv.Id = qhdct.IdNhanVien
		WHERE 
		@TenantId = qhd.TenantId
		AND qhd.IsDeleted = 0
		AND (@IdChiNhanh IS NULL OR (qhd.IdChiNhanh = @IdChiNhanh AND @IdChiNhanh IS NOT NULL)) 
		AND qhd.NgayLapHoaDon between @TimeFrom and @TimeTo
		AND qhdct.HinhThucThanhToan = 1
		AND (
			ISNULL(@Filter,'') = ''
			OR LOWER(qhd.MaHoaDon) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(18),qhd.NgayLapHoaDon)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(nv.TenNhanVien) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
			OR CONVERT(nvarchar(50),CASE WHEN qhd.IdLoaiChungTu = 11 THEN ISNULL(qhd.TongTienThu, 0) ELSE 0 END) = @Filter
			OR CONVERT(nvarchar(50),CASE WHEN qhd.IdLoaiChungTu = 12 THEN ISNULL(qhd.TongTienThu, 0) ELSE 0 END) = @Filter
		);
	SELECT * FROM @DataTable
	ORDER BY
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'maHoaDon' THEN MaHoaDon END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngayLapHoaDon' THEN NgayLapHoaDon END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tienThu' THEN TienThu END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tienChi' THEN TienChi END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tonLuyKe' THEN TonLuyKe END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'nguoiNop' THEN NguoiNop END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ghiChu' THEN GhiChu END ASC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'maHoaDon' THEN MaHoaDon END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngayLapHoaDon' THEN NgayLapHoaDon END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tienThu' THEN TienThu END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tienChi' THEN TienChi END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tonLuyKe' THEN TonLuyKe END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'nguoiNop' THEN NguoiNop END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ghiChu' THEN GhiChu END DESC,
		CASE WHEN ISNULL(@SortType,'') = '' AND ISNULL(@SortBy,'') = '' THEN NgayLapHoaDon END DESC
		OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;;
	SELECT COUNT(*) AS TotalCount FROM @DataTable
END;
            ");

            migrationBuilder.Sql(@"
CREATE PROC prc_baoCao_BaoCaoSoQuy_NganHang
	@TenantId INT,
	@Filter NVARCHAR(50),
	@TimeFrom DATETIME2,
	@TimeTo DATETIME2,
	@IdChiNhanh UNIQUEIDENTIFIER = null,
	@SkipCount INT = 0,
	@MaxResultCount INT = 10,
	@SortBy NVARCHAR(50) = '',
	@SortType NVARCHAR(4) = ''
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @DataTable TABLE(
		Id UNIQUEIDENTIFIER,
		MaHoaDon NVARCHAR(50),
		NgayLapHoaDon DATETIME2,
		TienThu DECIMAL(18,2),
		TienChi DECIMAL(18,2),
		TonLuyKe DECIMAL(18,2),
		NguoiNop NVARCHAR(100),
		GhiChu NVARCHAR(200)
	)
	INSERT INTO @DataTable
		SELECT 
		qhd.Id,
		qhd.MaHoaDon,
		qhd.NgayLapHoaDon,
		CASE WHEN qhd.IdLoaiChungTu = 11 THEN ISNULL(qhd.TongTienThu, 0) ELSE 0 END AS TienThu,
		CASE WHEN qhd.IdLoaiChungTu = 12 THEN ISNULL(qhd.TongTienThu, 0) ELSE 0 END AS TienChi,
		0 as TonLuyKe,
		CASE WHEN qhdct.IdKhachHang is null and qhdct.IdNhanVien is not null THEN nv.TenNhanVien
		WHEN qhdct.IdNhanVien is null and qhdct.IdKhachHang is not null THEN kh.TenKhachHang
		ELSE N'Khach vãng lai' END AS NguoiNop,
		qhd.NoiDungThu
		FROM QuyHoaDon qhd 
		JOIN QuyHoaDon_ChiTiet qhdct ON qhd.id = qhdct.IdQuyHoaDon AND qhdct.IsDeleted = 0
		LEFT JOIN DM_KhoanThuChi ktc ON qhdct.IdKhoanThuChi = ktc.Id
		LEFT JOIN DM_TaiKhoanNganHang tknh ON qhdct.IdTaiKhoanNganHang = tknh.id
		LEFT JOIN DM_KhachHang kh ON kh.Id = qhdct.IdKhachHang
		LEFT JOIN NS_NhanVien nv ON nv.Id = qhdct.IdNhanVien
		WHERE 
		@TenantId = qhd.TenantId
		AND qhd.IsDeleted = 0
		AND (@IdChiNhanh IS NULL OR (qhd.IdChiNhanh = @IdChiNhanh AND @IdChiNhanh IS NOT NULL)) 
		AND qhd.NgayLapHoaDon between @TimeFrom and @TimeTo
		AND qhdct.HinhThucThanhToan IN(2,3)
		AND (
			ISNULL(@Filter,'') = ''
			OR LOWER(qhd.MaHoaDon) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(18),qhd.NgayLapHoaDon)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(nv.TenNhanVien) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
			OR CONVERT(nvarchar(50),CASE WHEN qhd.IdLoaiChungTu = 11 THEN ISNULL(qhd.TongTienThu, 0) ELSE 0 END) = @Filter
			OR CONVERT(nvarchar(50),CASE WHEN qhd.IdLoaiChungTu = 12 THEN ISNULL(qhd.TongTienThu, 0) ELSE 0 END) = @Filter
		);
	SELECT * FROM @DataTable
	ORDER BY
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'maHoaDon' THEN MaHoaDon END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngayLapHoaDon' THEN NgayLapHoaDon END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tienThu' THEN TienThu END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tienChi' THEN TienChi END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tonLuyKe' THEN TonLuyKe END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'nguoiNop' THEN NguoiNop END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ghiChu' THEN GhiChu END ASC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'maHoaDon' THEN MaHoaDon END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngayLapHoaDon' THEN NgayLapHoaDon END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tienThu' THEN TienThu END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tienChi' THEN TienChi END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tonLuyKe' THEN TonLuyKe END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'nguoiNop' THEN NguoiNop END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ghiChu' THEN GhiChu END DESC,
		CASE WHEN ISNULL(@SortType,'') = '' AND ISNULL(@SortBy,'') = '' THEN NgayLapHoaDon END DESC
		OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;;
	SELECT COUNT(*) AS TotalCount FROM @DataTable
END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
