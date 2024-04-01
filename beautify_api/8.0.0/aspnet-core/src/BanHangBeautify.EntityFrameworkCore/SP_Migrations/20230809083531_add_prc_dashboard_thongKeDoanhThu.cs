using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcdashboardthongKeDoanhThu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROC prc_dashboard_thongKeDoanhThu
	@TenantId int,
	@IdChiNhanh uniqueidentifier = null
AS
BEGIN
	SET NOCOUNT ON;

    -- Create a temporary table to hold the month data
    CREATE TABLE #MonthlyData (
        MonthName NVARCHAR(20),
		MonthValue INT,
        ThangNay DECIMAL(18, 2),
        ThangTruoc DECIMAL(18, 2)
    );

    DECLARE @MonthCounter INT = 1;

    -- Loop through each month
    WHILE @MonthCounter <= 12
    BEGIN
        DECLARE @Month NVARCHAR(20) = N'Tháng ' + CAST(@MonthCounter AS NVARCHAR(2));

        INSERT INTO #MonthlyData (MonthName,MonthValue,ThangNay, ThangTruoc)
        SELECT
            @Month,
			@MonthCounter,
            SUM(CASE WHEN YEAR(hd.NgayLapHoaDon) = YEAR(GETDATE()) AND MONTH(hd.NgayLapHoaDon) = @MonthCounter THEN hdct.ThanhTienSauVAT ELSE 0 END),
            SUM(CASE WHEN YEAR(hd.NgayLapHoaDon) = YEAR(DATEADD(YEAR, -1, GETDATE())) AND MONTH(hd.NgayLapHoaDon) = @MonthCounter THEN hdct.ThanhTienSauVAT ELSE 0 END)
        FROM BH_HoaDon hd
        JOIN BH_HoaDon_ChiTiet hdct ON hdct.IdHoaDon = hd.Id AND hdct.IsDeleted = 0
        WHERE hd.IsDeleted = 0 AND hd.TenantId = @TenantId AND hd.IdChiNhanh = @IdChiNhanh
        GROUP BY DATEPART(MONTH, hd.NgayLapHoaDon);

        SET @MonthCounter = @MonthCounter + 1;
    END;

    -- Fetch data from the temporary table
    SELECT MonthName as Month ,SUM(ThangNay) AS ThangNay ,SUM(ThangTruoc) AS ThangTruoc 
	FROM #MonthlyData 
	GROUP BY MonthName,MonthValue ORDER BY MonthValue asc;

    -- Drop the temporary table
    DROP TABLE #MonthlyData;
    
END;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_dashboard_thongKeDoanhThu");
        }
    }
}
