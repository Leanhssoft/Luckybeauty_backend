using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_chietKhauHoaDon_getAll")]
    public partial class prc_chietKhauHoaDon_getAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_chietKhauHoaDon_getAll 
	@TenantId INT,
	@IdChiNhanh UNIQUEIDENTIFIER,
	@Filter NVARCHAR(200),
	@SortBy NVARCHAR(20),
	@SortType VARCHAR(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS
BEGIN
	SELECT 
		Id,
		ChungTuApDung,
		CASE WHEN LoaiChietKhau = 1 THEN CONVERT(NVARCHAR(MAX),GiaTriChietKhau) + N' % thực thu'
			 WHEN LoaiChietKhau = 2 THEN CONVERT(NVARCHAR(MAX),GiaTriChietKhau) + N' % doanh thu'
			 WHEN LoaiChietKhau = 3 THEN CONVERT(NVARCHAR(MAX),REPLACE(REPlace(REPLACE(FORMAT(GiaTriChietKhau, '###,###.##'),'.','@'),',','.'),'@',',')) + N' VNĐ'
		ELSE '0' END AS GiaTriChietKhau
		GhiChu
	FROM NS_ChietKhauHoaDon 
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(ChungTuApDung) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(MAX), GiaTriChietKhau)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
	ORDER BY 
		CASE WHEN @SortBy = 'chungTuApDung' AND @SortType = 'desc' THEN ChungTuApDung END DESC,
		CASE WHEN @SortBy = 'giaTriChietKhau' AND @SortType = 'desc' THEN GiaTriChietKhau END DESC,
		CASE WHEN @SortBy = 'chungTuApDung' AND @SortType = 'asc' THEN ChungTuApDung END ASC,
		CASE WHEN @SortBy = 'giaTriChietKhau' AND @SortType = 'asc' THEN GiaTriChietKhau END ASC,
		CASE WHEN ISNULL(@SortBy,'') = '' THEN CreationTime END DESC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
	SELECT COUNT(*) AS TotalCount FROM NS_ChietKhauHoaDon 
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(ChungTuApDung) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(MAX), GiaTriChietKhau)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_chietKhauHoaDon_getAll");
        }
    }
}
