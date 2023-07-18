using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_chiNhanh_getAll")]
    public partial class prc_ChiNhanh_GetAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_chiNhanh_getAll
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
		IdCongTy,
		MaChiNhanh,
		TenChiNhanh,
		SoDienThoai,
		DiaChi,
		MaSoThue,
		Logo,
		GhiChu,
		TrangThai,
		NgayApDung,
		NgayHetHan
	FROM DM_ChiNhanh 
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(MaChiNhanh) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(TenChiNhanh) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(DiaChi) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(MaSoThue) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(GhiChu) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(TrangThai) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), NgayApDung)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), NgayHetHan)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
	ORDER BY 
		CASE WHEN @SortBy = 'maChiNhanh' AND @SortType = 'desc' THEN MaChiNhanh END DESC,
		CASE WHEN @SortBy = 'tenChiNhanh' AND @SortType = 'desc' THEN TenChiNhanh END DESC,
		CASE WHEN @SortBy = 'soDienThoai' AND @SortType = 'desc' THEN SoDienThoai END DESC,
		CASE WHEN @SortBy = 'diaChi' AND @SortType = 'desc' THEN DiaChi END DESC,
		CASE WHEN @SortBy = 'maSoThue' AND @SortType = 'desc' THEN MaSoThue END DESC,
		CASE WHEN @SortBy = 'ghiChu' AND @SortType = 'desc' THEN GhiChu END DESC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'desc' THEN TrangThai END DESC,
		CASE WHEN @SortBy = 'ngayApDung' AND @SortType = 'desc' THEN NgayApDung END DESC,
		CASE WHEN @SortBy = 'ngayHetHan' AND @SortType = 'desc' THEN NgayHetHan END DESC,
		CASE WHEN @SortBy = 'maChiNhanh' AND @SortType = 'asc' THEN MaChiNhanh END ASC,
		CASE WHEN @SortBy = 'tenChiNhanh' AND @SortType = 'asc' THEN TenChiNhanh END ASC,
		CASE WHEN @SortBy = 'soDienThoai' AND @SortType = 'asc' THEN SoDienThoai END ASC,
		CASE WHEN @SortBy = 'diaChi' AND @SortType = 'asc' THEN DiaChi END ASC,
		CASE WHEN @SortBy = 'maSoThue' AND @SortType = 'asc' THEN MaSoThue END ASC,
		CASE WHEN @SortBy = 'ghiChu' AND @SortType = 'asc' THEN GhiChu END ASC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'asc' THEN TrangThai END ASC,
		CASE WHEN @SortBy = 'ngayApDung' AND @SortType = 'asc' THEN NgayApDung END ASC,
		CASE WHEN @SortBy = 'ngayHetHan' AND @SortType = 'asc' THEN NgayHetHan END ASC,
		CASE WHEN ISNULL(@SortBy,'') = '' THEN CreationTime END DESC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
	SELECT COUNT(*) AS TotalCOunt FROM DM_ChiNhanh 
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(MaChiNhanh) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(TenChiNhanh) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(DiaChi) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(MaSoThue) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(GhiChu) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(TrangThai) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), NgayApDung)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), NgayHetHan)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_chietKhauHoaDon_getAll");
        }
    }
}
