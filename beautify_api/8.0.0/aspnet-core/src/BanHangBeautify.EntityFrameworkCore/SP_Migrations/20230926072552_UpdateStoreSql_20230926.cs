using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20230926 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetNhatKyThanhToan_ofHoaDon]
	@IdHoaDonLienQuan uniqueidentifier 
AS
BEGIN
	
	SET NOCOUNT ON;

	select 
		qhd.*,
		iif(qhd.IdLoaiChungTu=11,N'Phiếu thu',N'Phiếu chi') as sLoaiPhieu,
		CASE WHEN qhd.TrangThai = 1 THEN N'Đã thanh toán' ELSE N'Đã hủy' END AS sTrangThai,
		STUFF(qct.sPhuongThucTT,len(qct.sPhuongThucTT),1,'') as sHinhThucThanhToan ----- (STUFF: xoa ki tu cuoi cung cua chuoi)
	from QuyHoaDon qhd
	join 
	(
		select qct.IdQuyHoaDon,
			(
			select		
				(case qctIn.HinhThucThanhToan
					when 1 then N'Tiền mặt'
					when 2 then N'Chuyển khoản'
					when 3 then N'Quyẹt thẻ'
					when 4 then N'Thẻ giá trị'
					when 5 then N'Sử dụng điểm'
				else ''
				end) + ', ' AS [text()]
			from QuyHoaDon_ChiTiet qctIn
			where IdHoaDonLienQuan= @IdHoaDonLienQuan and qctIn.IdQuyHoaDon = qct.IdQuyHoaDon
			For XML PATH ('') 
			) sPhuongThucTT 
		from QuyHoaDon_ChiTiet qct
		where IdHoaDonLienQuan= @IdHoaDonLienQuan
		group by qct.IdQuyHoaDon
	) qct on qhd.Id= qct.IdQuyHoaDon
END");

			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_SoQuy_GetAll]
                @TenantId INT = 3,
                @IdChiNhanh NVARCHAR(MAX) ='66AA813E-BB0F-4AEA-8729-44D391DDB567',
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
						qhd.IdLoaiChungTu,
                        lct.TenLoaiChungTu as LoaiPhieu,
						qhd.NgayLapHoaDon,
						qhd.MaHoaDon,                      
                        qhd.CreationTime,
						qhd.NoiDungThu,     
                        ktc.TenKhoanThuChi,
                        qhd.TongTienThu,
						qhd_ct.IdKhachHang,
						qhd_ct.IdNhanVien,
                        CASE
                            WHEN qhd_ct.HinhThucThanhToan = 1 THEN N'Tiền mặt'
                            WHEN qhd_ct.HinhThucThanhToan = 2 THEN N'Chuyển khoản'
                            WHEN qhd_ct.HinhThucThanhToan = 3 THEN N'Quẹt thẻ'
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
                                            WHEN qhd_ct.HinhThucThanhToan = 2 THEN N'Chuyển khoản'
                                            WHEN qhd_ct.HinhThucThanhToan = 3 THEN N'Quẹt thẻ'
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
							dtOut.IdLoaiChungTu,
							dtOut.NoiDungThu,
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
							dtOut.IdNhanVien,
							dtOut.IdLoaiChungTu,
							dtOut.NoiDungThu
						
					),
					count_cte
					as
					(
					select count(*) as TotalCount
					from tblView
					)
					select tbl.*,
						TotalCount,
						iif(tbl.IdKhachHang is not null, kh.SoDienThoai, iif(tbl.IdNhanVien is null, N'', nv.SoDienThoai)) as SDTNguoiNop,
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
					
END");

			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_khachHang_getAll]
					@TenantId INT,
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
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'createTime' THEN CreationTime END,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'createTime' THEN CreationTime END DESC,
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
