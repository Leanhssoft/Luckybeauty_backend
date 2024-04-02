using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class updateprcKhachHangGetAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
			IF EXISTS (
                SELECT * 
                FROM INFORMATION_SCHEMA.ROUTINES 
                WHERE SPECIFIC_SCHEMA = 'dbo' 
                AND SPECIFIC_NAME = 'prc_khachHang_getAll'
            )
            BEGIN
                DROP PROCEDURE [dbo].[prc_khachHang_getAll]
            END
			GO


			CREATE PROCEDURE [dbo].[prc_khachHang_getAll]
			@TenantId INT,
			@IdChiNhanh UNIQUEIDENTIFIER = null,
			@Filter NVARCHAR(max) =N'',
			@SkipCount INT = 1,
			@MaxResultCount INT = 10,
			@SortBy NVARCHAR(50)='CreateTime',
			@SortType VARCHAR(4)='desc',
			@IdNhomKhach UNIQUEIDENTIFIER = NULL,
			@GioiTinh BIT = NULL,
			@TongChiTieuTu FLOAT = NULL,
			@TongChiTieuDen FLOAT = NULL,
			@TimeFrom DATE = NULL,
			@TimeTo DATE = NULL
			AS
			BEGIN
				set nocount on;

				if @SkipCount > 0 set @SkipCount = @SkipCount - 1;
				if isnull(@SortType,'')!='' set @SortType = LOWER(@SortType)
				if isnull(@Filter,'')!='' set @Filter = concat(N'%', LOWER(@Filter), '%')

				
					SELECT  *
					into #tblCus
					FROM
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
							isnull(tblCheckIn.SoLanCheckIn,0) as SoLanCheckIn,					
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
								SELECT IdKhachHang, 
									count(id) as SoLanCheckIn
								FROM KH_CheckIn
								WHERE IsDeleted = 0
								GROUP BY IdKhachHang
							) tblCheckIn ON kh.Id = tblCheckIn.IdKhachHang						
							WHERE kh.TenantId = @TenantId 
							AND kh.IsDeleted = 0
							AND (ISNULL(@Filter,'') = ''
								OR LOWER(kh.TenKhachHang) LIKE @Filter
								OR LOWER(kh.TenKhachHang_KhongDau) LIKE @Filter
								OR LOWER(kh.SoDienThoai) LIKE @Filter
								OR LOWER(kh.DiaChi) LIKE @Filter
								OR LOWER(nkh.TenNhomKhach) LIKE @Filter
								OR LOWER(ngkh.TenNguon) LIKE @Filter
							)
							AND (@IdNhomKhach IS NULL OR (nkh.Id = @IdNhomKhach AND @IdNhomKhach IS NOT NULL))
							AND (@TimeFrom IS NULL OR @TimeTo IS NULL OR CAST(kh.CreationTime AS DATE) BETWEEN @TimeFrom AND @TimeTo)
							AND (@GioiTinh IS NULL OR kh.GioiTinhNam = @GioiTinh)
							AND (@TongChiTieuTu IS NULL OR @TongChiTieuDen IS NULL OR TongChiTieu BETWEEN @TongChiTieuTu AND @TongChiTieuDen)
						) as Result

					select cus.*,
						isnull(tblBooking.SoLanBooking,0) as SoLanBooking,
						tblBooking.CuocHenGanNhat,
						isnull(cus.TongChiTieu,0) - isnull(tblSoquy.TongThanhToan,0) as ConNo
					from #tblCus cus
					left join
					(
						------ get thu/chi of customer ---
						select 
							qct.IdKhachHang,
							----- 11.thu/12.chi
							sum(iif(qhd.IdLoaiChungTu =11, qct.TienThu, - qct.TienThu)) as TongThanhToan
						from QuyHoaDon qhd
						join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
						where qhd.IsDeleted='0'
						and qct.IsDeleted ='0'
						and qhd.TenantId = @TenantId
						and exists (select id from #tblCus cusIn where cusIn.Id = qct.IdKhachHang)
						group by qct.IdKhachHang
					)tblSoquy on cus.Id = tblSoquy.IdKhachHang
					left join
					(
						select IdKhachHang,
							max(BookingDate) as CuocHenGanNhat, 
							count(IdKhachHang) as SoLanBooking
						from Booking bk
						where bk.IsDeleted=0 
						and bk.TrangThai !=0 ---- trangthai=0: datlich, nhung sau do huy
						and exists (select id from #tblCus cusIn where cusIn.Id = bk.IdKhachHang)
						group by bk.IdKhachHang
					)tblBooking on cus.id = tblBooking.IdKhachHang
					ORDER BY
						CASE WHEN @SortType = 'asc' AND @SortBy = 'createTime' THEN CreationTime END,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'createTime' THEN CreationTime END DESC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'diaChi' THEN DiaChi END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'ngaySinh' THEN NgaySinh END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenNguonKhach' THEN TenNguonKhach END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'gioiTinh' THEN GioiTinh END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tongTichDiem' THEN TongTichDiem END,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'diaChi' THEN DiaChi END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'ngaySinh' THEN NgaySinh END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenNguonKhach' THEN TenNguonKhach END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'gioiTinh' THEN GioiTinh END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tongTichDiem' THEN TongTichDiem END DESC,						
						CASE WHEN LOWER(ISNULL(@SortType,'')) = '' THEN CreationTime END DESC
						OFFSET (@SkipCount* @MaxResultCount) ROWS
						FETCH NEXT @MaxResultCount ROWS ONLY
				

					SELECT COUNT(Id) as TotalCount
					FROM #tblCus 
					END;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_khachHang_getAll");
        }
    }
}
