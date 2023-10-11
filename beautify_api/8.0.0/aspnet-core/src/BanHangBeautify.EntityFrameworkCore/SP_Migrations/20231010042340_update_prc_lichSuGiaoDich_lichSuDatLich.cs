using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class updateprclichSuGiaoDichlichSuDatLich : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region prc_LichSuDatLich
            migrationBuilder.Sql(@"ALTER PROCEDURE prc_lichSuDatLich
	@TenantId INT,
	@IdKhachHang UNIQUEIDENTIFIER,
	@SortBy nvarchar(50),
	@SortType nvarchar(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS 
BEGIN
	DECLARE @lichSuDatLich as TABLE(
		BookingDate datetime,
		TenDichVu nvarchar(200),
		ThoiGianThucHien float,
		ThoiGianBatDau datetime,
		Gia money,
		NhanVienThucHien nvarchar(70),
		TrangThai nvarchar(50),
		CreationTime Datetime2
	)

	INSERT INTO @lichSuDatLich
	select
	b.BookingDate, 
	hh.TenHangHoa,
	hh.SoPhutThucHien,
	b.StartTime,
	dvqd.GiaBan,
	nv.TenNhanVien,
	CASE
            WHEN b.TrangThai = 0 THEN N'Xóa'
			WHEN B.TrangThai = 1 THEN N'Đặt lịch'
			wHEN b.TrangThai = 2 THEN N'Đã gọi (nhắn tin) cho khách'
            ELSE N''
        END AS TrangThai,
	b.CreationTime
	from booking b
	join BookingService bs on bs.IdBooking = b.id
	join BookingNhanVien bns on bns.IdBooking = b.id
	join NS_NhanVien nv on nv.id = bns.IdNhanVien
	join DM_DonViQuiDoi dvqd on dvqd.id = bs.IdDonViQuiDoi
	join DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	WHERE b.IsDeleted = 0 AND b.TenantId= @TenantId AND b.IdKhachHang = @IdKhachHang

SELECT * FROM @lichSuDatLich
ORDER BY 
		CASE WHEN @SortBy = 'bookingDate' AND @SortType = 'desc' THEN BookingDate END DESC,
		CASE WHEN @SortBy = 'tenDichVu' AND @SortType = 'desc' THEN TenDichVu END DESC,
		CASE WHEN @SortBy = 'thoiGianThucHien' AND @SortType = 'desc' THEN ThoiGianThucHien END DESC,
		CASE WHEN @SortBy = 'thoiGianBatDau' AND @SortType = 'desc' THEN ThoiGianBatDau END DESC,
		CASE WHEN @SortBy = 'gia' AND @SortType = 'desc' THEN Gia END DESC,
		CASE WHEN @SortBy = 'nhanVienThucHien' AND @SortType = 'desc' THEN NhanVienThucHien END DESC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'desc' THEN TrangThai END DESC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'desc' THEN CreationTime END DESC,
		CASE WHEN @SortBy = 'bookingDate' AND @SortType = 'asc' THEN BookingDate END ASC,
		CASE WHEN @SortBy = 'tenDichVu' AND @SortType = 'asc' THEN TenDichVu END ASC,
		CASE WHEN @SortBy = 'thoiGianThucHien' AND @SortType = 'asc' THEN ThoiGianThucHien END ASC,
		CASE WHEN @SortBy = 'thoiGianBatDau' AND @SortType = 'asc' THEN ThoiGianBatDau END ASC,
		CASE WHEN @SortBy = 'gia' AND @SortType = 'asc' THEN Gia END ASC,
		CASE WHEN @SortBy = 'nhanVienThucHien' AND @SortType = 'asc' THEN NhanVienThucHien END ASC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'asc' THEN TrangThai END ASC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'asc' THEN CreationTime END ASC,
		CASE WHEN ISNULL(@SortBy,'') = '' THEN CreationTime END DESC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
SELECT COUNT(*) as TotalCount FROM @lichSuDatLich
END;");
            #endregion
            #region prc_LichSuMuaHang
            migrationBuilder.Sql(@"ALTER procedure prc_lichSuGiaoDich
	@TenantId INT,
	@IdKhachHang UNIQUEIDENTIFIER,
	@SortBy nvarchar(50),
	@SortType nvarchar(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS
BEGIN
	declare @lichSuMuaHang AS TABLE(
		MaHoaDon nvarchar(20),
		NgayLapHoaDon DateTime2,
		TongTienHang money,
		TongGiamGia money,
		TongPhaiTra money,
		KhachDaTra money,
		ConNo money,
		TrangThai nvarchar(20),
		CreationTime dateTime2
	)
	INSERT INTO @lichSuMuaHang
		SELECT MaHoaDon,
		NgayLapHoaDon,
		TongTienHang,
		TongGiamGiaHD,
		TongTienHDSauVAT,
		TongThanhToan,
		(TongTienHDSauVAT-TongThanhToan) as ConNo,
		CASE WHEN TrangThai = 0 THEN N'Xóa'
			WHEN TrangThai =1 THEN N'Tạm lưu'
			WHEN TrangThai =2 THEN N'Đang xử lý'
			WHEN TrangThai =3 THEN N'Thanh toán'
			ELSE N'Tạm lưu'
		END AS TrangThai,
		CreationTime
		from BH_HoaDon
		WHERE TenantId = @TenantId AND IdKhachHang = @IdKhachHang AND IsDeleted =0
	SELECT * FROM @lichSuMuaHang 
	ORDER BY 
		CASE WHEN @SortBy = 'maHoaDon' AND @SortType = 'desc' THEN MaHoaDon END DESC,
		CASE WHEN @SortBy = 'ngayLapHoaDon' AND @SortType = 'desc' THEN NgayLapHoaDon END DESC,
		CASE WHEN @SortBy = 'tongTienHang' AND @SortType = 'desc' THEN TongTienHang END DESC,
		CASE WHEN @SortBy = 'tongGiamGia' AND @SortType = 'desc' THEN TongGiamGia END DESC,
		CASE WHEN @SortBy = 'tongPhaiTra' AND @SortType = 'desc' THEN TongPhaiTra END DESC,
		CASE WHEN @SortBy = 'khachDaTra' AND @SortType = 'desc' THEN KhachDaTra END DESC,
		CASE WHEN @SortBy = 'conNo' AND @SortType = 'desc' THEN ConNo END DESC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'desc' THEN TrangThai END DESC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'desc' THEN CreationTime END DESC,
		CASE WHEN @SortBy = 'maHoaDon' AND @SortType = 'asc' THEN MaHoaDon END ASC,
		CASE WHEN @SortBy = 'ngayLapHoaDon' AND @SortType = 'asc' THEN NgayLapHoaDon END ASC,
		CASE WHEN @SortBy = 'tongTienHang' AND @SortType = 'asc' THEN TongTienHang END ASC,
		CASE WHEN @SortBy = 'tongGiamGia' AND @SortType = 'asc' THEN TongGiamGia END ASC,
		CASE WHEN @SortBy = 'tongPhaiTra' AND @SortType = 'asc' THEN TongPhaiTra END ASC,
		CASE WHEN @SortBy = 'khachDaTra' AND @SortType = 'asc' THEN KhachDaTra END ASC,
		CASE WHEN @SortBy = 'conNo' AND @SortType = 'asc' THEN ConNo END ASC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'asc' THEN TrangThai END ASC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'asc' THEN CreationTime END ASC,
		CASE WHEN ISNULL(@SortBy,'') = '' THEN CreationTime END DESC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;;
	SELECT COUNT(*) AS TotalCount FROM @lichSuMuaHang;
END;
");
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
