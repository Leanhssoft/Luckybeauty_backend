using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcdashboardthongKeLichHen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC prc_dashboard_thongKeLichHen
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
        DECLARE @DayOfWeek NVARCHAR(20) = CASE WHEN @WeekCounter = 1 Then N'Chủ nhật'
		ELSE N'Thứ '
		+ CAST(@WeekCounter AS NVARCHAR(10)) end;

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
        }
    }
}
