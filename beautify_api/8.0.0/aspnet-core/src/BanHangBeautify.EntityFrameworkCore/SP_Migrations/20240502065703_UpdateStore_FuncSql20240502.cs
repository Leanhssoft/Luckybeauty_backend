﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreFuncSql20240502 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS [dbo].[fnGetBookingCode]");
			migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[fnGetBookingCode]
(
	@IdChiNhanh uniqueidentifier= null,
	@IdLoaiChungTu int= 1
)
RETURNS nvarchar(100)
AS
BEGIN
	-- Declare the return variable here
	declare @strReturn nvarchar(100)
	DECLARE @maxBookingCode float = 1
	declare @kihieuchungtu varchar(10),  @lenMaChungTu int =0

	set @kihieuchungtu = (select top 1 MaLoaiChungTu from DM_LoaiChungTu where ID= @IdLoaiChungTu)
	set @lenMaChungTu  = LEN(@kihieuchungtu)

	if isnull(@kihieuchungtu,'') ='' set @kihieuchungtu ='BK'

	SET @maxBookingCode = 
		(
			select  
				max(CAST(dbo.fnGetNumeric(RIGHT(BookingCode,LEN(BookingCode)- @lenMaChungTu))AS float)) as MaxNumber
			from
			(
				select BookingCode
				from dbo.Booking				
				where BookingCode like @kihieuchungtu +'%'			
			) a
			where ISNUMERIC(RIGHT(BookingCode,LEN(BookingCode)- @lenMaChungTu)) = 1
		)

	 if @maxBookingCode is null
		  RETURN  CONCAT(@kihieuchungtu, '001')		
	else
		begin
			set @maxBookingCode = FORMAT(@maxBookingCode + 1, 'F0') --- convert dạng mũ 10 về string
			declare @lenMaMax int = len(@maxBookingCode)

			if @maxBookingCode < 10  
				set @strReturn= CONCAT(@kihieuchungtu,'00',@maxBookingCode)
			else
				begin
					if @maxBookingCode < 100  
						set @strReturn=  CONCAT(@kihieuchungtu,'0',@maxBookingCode)
					else
						set @strReturn=  CONCAT(@kihieuchungtu,@maxBookingCode)
				end

		end

	return @strReturn
END");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListCustomer_byIdLoaiTin]	
	@IdLoaiTin int= 4,
	@IdChiNhanhs nvarchar(max)= '2324f320-30f3-4182-be92-e6d11b107601',
	@TrangThais varchar(50)='', ---- 100.thanhcong,1.nhap, 0.chuagui,else.thatbai
	@HinhThucGuiTins varchar(10)='2', ------ 1.sms, 2.zalo, 3.gmail
	@IsFilterCustomer bit ='1', ----- 1. nếu chỉ tìm kiếm khách hàng (sử dụng khi jqAutoCustomer)
	@LoaiUser_CoTheGuiTin tinyint = 2, ---- 0.all, 1.sms, 2.zalo, 3.gmail (vd: loai= 2: chỉ lấy khách hàng có tài khoản zalo)
	@TextSearch nvarchar(max)= '',
	@FromDate datetime= '2024-04-01',
	@ToDate datetime= '2024-05-27',
	@CurrentPage int=0,
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

	declare @tblTrangThai table(TrangThai int primary key)
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
										kh.Avatar,
										kh.IdKhachHangZOA,
										customerZalo.ZOAUserId,
										case kh.GioiTinhNam											
											when '1' then N'anh'
											when '0' then N'chị'
										else N'quý khách' end as XungHo,
										datepart(day,NgaySinh) as NgaySinhNhat,
										datepart(month,NgaySinh) as ThangSinhNhat
								from DM_KhachHang kh		
								left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHangZOA = customerZalo.Id
								where kh.TrangThai in (0,1)
								and kh.NgaySinh is not null
								and (@LoaiUser_CoTheGuiTin ='0' or (@LoaiUser_CoTheGuiTin = 2 and kh.IdKhachHangZOA is not null))
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
												when 100 then N'Đã gửi' ---- sms ---
												when 200 then N'Đã gửi' ---- zalo --
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
													where nk.ThoiGianTu <= @ToDate and nk.ThoiGianDen >= @FromDate														
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
							TotalRow
						from tblFilterTrangThai kh
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
										kh.Avatar,
										kh.IdKhachHangZOA,
										customerZalo.ZOAUserId,
										case kh.GioiTinhNam											
											when '1' then N'anh'
											when '0' then N'chị'
										else N'quý khách' end as XungHo,
										datepart(day,NgaySinh) as NgaySinhNhat,
										datepart(month,NgaySinh) as ThangSinhNhat
								from DM_KhachHang kh			
								left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHangZOA = customerZalo.Id
								where kh.TrangThai in (0,1)
								and kh.NgaySinh is not null
								and (@LoaiUser_CoTheGuiTin ='0' or (@LoaiUser_CoTheGuiTin = 2 and kh.IdKhachHangZOA is not null))
								and (SoDienThoai like @TextSearch 
									or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
									or TenKhachHang_KhongDau like @TextSearch
									or MaKhachHang like @TextSearch
									or TenKhachHang like @TextSearch
									or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
									or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
								) b
								where b.ThangSinhNhat = @birthday_monthFrom
								and b.NgaySinhNhat > @birthday_dayFrom
								and b.NgaySinhNhat < @birthday_dayTo
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
												when 200 then N'Đã gửi' --- zalo --
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
								TotalRow
							from tblFilterTrangThai kh							
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
						kh.IdKhachHangZOA,
						customerZalo.ZOAUserId,						
						kh.MaKhachHang,
						kh.TenKhachHang,
						kh.SoDienThoai,
						kh.Email,
						kh.Avatar,
						bk.Id,
						bk.Id as IdBooking,
						bk.IdChiNhanh,
						bk.BookingDate,
						bk.ThoiGianHen,
						qd.MaHangHoa,
						hh.TenHangHoa,
						bk.StartTime,
						bk.BookingCode,
						ChenhLech,
						----- used to: khách đặt hẹn ở chi nhánh/của hàng nào ---
						cn.TenChiNhanh,
						cn.SoDienThoai as SoDienThoaiChiNhanh,
						cn.DiaChi as DiaChiChiNhanh,
						congty.TenCongTy as TenCuaHang,
						congty.DiaChi as DiaChiCuaHang,
						congty.SoDienThoai as DienThoaiCuaHang,
						case kh.GioiTinhNam											
							when '1' then N'anh'
							when '0' then N'chị'
						else N'quý khách' end as XungHo
					from
					(
					select bk.Id,
						bk.IdKhachHang, 
						bk.IdChiNhanh,
						bk.BookingDate,
						bk.StartTime, 
						bk.BookingCode,
						----- gửi trước ....giờ/phút/giây : tính từ thời gian hẹn -> hiện tai: chênh lệch ..xx.. giây
						DATEDIFF(SECOND,GETDATE(), bk.StartTime) as ChenhLech, 					
						concat(FORMAT(bk.StartTime,'HH:mm'), ' - ', FORMAT(bk.EndTime,'HH:mm')) as ThoiGianHen
					from Booking bk
					where bk.TrangThai in (1,2) --- datlich/daxacnhan
					and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where bk.IdChiNhanh= cn.ID))
					and bk.BookingDate >= @FromDate and bk.BookingDate < @ToDate
					)bk
					join BookingService bkdv on bk.Id = bkdv.IdBooking
					join DM_DonViQuiDoi qd on bkdv.IdDonViQuiDoi = qd.Id
					join DM_HangHoa hh on qd.IdHangHoa = hh.Id
					join DM_KhachHang kh on bk.IdKhachHang= kh.Id
					left join DM_ChiNhanh cn on bk.IdChiNhanh = cn.Id
					left join HT_CongTy congty on cn.IdCongTy = congty.Id
					left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHangZOA = customerZalo.Id
					where kh.TrangThai in (0,1)
					and (@LoaiUser_CoTheGuiTin ='0' or (@LoaiUser_CoTheGuiTin = 2 and kh.IdKhachHangZOA is not null))
						and 
						----- tìm kiếm at Danh sách: filter khachhang + hanghoa --
						((@IsFilterCustomer ='0' and (kh.SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
							or TenKhachHang_KhongDau like @TextSearch
							or MaKhachHang like @TextSearch
							or TenKhachHang like @TextSearch
							or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
							or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI
							or hh.TenHangHoa like @TextSearch COLLATE Vietnamese_CI_AI
							or qd.MaHangHoa like @TextSearch COLLATE Vietnamese_CI_AI))
						----- tìm kiếm jqAutocustomer: chỉ tìm khachhang ----
						or (@IsFilterCustomer ='1'					
							and (kh.SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
							or TenKhachHang_KhongDau like @TextSearch
							or MaKhachHang like @TextSearch
							or TenKhachHang like @TextSearch
							or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
							or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)))
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
										when 100 then N'Đã gửi' -- sms --
										when 200 then N'Đã gửi' --- zalo --
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
									where sms.IdLoaiTin like '3%' ---- 31. nhắc lịch hẹn, 32. xác nhận lịch hẹn ---
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
					TotalRow
				from tblFilterTrangThai kh
				cross join count_cte
				order by BookingDate desc
				OFFSET (@CurrentPage* @PageSize) ROWS
				FETCH NEXT @PageSize ROWS ONLY
		end

		if(@IdLoaiTin=4)
		begin
			---- tin giao dich ---
			---- cộng thêm 1 ngày: vì bên ngoài chưa cộng --
			set @ToDate = DATEADD(day,1,@ToDate)

			; with data_cte
			as(
					select 
						kh.Id as IdKhachHang, 
						kh.IdKhachHangZOA,
						customerZalo.ZOAUserId,
						kh.MaKhachHang,
						kh.TenKhachHang,
						kh.SoDienThoai,
						kh.Email,
						kh.Avatar,
						hd.Id,
						hd.Id as IdHoaDon,
						hd.MaHoaDon,
						hd.NgayLapHoaDon,
						hd.TongThanhToan,
						hd.IdChiNhanh,
						cn.TenChiNhanh,
						cn.SoDienThoai as SoDienThoaiChiNhanh,
						cn.DiaChi as DiaChiChiNhanh,
						congty.TenCongTy as TenCuaHang,
						congty.DiaChi as DiaChiCuaHang,
						congty.SoDienThoai as DienThoaiCuaHang,
						case kh.GioiTinhNam											
							when '1' then N'anh'
							when '0' then N'chị'
						else N'quý khách' end as XungHo
					from
					(
					select 
						hd.Id,
						hd.IdKhachHang,
						hd.IdChiNhanh,
						hd.MaHoaDon,
						hd.NgayLapHoaDon,
						hd.TongThanhToan
					from BH_HoaDon hd
					where hd.TrangThai=3 -- hoanthanh
					and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where hd.IdChiNhanh= cn.ID))
					and hd.NgayLapHoaDon between @FromDate and @ToDate
					) hd 
					join DM_KhachHang kh on hd.IdKhachHang= kh.Id
					left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHangZOA = customerZalo.Id
					left join DM_ChiNhanh cn on hd.IdChiNhanh = cn.Id
					left join HT_CongTy congty on cn.IdCongTy= congty.Id
					where kh.TrangThai in (0,1)
					and (@LoaiUser_CoTheGuiTin ='0' or (@LoaiUser_CoTheGuiTin = 2 and kh.IdKhachHangZOA is not null))
						and 
						----- tìm kiếm at Danh sách: filter khachhang + hóa don --
						((@IsFilterCustomer = '0' 
						and (kh.SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
							or TenKhachHang_KhongDau like @TextSearch
							or MaHoaDon like @TextSearch
							or MaKhachHang like @TextSearch
							or TenKhachHang like @TextSearch
							or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
							or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI))
							----- tìm kiếm at jqAuto: chỉ tìm khachhang ---
						or ((@IsFilterCustomer = '1')
							and (kh.SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
							or TenKhachHang_KhongDau like @TextSearch
							or MaKhachHang like @TextSearch
							or TenKhachHang like @TextSearch
							or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
							or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)))

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
											when 100 then N'Đã gửi' --- sms: thanhcong --
											when 200 then N'Đã gửi' -- zalo: thanhcong
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
										------- cộng thêm 1 ngày ở Thời gian đến, 
										----- vì nếu gửi cho hd ngày 05/12, thời gian từ - đến chỉ lưu định dạng thời gian đạng 00:00
										and dt.NgayLapHoaDon between nky.ThoiGianTu and DATEADD(day,1, nky.ThoiGianDen))
									)tblHinhThuc
									where (@HinhThucGuiTins =''
									or exists (select HinhThucGui from @tblHinhThucGuiTin ht where ht.HinhThucGui = tblHinhThuc.HinhThucGui))
								) smsOut
								where smsOut.RN = 1
							----- neu gui tin tudong: chi get khach chua duoc gui ----					
						)nk on dt.Id = nk.IdHoaDon
					) tblLast where (@TrangThais ='' or exists (select TrangThai from @tblTrangThai tt where tt.TrangThai  = tblLast.TrangThai))
				),
				tblThuChi
				as
				(
					select 
						qct.IdHoaDonLienQuan,
						qct.HinhThucThanhToan,
						iif(qhd.IdLoaiChungTu =11, qct.TienThu, - qct.TienThu) as TienThu
					from QuyHoaDon qhd
					join QuyHoaDon_ChiTiet qct on qhd.Id = qct.IdQuyHoaDon
					where qhd.TrangThai ='1' and qhd.IsDeleted='0' and qct.IsDeleted='0'
					and exists (select id from tblFilterTrangThai hd where qct.IdHoaDonLienQuan = hd.Id)
				)
				,count_cte
				as(
					select count(Id) as TotalRow from tblFilterTrangThai
				)
				select kh.*,
					isnull(Left(sq.PhuongThucTT,Len(sq.PhuongThucTT)-1),'') As PTThanhToan,
					isnull(sq.TienThu,0) as DaThanhToan,
					TotalRow
				from tblFilterTrangThai kh		
				left join
				(
						select qct.IdHoaDonLienQuan,
							sum(qct.TienThu) as TienThu,
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
						from tblThuChi qctIn 
						where qctIn.IdHoaDonLienQuan = qct.IdHoaDonLienQuan
						For XML PATH ('') 
						) PhuongThucTT 
					from tblThuChi qct
					group by qct.IdHoaDonLienQuan
				)sq on kh.IdHoaDon = sq.IdHoaDonLienQuan
				cross join count_cte
				order by NgayLapHoaDon desc			
				OFFSET (@CurrentPage* @PageSize) ROWS
				FETCH NEXT @PageSize ROWS ONLY
		end    
END");

			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_getBookingInfo]
	@IdBooking UNIQUEIDENTIFIER,
	@TenantId INT
AS 
begin
SELECT
		b.Id,
		b.BookingCode,
		b.IdKhachHang,
		b.BookingDate,
        CONVERT(NVARCHAR(5), b.StartTime, 108) AS StartTime,
        CONVERT(NVARCHAR(5), b.EndTime, 108) AS EndTime,
        kh.Avatar as AvatarKhachHang,
        b.TenKhachHang,
		b.SoDienThoai,
        b.GhiChu,
		nv.TenNhanVien AS NhanVienThucHien,
        hh.TenHangHoa AS TenDichVu,
		ISNULL(dv.GiaBan,0) AS DonGia,
        CASE
            WHEN b.TrangThai = 0 THEN '#a19f9c'
			WHEN B.TrangThai = 1 THEN '#FF9900'
			wHEN b.TrangThai = 2 THEN '#7DC1FF'
			WHEN b.TrangThai = 3 THEN '#009EF7'
			WHEN b.TrangThai = 4 THEN '#50CD89'
            ELSE '#009EF7'
        END AS Color,
        b.TrangThai
    FROM
        Booking b
        LEFT JOIN DM_KhachHang kh on kh.Id= b.IdKhachHang
        left JOIN BookingNhanVien bnv ON bnv.IdBooking = b.Id and bnv.IsDeleted = 0
		LEFT JOIN NS_NhanVien nv on nv.id = bnv.IdNhanVien
        LEFT JOIN BookingService bs ON bs.IdBooking = b.Id and bs.IsDeleted = 0
        LEFT JOIN DM_DonViQuiDoi dv ON dv.Id = bs.IdDonViQuiDoi
        LEFT JOIN DM_HangHoa hh ON hh.id = dv.IdHangHoa
    WHERE
        b.TenantId = ISNULL(@TenantId, 1)
		AND b.Id = @IdBooking
        AND b.IsDeleted = 0;
end;");
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS [dbo].[fnGetBookingCode]");
        }
    }
}