using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcBaoCaoBanHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC prc_baoCao_BanHangChiTiet
	@TenantId int,
	@TimeFrom datetime2,
	@TimeTo datetime2,
	@Filter nvarchar(50),
	@SkipCount int = 0,
	@MaxResultCount int = 10,
	@SortBy nvarchar(50),
	@SortType nvarchar(4),
	@IdChiNhanh UNIQUEIDENTIFIER = null,
	@IdDichVu UNIQUEIDENTIFIER = null
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @DataTable TABLE
    (   
        MaHoaDon NVARCHAR(20),
        NgayLapHoaDon DATETIME2,
		TenKhachHang NVARCHAR(70),
        SoDienThoai NVARCHAR(12),
        NhomHangHoa NVARCHAR(50),
        TenHangHoa NVARCHAR(100),
        GiaBan DECIMAL(18,2),
        SoLuong INT,
        ThanhTien DECIMAL(18,2),
		CreationTime DateTime2
    );
	INSERT INTO @DataTable
		SELECT 
		hd.MaHoaDon,
		hd.NgayLapHoaDon,
		kh.TenKhachHang,
		kh.SoDienThoai,
		ISNULL(nhh.TenNhomHang,N'Chưa phân nhóm') as NhomHangHoa,
		hh.TenHangHoa,
		dvqd.GiaBan,
		hdct.SoLuong,
		hdct.SoLuong * dvqd.GiaBan as ThanhTien,
		hd.CreationTime
		FROM BH_HoaDon hd
		JOIN BH_HoaDon_ChiTiet hdct on hdct.IdHoaDon = hd.Id and hdct.IsDeleted = 0
		JOIN DM_DonViQuiDoi dvqd on hdct.IdDonViQuyDoi= dvqd.Id
		JOIN DM_HangHoa hh on dvqd.IdHangHoa = hh.Id
		LEFT JOIN DM_NhomHangHoa nhh on hh.IdNhomHangHoa = nhh.Id
		LEFT JOIN DM_KhachHang kh on hd.IdKhachHang = kh.Id
		WHERE hd.TenantId = @TenantId and hd.IsDeleted = 0
		AND hd.NgayLapHoaDon between @TimeFrom and @TimeTo
		AND (@IdDichVu IS NULL OR (hdct.IdDonViQuyDoi = @IdDichVu AND @IdDichVu IS NOT NULL))
		AND (@IdChiNhanh IS NULL OR (hd.IdChiNhanh = @IdChiNhanh AND @IdChiNhanh IS NOT NULL))
		AND (
			ISNULL(@Filter,'') = ''
			OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(kh.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(hd.MaHoaDon) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(ISNULL(nhh.TenNhomHang,N'Chưa phân nhóm')) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(hh.TenHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
			OR CONVERT(nvarchar(50),hdct.SoLuong) = @Filter
			OR CONVERT(nvarchar(50),hdct.SoLuong * dvqd.GiaBan) = @Filter
			OR CONVERT(nvarchar(50),dvqd.GiaBan) = @Filter
		);
	SELECT * FROM @DataTable
	ORDER BY
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'maHoaDon' THEN MaHoaDon END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngayLapHoaDon' THEN NgayLapHoaDon END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenHangHoa' THEN TenHangHoa END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'nhomHangHoa' THEN NhomHangHoa END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'giaBan' THEN GiaBan END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soLuong' THEN SoLuong END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'thanhTien' THEN ThanhTien END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'creationTime' THEN CreationTime END ASC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'maHoaDon' THEN MaHoaDon END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngayLapHoaDon' THEN NgayLapHoaDon END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenHangHoa' THEN TenHangHoa END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'nhomHangHoa' THEN NhomHangHoa END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'giaBan' THEN GiaBan END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soLuong' THEN SoLuong END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'thanhTien' THEN ThanhTien END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'creationTime' THEN CreationTime END DESC,
		CASE WHEN ISNULL(@SortType,'') = '' AND ISNULL(@SortBy,'') = '' THEN CreationTime END DESC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
	SELECT COUNT(*) as TotalCount * FROM @DataTable;
END;");
            migrationBuilder.Sql(@"CREATE PROC prc_baoCao_BanHangTongHop
	@TenantId int,
	@TimeFrom datetime2,
	@TimeTo datetime2,
	@Filter nvarchar(50),
	@SkipCount int = 0,
	@MaxResultCount int = 10,
	@SortBy nvarchar(50),
	@SortType nvarchar(4),
	@IdChiNhanh UNIQUEIDENTIFIER = null,
	@IdDichVu UNIQUEIDENTIFIER = null
AS
	BEGIN
	SET NOCOUNT ON;
	DECLARE @DataTable TABLE(
		TenHangHoa nvarchar(100),
		MaHangHoa nvarchar(15),
		NhomHangHoa nvarchar(50),
		GiaBan decimal(18,2),
		SoLuong int,
		DoanhThu decimal(18,2)
	);
	INSERT INTO @DataTable
		SELECT 
		hh.TenHangHoa,
		dvqd.MaHangHoa,
		ISNULL(nhh.TenNhomHang,N'Chưa phân nhóm') as NhomHangHoa,
		dvqd.GiaBan,
		SUM(hdct.SoLuong) as SoLuong,
		SUM(hdct.SoLuong) * dvqd.GiaBan as DoanhThu
		FROM BH_HoaDon hd
		JOIN BH_HoaDon_ChiTiet hdct on hdct.IdHoaDon = hd.Id and hdct.IsDeleted = 0
		JOIN DM_DonViQuiDoi dvqd on hdct.IdDonViQuyDoi= dvqd.Id
		JOIN DM_HangHoa hh on dvqd.IdHangHoa = hh.Id
		LEFT JOIN DM_NhomHangHoa nhh on hh.IdNhomHangHoa = nhh.Id
		WHERE hd.TenantId = @TenantId and hd.IsDeleted = 0
		AND hd.NgayLapHoaDon between @TimeFrom and @TimeTo
		AND (@IdDichVu IS NULL OR (hdct.IdDonViQuyDoi = @IdDichVu AND @IdDichVu IS NOT NULL))
		AND (@IdChiNhanh IS NULL OR (hd.IdChiNhanh = @IdChiNhanh AND @IdChiNhanh IS NOT NULL))
		AND (
			ISNULL(@Filter,'') = ''
			OR LOWER(hh.TenHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(dvqd.MaHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(ISNULL(nhh.TenNhomHang,N'Chưa phân nhóm')) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(hh.TenHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
			OR CONVERT(nvarchar(50),hdct.SoLuong) = @Filter
			OR CONVERT(nvarchar(50),hdct.SoLuong * dvqd.GiaBan) = @Filter
			OR CONVERT(nvarchar(50),dvqd.GiaBan) = @Filter
		)
		GROUP BY
		hh.TenHangHoa,
		dvqd.MaHangHoa,
		ISNULL(nhh.TenNhomHang,N'Chưa phân nhóm'),
		dvqd.GiaBan

	SELECT * FROM @DataTable
	ORDER BY
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenHangHoa' THEN TenHangHoa END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'maHangHoa' THEN MaHangHoa END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'nhomHangHoa' THEN NhomHangHoa END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'giaBan' THEN GiaBan END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soLuong' THEN SoLuong END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'thanhTien' THEN DoanhThu END ASC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenHangHoa' THEN TenHangHoa END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'maHangHoa' THEN MaHangHoa END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'nhomHangHoa' THEN NhomHangHoa END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'giaBan' THEN GiaBan END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soLuong' THEN SoLuong END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'thanhTien' THEN DoanhThu END DESC,
		CASE WHEN ISNULL(@SortType,'') = '' AND ISNULL(@SortBy,'') = '' THEN TenHangHoa END DESC
		OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

	SELECT COUNT(*) AS TotalCount FROM @DataTable;
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
