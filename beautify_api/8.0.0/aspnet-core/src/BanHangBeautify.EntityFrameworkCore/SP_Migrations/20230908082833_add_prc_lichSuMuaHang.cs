using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprclichSuMuaHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE procedure prc_lichSuMuaHang
	@TenantId INT,
	@IdKhachHang UNIQUEIDENTIFIER
AS
BEGIN
	declare @lichSuMuaHang AS TABLE(
		MaHoaDon nvarchar(20),
		NgayLapHoaDon DateTime,
		TongTienHang money,
		TongGiamGia money,
		TongPhaiTra money,
		KhachDaTra money,
		ConNo money,
		TrangThai nvarchar(20)
	)
	INSERT INTO @lichSuMuaHang
		select MaHoaDon,NgayLapHoaDon,TongTienHang,TongGiamGiaHD,TongTienHDSauVAT,TongThanhToan,(TongTienHDSauVAT-TongThanhToan) as ConNo,
		CASE WHEN TrangThai = 0 THEN N'Xóa'
		WHEN TrangThai =1 THEN N'Tạm lưu'
		WHEN TrangThai =2 THEN N'Đang xử lý'
		WHEN TrangThai =3 THEN N'Thanh toán'
		ELSE N'Tạm lưu'
		END AS TrangThai from BH_HoaDon
		WHERE TenantId = @TenantId AND IdKhachHang = @IdKhachHang AND IsDeleted =0
	SELECT * FROM @lichSuMuaHang;
	SELECT COUNT(*) AS TotalCount FROM @lichSuMuaHang;
END;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP procedure prc_lichSuMuaHang");
        }
    }
}
