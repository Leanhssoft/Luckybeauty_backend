using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20240115 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[GetAllUser]
	@TextSearch  nvarchar(max) ='',
	@IsActive varchar(10)= '',
	@CurrentPage int =0,
	@PageSize int = 10,
	@ColumnSort varchar(200)='creatTime',
	@TypeSort varchar(20) ='desc'
AS
BEGIN

	SET NOCOUNT ON;

	if @CurrentPage > 0 set @CurrentPage = @CurrentPage -1;

	if isnull(@TextSearch,'') !=''
		set @TextSearch = concat(N'%', @TextSearch, '%')
	else
		set @TextSearch=''
	if ISNULL(@IsActive,'') =''
		set @IsActive =''
	

	select 
		u.Id,
		u.NhanSuId,
		u.IdChiNhanhMacDinh,
		u.UserName,
		u.Name,
		u.Surname,
		u.EmailAddress,
		u.PhoneNumber,
		u.IsActive,
		u.IsAdmin,
		u.CreationTime,
		nv.TenNhanVien,
		nv.Avatar,
		cn.TenChiNhanh
	into #temp
	from AbpUsers u
	left join NS_NhanVien nv on u.NhanSuId = nv.Id
	left join DM_ChiNhanh cn on u.IdChiNhanhMacDinh = cn.Id
	where u.IsDeleted='0'
	and (@IsActive ='' or u.IsActive = @IsActive)
	and (@TextSearch =''
		or u.UserName like @TextSearch
		or u.Name like @TextSearch
		or u.Surname like @TextSearch
		or nv.TenNhanVien like @TextSearch
		or cn.TenChiNhanh like @TextSearch	
		)


		select Count(Id) as TotalCount from  #temp

		select tbl.*,
			roles.RoleNames,
			iif(tbl.IsActive='1',N'Hoạt động', N'Ngừng hoạt động') as TxtTrangThai
		from #temp tbl
		left join (
			select distinct userRole.UserId,
				(
				select r.Name + ', ' AS [text()]
				from AbpUserRoles urole
				join AbpRoles r on urole.RoleId = r.Id				
				where urole.UserId = userRole.UserId
				For XML PATH ('')
				) as RoleNames
			from AbpUserRoles userRole
		) roles on roles.UserId = tbl.Id
		ORDER BY
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'name' THEN Name END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'surname' THEN Surname END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'emailAddress' THEN EmailAddress END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'phoneNumber' THEN PhoneNumber END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'tenNhanVien' THEN TenNhanVien END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'tenChiNhanh' THEN TenChiNhanh END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'creationTime' THEN CreationTime END ASC,	

		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'name' THEN Name END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'surname' THEN Surname END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'emailAddress' THEN EmailAddress END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'phoneNumber' THEN PhoneNumber END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'tenNhanVien' THEN TenNhanVien END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'tenChiNhanh' THEN TenChiNhanh END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'creationTime' THEN CreationTime END DESC
		OFFSET  @CurrentPage ROWS FETCH NEXT @PageSize ROWS ONLY;
END");


			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListBandname]
	@TenantId int = 1,
    @Keyword nvarchar(max) ='',
	@TrangThais varchar(10) ='0,1', --- 0.chuakichhoat, 1.kichhoat
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
		tenant.TenancyName,
		tenant.Name as DisplayTenantName,
		iif(br.TrangThai=1,N'Kích hoạt',N'Chưa kích hoạt') as TxtTrangThai
	from HT_SMSBrandname br
	Left join AbpTenants tenant on br.TenantId = tenant.Id
	where br.IsDeleted='0'
	and (@TenantId = 1 or br.TenantId = @TenantId) --- get all brandname (if HOST) - or only get brandname by tenantId
	and (@Keyword =''
		or br.Brandname like @Keyword
		or tenant.TenancyName like @Keyword
		or tenant.Name like @Keyword
		or br.SDTCuaHang like @Keyword
		)
	and (@TrangThais ='' or exists (select GiaTri from dbo.fnSplitstring(@TrangThais) tt where br.TrangThai = tt.GiaTri))
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
	ORDER BY
		CASE WHEN @SortType = 'asc' AND @SortBy = 'brandname' THEN Brandname END ASC,
		CASE WHEN @SortType = 'asc' AND @SortBy = 'sdtCuaHang' THEN SDTCuaHang END ASC,
		CASE WHEN @SortType = 'asc' AND @SortBy = 'ngayKichHoat' THEN NgayKichHoat END ASC,
		CASE WHEN @SortType = 'asc' AND @SortBy = 'tenancyName' THEN TenancyName END ASC,
		CASE WHEN @SortType = 'asc' AND @SortBy = 'displayTenantName' THEN DisplayTenantName END ASC,
		CASE WHEN @SortType = 'asc' AND @SortBy = 'tongTienNap' THEN tblThuChi.TongTienNap END ASC,

		CASE WHEN @SortType = 'desc' AND @SortBy = 'brandname' THEN Brandname END DESC,
		CASE WHEN @SortType = 'desc' AND @SortBy = 'sdtCuaHang' THEN SDTCuaHang END DESC,
		CASE WHEN @SortType = 'desc' AND @SortBy = 'tenancyName' THEN NgayKichHoat END DESC,
		CASE WHEN @SortType = 'desc' AND @SortBy = 'tenancyName' THEN TenancyName END DESC,
		CASE WHEN @SortType = 'desc' AND @SortBy = 'displayTenantName' THEN DisplayTenantName END DESC,
		CASE WHEN @SortType = 'desc' AND @SortBy = 'tongTienNap' THEN tblThuChi.TongTienNap END DESC
	OFFSET  @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

END");
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListCustomer_byIdLoaiTin]	
	@IdLoaiTin int= 4,
	@IdChiNhanhs nvarchar(max)= '3EBE69E5-AA23-48FC-9EB1-70F713E012D1',
	@TrangThais varchar(50)='', ---- 100.thanhcong,1.nhap, 0.chuagui,else.thatbai
	@HinhThucGuiTins varchar(10)='', ------ 1.sms, 2.zalo, 3.gmail
	@TextSearch nvarchar(max)= '',
	@FromDate datetime= '2024-01-06',
	@ToDate datetime= '2024-01-06',
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
						hh.TenHangHoa,
						bk.StartTime,
						ChenhLech
					from
					(
					select bk.Id,
						bk.IdKhachHang, 
						bk.BookingDate,
						bk.StartTime, 
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
			---- cộng thêm 1 ngày: vì bên ngoài chưa cộng --
			set @ToDate = DATEADD(day,1,@ToDate)

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

			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_nhanVien_getAll]
				@TenantId INT,
				@IdChiNhanh UNIQUEIDENTIFIER = NULL,
				@IdChucVu UNIQUEIDENTIFIER = NULL,
				@Filter NVARCHAR(MAX),
				@SortBy NVARCHAR(50),
				@SortType NVARCHAR(4),
				@SkipCount INT = 0,
				@MaxResultCount INT = 10
			AS
			BEGIN
			set nocount on;
			--if isnull(@IdChucVu,'')='' set @IdChucVu =''

				SELECT 
				nv.Id,
				nv.Avatar,
				nv.Ho,
				nv.TenLot,
				nv.TenNhanVien,
				nv.SoDienThoai,
				nv.CCCD,
				nv.NgaySinh,
				nv.KieuNgaySinh,
				nv.GioiTinh,
				nv.NgayCap,
				nv.NoiCap,
				nv.DiaChi,
				cv.TenChucVu,
				nv.CreationTime AS NgayVaoLam,
				CASE WHEN nv.IsDeleted = 0 THEN N'Hoạt động' ELSE N'Ngừng hoạt động' END AS TrangThai
				into #temp
				FROM NS_NhanVien nv 
				LEFT JOIN NS_ChucVu cv ON cv.Id = nv.IdChucVu
				JOIN (
					SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
					WHERE IsDeleted = 0 AND TenantId= @TenantId 
					GROUP BY IdNhanVien,IdChiNhanh
				) AS qtct ON qtct.IdNhanVien = nv.Id
				WHERE nv.TenantId = @TenantId
				AND (qtct.IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
				and (@IdChucVu is null or cv.Id = @IdChucVu)
				AND (
					ISNULL(@Filter,'') = ''
					OR LOWER(nv.TenNhanVien) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.SoDienThoai) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.CCCD) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.DiaChi) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(cv.TenChucVu) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.DiaChi) LIKE N'%'+LOWER(@Filter)+N'%'
				)
				ORDER BY
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenNhanVien' THEN nv.TenNhanVien END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soDienThoai' THEN nv.SoDienThoai END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'diaChi' THEN nv.DiaChi END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngaySinh' THEN nv.NgaySinh END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'gioiTinh' THEN nv.GioiTinh END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'chucVu' THEN cv.TenChucVu END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngayThamGia' THEN nv.CreationTime END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'trangThai' THEN DiaChi END,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenNhanVien' THEN nv.TenNhanVien END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soDienThoai' THEN nv.SoDienThoai END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'diaChi' THEN nv.DiaChi END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngaySinh' THEN nv.NgaySinh END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'gioiTinh' THEN nv.GioiTinh END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'chucVu' THEN cv.TenChucVu END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngayThamGia' THEN nv.CreationTime END DESC,
					CASE WHEN ISNULL(@SortType,'') = '' AND ISNULL(@SortBy,'') = '' THEN nv.LastModificationTime END DESC
				OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

				select * from #temp
				SELECT COUNT(Id) AS TotalCount
				FROM #temp
				
			END;");
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_getBookingInfo]
	@IdBooking UNIQUEIDENTIFIER,
	@TenantId INT
AS 
begin
SELECT
		b.Id,
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
            WHEN b.TrangThai = 0 THEN '#F1416C'
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
            
        }
    }
}
