using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("202308007083903_add_prc_khachHang_getAll")]
    public partial class addProcedureKhachHangGetAll : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_khachHang_getAll]");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE prc_khachHang_getAll
					@TenantId INT,
					@IdChiNhanh UNIQUEIDENTIFIER,
					@Filter NVARCHAR,
					@SkipCount INT = 0,
					@MaxResultCount INT = 10,
					@SortBy NVARCHAR(50),
					@SortType VARCHAR(4)
				AS
				BEGIN
					SELECT * FROM
						(SELECT 
						kh.Id,
						kh.MaKhachHang,
						kh.TenKhachHang,
						kh.Avatar,
						kh.SoDienThoai,
						kh.DiaChi,
						kh.NgaySinh,
						nkh.TenNhomKhach,
						CASE when kh.GioiTinhNam = 1 THEN N'Nam' ELSE N'Nữ' END as GioiTinh,
						kh.TongTichDiem,
						kh.CreationTime,
						hd.TongChiTieu,
						ISNULL(hd.MaxCreationTime,kh.CreationTime) AS CuocHenGanNhat
						FROM DM_KhachHang kh
						LEFT JOIN DM_NhomKhachHang nkh on nkh.Id = kh.IdNhomKhach
						LEFT JOIN (
							SELECT IdKhachHang, MAX(CreationTime) AS MaxCreationTime,SUM(TongTienHDSauVAT) as TongChiTieu
							FROM BH_HoaDon
							WHERE IsDeleted = 0
							GROUP BY IdKhachHang
						) hd ON hd.IdKhachHang = kh.Id
						WHERE kh.TenantId = @TenantId 
						AND kh.IsDeleted = 0
						AND (ISNULL(@Filter,'') = ''
							OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(kh.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(kh.DiaChi) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(nkh.TenNhomKhach) LIKE N'%' + LOWER(@Filter) + N'%'
						)
					) as Result
					ORDER BY
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'diaChi' THEN DiaChi END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngaySinh' THEN NgaySinh END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'gioiTinh' THEN GioiTinh END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tongTichDiem' THEN TongTichDiem END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'cuocHenGanNhat' THEN CuocHenGanNhat END,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'diaChi' THEN DiaChi END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngaySinh' THEN NgaySinh END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'gioiTinh' THEN GioiTinh END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tongTichDiem' THEN TongTichDiem END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'cuocHenGanNhat' THEN CuocHenGanNhat END DESC,
						CASE WHEN LOWER(ISNULL(@SortType,'')) = '' THEN CreationTime END DESC
					OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

					SELECT COUNT(*) as TotalCount
					FROM DM_KhachHang kh
					LEFT JOIN DM_NhomKhachHang nkh on nkh.Id = kh.IdNhomKhach
					LEFT JOIN (
						SELECT IdKhachHang, MAX(CreationTime) AS MaxCreationTime,SUM(TongTienHDSauVAT) as TongChiTieu
						FROM BH_HoaDon
						WHERE IsDeleted = 0
						GROUP BY IdKhachHang
					) hd ON hd.IdKhachHang = kh.Id
					WHERE kh.TenantId = @TenantId 
					AND kh.IsDeleted = 0
					AND (ISNULL(@Filter,'') = ''
						OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(kh.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(kh.DiaChi) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(nkh.TenNhomKhach) LIKE N'%' + LOWER(@Filter) + N'%'
					);
				END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_khachHang_getAll");
        }
    }
}
