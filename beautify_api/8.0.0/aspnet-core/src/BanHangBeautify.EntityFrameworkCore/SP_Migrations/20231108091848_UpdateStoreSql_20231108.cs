using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20231108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[spInsertNhatKyGuiTinSMS]");
			migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spInsertNhatKyGuiTinSMS]
	@TenantId int =1,
	@IdHeThongSMS uniqueidentifier = null,
	@IdCustomer uniqueidentifier = null,
	@IdChiNhanh uniqueidentifier = null,
	@IdLoaiTin int = 1,
	@FromDate datetime = null,
	@ToDate datetime = null
AS
BEGIN

	SET NOCOUNT ON;

	if @IdLoaiTin = 2 ---- sinhnhat ---
		begin
			insert into SMS_NhatKy_GuiTin (Id, TenantId, IdHeThongSMS, ThoiGianTu, ThoiGianDen, CreationTime, IsDeleted)
			values (NEWID(), @TenantId, @IdHeThongSMS, @FromDate, @ToDate, GETDATE(), '0')
		end

	if @IdLoaiTin = 3 --- lichhen ---
		begin
				
			insert into SMS_NhatKy_GuiTin (Id,TenantId, IdHeThongSMS, ThoiGianTu, ThoiGianDen, IdBooking, CreationTime, IsDeleted)
			----- get list booking by idcustomer---
			select 
				NEWID(),
				@TenantId,
				@IdHeThongSMS,
				@FromDate,
				@ToDate,
				bk.Id as IdBooking,
				 GETDATE(), '0'
			from Booking bk
			where bk.TrangThai in (1,2) --- datlich/daxacnhan
			and bk.IdChiNhanh = @IdChiNhanh
			and bk.IdKhachHang = @IdCustomer
			and bk.BookingDate between @FromDate and @ToDate

		end

		if @IdLoaiTin = 4 --- giaodich ---
		begin
				
			insert into SMS_NhatKy_GuiTin (Id,TenantId, IdHeThongSMS, ThoiGianTu, ThoiGianDen, IdHoaDon, CreationTime, IsDeleted)
			----- get list hoadon by idcustomer---
			select 
				NEWID(),
				@TenantId,
				@IdHeThongSMS,
				@FromDate,
				@ToDate,
				hd.Id as IdHoaDon,
				GETDATE(), '0'
			from BH_HoaDon hd
			where hd.TrangThai=3 -- hoanthanh
			and hd.IdChiNhanh = @IdChiNhanh
			and hd.IdKhachHang = @IdCustomer
			and hd.NgayLapHoaDon between @FromDate and @ToDate

		end
    
END");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListBandname]
	@TenantId int = 1,
    @Keyword nvarchar(max) ='2',
    @SkipCount int = 1,
    @MaxResultCount int = 10,
    @SortBy nvarchar(200)='CreateTime',
    @SortType varchar(20)='desc'
AS
BEGIN

	SET NOCOUNT ON;

	if(@SkipCount> 0) set @SkipCount = @SkipCount- 1;
	if(ISNULL(@Keyword,'')!='') set @Keyword = CONCAT('%',@Keyword,'%')


	;with data_cte
	as
	(
    select 
		br.Id,
		br.TenantId,
		br.Brandname,
		br.SDTCuaHang,
		br.NgayKichHoat,
		br.TrangThai,
		iif(br.TrangThai=1,N'Kích hoạt',N'Chưa kích hoạt') as TxtTrangThai
	from HT_SMSBrandname br
	where br.IsDeleted='0'
	and (@TenantId = 1 or br.TenantId = @TenantId) --- get all brandname (if HOST) - or only get brandname by tenantId
	and (@Keyword ='' or br.Brandname like @Keyword)
	),
	count_cte
	as 
	(
		select count(Id) as TotalRow
		from data_cte
	)
	select dt.*,
		count_cte.*,
		isnull(tblThuChi.TongTienNap,0) as TongTienNap,
		isnull(tblThuChi.TongTienNap,0) - DaSuDung as ConLai
	from data_cte dt
	cross join count_cte
	left join
	(
		----- tongnaptien ---
		select 
			 qhd.IdBrandname,
			 qhd.TenantId,
			 sum(iif(qhd.IdLoaiChungTu=11, qhd.TongTienThu, - qhd.TongTienThu)) as TongTienNap,
			 0 as DaSuDung
		from QuyHoaDon qhd
		where qhd.TrangThai=1
		and exists (select id from data_cte dt where qhd.IdBrandname = dt.Id)
		group by qhd.IdBrandname, qhd.TenantId
	)tblThuChi
	on dt.Id = tblThuChi.IdBrandname

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

							select *
							from
							(
								select  kh.Id as IdKhachHang, 
										kh.MaKhachHang, 
										kh.TenKhachHang, 
										kh.SoDienThoai,
										kh.TongTichDiem,						
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
							
					end
				else
					begin
							----- lọc ngày sinh cung  ------		
							select *
							from
							(
								select  kh.Id as IdKhachHang,
										kh.MaKhachHang, 
										kh.TenKhachHang,
										kh.SoDienThoai,
										kh.TongTichDiem,					
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
			where kh.TrangThai in (0,1)
				and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
					or TenKhachHang_KhongDau like @TextSearch
					or MaKhachHang like @TextSearch
					or TenKhachHang like @TextSearch
					or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
					or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
		end
    
END");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListCustomer_byIdLoaiTin]	
	@IdLoaiTin int= 2,
	@IdChiNhanhs nvarchar(max)= 'c4fbe44f-c26e-499f-9033-af9c4e3c6fc3',
	@TextSearch nvarchar(max)= 'anh',
	@FromDate datetime= '2023-11-01',
	@ToDate datetime= '2023-11-30',
	@CurrentPage int=1,
	@PageSize int = 50
AS
BEGIN
	
	SET NOCOUNT ON;
	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1;

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
						; with data_cte
						as(
							select *
							from
							(
								select  kh.Id as IdKhachHang, 
										kh.id,
										kh.MaKhachHang, 
										kh.TenKhachHang, 
										kh.SoDienThoai,
										kh.TongTichDiem,		
										kh.NgaySinh,
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
						count_cte
						as(
							select count(IdKhachHang) as TotalRow
							from data_cte
						)
						select dt.*, 
							TotalRow,
							iif(nk.IdKhachHang is null, N'Chưa gửi',nk.STrangThai) as STrangThaiGuiTinNhan									
						from data_cte dt
						cross join count_cte
						left join
						(
						   select tbl.*,
							case tbl.TrangThai
								when 1 then N'Lưu nháp'
								when 100 then N'Đã gửi'
							else N'Gửi thất bại' end as STrangThai
						   from
						   (
					------ get khoảng thời gian giao nhau (giữa bộ lọc - và nhật ký sms)			
							select 
								IdKhachHang,
								TrangThai,
								max(ThoiGianTu) over (order by ThoiGianTu desc) as maxfromdate,
								min(ThoiGianDen) over (order by ThoiGianDen ) as mintodate
							from
							(
							select  sms.IdKhachHang,
								sms.TrangThai,
								nky.ThoiGianTu,
								nky.ThoiGianDen
							from HeThong_SMS sms
							join SMS_NhatKy_GuiTin nky on sms.Id = nky.IdHeThongSMS
							where sms.IdLoaiTin= 2 
							and exists (select * from data_cte dt where sms.IdKhachHang = dt.IdKhachHang)

							union all

							select 
								'00000000-0000-0000-0000-000000000000' as IdKhachHang,
								0 as TrangThai,
								@FromDate as fromdate, 
								@ToDate as todate 
							)tblUnion
							) tbl
							where maxfromdate is not null and mintodate is not null
							and maxfromdate <= mintodate
							
						)nk on dt.IdKhachHang = nk.IdKhachHang
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
							count_cte
							as(
								select count(IdKhachHang) as TotalRow
								from data_cte
							)
							select dt.*, TotalRow,
								iif(nk.IdKhachHang is null, N'Chưa gửi',nk.STrangThai) as STrangThaiGuiTinNhan		
							from data_cte dt
							cross join count_cte
							left join
						(
						   select tbl.*,
							case tbl.TrangThai
								when 1 then N'Lưu nháp'
								when 100 then N'Đã gửi'
							else N'Gửi thất bại' end as STrangThai
						   from
						   (
						------ get khoảng thời gian giao nhau (giữa bộ lọc - và nhật ký sms)			
								select 
									IdKhachHang,
									TrangThai,
									max(ThoiGianTu) over (order by ThoiGianTu desc) as maxfromdate,
									min(ThoiGianDen) over (order by ThoiGianDen ) as mintodate
								from
								(
								select  sms.IdKhachHang,
									sms.TrangThai,
									nky.ThoiGianTu,
									nky.ThoiGianDen
								from HeThong_SMS sms
								join SMS_NhatKy_GuiTin nky on sms.Id = nky.IdHeThongSMS
								where sms.IdLoaiTin= 2 
								and exists (select * from data_cte dt where sms.IdKhachHang = dt.IdKhachHang)

								union all

								select 
									'00000000-0000-0000-0000-000000000000' as IdKhachHang,
									0 as TrangThai,
									@FromDate as fromdate, 
									@ToDate as todate 
								)tblUnion
								) tbl
								where maxfromdate is not null and mintodate is not null
								and maxfromdate <= mintodate
							
							)nk on dt.IdKhachHang = nk.IdKhachHang
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
				count_cte
				as(
					select count(Id) as TotalRow
					from data_cte
				)
				select dt.*,
					TotalRow,
					iif(nk.IdKhachHang is null, N'Chưa gửi',nk.STrangThai) as STrangThaiGuiTinNhan		
				from data_cte dt				
				cross join count_cte
				left join
				(
					select 
						smsOut.IdKhachHang,
						smsOut.IdBooking,
						case smsOut.TrangThai
								when 1 then N'Lưu nháp'
								when 100 then N'Đã gửi'
						else N'Gửi thất bại' end as STrangThai
					from
					(
						select 
							sms.IdKhachHang,
							sms.TrangThai,
							sms.ThoiGianGui,
							nky.IdBooking,
							---- nếu gửi nhiều lần - chỉ get lần gửi cuối cùng
							max(sms.ThoiGianGui) over (partition by nky.IdBooking order by ThoiGianGui desc) as LastDateSend
						from HeThong_SMS sms
						join SMS_NhatKy_GuiTin nky on sms.Id = nky.IdHeThongSMS
						where sms.IdLoaiTin= 3 
							and exists (select * from data_cte dt 
							where sms.IdKhachHang = dt.IdKhachHang 
							and dt.BookingDate between nky.ThoiGianTu and nky.ThoiGianDen
							)
					) smsOut
					where smsOut.ThoiGianGui = smsOut.LastDateSend				
				)nk on dt.Id= nk.IdBooking
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
				count_cte
				as(
					select count(Id) as TotalRow
					from data_cte
				)
				select 
					dt.*,
					TotalRow,
					iif(nk.IdKhachHang is null, N'Chưa gửi',nk.STrangThai) as STrangThaiGuiTinNhan	
				from data_cte dt
				cross join count_cte
				left join
				(
					select 
						smsOut.IdKhachHang,
						smsOut.IdHoaDon,
						case smsOut.TrangThai
								when 1 then N'Lưu nháp'
								when 100 then N'Đã gửi'
						else N'Gửi thất bại' end as STrangThai
					from
					(
						select 
							sms.IdKhachHang,
							sms.TrangThai,
							sms.ThoiGianGui,
							nky.IdHoaDon,
							---- nếu gửi nhiều lần - chỉ get lần gửi cuối cùng
							max(sms.ThoiGianGui) over (partition by nky.IdHoaDon order by sms.ThoiGianGui desc) as LastDateSend
						from HeThong_SMS sms
						join SMS_NhatKy_GuiTin nky on sms.Id = nky.IdHeThongSMS
						where sms.IdLoaiTin= 4 
						and exists (select * from data_cte dt 
							where sms.IdKhachHang = dt.IdKhachHang 
							and dt.NgayLapHoaDon between nky.ThoiGianTu and nky.ThoiGianDen)
					) smsOut
					where smsOut.ThoiGianGui = smsOut.LastDateSend	--- - chỉ get lần gửi cuối cùng			
				)nk on dt.Id = nk.IdHoaDon
				order by NgayLapHoaDon desc
				OFFSET (@CurrentPage* @PageSize) ROWS
				FETCH NEXT @PageSize ROWS ONLY
		end
    
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[spInsertNhatKyGuiTinSMS]");
        }
    }
}
