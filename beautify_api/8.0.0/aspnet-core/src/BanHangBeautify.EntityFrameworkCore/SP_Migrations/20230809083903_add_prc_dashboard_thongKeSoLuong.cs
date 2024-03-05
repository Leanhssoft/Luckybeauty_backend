using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcdashboardthongKeSoLuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC prc_dashboard_thongKeSoLuong
	@TenantId int,
	@UserId int,
	@ThoiGianTu DateTime = null,
	@ThoiGianDen DateTime = null,
	@IdChiNhanh uniqueidentifier = null
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @isAdmin bit = 0,
			@khachHangSinhNhat INT = 0,
			@tongSoKhachHang INT = 0,
			@tongDoanhThu FLOAT = 0,
			@tongLichHen INT = 0;
	-- lấy tổng khách hàng có sinh nhật từ @ThoiGianTu đến @ThoiGianDen
	SELECT @isAdmin = IsAdmin FROM AbpUsers where id = @UserId;

	SELECT @khachHangSinhNhat = Count(*) 
	FROM DM_KhachHang kh
	--JOIN (SELECT id,NhanSuId,IsAdmin FROM AbpUsers Where IsActive = 1) as us on us.id = kh.CreatorUserId
	--JOIN (SELECT id FROM NS_NhanVien) as nv on nv.Id = us.NhanSuId
	--JOIN 
	--	(
	--		SELECT top(1) IdChiNhanh,Id,IdNhanVien 
	--		from NS_QuaTrinh_CongTac 
	--		order by CreationTime desc
	--	) as qtct on qtct.IdNhanVien = nv.Id
	WHERE kh.IsDeleted = 0 AND kh.TenantId = @TenantId 
	--AND (
 --       (us.IsAdmin = 0 AND qtct.IdChiNhanh = @IdChiNhanh) OR us.IsAdmin <> 0
 --   )
	AND datepart(dd,kh.NgaySinh) between datepart(dd,@ThoiGianTu) AND datepart(dd,@ThoiGianDen)  
	AND datepart(mm,kh.NgaySinh) between datepart(mm,@ThoiGianTu) AND datepart(mm,@ThoiGianDen);
	-- lấy tổng khách hàng thêm mới từ @ThoiGianTu đến @ThoiGianDen
	SELECT @tongSoKhachHang = Count(*) 
	FROM DM_KhachHang kh
	--JOIN (SELECT id,NhanSuId,IsAdmin FROM AbpUsers where IsActive= 1) as us on us.id = kh.CreatorUserId
	--JOIN (SELECT id FROM NS_NhanVien) as nv on nv.Id = us.NhanSuId
	--JOIN 
	--	(
	--		SELECT top(1) IdChiNhanh,Id,IdNhanVien 
	--		from NS_QuaTrinh_CongTac 
	--		order by CreationTime desc
	--	) as qtct on qtct.IdNhanVien = nv.Id
	WHERE kh.IsDeleted = 0 AND kh.TenantId = @TenantId 
	--AND (
 --       (us.IsAdmin = 0 AND qtct.IdChiNhanh = @IdChiNhanh) OR us.IsAdmin <> 0
 --   )
	AND CAST(kh.CreationTime AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE);

	-- lấy tổng lịch hẹn trong khoảng thời gian @ThoiGianTu đến @ThoiGianDen
	SELECT @tongLichHen = COUNT(*) FROM Booking where TenantId = @TenantId and IsDeleted = 0
	AND CAST(BookingDate AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE)
	AND IdChiNhanh = @IdChiNhanh;
	-- lấy tổng doanh thu từ @ThoiGianTu đến @ThoiGianDen
	SELECT @tongDoanhThu = SUM(TongThanhToan) FROM BH_HoaDon where TenantId = 1 and IsDeleted = 0 and IdChiNhanh = @IdChiNhanh
	AND CAST(CreationTime AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE);

	SELECT 
		ISNULL(@khachHangSinhNhat,0) as TongKhachHangSinhNhat,
		ISNULL(@tongSoKhachHang,0) as TongKhachHang,
		ISNULL(@tongDoanhThu,0) as TongDoanhThu,
		ISNULL(@tongLichHen,0) as TongLichHen
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP PROCEDURE prc_dashboard_thongKeSoLuong");
        }
    }
}
