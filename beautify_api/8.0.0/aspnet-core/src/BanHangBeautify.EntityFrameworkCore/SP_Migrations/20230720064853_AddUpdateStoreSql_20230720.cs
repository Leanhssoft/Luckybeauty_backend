using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreSql20230720 : Migration
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
				(case qct.HinhThucThanhToan
					when 1 then N'Tiền mặt'
					when 2 then N'Chuyển khoản'
					when 3 then N'Quyẹt thẻ'
					when 4 then N'Thẻ giá trị'
					when 5 then N'Sử dụng điểm'
				else ''
				end) + ', ' AS [text()]
			from QuyHoaDon_ChiTiet qct
			where IdHoaDonLienQuan= @IdHoaDonLienQuan
			For XML PATH ('') 
			) sPhuongThucTT 
		from QuyHoaDon_ChiTiet qct
		where IdHoaDonLienQuan= @IdHoaDonLienQuan
		group by qct.IdQuyHoaDon
	) qct on qhd.Id= qct.IdQuyHoaDon
END");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_SoQuy_GetAll]
                @TenantId INT = 7,
                @IdChiNhanh NVARCHAR(MAX) ='ecf5ec7a-15e6-4b42-9b97-ed84dcbf6d05',
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
					
END;");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_getKhachHang_Booking]
	@TenantId int= 7,
	@IdChiNhanhs nvarchar(max)= null,
	@TextSearch nvarchar(max) =null,
	@CurrentPage int =0,
	@PageSize int = 10,
	@TrangThaiBook int =3  ---0.xoa, 3.all
AS
BEGIN
	
	SET NOCOUNT ON;

	if(isnull(@TextSearch,'')!='') set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch=''

	declare @tblChiNhanh table (ID uniqueidentifier)
	if(isnull(@IdChiNhanhs,'') !='')
		insert into @tblChiNhanh
		select GiaTri from  dbo.fnSplitstring(@IdChiNhanhs)
	else
		set @IdChiNhanhs=''
		

	; with data_cte
	as
	(
    select 
		bk.id as IdBooking,
		bk.IdKhachHang,
		bk.TrangThai,
		bk.StartTime,
		bk.EndTime,
		bk.BookingDate,
		case bk.TrangThai
			when 1 then N'Chưa xác nhận'
			when 2 then N'Đã xác nhận'
			when 3 then N'Đã check in'
		else '' end as TxtTrangThaiBook,
		kh.MaKhachHang,
		kh.TenKhachHang,
		kh.SoDienThoai,
		qd.MaHangHoa,
		qd.GiaBan,
		hh.TenHangHoa

	from Booking bk
	left join DM_KhachHang kh on bk.IdKhachHang = kh.Id
	left join BookingService bkS on bk.Id= bkS.IdBooking
	left join DM_DonViQuiDoi qd on bkS.IdDonViQuiDoi= qd.Id
	left join DM_HangHoa hh on qd.IdHangHoa= hh.Id
	left join BookingNhanVien bkN on bk.Id = bkN.IdNhanVien
	left join NS_NhanVien nv on bkN.IdNhanVien = nv.Id
	where bk.TenantId = @TenantId
	and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where bk.IdChiNhanh= cn.ID))
	and (@TrangThaiBook = 3 or  bk.TrangThai = @TrangThaiBook)
	and not exists (select id from Booking_CheckIn_HoaDon bkHD where bk.Id = bkHD.IdBooking) ---khong lay khach dang checkin 
	and (@TextSearch =''
	 or kh.MaKhachHang like @TextSearch
	 or kh.TenKhachHang  like @TextSearch
	 or kh.TenKhachHang_KhongDau  like @TextSearch
	 or kh.SoDienThoai like @TextSearch
	 or qd.MaHangHoa like @TextSearch
	 or hh.TenHangHoa like @TextSearch
	 or hh.TenHangHoa_KhongDau like @TextSearch
	 )
	 ),
	 count_cte
	 as(
	 select count(IdBooking) as TotalRow
	 from data_cte
	 )
	 select *
	 from data_cte 
	 cross join count_cte
	 order by BookingDate
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY

END");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_getKhachHang_noBooking]
		@TenantId INT = 7,
		@Filter NVARCHAR(max) =N'xx',
		@SkipCount INT = 0,
		@MaxResultCount INT = 10
	AS
	BEGIN	
	;with data_cte
	as
	(
	
		SELECT 
			kh.Id,
			kh.MaKhachHang,
			kh.TenKhachHang,
			kh.Avatar,
			kh.SoDienThoai,
			kh.DiaChi,
			kh.NgaySinh,
			kh.IdNguonKhach,
			kh.GioiTinhNam,
			kh.TongTichDiem,		
			ISNULL(tblCheckIn.CuocHenGanNhat,kh.CreationTime) AS CuocHenGanNhat,
			isnull(tblCheckIn.SoLanCheckIn,0) as SoLanCheckIn,
			isnull(tblChecking.TrangThaiCheckIn,0) as TrangThaiCheckIn			
		FROM DM_KhachHang kh			
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
		and not exists
			(
			---- khong lay khachhang da booking, nhung chua tontai trong bang bkHoadon
			select bk.Id from Booking bk where bk.IdKhachHang = kh.Id 
				and not exists (select id from Booking_CheckIn_HoaDon bkHD where bk.Id = bkHD.IdBooking)
			)
		AND (ISNULL(@Filter,'') = ''
			OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(kh.TenKhachHang_KhongDau) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(kh.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(kh.DiaChi) LIKE N'%' + LOWER(@Filter) + N'%'			
		)
	)
	select *
	from data_cte
	ORDER BY MaKhachHang desc		
	OFFSET (@SkipCount* @MaxResultCount) ROWS
	FETCH NEXT @MaxResultCount ROWS ONLY

	
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[prc_getKhachHang_noBooking]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[prc_getKhachHang_Booking]");
        }
    }
}
