using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("prc_chietKhauDichVu_getAll")]
    public partial class prc_chietKhauDichVu_getAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE PROCEDURE prc_chietKhauDichVu_getAll
					@TenantId INT,
					@IdNhanVien UNIQUEIDENTIFIER,
					@IdChiNhanh UNIQUEIDENTIFIER, 
					@Filter NVARCHAR(50),
					@SkipCount INT,
					@MaxResultCount INT,
					@SortBy NVARCHAR(30),
					@SortType VARCHAR(4) = 'desc'
				AS
				BEGIN
					-- Danh sách dịch vụ chiết khấu theo nhân viên
					SELECT 
						ckdv.Id,
						hh.TenHangHoa AS TenDichVu,
						nhh.TenNhomHang AS TenNhomDichVu,
						ckdv.GiaTri,
						ckdv.LaPhanTram,
						dvqd.GiaBan AS GiaDichVu,
						CASE WHEN ckdv.LoaiChietKhau = 1 THEN ckdv.GiaTri ELSE 0 END AS HoaHongThucHien,
						CASE WHEN ckdv.LoaiChietKhau = 2 THEN ckdv.GiaTri ELSE 0 END AS HoaHongYeuCauThucHien,
						CASE WHEN ckdv.LoaiChietKhau = 3 THEN ckdv.GiaTri ELSE 0 END AS HoaHongTuVan
					FROM NS_ChietKhauDichVu ckdv
					JOIN DM_DonViQuiDoi dvqd ON dvqd.id = ckdv.IdDonViQuiDoi
					JOIN DM_HangHoa hh ON hh.Id = dvqd.IdHangHoa
					LEFT JOIN DM_NhomHangHoa nhh ON nhh.Id = hh.IdNhomHangHoa
					WHERE ckdv.TenantId = @TenantId
						AND ckdv.IdChiNhanh = @IdChiNhanh
						AND ckdv.IdNhanVien = @IdNhanVien
						AND (
							ISNULL(@Filter,'') = ''
							OR LOWER(hh.TenHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(nhh.TenNhomHang) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(CONVERT(NVARCHAR(MAX), ckdv.GiaTri)) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(CONVERT(NVARCHAR(MAX), dvqd.GiaBan)) LIKE N'%' + LOWER(@Filter) + N'%'
						)
					ORDER BY
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenDichVu' THEN hh.TenHangHoa END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenNhomDichVu' THEN nhh.TenNhomHang END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'giaDichVu' THEN dvqd.GiaBan END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'hoaHongThucHien' THEN (CASE WHEN ckdv.LoaiChietKhau = 1 THEN ckdv.GiaTri ELSE 0 END) END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'hoaHongYeuCauThucHien' THEN (CASE WHEN ckdv.LoaiChietKhau = 2 THEN ckdv.GiaTri ELSE 0 END) END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'hoaHongTuVan' THEN CASE WHEN ckdv.LoaiChietKhau = 3 THEN ckdv.GiaTri ELSE 0 END END ASC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenDichVu' THEN hh.TenHangHoa END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenNhomDichVu' THEN nhh.TenNhomHang END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'giaDichVu' THEN dvqd.GiaBan END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'hoaHongThucHien' THEN (CASE WHEN ckdv.LoaiChietKhau = 1 THEN ckdv.GiaTri ELSE 0 END) END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'hoaHongYeuCauThucHien' THEN (CASE WHEN ckdv.LoaiChietKhau = 2 THEN ckdv.GiaTri ELSE 0 END) END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'hoaHongTuVan' THEN (CASE WHEN ckdv.LoaiChietKhau = 3 THEN ckdv.GiaTri ELSE 0 END) END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = '' THEN ckdv.CreationTime END DESC
					OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
					-- Tổng số bản ghi 
					SELECT COUNT(*) AS TotalCount FROM NS_ChietKhauDichVu ckdv
					JOIN DM_DonViQuiDoi dvqd ON dvqd.id = ckdv.IdDonViQuiDoi
					JOIN DM_HangHoa hh ON hh.Id = dvqd.IdHangHoa
					LEFT JOIN DM_NhomHangHoa nhh ON nhh.Id = hh.IdNhomHangHoa
					WHERE ckdv.TenantId = @TenantId
						AND ckdv.IdChiNhanh = @IdChiNhanh
						AND ckdv.IdNhanVien = @IdNhanVien
						AND (
							ISNULL(@Filter,'') = ''
							OR LOWER(hh.TenHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(nhh.TenNhomHang) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(CONVERT(NVARCHAR(MAX), ckdv.GiaTri)) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(CONVERT(NVARCHAR(MAX), dvqd.GiaBan)) LIKE N'%' + LOWER(@Filter) + N'%'
						);
				END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_chietKhauDichVu_getAll");
        }
    }
}
