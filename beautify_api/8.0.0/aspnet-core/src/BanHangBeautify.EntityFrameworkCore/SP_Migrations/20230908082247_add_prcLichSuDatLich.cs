using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcLichSuDatLich : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_lichSuDatLich
	@TenantId INT,
	@IdKhachHang UNIQUEIDENTIFIER
AS 
BEGIN
	DECLARE @lichSuDatLich as TABLE(
		BookingDate datetime,
		TenDichVu nvarchar(200),
		ThoiGianThucHien float,
		ThoiGianBatDau datetime,
		Gia money,
		NhanVienThucHien nvarchar(70),
		TrangThai nvarchar(50)
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
            ELSE N'Đặt lịch'
        END AS TrangThai
	from booking b
	join BookingService bs on bs.IdBooking = b.id
	join BookingNhanVien bns on bns.IdBooking = b.id
	join NS_NhanVien nv on nv.id = bns.IdNhanVien
	join DM_DonViQuiDoi dvqd on dvqd.id = bs.IdDonViQuiDoi
	join DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	WHERE b.IsDeleted = 0 AND b.TenantId= @TenantId AND b.IdKhachHang = @IdKhachHang

SELECT * FROM @lichSuDatLich
SELECT COUNT(*) as TotalCount FROM @lichSuDatLich
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP PROCEDURE prc_lichSuDatLich");
        }
    }
}
