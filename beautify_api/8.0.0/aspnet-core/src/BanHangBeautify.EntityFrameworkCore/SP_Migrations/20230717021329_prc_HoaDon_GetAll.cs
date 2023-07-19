using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_HoaDon_GetAll")]
    public partial class prc_HoaDon_GetAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE prc_HoaDon_GetAll 
					@TenantId int,
					@IdChiNhanh UNIQUEIDENTIFIER,  
					@Filter NVARCHAR(MAX),
					@SortBy VARCHAR(50),
					@SortType VARCHAR(4) = 'desc',
					@SkipCount INT = 0,
					@MaxResultCount INT = 10 
				AS 
				BEGIN
					SELECT * FROM (
						SELECT hd.MaHoaDon,
						hd.CreationTime as NgayBan,
						kh.TenKhachHang,
						hd.TongTienHang,
						hd.TongGiamGiaHD as TongGiamGia,
						hd.TongTienHDSauVAT - hd.TongGiamGiaHD AS TongPhaiTra,
						hd.TongThanhToan,
						hd.TongThanhToan - (hd.TongTienHDSauVAT - hd.TongGiamGiaHD) AS ConNo,
						CASE WHEN hd.TrangThai = 0 THEN N'Đã xóa' 
							 WHEN hd.TrangThai = 1 Then N'Tạm lưu'
							 WHEN hd.TrangThai = 2 THEN N'Đang xử lý'
							 WHEN hd.TrangThai = 3 THEN N'Hoàn thành'
							 ELSE '' END AS TrangThai
						FROM BH_HoaDon hd
						JOIN DM_KhachHang kh ON kh.id =hd.IdKhachHang
						WHERE hd.TenantId = @TenantId 
						AND (hd.IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
						AND hd.IsDeleted = 0
						AND (
							ISNULL(@Filter, '') = ''
							OR LOWER(hd.MaHoaDon) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.CreationTime) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.TongTienHang) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.TongGiamGiaHD) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.TongTienHDSauVAT) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.TongThanhToan) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(kh.TenKhachHang) LIKE N'%'+ LOWER(@Filter) + '%'
						)
					) AS RESULT
					ORDER BY  
						CASE WHEN @SortBy = 'maHoaDon' AND LOWER(@SortType) = 'asc' THEN MaHoaDon END ASC,
						CASE WHEN @SortBy = 'ngayBan' AND LOWER(@SortType) = 'asc' THEN NgayBan END ASC,
						CASE WHEN @SortBy = 'tenKhachHang' AND LOWER(@SortType) = 'asc' THEN TenKhachHang END ASC,
						CASE WHEN @SortBy = 'tongGiamGia' AND LOWER(@SortType) = 'asc' THEN TongGiamGia END ASC,
						CASE WHEN @SortBy = 'tongPhaiTra' AND LOWER(@SortType) = 'asc' THEN TongPhaiTra END ASC,
						CASE WHEN @SortBy = 'tongThanhToan' AND LOWER(@SortType) = 'asc' THEN TongThanhToan END ASC,
						CASE WHEN @SortBy = 'trangThai' AND LOWER(@SortType) = 'asc' THEN TrangThai END ASC,
						CASE WHEN @SortBy = 'maHoaDon' AND LOWER(@SortType) = 'desc' THEN MaHoaDon END DESC,
						CASE WHEN @SortBy = 'ngayBan' AND LOWER(@SortType) = 'desc' THEN NgayBan END DESC,
						CASE WHEN @SortBy = 'tenKhachHang' AND LOWER(@SortType) = 'desc' THEN TenKhachHang END DESC,
						CASE WHEN @SortBy = 'tongGiamGia' AND LOWER(@SortType) = 'desc' THEN TongGiamGia END DESC,
						CASE WHEN @SortBy = 'tongPhaiTra' AND LOWER(@SortType) = 'desc' THEN TongPhaiTra END DESC,
						CASE WHEN @SortBy = 'tongThanhToan' AND LOWER(@SortType) = 'desc' THEN TongThanhToan END DESC,
						CASE WHEN @SortBy = 'trangThai' AND LOWER(@SortType) = 'desc' THEN TrangThai END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' THEN NgayBan END DESC
					OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

					SELECT COUNT(*) as TotalCount
						FROM BH_HoaDon hd
						JOIN DM_KhachHang kh ON kh.id =hd.IdKhachHang
						WHERE hd.TenantId = @TenantId 
						AND (hd.IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
						AND (
							ISNULL(@Filter, '') = ''
							OR LOWER(hd.MaHoaDon) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.CreationTime) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.TongTienHang) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.TongGiamGiaHD) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.TongTienHDSauVAT) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(hd.TongThanhToan) LIKE N'%'+ LOWER(@Filter) + '%'
							OR LOWER(kh.TenKhachHang) LIKE N'%'+ LOWER(@Filter) + '%'
						);
				END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_HoaDon_GetAll");
        }
    }
}
