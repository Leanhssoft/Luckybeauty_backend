using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcbaoCaoLichHen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"CREATE PROC prc_baoCao_ThongKeLichHen
	@TenantId int,
	@TimeFrom datetime2,
	@TimeTo datetime2,
	@Filter nvarchar(50),
	@SkipCount int = 0,
	@MaxResultCount int = 10,
	@SortBy nvarchar(50) = '',
	@SortType nvarchar(4) = '',
	@IdChiNhanh UNIQUEIDENTIFIER = null,
	@IdDichVu UNIQUEIDENTIFIER = null,
	@IdKhachHang UNIQUEIDENTIFIER = null,
	@TrangThai int = null
AS
	BEGIN
	SET NOCOUNT ON;
	DECLARE @DataTable TABLE(
		BookingDate datetime2,
		TenKhachHang nvarchar(150),
		SoDienThoai nvarchar(13),
		TenHangHoa nvarchar(150),
		TrangThai int,
		GhiChu nvarchar(200)
	);
	INSERT INTO @DataTable
		SELECT 
		bk.BookingDate,
		bk.TenKhachHang,
		bk.SoDienThoai,
		hh.TenHangHoa,
		bk.TrangThai,
		bk.GhiChu
		FROM Booking bk
		JOIN BookingService bs on bs.IdBooking = bk.Id
		JOIN DM_DonViQuiDoi dvqd on bs.IdDonViQuiDoi = dvqd.Id
		JOIN DM_HangHoa hh on dvqd.IdHangHoa = hh.Id
		WHERE bk.TenantId = @TenantId and bk.IsDeleted = 0
		AND bk.BookingDate between @TimeFrom and @TimeTo
		AND (@IdDichVu IS NULL OR (bs.IdDonViQuiDoi = @IdDichVu AND @IdDichVu IS NOT NULL))
		AND (@IdChiNhanh IS NULL OR (bk.IdChiNhanh = @IdChiNhanh AND @IdChiNhanh IS NOT NULL))
		AND (@IdKhachHang IS NULL OR (bk.IdKhachHang = @IdKhachHang AND @IdKhachHang IS NOT NULL))
		AND (@TrangThai IS NULL OR (bk.TrangThai = @TrangThai AND @TrangThai IS NOT NULL))
		AND (
			ISNULL(@Filter,'') = ''
			OR LOWER(hh.TenHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(bk.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(bk.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(bk.BookingDate) LIKE N'%' + LOWER(@Filter) + N'%'
		)

	SELECT * FROM @DataTable
	ORDER BY
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenHangHoa' THEN TenHangHoa END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'trangThai' THEN TrangThai END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ghiChu' THEN GhiChu END ASC,
		CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'bookingDate' THEN BookingDate END ASC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenHangHoa' THEN TenHangHoa END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'trangThai' THEN TrangThai END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ghiChu' THEN GhiChu END DESC,
		CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'bookingDate' THEN BookingDate END DESC,
		CASE WHEN ISNULL(@SortType,'') = '' AND ISNULL(@SortBy,'') = '' THEN BookingDate END DESC
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
