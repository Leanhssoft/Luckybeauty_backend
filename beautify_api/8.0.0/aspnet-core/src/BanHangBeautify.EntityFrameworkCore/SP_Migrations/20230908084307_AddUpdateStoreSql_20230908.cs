using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreSql20230908 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[GetDetailProduct]
	@IdDonViQuyDoi uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON;

	select 
		hh.Id,
		hh.IdLoaiHangHoa,
		hh.IdNhomHangHoa,
		Isnull(hh.Image,'') as Image,
		isnull(hh.SoPhutThucHien,0) as SoPhutThucHien,
		qd.Id as IdDonViQuyDoi,
		qd.MaHangHoa,
		qd.GiaBan,
		hh.TenHangHoa,
		loai.TenLoaiHangHoa,
		hh.MoTa,
		hh.TrangThai,	
		qd.LaDonViTinhChuan,
		qd.TyLeChuyenDoi,	
		qd.TenDonViTinh,
		isnull(nhom.TenNhomHang,N'Nhóm mặc định') as TenNhomHang,
		iif(hh.TrangThai =1, N'Đang kinh doanh',N'Ngừng kinh doanh') as TxtTrangThaiHang		
	from DM_HangHoa hh
	join DM_LoaiHangHoa loai on hh.IdLoaiHangHoa= loai.Id
	join DM_DonViQuiDoi qd on hh.Id = qd.IdHangHoa
	left join DM_NhomHangHoa nhom on hh.IdNhomHangHoa= nhom.Id
	where qd.Id = @IdDonViQuyDoi 
END");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_getKhachHang_Booking]
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
		isnull(kh.Avatar,'') as Avatar,
		qd.MaHangHoa,
		qd.GiaBan,
		hh.TenHangHoa,
		qd.Id as IdDonViQuyDoi,
		qd.IdHangHoa,
		hh.IdNhomHangHoa,
		hh.IdLoaiHangHoa
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
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListCustomerChecking]
	@TenantId int= 7,
	@TextSearch nvarchar(max)='',
	@CurrentPage int=0,
	@PageSize int = 50
AS
BEGIN
	
	SET NOCOUNT ON;


	if ISNULL(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch='%%'

	;with data_cte
	as(
	select
		checkin.Id, 
		checkin.Id as IdCheckIn, 
		checkin.TenantId,
		checkin.IdKhachHang,
		checkin.IdChiNhanh,
		checkin.DateTimeCheckIn,
		upper((select dbo.fnGetFirstCharOfString(kh.TenKhachHang))) as TenKhach_KiTuDau,
		kh.MaKhachHang, 
		kh.TenKhachHang, 
		kh.SoDienThoai,
		kh.TongTichDiem,
		kh.GioiTinhNam,
		nhom.TenNhomKhach,
		kh.IdNhomKhach, 
		isnull(kh.Avatar,'') as Avatar,
		FORMAT(checkin.DateTimeCheckIn,'dd/MM/yyyy') as DateCheckIn,
		FORMAT(checkin.DateTimeCheckIn,'hh:mm tt') as TimeCheckIn,
		N'Đang chờ' as TxtTrangThaiCheckIn
	from KH_CheckIn checkin
	join DM_KhachHang kh on checkin.IdKhachHang= kh.Id
	left join DM_NhomKhachHang nhom on kh.IdNhomKhach= nhom.Id	
	where checkin.TrangThai = 1
	and kh.TenantId= @TenantId	
	and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
		or TenKhachHang_KhongDau like @TextSearch
		or MaKhachHang like @TextSearch
		or TenKhachHang like @TextSearch
		or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
		or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
	)
	select *
	from data_cte
	order by DateCheckIn desc
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY 
END");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListHoaDon]
			@TenantId int =7,
			@IdChiNhanhs nvarchar(max)=null,
			@IdLoaiChungTus varchar(20)= null,
			@DateFrom datetime=null,
			@DateTo datetime=null,
			@TextSearch nvarchar(max)=null,
			@CurrentPage int= 1, ---- 1.call at DmHangHoa -- else seachHangHoa (at banhang)
			@PageSize int = 50,
			@ColumnSort varchar(50)='NgayLapHoaDon',
			@TypeSort varchar(5) = 'DESC'
		AS
		BEGIN
	
		SET NOCOUNT ON;
		set @CurrentPage= @CurrentPage - 1
	
		declare @tblLoaiChungTu table(ID varchar(40))
		if isnull(@IdLoaiChungTus,'') ='' set @IdLoaiChungTus =''
		else 
			insert into @tblLoaiChungTu
			select * from dbo.fnSplitstring(@IdLoaiChungTus)

		declare @tblChiNhanh table(ID varchar(40))
		if isnull(@IdChiNhanhs,'') ='' set @IdChiNhanhs =''
		else 
			insert into @tblChiNhanh
			select * from dbo.fnSplitstring(@IdChiNhanhs)

			
		declare @tblSearch table(Txt nvarchar(max))	
		if isnull(@TextSearch,'') =''
			begin
				set @TextSearch=''
			end
			else 
				set @TextSearch = N'%' + @TextSearch + '%'

		;with data_cte
		as
		(
		select 
			hd.*,			
			nvlap.UserName,
			kh.MaKhachHang,
			isnull(kh.TenKhachHang,N'Khách lẻ') as TenKhachHang,
			isnull(kh.Avatar,'') as Avatar,
			kh.SoDienThoai,
			nv.TenNhanVien,
			cn.TenChiNhanh,
			case hd.TrangThai
				when 1 then N'Tạm lưu'
				when 2 then N'Đang xử lý'
				when 3 then N'Hoàn thành'
			else N'Đã hủy' end as TxtTrangThaiHD
		from BH_HoaDon hd
		left join DM_KhachHang kh on hd.IdKhachHang = kh.Id
		left join DM_ChiNhanh cn on hd.IdChiNhanh = cn.Id
		left join NS_NhanVien nv on hd.IdNhanVien= nv.id
		left join AbpUsers nvlap on hd.CreatorUserId = nvlap.id
		where hd.TenantId = @TenantId
		and (@IdChiNhanhs =''  or exists (select * from @tblChiNhanh cn where hd.IdChiNhanh = cn.ID))
		and (@IdLoaiChungTus ='' or exists (select * from @tblLoaiChungTu ct where hd.IdLoaiChungTu = ct.ID))
		and (@DateFrom is null or hd.NgayLapHoaDon > @DateFrom)
		and (@DateTo is null or hd.NgayLapHoaDon < @DateTo)
		and (@TextSearch ='' or  
			(hd.MaHoaDon like @TextSearch or hd.GhiChuHD like @TextSearch 
			 or kh.MaKhachHang like @TextSearch or  kh.TenKhachHang like @TextSearch or kh.TenKhachHang_KhongDau like @TextSearch))
			),			
			tblThuChi 	as		
			(
				select qct.IdHoaDonLienQuan,
					sum(iif(qhd.IdLoaiChungTu=11, qct.TienThu, - qct.TienThu)) as DaThanhToan
				from QuyHoaDon_ChiTiet qct
				join QuyHoaDon qhd on qct.IdQuyHoaDon = qhd.Id
				where qhd.TrangThai= 1
				and exists (select * from data_cte hd where qct.IdHoaDonLienQuan = hd.Id)
				group by qct.IdHoaDonLienQuan
			),
			count_cte
			as
			(
				select  
					count(Id) as TotalRow,
					ceiling(count(Id)/ CAST(@PageSize as float)) as TotalPage,
					sum(TongTienHang) as SumTongTienHang,
					sum(TongGiamGiaHD) as SumTongGiamGiaHD,
					sum(TongThanhToan) as SumTongThanhToan
				from data_cte
			),
			sumSQ
			as
			(
				select  
					sum(DaThanhToan) as SumDaThanhToan
				from tblThuChi
			)
						
			select hd.*,
				isnull(sq.DaThanhToan,0)  as DaThanhToan,
				iif(hd.TrangThai=0,0,hd.TongThanhToan - isnull(sq.DaThanhToan,0)) as ConNo,
				TotalRow,
				TotalPage,
				SumTongTienHang,
				SumTongGiamGiaHD,
				SumTongThanhToan,
				SumDaThanhToan
			from data_cte hd
			left join tblThuChi sq on hd.Id= sq.IdHoaDonLienQuan
			cross join count_cte
			cross join sumSQ
			order by 
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='MaHoaDon' then MaHoaDon end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='MaHoaDon' then MaHoaDon end DESC,
			case when @TypeSort <> 'ASC' then ''
			when @ColumnSort='NgayLapHoaDon' then NgayLapHoaDon end ASC,
			case when @TypeSort <> 'DESC' then ''
			when @ColumnSort='NgayLapHoaDon' then NgayLapHoaDon end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='MaKhachHang' then MaKhachHang end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='MaKhachHang' then MaKhachHang end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='TenKhachHang' then TenKhachHang end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='TenKhachHang' then TenKhachHang end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='TongTienHang' then TongTienHang end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='TongTienHang' then TongTienHang end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='TongGiamGiaHD' then TongGiamGiaHD end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='TongGiamGiaHD' then TongGiamGiaHD end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='TongThanhToan' then TongThanhToan end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='TongThanhToan' then TongThanhToan end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='DaThanhToan' then DaThanhToan end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='DaThanhToan' then DaThanhToan end DESC
		OFFSET (@CurrentPage* @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY
END");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetInforHoaDon_byId]
	@Id uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON;

   select 
			hd.*,
			kh.MaKhachHang,
			isnull(kh.TenKhachHang,N'Khách lẻ') as TenKhachHang,
			kh.SoDienThoai,
			isnull(kh.Avatar,'') as Avatar,
			nv.TenNhanVien,
			cn.TenChiNhanh,
			hd.TongTienHDSauVAT - isnull(sq.DaThanhToan,0)  as ConNo,
			isnull(sq.DaThanhToan,0) as DaThanhToan,
			case hd.TrangThai
				when 1 then N'Tạm lưu'
				when 2 then N'Đang xử lý'
				when 3 then N'Hoàn thành'
			else N'Đã hủy' end as TxtTrangThaiHD
		from BH_HoaDon hd
		left join DM_KhachHang kh on hd.IdKhachHang = kh.Id
		left join DM_ChiNhanh cn on hd.IdChiNhanh = cn.Id
		left join NS_NhanVien nv on hd.IdNhanVien= nv.id
		left join
		(		
			select qct.IdHoaDonLienQuan,
					sum(iif(qhd.IdLoaiChungTu=11, qct.TienThu, - qct.TienThu)) as DaThanhToan
				from QuyHoaDon_ChiTiet qct
				join QuyHoaDon qhd on qct.IdQuyHoaDon = qhd.Id
				where qhd.TrangThai= 1
				and qct.IdHoaDonLienQuan = @Id
				group by qct.IdHoaDonLienQuan
				
		) sq on hd.id= sq.IdHoaDonLienQuan
		where hd.Id= @Id
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
						qhd.IdLoaiChungTu,
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

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spImportDanhMucHangHoa]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetQuyChiTiet_byIQuyHoaDon]");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spImportDanhMucHangHoa]
	@TenantId int = 1,
	@CreatorUserId int,
	@TenNhomHangHoa nvarchar(max)='',
	@MaHangHoa nvarchar(max)='',
	@TenHangHoa nvarchar(max)='',
	@IdLoaiHangHoa int = 2,
	@GiaBan float =0,
	@SoPhutThucHien float =0,
	@GhiChu nvarchar(max)=''
AS
BEGIN
	
	SET NOCOUNT ON;
	if(isnull(@GiaBan,'0')='') set @GiaBan ='0'
	declare @idNhomHangHoa uniqueidentifier = null

	if (ISNULL(@TenNhomHangHoa,'')!='') --- tên nhóm != rỗng
		begin
			select top 1 @idNhomHangHoa = Id from DM_NhomHangHoa where TenNhomHang like @TenNhomHangHoa and IsDeleted='0'
			if @idNhomHangHoa is null
				begin
					--- thêm nhóm mới nếu chưa tồn tại ---
					set @idNhomHangHoa = NEWID()
					insert into DM_NhomHangHoa (TenantId,Id, MaNhomHang, TenNhomHang, TenNhomHang_KhongDau, LaNhomHangHoa, CreationTime, CreatorUserId, IsDeleted)
					values(@TenantId, @idNhomHangHoa, 
						(select dbo.fnGetFirstCharOfString(@TenNhomHangHoa)), --- manhomhang: lay ki tu dau --
						@TenNhomHangHoa, 
						(select dbo.fnConvertStringToUnsign(@TenNhomHangHoa)), --- tenkhongdau --
						iif(@IdLoaiHangHoa !=1,'1','0'), 
						getdate(),
						@CreatorUserId,
						'0')
				end
		end

	------ DmHangHoa + Dm_DonViQuyDoi ---
	declare @tenHangHoa_KhongDau nvarchar(max) = (select dbo.fnConvertStringToUnsign(@TenHangHoa))
	declare @idHangHoa uniqueidentifier = newid(), @idQuyDoi uniqueidentifier = null
	declare @isUpdate bit ='0'
	if (ISNULL(@MaHangHoa,'')='') ---- nếu mã rỗng: phát sinh mã mới --
		begin
			declare @tblMaHangHoa table(MaLoaiHang varchar(5), MaxVal float)
			insert into @tblMaHangHoa
			exec dbo.SpGetProductCode @TenantId, @IdLoaiHangHoa

			declare @max float, @maloaihang varchar(5)
			select top 1 @maloaihang = MaLoaiHang, @max = MaxVal from @tblMaHangHoa

			set @MaHangHoa = concat(@maloaihang, iif(@max < 10, '0'+ @max, @max)) --- nếu mã <10: thêm số 0 ở đầu		
		end
	else
		begin		
			Set @MaHangHoa = UPPER(@MaHangHoa)
			select top 1 @idHangHoa = IdHangHoa, @idQuyDoi = Id from DM_DonViQuiDoi where MaHangHoa like @MaHangHoa and IsDeleted='0'
			if @idQuyDoi is not null set @isUpdate ='1'
		end

	if  @isUpdate ='0'
		begin
				insert into DM_HangHoa (TenantId, Id, IdLoaiHangHoa, IdNhomHangHoa, TenHangHoa, TenHangHoa_KhongDau, SoPhutThucHien, MoTa, TrangThai, IsDeleted, CreatorUserId, CreationTime)
				values (@TenantId, @idHangHoa, @IdLoaiHangHoa, @idNhomHangHoa, @TenHangHoa, @tenHangHoa_KhongDau, @SoPhutThucHien,	@GhiChu,			
					1,'0',@CreatorUserId, GETDATE())

				insert into DM_DonViQuiDoi (TenantId, Id, IdHangHoa, MaHangHoa, TyLeChuyenDoi, LaDonViTinhChuan, TenDonViTinh, GiaBan, CreatorUserId, CreationTime, IsDeleted)
				values (@TenantId, NEWID(), @idHangHoa, @MaHangHoa,1,1,'',@GiaBan, @CreatorUserId, GETDATE(), '0')
			
		end
	else
		begin
			update DM_HangHoa set IdLoaiHangHoa = @IdLoaiHangHoa,
								IdNhomHangHoa = @idNhomHangHoa,
								TenHangHoa = @TenHangHoa, 
								TenHangHoa_KhongDau = @tenHangHoa_KhongDau,
								SoPhutThucHien = @SoPhutThucHien,
								MoTa = @GhiChu,
								LastModifierUserId = @CreatorUserId,
								LastModificationTime =  GETDATE()					
			where Id = @idHangHoa

			update DM_DonViQuiDoi set MaHangHoa = @MaHangHoa,
								GiaBan = @GiaBan,
								LastModifierUserId = @CreatorUserId,
								LastModificationTime =  GETDATE()	
			where Id = @idQuyDoi
		end

  
END");
			migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetQuyChiTiet_byIQuyHoaDon]
	@IdQuyHoaDon uniqueidentifier 
AS
BEGIN
	
	SET NOCOUNT ON;

	select 
		qct.Id,
		qct.IdQuyHoaDon,
		qct.IdHoaDonLienQuan,
		qct.IdKhachHang,
		qct.IdNhanVien,
		qct.IdTaiKhoanNganHang,
		qct.IdKhoanThuChi,
		qct.HinhThucThanhToan,
		qct.TienThu,
		hd.MaHoaDon as MaHoaDonLienQuan		
	from QuyHoaDon_ChiTiet qct
	left join BH_HoaDon hd on qct.IdHoaDonLienQuan= hd.Id
	where qct.IdQuyHoaDon= @IdQuyHoaDon
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spImportDanhMucHangHoa]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetQuyChiTiet_byIQuyHoaDon]");
        }
    }
}
