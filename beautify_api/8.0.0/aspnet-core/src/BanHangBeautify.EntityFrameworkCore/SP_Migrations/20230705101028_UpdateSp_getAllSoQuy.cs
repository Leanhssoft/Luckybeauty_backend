using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateSpgetAllSoQuy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
 ALTER PROCEDURE [dbo].[prc_SoQuy_GetAll]
                @TenantId INT = 7,
                @IdChiNhanh NVARCHAR(MAX) ='4ea9b488-0f53-416b-be8b-041f255114ba',
				@FromDate datetime = null,
				@ToDate datetime = null,
                @Filter NVARCHAR(MAX) ='',
                @SortBy VARCHAR(50) ='tongTienThu', 
                @SortType VARCHAR(4)='desc', 
                @SkipCount INT = 1,
                @MaxResultCount INT = 1000
            AS
            BEGIN
			if(ISNULL(@ToDate,'')!='') set @ToDate = DATEADD(DAY,1,@ToDate)
			if(@SkipCount > 0) set @SkipCount = @SkipCount -1


			;with data_cte
			as
			(
							
                    SELECT qhd.Id,
                        qhd.IdChiNhanh,
                        lct.TenLoaiChungTu as LoaiPhieu,
						qhd.NgayLapHoaDon,
						qhd.MaHoaDon,                      
                        qhd.CreationTime,
                        ktc.TenKhoanThuChi,
                        qhd.TongTienThu,
						qhd_ct.IdKhachHang,
						qhd_ct.IdNhanVien,
                        CASE
                            WHEN qhd_ct.HinhThucThanhToan = 1 THEN N'Tiền mặt'
                            WHEN qhd_ct.HinhThucThanhToan = 2 THEN N'Pos'
                            WHEN qhd_ct.HinhThucThanhToan = 3 THEN N'Chuyển khoản'
                            WHEN qhd_ct.HinhThucThanhToan = 4 THEN N'Thẻ giá trị'
                            WHEN qhd_ct.HinhThucThanhToan = 5 THEN N'Sử dụng điểm'
                            ELSE N''
                        END AS HinhThucThanhToan,
						qhd.TrangThai,
                        CASE WHEN qhd.TrangThai = 1 THEN N'Đã thanh toán' ELSE N'Đã hủy' END AS TxtTrangThai
                    FROM QuyHoaDon qhd
		            JOIN QuyHoaDon_ChiTiet qhd_ct ON qhd_ct.IdQuyHoaDon = qhd.id
                    JOIN DM_LoaiChungTu lct ON lct.id = qhd.IdLoaiChungTu
                    LEFT JOIN DM_KhoanThuChi ktc ON ktc.id = qhd_ct.IdKhoanThuChi
                    WHERE qhd.TenantId = @TenantId
                        AND (@IdChiNhanh ='' OR exists (select * from dbo.fnSplitstring(@IdChiNhanh) cn where qhd.IdChiNhanh= cn.GiaTri))
						and (@FromDate is null or qhd.NgayLapHoaDon > @FromDate)
						and (@ToDate is null or qhd.NgayLapHoaDon < @ToDate)
			            AND qhd.IsDeleted = 0
                        AND (ISNULL(@Filter, '') = ''
                            OR LOWER(CASE WHEN qhd_ct.IdKhoanThuChi IS NULL THEN qhd.MaHoaDon ELSE ktc.TenKhoanThuChi + qhd.MaHoaDon END) LIKE N'%' + LOWER(@Filter) + '%'
                            OR LOWER(ktc.TenKhoanThuChi) LIKE N'%'+LOWER(@Filter)+'%'
                            OR LOWER(CASE WHEN qhd_ct.HinhThucThanhToan = 1 THEN N'Tiền mặt'
                                            WHEN qhd_ct.HinhThucThanhToan = 2 THEN N'Pos'
                                            WHEN qhd_ct.HinhThucThanhToan = 3 THEN N'Chuyển khoản'
                                            WHEN qhd_ct.HinhThucThanhToan = 4 THEN N'Thẻ giá trị'
                                            WHEN qhd_ct.HinhThucThanhToan = 5 THEN N'Sử dụng điểm'
                                    ELSE N'' END) LIKE N'%'+LOWER(@Filter) +'%'
                            OR LOWER(CASE WHEN qhd.TrangThai = 1 THEN N'Đã thanh toán' ELSE N'Đã hủy' END) LIKE N'%' + LOWER(@Filter) + '%'
                            OR LOWER(ktc.TenKhoanThuChi) LIKE N'%' + LOWER(@Filter) + '%'
                            OR LOWER(lct.TenLoaiChungTu) LIKE N'%' + LOWER(@Filter) + '%'
                        )                    
					),
					tblView
					as(
						select dtOut.Id,
							dtOut.IdChiNhanh,
							dtOut.LoaiPhieu,
							dtOut.MaHoaDon,
							dtOut.NgayLapHoaDon,
							dtOut.TenKhoanThuChi,
							dtOut.TongTienThu,
							dtOut.TrangThai,
							dtOut.TxtTrangThai,
							dtOut.IdKhachHang,
							dtOut.IdNhanVien,
							(
							select 											
								HinhThucThanhToan + ', ' AS [text()]
							from data_cte dtInt 
							where dtOut.Id = dtInt.Id
							for xml path('')
							) sPhuongThuc
						from data_cte dtOut											
						group by dtOut.Id,
							dtOut.IdChiNhanh,
							dtOut.LoaiPhieu,
							dtOut.MaHoaDon,
							dtOut.NgayLapHoaDon,
							dtOut.TongTienThu,
							dtOut.TenKhoanThuChi,
							dtOut.TrangThai,
							dtOut.TxtTrangThai,
							dtOut.IdKhachHang,
							dtOut.IdNhanVien
						
					),
					count_cte
					as
					(
					select count(*) as TotalCount
					from tblView
					)
					select tbl.*,
						TotalCount,
						iif(tbl.IdKhachHang is not null, kh.TenKhachHang, iif(tbl.IdNhanVien is null, N'Khách lẻ', nv.TenNhanVien)) as TenNguoiNop,
						STUFF(sPhuongThuc,len(sPhuongThuc),1,'') as SHinhThucThanhToan  --- bỏ dấu phẩy ở cuối chuỗi
					from tblView tbl
					cross join count_cte
					left join DM_KhachHang kh on tbl.IdKhachHang = kh.Id
					left join NS_NhanVien nv on  tbl.IdNhanVien = nv.Id					
                    ORDER BY
                        CASE WHEN @SortBy = 'loaiPhieu' AND LOWER(@SortType) = 'asc' THEN LoaiPhieu END ASC,
                        CASE WHEN @SortBy = 'tongTienThu' AND LOWER(@SortType) = 'asc' THEN TongTienThu END ASC,
                        CASE WHEN @SortBy = 'maHoaDon' AND LOWER(@SortType) = 'asc' THEN MaHoaDon END ASC,
                        CASE WHEN @SortBy = 'phuongThucTT' AND LOWER(@SortType) = 'asc' THEN sPhuongThuc END ASC,
                        CASE WHEN @SortBy = 'trangThai' AND LOWER(@SortType) = 'asc' THEN tbl.TrangThai END ASC,                     
						CASE WHEN @SortBy = 'ngayLapHoaDon' AND LOWER(@SortType) = 'asc' THEN NgayLapHoaDon END asc,

                        CASE WHEN @SortBy = 'loaiPhieu' AND LOWER(@SortType) = 'desc' THEN LoaiPhieu END DESC,
                        CASE WHEN @SortBy = 'tongTienThu' AND LOWER(@SortType) = 'desc' THEN TongTienThu END DESC,
                        CASE WHEN @SortBy = 'maHoaDon' AND LOWER(@SortType) = 'desc' THEN MaHoaDon END DESC,
                        CASE WHEN @SortBy = 'phuongThucTT' AND LOWER(@SortType) = 'desc' THEN sPhuongThuc END DESC,
                        CASE WHEN @SortBy = 'trangThai' AND LOWER(@SortType) = 'desc' THEN tbl.TrangThai END DESC,

                        CASE WHEN @SortBy = 'ngayLapHoaDon' AND LOWER(@SortType) = 'desc' THEN NgayLapHoaDon END DESC

						OFFSET (@SkipCount* @MaxResultCount) ROWS
						FETCH NEXT @MaxResultCount ROWS ONLY
					
END;
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
