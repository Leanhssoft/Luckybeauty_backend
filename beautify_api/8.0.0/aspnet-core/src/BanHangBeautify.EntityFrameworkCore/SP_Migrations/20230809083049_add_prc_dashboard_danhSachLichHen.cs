using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcdashboarddanhSachLichHen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC prc_dashboard_danhSachLichHen
	@TenantId int,
	@UserId int,
	@ThoiGianTu DateTime = null,
	@ThoiGianDen DateTime = null,
	@IdChiNhanh uniqueidentifier = null
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Appointment TABLE
    (
		Avatar nvarchar(max),
        TenKhachHang nvarchar(100),
		StartTime datetime2,
		EndTime datetime2,
		DichVu nvarchar(200),
		TongTien decimal(18,2),
		TrangThai nvarchar(20)
    );
	INSERT INTO @Appointment 
	SELECT TOP(3) 
	kh.Avatar,b.TenKhachHang,b.StartTime,b.EndTime,hh.TenHangHoa,dvqd.GiaBan,
	CASE 
		WHEN b.TrangThai = 1 THEN N'Đặt lịch' 
		WHEN b.TrangThai = 2 THEN N'Đã xác nhận' 
		WHEN b.TrangThai = 3 THEN N'Đang Chờ' 
		ELSE ''
	END as TrangThai 
	FROM Booking b 
	JOIN BookingService bs on bs.IdBooking = b.Id and b.IsDeleted = 0 and b.TenantId = @TenantId
	JOIN DM_DonViQuiDoi dvqd on dvqd.Id = bs.IdDonViQuiDoi
	RIGHT JOIN DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	JOIN DM_KhachHang kh on kh.Id = b.IdKhachHang
	WHERE b.IdChiNhanh = @IdChiNhanh AND b.IsDeleted = 0 AND b.TenantId = @TenantId
	ORDER BY b.CreationTime DESC;

	Select * FROM @Appointment;
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP PROCEDURE prc_dashboard_danhSachLichHen");
        }
    }
}
