using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreSql20240120 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetNhatKyHoatDong_ofKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetCustomerDetail_FullInfor]");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetNhatKyHoatDong_ofKhachHang]
	@IdKhachHang uniqueidentifier
AS
BEGIN


	
	SET NOCOUNT ON;

	select top 5 *
	from
	(
		select 					 
			DATEADD(second, DATEDIFF(second, bk.BookingDate, bk.StartTime), bk.BookingDate) as ThoiGian,
			concat(N'Đặt lịch hẹn ', hh.TenHangHoa) as HoatDong
		from Booking bk
		left join BookingService bkDV on bk.Id = bkDV.IdBooking
		left join DM_DonViQuiDoi qd on bkDV.IdDonViQuiDoi = qd.Id
		left join DM_HangHoa hh on qd.IdHangHoa = hh.Id
		where bk.IdKhachHang= @IdKhachHang
		and bk.IsDeleted='0'

		union all

		  select 
			hd.NgayLapHoaDon as ThoiGian,
			concat(N'Mua hàng với giá trị ', format(hd.TongTienHang, '#,###')) as HoatDong
		from BH_HoaDon hd
		where hd.IdKhachHang= @IdKhachHang
		and hd.IsDeleted='0' 
		and hd.TrangThai= 3 --- 1.tamluu, 2.dangxuly, 3.hoanthanh
	) tbl
	order by ThoiGian desc
END");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetCustomerDetail_FullInfor]
	@IdKhachHang uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON;

	select 
		cus.Id,
		cus.IdNhomKhach,
		cus.IdLoaiKhach,
		cus.IdNguonKhach,
		cus.MaKhachHang,
		cus.TenKhachHang,
		cus.Avatar,
		cus.SoDienThoai,
		cus.DiaChi,
		cus.NgaySinh,
		cus.TongTichDiem,
		cus.MaSoThue,
		loai.TenLoaiKhachHang,
		nguon.TenNguon as TenNguonKhach,
		isnull(nhom.TenNhomKhach,N'Nhóm mặc định') as TenNhomKhach,
		iif(cus.GioiTinhNam = 1, N'Nam',N'Nữ') as GioiTinh,	
		isnull(hd.TongChiTieu,0) as TongChiTieu,
		isnull(tblBooking.SoLanBooking,0) as SoLanBooking,
		isnull(hd.TongChiTieu,0)  - isnull(tblSoquy.TongThanhToan,0) as ConNo,		
		tblBooking.CuocHenGanNhat,
		isnull(tblCheckIn.SoLanCheckIn,0) as SoLanCheckIn							
	from DM_KhachHang cus
	left join DM_NhomKhachHang nhom on cus.IdNhomKhach = nhom.Id
	left join DM_LoaiKhach loai on cus.IdNhomKhach = nhom.Id
	left join DM_NguonKhach nguon on cus.IdNguonKhach = nguon.Id
	left join (
				SELECT IdKhachHang, SUM(TongTienHDSauVAT) as TongChiTieu
				FROM BH_HoaDon
				WHERE IsDeleted = 0 and IdKhachHang = @IdKhachHang
				GROUP BY IdKhachHang
	) hd ON hd.IdKhachHang = cus.Id
	left join
	(
		------ get thu/chi of customer ---
		select 
			qct.IdKhachHang,
			----- 11.thu/12.chi
			sum(iif(qhd.IdLoaiChungTu =11, qct.TienThu, - qct.TienThu)) as TongThanhToan
		from QuyHoaDon qhd
		join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
		where qhd.IsDeleted = 0
		and qct.IsDeleted = 0
		and qct.IdKhachHang =@IdKhachHang
		group by qct.IdKhachHang
	)tblSoquy on cus.Id = tblSoquy.IdKhachHang
	left join
	(
		select IdKhachHang,
			count(IdKhachHang) as SoLanBooking,
			max(BookingDate) as CuocHenGanNhat ---- lan checkin gannhat
		from Booking bk
		where bk.IsDeleted=0 
		and bk.TrangThai !=0 ---- trangthai=0: datlich, nhung sau do huy
		and bk.IdKhachHang =@IdKhachHang
		group by bk.IdKhachHang
	)tblBooking on cus.id = tblBooking.IdKhachHang
	left join (
		SELECT IdKhachHang, 			
			count(id) as SoLanCheckIn
		FROM KH_CheckIn
		WHERE IsDeleted = 0
		and IdKhachHang =@IdKhachHang
		GROUP BY IdKhachHang
	) tblCheckIn ON cus.Id = tblCheckIn.IdKhachHang		
	where cus.Id= @IdKhachHang
    
END");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_khachHang_getAll]
@TenantId INT,
@IdChiNhanh UNIQUEIDENTIFIER = null,
@Filter NVARCHAR(max) =N'',
@SkipCount INT = 1,
@MaxResultCount INT = 10,
@SortBy NVARCHAR(50)='CreateTime',
@SortType VARCHAR(4)='desc',
@IdNhomKhach UNIQUEIDENTIFIER = NULL
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
END;");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_lichSuDatLich]
	@TenantId INT = 1,
	@IdKhachHang UNIQUEIDENTIFIER ='66a7c340-8a01-4766-8791-4e071f9d884f',
	@SortBy nvarchar(50)='',
	@SortType nvarchar(4)='',
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS 
BEGIN

	select
		b.Id,		
		b.BookingDate, 
		b.TrangThai,		
		b.CreationTime,
		bs.IdDonViQuiDoi,
		hh.TenHangHoa,
		hh.SoPhutThucHien,	
		dvqd.GiaBan,
		nv.TenNhanVien, ----- NV này là NV khách chọn lúc book ---		
		concat(CONVERT(NVARCHAR(5), b.StartTime, 108) , ' - ', CONVERT(NVARCHAR(5), b.EndTime, 108)) as ThoiGianHen,
		CASE b.TrangThai
            WHEN 0 THEN N'Xóa'
			WHEN 1 THEN N'Đặt lịch'
			wHEN 2 THEN N'Đã xác nhận'
			wHEN 3 THEN N'Checkin'
			wHEN 4 THEN N'Hoàn thành'
            ELSE N''
        END AS TxtTrangThai	
	into #tempBook
	from booking b
	join BookingService bs on bs.IdBooking = b.id
	join DM_DonViQuiDoi dvqd on dvqd.id = bs.IdDonViQuiDoi
	join DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	left join BookingNhanVien bns on bns.IdBooking = b.id
	left join NS_NhanVien nv on nv.id = bns.IdNhanVien	
	WHERE b.IsDeleted = 0 
		AND b.TenantId= @TenantId AND b.IdKhachHang = @IdKhachHang
	order by
	CASE WHEN @SortBy = 'bookingDate' AND @SortType = 'desc' THEN BookingDate END DESC,
		CASE WHEN @SortBy = 'tenDichVu' AND @SortType = 'desc' THEN hh.TenHangHoa END DESC,
		CASE WHEN @SortBy = 'thoiGianThucHien' AND @SortType = 'desc' THEN hh.SoPhutThucHien END DESC,
		CASE WHEN @SortBy = 'thoiGianBatDau' AND @SortType = 'desc' THEN b.StartTime END DESC,
		CASE WHEN @SortBy = 'gia' AND @SortType = 'desc' THEN dvqd.GiaBan END DESC,
		CASE WHEN @SortBy = 'nhanVienThucHien' AND @SortType = 'desc' THEN nv.TenNhanVien END DESC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'desc' THEN b.TrangThai END DESC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'desc' THEN b.CreationTime END DESC,
		CASE WHEN @SortBy = 'bookingDate' AND @SortType = 'asc' THEN BookingDate END ASC,
		CASE WHEN @SortBy = 'tenDichVu' AND @SortType = 'asc' THEN hh.TenHangHoa END ASC,
		CASE WHEN @SortBy = 'thoiGianThucHien' AND @SortType = 'asc' THEN hh.SoPhutThucHien END ASC,
		CASE WHEN @SortBy = 'thoiGianBatDau' AND @SortType = 'asc' THEN b.StartTime END ASC,
		CASE WHEN @SortBy = 'gia' AND @SortType = 'asc' THEN dvqd.GiaBan END ASC,
		CASE WHEN @SortBy = 'nhanVienThucHien' AND @SortType = 'asc' THEN nv.TenNhanVien END ASC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'asc' THEN b.TrangThai END ASC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'asc' THEN b.CreationTime END ASC,
		CASE WHEN ISNULL(@SortBy,'') = '' THEN b.CreationTime END DESC	
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

	; with data_cte
	as
	(
		select 
			nvth.Idboooking, 
			nv.TenNhanVien
		from(
			----- get NV thực tế đã làm cho khách----
			select 
				bk.Id as Idboooking,
				th.IdNhanVien,
				ct.IdDonViQuyDoi
			from #tempBook bk
			left join Booking_CheckIn_HoaDon ckHD on bk.Id = ckHD.IdBooking
			left join BH_HoaDon hd on ckHD.IdHoaDon = hd.Id and hd.IsDeleted='0'
			left join BH_HoaDon_ChiTiet ct on hd.Id= ct.IdHoaDon and bk.IdDonViQuiDoi = ct.IdDonViQuyDoi and ct.IsDeleted='0'
			left join BH_NhanVienThucHien th on ct.Id = th.IdHoaDonChiTiet and th.IsDeleted='0'
		) nvth
		join NS_NhanVien nv on nvth.IdNhanVien = nv.Id
		
	),
	tblNVTH
	as(
		select distinct Idboooking,
			(select TenNhanVien + ', ' as [text()]
			from data_cte dtIn
			where dtIn.Idboooking = dtOut.Idboooking
			For XML PATH ('')
			) as NVThucHiens
		from data_cte dtOut
	),
	tblBookNvth
	as
	(
		select distinct dt.Idboooking, nvth.NVThucHiens
		from data_cte dt
		left join tblNVTH nvth on dt.Idboooking = nvth.Idboooking 
	)
	select bk.*, nv.NVThucHiens
	from #tempBook bk
	left join tblBookNvth nv on bk.Id= nv.Idboooking

	SELECT COUNT(Id) as TotalCount FROM #tempBook
	drop table #tempBook
END;");
            migrationBuilder.Sql(@"ALTER procedure [dbo].[prc_lichSuGiaoDich]
	@TenantId INT,
	@IdKhachHang UNIQUEIDENTIFIER,
	@SortBy nvarchar(50),
	@SortType nvarchar(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS
BEGIN
	
	set nocount on;

	----- get hd into temp table ---
		SELECT 
			HD.Id,
			hd.MaHoaDon,
			hd.NgayLapHoaDon,
			hd.TongTienHang,
			hd.TongGiamGiaHD,
			hd.TongTienHDSauVAT,
			hd.TongThanhToan,		
			hd.TrangThai,
			hd.CreationTime,
			isnull(sq.KhachDaTra,0) as KhachDaTra,
			hd.TongThanhToan - isnull(sq.KhachDaTra,0) as ConNo,
			CASE WHEN TrangThai = 0 THEN N'Xóa'
				WHEN TrangThai =1 THEN N'Tạm lưu'
				WHEN TrangThai =2 THEN N'Đang xử lý'
				WHEN TrangThai =3 THEN N'Hoàn thành'
				ELSE N'Tạm lưu'
			END AS TxtTrangThai
		into #tblHD
		from BH_HoaDon hd
		left join (			
			select 
				qct.IdHoaDonLienQuan,
				----- 11.thu/12.chi
				sum(iif(qhd.IdLoaiChungTu =11, qct.TienThu, - qct.TienThu)) as KhachDaTra
			from QuyHoaDon qhd
			join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
			where qhd.IsDeleted = 0
			and qct.IsDeleted =0
			and qct.IdKhachHang = @IdKhachHang
			group by qct.IdHoaDonLienQuan
		)sq on sq.IdHoaDonLienQuan = hd.Id
		WHERE TenantId = @TenantId 
		AND IdKhachHang = @IdKhachHang AND IsDeleted =0


	select *
	from #tblHD
	ORDER BY 
		CASE WHEN @SortBy = 'maHoaDon' AND @SortType = 'desc' THEN MaHoaDon END DESC,
		CASE WHEN @SortBy = 'ngayLapHoaDon' AND @SortType = 'desc' THEN NgayLapHoaDon END DESC,
		CASE WHEN @SortBy = 'tongTienHang' AND @SortType = 'desc' THEN TongTienHang END DESC,
		CASE WHEN @SortBy = 'tongGiamGia' AND @SortType = 'desc' THEN TongGiamGiaHD END DESC,
		CASE WHEN @SortBy = 'tongThanhToan' AND @SortType = 'desc' THEN TongThanhToan END DESC,
		CASE WHEN @SortBy = 'khachDaTra' AND @SortType = 'desc' THEN KhachDaTra END DESC,
		CASE WHEN @SortBy = 'conNo' AND @SortType = 'desc' THEN ConNo END DESC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'desc' THEN TrangThai END DESC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'desc' THEN CreationTime END DESC,
		CASE WHEN @SortBy = 'maHoaDon' AND @SortType = 'asc' THEN MaHoaDon END ASC,
		CASE WHEN @SortBy = 'ngayLapHoaDon' AND @SortType = 'asc' THEN NgayLapHoaDon END ASC,
		CASE WHEN @SortBy = 'tongTienHang' AND @SortType = 'asc' THEN TongTienHang END ASC,
		CASE WHEN @SortBy = 'tongGiamGia' AND @SortType = 'asc' THEN TongGiamGiaHD END ASC,
		CASE WHEN @SortBy = 'tongThanhToan' AND @SortType = 'asc' THEN TongThanhToan END ASC,
		CASE WHEN @SortBy = 'khachDaTra' AND @SortType = 'asc' THEN KhachDaTra END ASC,
		CASE WHEN @SortBy = 'conNo' AND @SortType = 'asc' THEN ConNo END ASC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'asc' THEN TrangThai END ASC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'asc' THEN CreationTime END ASC,
		CASE WHEN ISNULL(@SortBy,'') = '' THEN CreationTime END DESC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

	SELECT COUNT(Id) AS TotalCount FROM #tblHD;
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetNhatKyHoatDong_ofKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetCustomerDetail_FullInfor]");
        }
    }
}
