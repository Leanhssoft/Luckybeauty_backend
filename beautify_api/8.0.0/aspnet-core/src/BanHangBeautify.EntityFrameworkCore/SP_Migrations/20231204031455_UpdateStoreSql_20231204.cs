﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20231204 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListCustomer_byIdLoaiTin]	
	@IdLoaiTin int= 3,
	@IdChiNhanhs nvarchar(max)= 'C4FBE44F-C26E-499F-9033-AF9C4E3C6FC3',
	@TrangThais varchar(50)='', ---- 100.thanhcong,1.nhap, 0.chuagui,else.thatbai
	@HinhThucGuiTins varchar(10)='', ------ 1.sms, 2.zalo, 3.gmail
	@TextSearch nvarchar(max)= '',
	@FromDate datetime= '2023-01-01',
	@ToDate datetime= '2023-11-30',
	@CurrentPage int=1,
	@PageSize int = 50
AS
BEGIN
	
	SET NOCOUNT ON;
	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1;

	set @FromDate = FORMAT(@FromDate,'yyyy-MM-dd') ---- paramSearch khi gửi tin tư động: đang truyền đạng 12:00:AM
	set @ToDate = FORMAT(@ToDate,'yyyy-MM-dd')


	if ISNULL(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch='%%'

	declare @tblTrangThai table(TrangThai tinyint primary key)
	if isnull(@TrangThais,'')!=''
		begin
			insert into @tblTrangThai
			select GiaTri from dbo.fnSplitstring(@TrangThais)
		end
	else
		set @TrangThais=''		

	declare @tblHinhThucGuiTin table(HinhThucGui tinyint primary key)
	if isnull(@HinhThucGuiTins,'')!=''
		begin
			insert into @tblHinhThucGuiTin
			select GiaTri from dbo.fnSplitstring(@HinhThucGuiTins)
		end
	else
		set @HinhThucGuiTins=''		


	declare @tblChiNhanh table(ID uniqueidentifier primary key)
	if(isnull(@IdChiNhanhs,'')!='')
		insert into @tblChiNhanh
		select distinct GiaTri from dbo.fnSplitstring(@IdChiNhanhs)

	if(@IdLoaiTin=2) ---- khách hàng có sinh nhật từ - đến
		begin
			declare @birthday_dayFrom int = datepart(day,@FromDate) - 1, ---- trừ 1: để tránh >= dateFrom
					@birthday_dayTo int = datepart(day,@ToDate) + 1, ---- cộng 1: để tránh <= dateTo
					@birthday_monthFrom  int = datepart(month,@FromDate),
					@birthday_monthTo  int = datepart(month,@ToDate)


			if(@birthday_monthFrom!= @birthday_monthTo)
					begin
						----- lọc ngày sinh # tháng (vd: 28/09 -->22/11)	 -----
						; with data_cte
						as(
							select *
							from
							(
								select  kh.Id as IdKhachHang, 
										kh.Id,
										kh.MaKhachHang, 
										kh.TenKhachHang, 
										kh.SoDienThoai,
										kh.TongTichDiem,		
										kh.NgaySinh,
										kh.Email,
										datepart(day,NgaySinh) as NgaySinhNhat,
										datepart(month,NgaySinh) as ThangSinhNhat
								from DM_KhachHang kh								
								where kh.TrangThai in (0,1)
								and kh.NgaySinh is not null
								and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
									or TenKhachHang_KhongDau like @TextSearch
									or MaKhachHang like @TextSearch
									or TenKhachHang like @TextSearch
									or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
									or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
								) b
								where (b.ThangSinhNhat > @birthday_monthFrom and b.ThangSinhNhat < @birthday_monthTo)
								or (b.ThangSinhNhat = @birthday_monthFrom and b.NgaySinhNhat > @birthday_dayFrom)
								or (b.ThangSinhNhat = @birthday_monthTo and b.NgaySinhNhat < @birthday_dayTo)
						),
						tblFilterTrangThai
						as
						(
								select 
									tblLast.*								
								from
								(
									----- get all customer has birthday && get TrangThai
									select dt.*, 
										isnull(nk.TrangThai,0) as TrangThai,							
										nk.RN,
										nk.ThoiGianTu,
										nk.ThoiGianDen,
										iif(nk.IdKhachHang is null, N'Chưa gửi',nk.STrangThai) as STrangThaiGuiTinNhan		
									from data_cte dt
									left join
									(
								
										   select tblRN.*,								
											case tblRN.TrangThai
												when 0 then N'Chưa gửi'
												when 1 then N'Lưu nháp'
												when 100 then N'Đã gửi'
											else N'Gửi thất bại' end as STrangThai
										   from
										   (	
										   
												select tblHinhThuc.*,																					
														ROW_NUMBER() over (partition by IdKhachHang order by CreationTime desc) as RN
												from
												(
													select 
														nk.*
													from
													(
														select sms.IdKhachHang,
															sms.TrangThai,
															sms.CreationTime,
															sms.HinhThucGui,
															format(nky.ThoiGianTu,'yyyy-MM-dd') as ThoiGianTu ,
															format(nky.ThoiGianDen,'yyyy-MM-dd') as ThoiGianDen
														from HeThong_SMS sms
														join SMS_NhatKy_GuiTin nky on sms.Id = nky.IdHeThongSMS											
														where sms.IdLoaiTin= 2 													
														and exists (select * from data_cte dt where sms.IdKhachHang = dt.IdKhachHang)		
													) nk
												--	where nk.ThoiGianTu <= @ToDate and nk.ThoiGianDen >= @FromDate														
												) tblHinhThuc
												where (@HinhThucGuiTins ='' 
												or exists (select HinhThucGui from @tblHinhThucGuiTin ht where ht.HinhThucGui = tblHinhThuc.HinhThucGui))
											) tblRN where tblRN.RN= 1 
											
								)nk on dt.IdKhachHang = nk.IdKhachHang
							) tblLast						
							where (@TrangThais ='' or exists (select TrangThai from @tblTrangThai tt where tt.TrangThai  = tblLast.TrangThai))	----- neu gui tin tudong: chi get khach chua duoc gui ----					

						),
						count_cte
						as
						(
							select count(Id) as TotalRow
							from tblFilterTrangThai
						)
						select kh.*,
							customerZalo.ZOAUserId,
							TotalRow
						from tblFilterTrangThai kh
						left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHang = customerZalo.IdKhachHang
						cross join count_cte
						order by MaKhachHang desc
						OFFSET (@CurrentPage* @PageSize) ROWS
						FETCH NEXT @PageSize ROWS ONLY
							
					end
				else
					begin
							----- lọc ngày sinh cùng thang  ------		
						; with data_cte
						as(							
							select *
							from
							(
								select  kh.Id as IdKhachHang,
										kh.Id,
										kh.MaKhachHang, 
										kh.TenKhachHang,
										kh.SoDienThoai,
										kh.NgaySinh,
										kh.TongTichDiem,		
										kh.Email,
										datepart(day,NgaySinh) as NgaySinhNhat,
										datepart(month,NgaySinh) as ThangSinhNhat
								from DM_KhachHang kh							
								where kh.TrangThai in (0,1)
								and kh.NgaySinh is not null
								and (SoDienThoai like @TextSearch 
									or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
									or TenKhachHang_KhongDau like @TextSearch
									or MaKhachHang like @TextSearch
									or TenKhachHang like @TextSearch
									or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
									or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
								) b
								where b.ThangSinhNhat = @birthday_monthFrom
								and b.NgaySinhNhat between (@birthday_dayFrom + 1) and @birthday_dayTo
							),
							tblFilterTrangThai
							as
							(
							select 
									tblLast.*								
								from
								(
									----- get all customer has birthday && get TrangThai
									select dt.*, 
										isnull(nk.TrangThai,0) as TrangThai,				
										nk.RN,
										iif(nk.IdKhachHang is null, N'Chưa gửi',nk.STrangThai) as STrangThaiGuiTinNhan		
									from data_cte dt
									left join
									(
								
										   select tbl.*,								
											case tbl.TrangThai
												when 0 then N'Chưa gửi'
												when 1 then N'Lưu nháp'
												when 100 then N'Đã gửi'
											else N'Gửi thất bại' end as STrangThai
										   from
										   (	
												select 
													tblHinhThuc.*,
													ROW_NUMBER() over (partition by IdKhachHang order by CreationTime desc) as RN	
												from
												(
													select 
														nk.*
													from
													(
														select  sms.IdKhachHang,
															sms.TrangThai,
															sms.CreationTime,
															sms.HinhThucGui,
															format(nky.ThoiGianTu,'yyyy-MM-dd') as ThoiGianTu ,
															format(nky.ThoiGianDen,'yyyy-MM-dd') as ThoiGianDen
														from HeThong_SMS sms
														join SMS_NhatKy_GuiTin nky on sms.Id = nky.IdHeThongSMS											
														where sms.IdLoaiTin= 2 
														and exists (select * from data_cte dt where sms.IdKhachHang = dt.IdKhachHang)		
													) nk
													where nk.ThoiGianTu <= @ToDate and nk.ThoiGianDen >= @FromDate		
												) tblHinhThuc
												where (@HinhThucGuiTins =''
												or exists (select HinhThucGui from @tblHinhThucGuiTin ht where ht.HinhThucGui = tblHinhThuc.HinhThucGui))
											) tbl where RN= 1
												
								)nk on dt.IdKhachHang = nk.IdKhachHang
							) tblLast						
							where (@TrangThais ='' or exists (select TrangThai from @tblTrangThai tt where tt.TrangThai  = tblLast.TrangThai))
							),
							count_cte
							as(
								select count(IdKhachHang) as TotalRow
								from tblFilterTrangThai
							)
							select kh.*,
								customerZalo.ZOAUserId,
								TotalRow
							from tblFilterTrangThai kh
							left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHang = customerZalo.IdKhachHang
							cross join count_cte
							order by MaKhachHang desc
							OFFSET (@CurrentPage* @PageSize) ROWS
							FETCH NEXT @PageSize ROWS ONLY
					end	
		end

	if(@IdLoaiTin=3) ---- khách hàng có lịch hẹn từ - đến
		begin
			set @ToDate = DATEADD(day,1,@ToDate)
			; with data_cte
			as(
					select 
						kh.Id as IdKhachHang, 
						kh.MaKhachHang,
						kh.TenKhachHang,
						kh.SoDienThoai,
						kh.Email,
						bk.Id,
						bk.BookingDate,
						bk.ThoiGianHen,
						qd.MaHangHoa,
						hh.TenHangHoa
					from
					(
					select bk.Id,
						bk.IdKhachHang, 
						bk.BookingDate,
						concat(FORMAT(bk.StartTime,'HH:mm'), ' - ', FORMAT(bk.EndTime,'HH:mm')) as ThoiGianHen
					from Booking bk
					where bk.TrangThai in (1,2) --- datlich/daxacnhan
					and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where bk.IdChiNhanh= cn.ID))
					and bk.BookingDate between @FromDate and @ToDate
					)bk
					join BookingService bkdv on bk.Id = bkdv.IdBooking
					join DM_DonViQuiDoi qd on bkdv.IdDonViQuiDoi = qd.Id
					join DM_HangHoa hh on qd.IdHangHoa = hh.Id
					join DM_KhachHang kh on bk.IdKhachHang= kh.Id
					where kh.TrangThai in (0,1)
						and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
							or TenKhachHang_KhongDau like @TextSearch
							or MaKhachHang like @TextSearch
							or TenKhachHang like @TextSearch
							or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
							or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI
							or hh.TenHangHoa like @TextSearch COLLATE Vietnamese_CI_AI
							or qd.MaHangHoa like @TextSearch COLLATE Vietnamese_CI_AI)
					),
				tblFilterTrangThai
				as
				(				
					select *
					from
					(
						select dt.*,
							isnull(nk.TrangThai,0) as TrangThai,
							iif(nk.IdKhachHang is null, N'Chưa gửi',nk.STrangThai) as STrangThaiGuiTinNhan		
						from data_cte dt				
						left join
						(
							select 
								smsOut.IdKhachHang,
								smsOut.IdBooking,
								smsOut.TrangThai,
								case smsOut.TrangThai
										when 1 then N'Lưu nháp'
										when 100 then N'Đã gửi'
								else N'Gửi thất bại' end as STrangThai
							from
							(
								select tblHinhThuc.*,
										---- nếu gửi nhiều lần - chỉ get lần gửi cuối cùng
										row_number() over (partition by tblHinhThuc.IdBooking order by tblHinhThuc.ThoiGianGui desc) as RN
								from
								(
									----- get khach hang co lich hen trong khoang thoi gian ----
									select 
										sms.IdKhachHang,
										sms.TrangThai,
										sms.ThoiGianGui,
										nky.IdBooking,
										sms.HinhThucGui										
									from HeThong_SMS sms
									join SMS_NhatKy_GuiTin nky on sms.Id = nky.IdHeThongSMS
									where sms.IdLoaiTin= 3 
										and exists (select * from data_cte dt 
													where sms.IdKhachHang = dt.IdKhachHang 
													and dt.BookingDate between nky.ThoiGianTu and nky.ThoiGianDen
													)
								)tblHinhThuc
								where (@HinhThucGuiTins =''
									or exists (select HinhThucGui from @tblHinhThucGuiTin ht where ht.HinhThucGui = tblHinhThuc.HinhThucGui))
							) smsOut
							where smsOut.RN = 1										
						)nk on dt.Id= nk.IdBooking
					) tblLast where (@TrangThais ='' or exists (select TrangThai from @tblTrangThai tt where tt.TrangThai  = tblLast.TrangThai))
				),
				count_cte
				as
				(
					select count(Id) as TotalRow
					from tblFilterTrangThai
				)
				select kh.*,
					customerZalo.ZOAUserId,
					TotalRow
				from tblFilterTrangThai kh
				left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHang = customerZalo.IdKhachHang
				cross join count_cte
				order by BookingDate desc
				OFFSET (@CurrentPage* @PageSize) ROWS
				FETCH NEXT @PageSize ROWS ONLY
		end

		if(@IdLoaiTin=4)
		begin
			---- tin giao dich ---
			; with data_cte
			as(
					select 
						kh.Id as IdKhachHang, 
						kh.MaKhachHang,
						kh.TenKhachHang,
						kh.SoDienThoai,
						kh.Email,
						hd.Id,
						hd.MaHoaDon,
						hd.NgayLapHoaDon
					from
					(
					select 
						hd.Id,
						hd.IdKhachHang,
						hd.MaHoaDon,
						hd.NgayLapHoaDon
					from BH_HoaDon hd
					where hd.TrangThai=3 -- hoanthanh
					and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where hd.IdChiNhanh= cn.ID))
					and hd.NgayLapHoaDon between @FromDate and @ToDate
					) hd 
					join DM_KhachHang kh on hd.IdKhachHang= kh.Id
					where kh.TrangThai in (0,1)
						and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
							or TenKhachHang_KhongDau like @TextSearch
							or MaKhachHang like @TextSearch
							or TenKhachHang like @TextSearch
							or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
							or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
				),
				tblFilterTrangThai
				as
				(				
						select *
						from
						(
							select 
								dt.*,
								isnull(nk.TrangThai,0) as TrangThai,
								iif(nk.IdKhachHang is null, N'Chưa gửi',nk.STrangThai) as STrangThaiGuiTinNhan	
							from data_cte dt
							left join
							(
								select 
									smsOut.IdKhachHang,
									smsOut.IdHoaDon,
									smsOut.TrangThai,
									case smsOut.TrangThai
											when 1 then N'Lưu nháp'
											when 100 then N'Đã gửi'
									else N'Gửi thất bại' end as STrangThai
								from
								(
									select 
										tblHinhThuc.*,
										---- chỉ get lần gửi cuối cùng	
										ROW_NUMBER() over (partition by tblHinhThuc.IdHoaDon order by tblHinhThuc.ThoiGianGui desc) as RN
									from
									(
									select 
										sms.IdKhachHang,
										sms.TrangThai,
										sms.ThoiGianGui,
										sms.HinhThucGui,
										nky.IdHoaDon									
									from HeThong_SMS sms
									join SMS_NhatKy_GuiTin nky on sms.Id = nky.IdHeThongSMS
									where sms.IdLoaiTin= 4 
									and exists (select * from data_cte dt 
										where sms.IdKhachHang = dt.IdKhachHang 
										and dt.NgayLapHoaDon between nky.ThoiGianTu and nky.ThoiGianDen)
									)tblHinhThuc
									where (@HinhThucGuiTins =''
									or exists (select HinhThucGui from @tblHinhThucGuiTin ht where ht.HinhThucGui = tblHinhThuc.HinhThucGui))
								) smsOut
								where smsOut.RN = 1
							----- neu gui tin tudong: chi get khach chua duoc gui ----					
						)nk on dt.Id = nk.IdHoaDon
					) tblLast where (@TrangThais ='' or exists (select TrangThai from @tblTrangThai tt where tt.TrangThai  = tblLast.TrangThai))
				)
				,count_cte
				as(
					select count(Id) as TotalRow from tblFilterTrangThai
				)
				select kh.*,
					customerZalo.ZOAUserId,
					TotalRow
				from tblFilterTrangThai kh
				left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHang = customerZalo.IdKhachHang
				cross join count_cte
				order by NgayLapHoaDon desc			
				OFFSET (@CurrentPage* @PageSize) ROWS
				FETCH NEXT @PageSize ROWS ONLY
		end
    
END");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spJqAutoCustomer]
	@TenantId int= 3,
	@LoaiDoiTuong int= 1,
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
	select  kh.Id, kh.TenantId, kh.MaKhachHang, kh.TenKhachHang, kh.SoDienThoai,
		 kh.TongTichDiem,
		 kh.GioiTinhNam,
		nhom.TenNhomKhach
	from DM_KhachHang kh
	left join DM_NhomKhachHang nhom on kh.IdNhomKhach= nhom.Id	
	where kh.TrangThai in (0,1)
	and kh.TenantId= @TenantId
	and IdLoaiKhach in ( @LoaiDoiTuong,0)
	and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
		or TenKhachHang_KhongDau like @TextSearch
		or MaKhachHang like @TextSearch
		or TenKhachHang like @TextSearch
		or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
		or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
	)
	select kh.*,
		customerZalo.ZOAUserId
	from data_cte kh
	left join Zalo_KhachHangThanhVien customerZalo on kh.Id = customerZalo.IdKhachHang
	order by MaKhachHang
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
END");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spJqAutoCustomer_byIdLoaiTin]	
	@IdLoaiTin int= 2,
	@IdChiNhanhs nvarchar(max)= 'c4fbe44f-c26e-499f-9033-af9c4e3c6fc3',
	@TextSearch nvarchar(max)= '09',
	@FromDate datetime= '2023-11-06',
	@ToDate datetime= '2023-11-12',
	@CurrentPage int=0,
	@PageSize int = 50
AS
BEGIN
	
	SET NOCOUNT ON;

	if ISNULL(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch='%%'

	declare @tblChiNhanh table(ID uniqueidentifier primary key)
	if(isnull(@IdChiNhanhs,'')!='')
		insert into @tblChiNhanh
		select distinct GiaTri from dbo.fnSplitstring(@IdChiNhanhs)

	if(@IdLoaiTin=2) ---- khách hàng có sinh nhật từ - đến
		begin
			declare @birthday_dayFrom int = datepart(day,@FromDate) - 1, ---- trừ 1: để tránh >= dateFrom
					@birthday_dayTo int = datepart(day,@ToDate) + 1, ---- cộng 1: để tránh <= dateTo
					@birthday_monthFrom  int = datepart(month,@FromDate),
					@birthday_monthTo  int = datepart(month,@ToDate)
		
			if(@birthday_monthFrom!= @birthday_monthTo)
					begin
						----- lọc ngày sinh # tháng (vd: 28/09 -->22/11)	 -----

							select b.*,
								customerZalo.ZOAUserId
							from
							(
								select  kh.Id as IdKhachHang, 
										kh.MaKhachHang, 
										kh.TenKhachHang, 
										kh.SoDienThoai,
										kh.TongTichDiem,
										kh.Email,
										datepart(day,NgaySinh) as NgaySinhNhat,
										datepart(month,NgaySinh) as ThangSinhNhat
								from DM_KhachHang kh								
								where kh.TrangThai in (0,1)
								and kh.NgaySinh is not null
								and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
									or TenKhachHang_KhongDau like @TextSearch
									or MaKhachHang like @TextSearch
									or TenKhachHang like @TextSearch
									or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
									or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
								) b
								left join Zalo_KhachHangThanhVien customerZalo on b.IdKhachHang = customerZalo.IdKhachHang
								where (b.ThangSinhNhat > @birthday_monthFrom and b.ThangSinhNhat < @birthday_monthTo)
								or (b.ThangSinhNhat = @birthday_monthFrom and b.NgaySinhNhat > @birthday_dayFrom)
								or (b.ThangSinhNhat = @birthday_monthTo and b.NgaySinhNhat < @birthday_dayTo)
							
					end
				else
					begin
							----- lọc ngày sinh cung  ------		
							select b.*,
								customerZalo.ZOAUserId
							from
							(
								select  kh.Id as IdKhachHang,
										kh.MaKhachHang, 
										kh.TenKhachHang,
										kh.SoDienThoai,
										kh.TongTichDiem,		
										kh.Email,
										datepart(day,NgaySinh) as NgaySinhNhat,
										datepart(month,NgaySinh) as ThangSinhNhat
								from DM_KhachHang kh													
								where kh.TrangThai in (0,1)
								and kh.NgaySinh is not null
								and (SoDienThoai like @TextSearch 
									or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
									or TenKhachHang_KhongDau like @TextSearch
									or MaKhachHang like @TextSearch
									or TenKhachHang like @TextSearch
									or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
									or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
								) b
								left join Zalo_KhachHangThanhVien customerZalo on b.IdKhachHang = customerZalo.IdKhachHang
								where b.ThangSinhNhat = @birthday_monthFrom
								and b.NgaySinhNhat between (@birthday_dayFrom + 1) and @birthday_dayTo
							
					end	
		end

	if(@IdLoaiTin=3) ---- khách hàng có lịch hẹn từ - đến
		begin
			set @ToDate = DATEADD(day,1,@ToDate)

			select distinct
				kh.Id as IdKhachHang, 
				kh.MaKhachHang,
				kh.TenKhachHang,
				kh.SoDienThoai,
				kh.Email,
				customerZalo.ZOAUserId,
				isnull(kh.TongTichDiem,0) as TongTichDiem
			from
			(
			select bk.IdKhachHang
			from Booking bk
			where bk.TrangThai in (1,2) --- datlich/daxacnhan
			and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where bk.IdChiNhanh= cn.ID))
			and bk.BookingDate between @FromDate and @ToDate
			)bk
			join DM_KhachHang kh on bk.IdKhachHang= kh.Id
			left join Zalo_KhachHangThanhVien customerZalo on kh.Id = customerZalo.IdKhachHang
			where kh.TrangThai in (0,1)
				and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
					or TenKhachHang_KhongDau like @TextSearch
					or MaKhachHang like @TextSearch
					or TenKhachHang like @TextSearch
					or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
					or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
		end

		if(@IdLoaiTin=4)
		begin
			---- tin giao dich ---
			set @ToDate = DATEADD(day,1,@ToDate)
			select distinct
				kh.Id as IdKhachHang, 
				kh.MaKhachHang,
				kh.TenKhachHang,
				kh.SoDienThoai,
				kh.Email,
				customerZalo.ZOAUserId,
				isnull(kh.TongTichDiem,0) as TongTichDiem
			from
			(
			select 
				hd.IdKhachHang
			from BH_HoaDon hd
			where hd.TrangThai=3 -- hoanthanh
			and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where hd.IdChiNhanh= cn.ID))
			and hd.NgayLapHoaDon between @FromDate and @ToDate
			) hd 
			join DM_KhachHang kh on hd.IdKhachHang= kh.Id
			left join Zalo_KhachHangThanhVien customerZalo on kh.Id = customerZalo.IdKhachHang
			where kh.TrangThai in (0,1)
				and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
					or TenKhachHang_KhongDau like @TextSearch
					or MaKhachHang like @TextSearch
					or TenKhachHang like @TextSearch
					or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
					or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
		end
    
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
