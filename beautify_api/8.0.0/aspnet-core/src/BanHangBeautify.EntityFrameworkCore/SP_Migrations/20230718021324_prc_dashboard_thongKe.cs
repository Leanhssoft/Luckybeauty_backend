using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_dashboard_thongKe")]
    public class prc_dashboard_thongKe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE pcr_ThongKeThongTin 
	@TenantId int,
	@UserId int,
	@ThoiGianTu DateTime = null,
	@ThoiGianDen DateTime = null,
	@IdChiNhanh uniqueidentifier = null
AS
BEGIN
	 SET NOCOUNT ON;
	  DECLARE @totalBirthdayCustomer INT = 0,
			  @totalCustomer INT = 0,
			  @totalRevenue FLOAT = 0,
			  @totalApointment INT = 0;
	-- lấy tổng khách hàng có sinh nhật từ @ThoiGianTu đến @ThoiGianDen
	SELECT @totalBirthdayCustomer = Count(*) 
	FROM DM_KhachHang WHERE IsDeleted = 0 AND TenantId = @TenantId
	AND CAST(NgaySinh AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE);
	-- lấy tổng khách hàng thêm mới từ @ThoiGianTu đến @ThoiGianDen
	SELECT @totalCustomer = Count(*) 
	FROM DM_KhachHang WHERE IsDeleted = 0 AND TenantId = @TenantId
	AND CAST(CreationTime AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE);

	-- lấy tổng lịch hẹn trong khoảng thời gian @ThoiGianTu đến @ThoiGianDen
	SELECT @totalApointment = COUNT(*) FROM Booking where TenantId = @TenantId and IsDeleted = 0
	AND CAST(CreationTime AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE);
	-- lấy tổng doanh thu từ @ThoiGianTu đến @ThoiGianDen
	SELECT @totalRevenue = SUM(TongThanhToan) FROM BH_HoaDon where TenantId = 1 and IsDeleted = 0 and IdChiNhanh = @IdChiNhanh
	AND CAST(CreationTime AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE);
	--Tạo bảng lấy top 5 dịch vụ được sử dụng nhiều nhất
	 DECLARE @HotService TABLE
    (
        TenDichVu nvarchar(100),
		TongDoanhThu decimal
    );
	INSERT INTO @HotService 
	SELECT TOP(5) hh.TenHangHoa,SUM(dvqd.GiaBan) AS TongTien
	FROM Booking b 
	JOIN BookingService bs on bs.IdBooking = b.Id and b.IsDeleted = 0 and b.TenantId = @TenantId
	JOIN DM_DonViQuiDoi dvqd on dvqd.Id = bs.IdDonViQuiDoi
	RIGHT JOIN DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	WHERE hh.TenantId = @TenantId and b.IdChiNhanh = @IdChiNhanh
	GROUP BY hh.TenHangHoa
	ORDER BY COUNT(hh.id) DESC;
	--Tạo bảng lấy top 3 lịch hẹn gần nhất
	DECLARE @Appointment TABLE
    (
		Avatar nvarchar(max),
        TenKhachHang nvarchar(50),
		StartTime datetime2,
		EndTime datetime2,
		DichVu nvarchar(50),
		TongTien float,
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

	SELECT 
	ISNULL(@totalBirthdayCustomer,0) as TotalBirthdayCustomer,
	ISNULL(@totalCustomer,0) as TotalCustomer,
	ISNULL(@totalApointment,0) as TotalAppointment,
	ISNULL(@totalRevenue,0) as TotalRevenue

	SELECT * FROM @HotService;
	Select * FROM @Appointment;
END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE pcr_ThongKeThongTin");
        }
    }
}
