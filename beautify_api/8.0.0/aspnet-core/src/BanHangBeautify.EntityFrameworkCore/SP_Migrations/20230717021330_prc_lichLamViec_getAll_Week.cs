using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_lichLamViec_getAll_Week")]
    public partial class prc_lichLamViec_getAll_Week : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_lichLamViec_getAll_Week]
	@TenantId INT,
	@IdChiNhanh uniqueidentifier,
    @IdNhanVien uniqueidentifier = null,
	@SkipCount INT = 0,
	@MaxResultCount INT = 10,
	@DateFrom Date,
	@DateTo Date
AS
BEGIN
    -- Declare necessary variables
    DECLARE @ListItems TABLE (
        Avatar VARCHAR(MAX),
        IdNhanVien uniqueidentifier,
		IdCaLamViec uniqueidentifier,
        TenNhanVien VARCHAR(MAX),
        TongThoiGian Float,
        Monday VARCHAR(MAX),
        Tuesday VARCHAR(MAX),
        Wednesday VARCHAR(MAX),
        Thursday VARCHAR(MAX),
        Friday VARCHAR(MAX),
        Saturday VARCHAR(MAX),
        Sunday VARCHAR(MAX)
    )
	DECLARE @TotalCount INT
    -- Retrieve data from the database
    DECLARE @NhanVien TABLE (
        -- Define necessary columns from the NhanVien table
        -- Adjust the column data types as per your actual table structure
        Id uniqueidentifier,
        Avatar VARCHAR(MAX),
        TenNhanVien VARCHAR(MAX)
    )

    -- Retrieve the list of NhanVien
    INSERT INTO @NhanVien (Id, Avatar, TenNhanVien)
    SELECT nv.Id, nv.Avatar, nv.TenNhanVien
    FROM NS_NhanVien nv
	LEFT JOIN NS_ChucVu cv ON cv.Id = nv.IdChucVu
	JOIN (
		SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
		WHERE IsDeleted = 0 AND TenantId= @TenantId 
		GROUP BY IdNhanVien,IdChiNhanh
	) AS qtct ON qtct.IdNhanVien = nv.Id
    WHERE nv.IsDeleted = 0 -- Assuming IsDeleted is a column in the NhanVien table
        AND nv.TenantId = ISNULL(@TenantId, 1) -- Assuming TenantId is a column in the NhanVien table
		AND qtct.IdChiNhanh = @IdChiNhanh
        AND (@IdNhanVien IS NULL OR (nv.Id = @IdNhanVien AND @IdNhanVien IS NOT NULL))
	ORDER BY nv.TenNhanVien DESC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY

    -- Get the total count
    SET @TotalCount = (
			SELECT COUNT(*) FROM NS_NhanVien nv 
			LEFT JOIN NS_ChucVu cv ON cv.Id = nv.IdChucVu
			JOIN (
				SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
				WHERE IsDeleted = 0 AND TenantId= @TenantId 
				GROUP BY IdNhanVien,IdChiNhanh
			) AS qtct ON qtct.IdNhanVien = nv.Id
			WHERE nv.IsDeleted = 0 -- Assuming IsDeleted is a column in the NhanVien table
				AND nv.TenantId = ISNULL(@TenantId, 1) -- Assuming TenantId is a column in the NhanVien table
				AND qtct.IdChiNhanh = @IdChiNhanh
                AND (@IdNhanVien IS NULL OR (nv.Id = @IdNhanVien AND @IdNhanVien IS NOT NULL))
		)

    -- Retrieve the list of LichLamViecNhanVien
    INSERT INTO @ListItems (Avatar, IdNhanVien,IdCaLamViec, TenNhanVien, TongThoiGian, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday)
    
	SELECT
        N.Avatar,
        N.Id,
		C.Id,
        N.TenNhanVien,
        CASE
            WHEN C.Id IS NOT NULL THEN  C.TongGioCong
            ELSE 0
        END AS TongThoiGian,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 2 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Monday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 3 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Tuesday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 4 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Wednesday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY,LC.NgayLamViec) = 5 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Thursday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 6 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Friday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 7 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Saturday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 1 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Sunday
    FROM @NhanVien N
    LEFT JOIN NS_LichLamViec L ON L.IdNhanVien = N.Id AND L.TuNgay BETWEEN @DateFrom AND @DateTo AND L.IsDeleted = 0
	LEFT JOIN NS_LichLamViec_Ca LC ON LC.IdLichLamViec = l.Id AND LC.IsDeleted = 0
    LEFT JOIN NS_CaLamViec C ON C.Id = LC.IdCaLamViec AND C.IsDeleted = 0
	--WHERE L.TuNgay BETWEEN @DateFrom AND @DateTo
    GROUP BY N.Id, N.Avatar, N.TenNhanVien,C.Id,C.TongGioCong, LC.NgayLamViec, C.GioVao, C.GioRa

    -- Return the result
    SELECT
	Avatar,
	IdNhanVien,
	TenNhanVien,
	CONVERT(NVARCHAR(Max), SUM(TongThoiGian)) as TongThoiGian,
	MAX(Monday) as Monday,
	MAX(Tuesday) as Tuesday,
	MAX(Wednesday) as Wendnesday,
	MAX(Thursday) as Thursday,
	MAX(Friday) as Friday,
	MAX(Saturday) as Saturday,
	MAX(Sunday) as Sunday
    FROM @ListItems 
	Group by Avatar, IdNhanVien, TenNhanVien,IdCaLamViec
	ORDER BY TenNhanVien DESC;

	SELECT @TotalCount as TotalCount;
END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_lichLamViec_getAll_Week");
        }
    }
}
