using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_caLamViec_getAll")]
    public partial class prc_caLamViec_getAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_caLamViec_getAll
	@TenantId INT,
	@IdChiNhanh UNIQUEIDENTIFIER = NULL,
	@Filter NVARCHAR(200),
	@SortBy NVARCHAR(20),
	@SortType VARCHAR(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS
BEGIN
	SELECT 
		Id,
		MaCa,
		TenCa,
		GioVao,
		GioRa,
		TongGioCong
	FROM NS_CaLamViec clv
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(MaCa) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(TenCa) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(10), TongGioCong)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), GioVao)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), GioRa)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
	ORDER BY 
		CASE WHEN @SortBy = 'maCa' AND @SortType = 'desc' THEN MaCa END DESC,
		CASE WHEN @SortBy = 'tenCa' AND @SortType = 'desc' THEN TenCa END DESC,
		CASE WHEN @SortBy = 'gioVao' AND @SortType = 'desc' THEN GioVao END DESC,
		CASE WHEN @SortBy = 'gioRa' AND @SortType = 'desc' THEN GioRa END DESC,
		CASE WHEN @SortBy = 'tongGioCong' AND @SortType = 'desc' THEN TongGioCong END DESC,
		CASE WHEN @SortBy = 'maCa' AND @SortType = 'asc' THEN MaCa END ASC,
		CASE WHEN @SortBy = 'tenCa' AND @SortType = 'asc' THEN TenCa END ASC,
		CASE WHEN @SortBy = 'gioVao' AND @SortType = 'asc' THEN GioVao END ASC,
		CASE WHEN @SortBy = 'gioRa' AND @SortType = 'asc' THEN GioRa END ASC,
		CASE WHEN @SortBy = 'tongGioCong' AND @SortType = 'asc' THEN TongGioCong END ASC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
	SELECT COUNT(*) as TotalCount FROM NS_CaLamViec WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(MaCa) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(TenCa) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(10), TongGioCong)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), GioVao)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), GioRa)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_caLamViec_getAll");
        }
    }
}
