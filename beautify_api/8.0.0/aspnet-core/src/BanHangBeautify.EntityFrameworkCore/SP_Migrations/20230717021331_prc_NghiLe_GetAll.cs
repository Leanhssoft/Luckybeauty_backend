using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_NghiLe_GetAll")]
    public partial class prc_NghiLe_GetAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_nghiLe_getAll 
	@TenantId INT,
	@Filter NVARCHAR(70),
	@SortBy NVARCHAR(20),
	@SortType VARCHAR(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS
BEGIN
	SELECT 
		Id,
		TenNgayLe,
		TuNgay,
		DenNgay,
		TongSoNgay AS TongSoNgay 
	FROM DM_NgayNghiLe  
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(TenNgayLe) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(10), TongSoNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), TuNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), DenNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
	ORDER BY 
		CASE WHEN @SortBy = 'tenNgayLe' AND @SortType = 'desc' THEN TenNgayLe END DESC,
		CASE WHEN @SortBy = 'tuNgay' AND @SortType = 'desc' THEN TuNgay END DESC,
		CASE WHEN @SortBy = 'denNgay' AND @SortType = 'desc' THEN DenNgay END DESC,
		CASE WHEN @SortBy = 'tongSoNgay' AND @SortType = 'desc' THEN TongSoNgay END DESC,
		CASE WHEN @SortBy = 'tenNgayLe' AND @SortType = 'asc' THEN TenNgayLe END ASC,
		CASE WHEN @SortBy = 'tuNgay' AND @SortType = 'asc' THEN TuNgay END ASC,
		CASE WHEN @SortBy = 'denNgay' AND @SortType = 'asc' THEN DenNgay END ASC,
		CASE WHEN @SortBy = 'tongSoNgay' AND @SortType = 'asc' THEN TongSoNgay END ASC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
	SELECT COUNT(*) as TotalCount FROM DM_NgayNghiLe WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(TenNgayLe) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(10), TongSoNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), TuNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), DenNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_nghiLe_getAll");
        }
    }
}
