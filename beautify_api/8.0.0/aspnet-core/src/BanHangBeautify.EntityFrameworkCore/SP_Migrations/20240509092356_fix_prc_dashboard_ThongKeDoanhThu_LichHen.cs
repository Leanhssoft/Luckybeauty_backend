using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class fixprcdashboardThongKeDoanhThuLichHen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER PROC prc_dashboard_thongKeDoanhThu
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
        DECLARE @Month NVARCHAR(20) = CAST(@MonthCounter AS NVARCHAR(2));

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
"); migrationBuilder.Sql(@"ALTER PROC prc_dashboard_thongKeLichHen
	@TenantId int,
	@IdChiNhanh uniqueidentifier = null
AS
BEGIN
	SET NOCOUNT ON;

    -- Create a temporary table to hold the month data
    CREATE TABLE #WeeklyData (
        WeekName NVARCHAR(20),
		WeekValue INT,
        TuanNay INT,
        TuanTruoc INT
    );

    DECLARE @WeekCounter INT = 1;

    -- Loop through each month
    WHILE @WeekCounter <= 7
    BEGIN
        DECLARE @DayOfWeek NVARCHAR(20) = CASE WHEN @WeekCounter = 1 Then N'0'
		ELSE CAST(@WeekCounter AS NVARCHAR(10)) end;

        INSERT INTO #WeeklyData (WeekName,WeekValue,TuanNay, TuanTruoc)
        SELECT
            @DayOfWeek,
			@WeekCounter,
            SUM(CASE WHEN DATEPART(WEEK,BookingDate) =  DATEPART(WEEK, GETDATE()) AND DATEPART(WEEKDAY,BookingDate) = @WeekCounter THEN 1 ELSE 0 END),
            SUM(CASE WHEN DATEPART(WEEK,BookingDate) =  DATEPART(WEEK,DATEADD(WEEK, -1, GETDATE())) AND DATEPART(WEEKDAY,BookingDate)= @WeekCounter THEN 1 ELSE 0 END)
        FROM Booking
        WHERE IsDeleted = 0 AND TenantId = @TenantId AND IdChiNhanh = @IdChiNhanh
        GROUP BY DATEPART(WEEK, BookingDate);

        SET @WeekCounter = @WeekCounter + 1;
    END;

    -- Fetch data from the temporary table
    SELECT WeekName as Tuan ,SUM(TuanNay) AS TuanNay ,SUM(TuanTruoc) AS TuanTruoc 
	FROM #WeeklyData 
	GROUP BY WeekName,WeekValue ORDER BY WeekName asc;

    -- Drop the temporary table
    DROP TABLE #WeeklyData;
    
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_dashboard_thongKeLichHen");
            migrationBuilder.Sql("DROP PROCEDURE prc_dashboard_thongKeDoanhThu");
        }
    }
}
