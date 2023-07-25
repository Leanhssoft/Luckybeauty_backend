using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_NhanVien_GetAll")]
    public partial class prc_NhanVien_GetAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_nhanVien_getAll
				@TenantId INT,
				@IdChiNhanh UNIQUEIDENTIFIER = NULL,
				@Filter NVARCHAR,
				@SortBy NVARCHAR(50),
				@SortType NVARCHAR(4),
				@SkipCount INT = 0,
				@MaxResultCount INT = 10
			AS
			BEGIN
				SELECT 
				nv.Id,
				nv.Avatar,
				nv.Ho,
				nv.TenLot,
				nv.TenNhanVien,
				nv.SoDienThoai,
				nv.CCCD,
				nv.NgaySinh,
				nv.KieuNgaySinh,
				nv.GioiTinh,
				nv.NgayCap,
				nv.NoiCap,
				nv.DiaChi,
				cv.TenChucVu,
				nv.CreationTime AS NgayVaoLam,
				CASE WHEN nv.IsDeleted = 0 THEN N'Hoạt động' ELSE N'Ngừng hoạt động' END AS TrangThai
				FROM
				NS_NhanVien nv 
				LEFT JOIN NS_ChucVu cv ON cv.Id = nv.IdChucVu
				JOIN (
					SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
					WHERE IsDeleted = 0 AND TenantId= @TenantId 
					GROUP BY IdNhanVien,IdChiNhanh
				) AS qtct ON qtct.IdNhanVien = nv.Id
				WHERE nv.TenantId = @TenantId
				AND (qtct.IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
				AND (
					ISNULL(@Filter,'') = ''
					OR LOWER(nv.TenNhanVien) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.SoDienThoai) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.CCCD) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.DiaChi) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(cv.TenChucVu) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.DiaChi) LIKE N'%'+LOWER(@Filter)+N'%'
				)
				ORDER BY
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenNhanVien' THEN nv.TenNhanVien END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soDienThoai' THEN nv.SoDienThoai END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'diaChi' THEN nv.DiaChi END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngaySinh' THEN nv.NgaySinh END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'gioiTinh' THEN nv.GioiTinh END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'chucVu' THEN cv.TenChucVu END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngayThamGia' THEN nv.CreationTime END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'trangThai' THEN DiaChi END,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenNhanVien' THEN nv.TenNhanVien END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soDienThoai' THEN nv.SoDienThoai END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'diaChi' THEN nv.DiaChi END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngaySinh' THEN nv.NgaySinh END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'gioiTinh' THEN nv.GioiTinh END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'chucVu' THEN cv.TenChucVu END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngayThamGia' THEN nv.CreationTime END DESC,
					CASE WHEN ISNULL(@SortType,'') = '' AND ISNULL(@SortBy,'') = '' THEN nv.LastModificationTime END DESC
				OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

				SELECT COUNT(*) AS TotalCount
				FROM
				NS_NhanVien nv 
				LEFT JOIN NS_ChucVu cv ON cv.Id = nv.IdChucVu
				JOIN (
					SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
					WHERE IsDeleted = 0 AND TenantId= @TenantId 
					GROUP BY IdNhanVien,IdChiNhanh
				) AS qtct ON qtct.IdNhanVien = nv.Id
				WHERE nv.TenantId = @TenantId
				AND (qtct.IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
				AND (
					ISNULL(@Filter,'') = ''
					OR LOWER(nv.TenNhanVien) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.SoDienThoai) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.CCCD) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.DiaChi) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(cv.TenChucVu) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.DiaChi) LIKE N'%'+LOWER(@Filter)+N'%'
				)
			END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_nhanVien_getAll");
        }
    }
}
