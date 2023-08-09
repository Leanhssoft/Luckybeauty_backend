using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcdashboardhotService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC prc_dashboard_hotService
	@TenantId int,
	@UserId int = NULL,
	@ThoiGianTu DateTime = null,
	@ThoiGianDen DateTime = null,
	@IdChiNhanh uniqueidentifier = null
AS
BEGIN
	SET NOCOUNT ON;
	 DECLARE @HotService TABLE
    (
        TenDichVu nvarchar(100),
		Color nvarchar(15),
		TongDoanhThu decimal
    );
	DECLARE @TongDoanhThuDichVu decimal(18,2);

	SELECT @TongDoanhThuDichVu = SUM(hdct.ThanhTienSauVAT) FROM
	BH_HoaDon hd JOIN BH_HoaDon_ChiTiet hdct on hdct.IdHoaDon = hd.Id
	WHERE CAST(hd.CreationTime AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE) 
		and hd.TenantId = @TenantId 
		and hd.IdChiNhanh = @IdChiNhanh;

	INSERT INTO @HotService 
	SELECT top(5) hh.TenHangHoa,hh.Color,SUM(hdct.ThanhTienSauVAT) AS TongTien
	FROM (
		SELECT Id,TenantId,IdChiNhanh 
		FROM BH_HoaDon 
		WHERE CAST(CreationTime AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE) 
		and TenantId = @TenantId 
		and IdChiNhanh = @IdChiNhanh) 
	as hd 
	JOIN BH_HoaDon_ChiTiet hdct on hdct.IdHoaDon = hd.Id
	right JOIN DM_DonViQuiDoi dvqd on dvqd.Id = hdct.IdDonViQuyDoi
	right JOIN DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	GROUP BY hh.TenHangHoa,dvqd.Id,hh.Color
	ORDER BY COUNT(hh.id) DESC;

	SELECT TenDichVu,TongDoanhThu,Color, (TongDoanhThu / @TongDoanhThuDichVu) * 100 as PhanTram FROM @HotService;
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP PROCEDURE prc_dashboard_hotService");
        }
    }
}
