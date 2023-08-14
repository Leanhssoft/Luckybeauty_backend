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
    [Migration("updateProcedure_prc_khachHang_getAll")]
    public class updateProcedure_prc_khachHang_getAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[prc_khachHang_getAll]
					@TenantId INT = 7,
					@IdChiNhanh UNIQUEIDENTIFIER = null,
					@Filter NVARCHAR(max) =N'',
					@SkipCount INT = 0,
					@MaxResultCount INT = 10,
					@SortBy NVARCHAR(50)='CreateTime',
					@SortType VARCHAR(4)='desc',
					@IdNhomKhach UNIQUEIDENTIFIER = NULL
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
						'' AS NhanVienPhuTrach,
						kh.TongTichDiem,
						kh.CreationTime,
						hd.TongChiTieu,
						ISNULL(tblCheckIn.CuocHenGanNhat,kh.CreationTime) AS CuocHenGanNhat,
						isnull(tblCheckIn.SoLanCheckIn,0) as SoLanCheckIn,
						isnull(tblChecking.TrangThaiCheckIn,0) as TrangThaiCheckIn,
						ngkh.TenNguon as TenNguonKhach
						FROM DM_KhachHang kh
						LEFT JOIN DM_NhomKhachHang nkh on nkh.Id = kh.IdNhomKhach
						LEFT JOIN DM_NguonKhach ngkh on ngkh.Id = kh.IdNguonKhach
						LEFT JOIN (
							SELECT IdKhachHang, MAX(CreationTime) AS MaxCreationTime,SUM(TongTienHDSauVAT) as TongChiTieu
							FROM BH_HoaDon
							WHERE IsDeleted = 0
							GROUP BY IdKhachHang
						) hd ON hd.IdKhachHang = kh.Id
						LEFT JOIN (
							SELECT IdKhachHang, max(DateTimeCheckIn) as CuocHenGanNhat, count(id) as SoLanCheckIn
							FROM KH_CheckIn
							WHERE IsDeleted = 0
							GROUP BY IdKhachHang
						) tblCheckIn ON kh.Id = tblCheckIn.IdKhachHang
						left join (
							SELECT IdKhachHang, 1 as TrangThaiCheckIn
							FROM KH_CheckIn
							WHERE IsDeleted = 0 and TrangThai= 1
							GROUP BY IdKhachHang
						) tblChecking ON kh.Id = tblChecking.IdKhachHang --- khach dang check in
						WHERE kh.TenantId = @TenantId 
						AND kh.IsDeleted = 0
						AND (ISNULL(@Filter,'') = ''
							OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(kh.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(kh.DiaChi) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(nkh.TenNhomKhach) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(ngkh.TenNguon) LIKE N'%' + LOWER(@Filter) + N'%'
						)
						AND (@IdNhomKhach IS NULL OR (nkh.Id = @IdNhomKhach AND @IdNhomKhach IS NOT NULL))
					) as Result
					ORDER BY
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'diaChi' THEN DiaChi END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngaySinh' THEN NgaySinh END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenNguonKhach' THEN TenNguonKhach END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'gioiTinh' THEN GioiTinh END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tongTchDiem' THEN TongTichDiem END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'cuocHenGanNhat' THEN CuocHenGanNhat END,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'diaChi' THEN DiaChi END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngaySinh' THEN NgaySinh END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenNguonKhach' THEN TenNguonKhach END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'gioiTinh' THEN GioiTinh END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tongTchDiem' THEN TongTichDiem END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'cuocHenGanNhat' THEN CuocHenGanNhat END DESC,
						CASE WHEN LOWER(ISNULL(@SortType,'')) = '' THEN CreationTime END DESC
					OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

					SELECT COUNT(*) as TotalCount
					FROM DM_KhachHang kh
					LEFT JOIN DM_NhomKhachHang nkh on nkh.Id = kh.IdNhomKhach
					LEFT JOIN DM_NguonKhach ngkh on ngkh.Id = kh.IdNguonKhach
					LEFT JOIN (
						SELECT IdKhachHang, MAX(CreationTime) AS MaxCreationTime,SUM(TongTienHDSauVAT) as TongChiTieu
						FROM BH_HoaDon
						WHERE IsDeleted = 0
						GROUP BY IdKhachHang
					) hd ON hd.IdKhachHang = kh.Id
					WHERE kh.TenantId = @TenantId 
					AND kh.IsDeleted = 0
					AND (@IdNhomKhach IS NULL OR (nkh.Id = @IdNhomKhach AND @IdNhomKhach IS NOT NULL))
					AND (ISNULL(@Filter,'') = ''
						OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(kh.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(kh.DiaChi) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(nkh.TenNhomKhach) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(ngkh.TenNguon) LIKE N'%' + LOWER(@Filter) + N'%'
					);
				END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP PROCEDURE prc_khachHang_getAll");
        }
    }
}
