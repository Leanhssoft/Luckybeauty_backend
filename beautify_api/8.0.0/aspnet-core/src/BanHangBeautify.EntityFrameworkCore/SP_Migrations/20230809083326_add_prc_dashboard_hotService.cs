using Microsoft.EntityFrameworkCore.Migrations;
using System;

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
		TongDoanhThu decimal,
		SoLanThucHien int
    );
	DECLARE @TongDichVu int;

	SELECT @TongDichVu = Count(hh.id) FROM
	BH_HoaDon hd JOIN BH_HoaDon_ChiTiet hdct on hdct.IdHoaDon = hd.Id
	JOIN DM_DonViQuiDoi dvqd on dvqd.Id = hdct.IdDonViQuyDoi
	JOIN DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	WHERE CAST(hd.CreationTime AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE) 
		and hd.TenantId = @TenantId 
		and hd.IdChiNhanh = @IdChiNhanh AND hd.IsDeleted = 0;

	INSERT INTO @HotService 
	SELECT hh.TenHangHoa,hh.Color,hdct.ThanhTienSauVAT AS TongTien,
	CASE WHEN hd.Id is null then 0 else 1 end AS SoLanThucHien
	FROM (
		SELECT Id,TenantId,IdChiNhanh 
		FROM BH_HoaDon
		WHERE CAST(CreationTime AS DATE) BETWEEN CAST(@ThoiGianTu AS DATE) AND CAST(@ThoiGianDen AS DATE) 
		and TenantId = @TenantId 
		and IdChiNhanh = @IdChiNhanh AND IsDeleted = 0) 
	as hd 
	JOIN BH_HoaDon_ChiTiet hdct on hdct.IdHoaDon = hd.Id
	RIGHT JOIN DM_DonViQuiDoi dvqd on dvqd.Id = hdct.IdDonViQuyDoi
	RIGHT JOIN DM_HangHoa hh on hh.id = dvqd.IdHangHoa;

	SELECT TOP 5
        TenDichVu,
        SUM(TongDoanhThu) AS TongDoanhThu,
        Color,
        CASE WHEN @TongDichVu = 0 THEN 0 ELSE SUM(SoLanThucHien) / CAST(@TongDichVu AS DECIMAL) * 100 END AS PhanTram
    FROM @HotService 
    GROUP BY 
        TenDichVu,
        Color
    ORDER BY SUM(SoLanThucHien) DESC;
END;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_dashboard_hotService");
        }
    }
}
