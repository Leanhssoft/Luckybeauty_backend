using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_getAllBooking")]
    public class prc_getAllBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_booking_getAll
    @TenantId INT,
    @IdChiNhanh UNIQUEIDENTIFIER,
    @IdNhanVien UNIQUEIDENTIFIER = NULL,
	@IdDichVu UNIQUEIDENTIFIER = NULL,
    @TimeFrom DATETIME,
    @TimeTo DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Result TABLE
    (
        SourceId UNIQUEIDENTIFIER,
		Employee NVARCHAR(50),
        StartTime NVARCHAR(5),
        EndTime NVARCHAR(5),
        Customer NVARCHAR(100),
        Services NVARCHAR(MAX),
        DayOfWeek NVARCHAR(10),
        Color NVARCHAR(10),
		BookingDate DateTime2
    );

    INSERT INTO @Result
    SELECT
        bnv.IdNhanVien AS SourceId,
		nv.TenNhanVien AS Employee,
        CONVERT(NVARCHAR(5), b.StartTime, 108) AS StartTime,
        CONVERT(NVARCHAR(5), b.EndTime, 108) AS EndTime,
        b.TenKhachHang AS Customer,
        STRING_AGG(hh.TenHangHoa, '-') AS Services,
        CASE DATEPART(WEEKDAY, b.BookingDate)
            WHEN 1 THEN N'Chủ nhật'
            WHEN 2 THEN N'Thứ hai'
            WHEN 3 THEN N'Thứ ba'
            WHEN 4 THEN N'Thứ tư'
            WHEN 5 THEN N'Thứ năm'
            WHEN 6 THEN N'Thứ sáu'
            WHEN 7 THEN N'Thứ bảy'
            ELSE 'Thứ hai'
        END AS DayOfWeek,
        CASE
            WHEN b.TrangThai = 0 THEN '#F1416C'
			WHEN B.TrangThai = 1 THEN '#FF9900'
			wHEN b.TrangThai = 2 THEN '#7C3367'
			WHEN b.TrangThai = 3 THEN '#50CD89'
            ELSE '#009EF7'
        END AS Color,
		b.BookingDate
    FROM
        Booking b
        INNER JOIN BookingNhanVien bnv ON bnv.IdBooking = b.Id and bnv.IsDeleted = 0
		LEFT JOIN NS_NhanVien nv on nv.id = bnv.IdNhanVien
        LEFT JOIN BookingService bs ON bs.IdBooking = b.Id and bs.IsDeleted = 0
        LEFT JOIN DM_DonViQuiDoi dv ON dv.Id = bs.IdDonViQuiDoi
        LEFT JOIN DM_HangHoa hh ON hh.id = dv.IdHangHoa
    WHERE
        (b.TenantId = ISNULL(@TenantId, 1))
        AND (b.IsDeleted = 0)
        AND (b.IdChiNhanh = @IdChiNhanh)
        AND (b.BookingDate BETWEEN @TimeFrom AND @TimeTo)
        AND (@IdNhanVien IS NULL OR (bnv.IdNhanVien = @IdNhanVien AND @IdNhanVien IS NOT NULL))
		AND (@IdDichVu IS NULL OR (bs.IdDonViQuiDoi = @IdDichVu AND @IdDichVu IS NOT NULL))
    GROUP BY
        bnv.IdNhanVien,
        b.StartTime,
        b.EndTime,
		nv.TenNhanVien,
        b.TenKhachHang,
        DATEPART(WEEKDAY, b.BookingDate),
		B.TrangThai,
		b.BookingDate;
    SELECT * FROM @Result ORDER BY BookingDate DESC,StartTime ASC;
END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_booking_getAll");
        }
    }
}
