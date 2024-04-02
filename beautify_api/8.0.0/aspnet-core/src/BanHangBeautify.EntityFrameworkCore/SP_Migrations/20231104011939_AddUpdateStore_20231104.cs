using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStore20231104 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[spJqAutoCustomer_byIdLoaiTin]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[spGetListCustomer_byIdLoaiTin]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[spGetListSMS]");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spJqAutoCustomer_byIdLoaiTin]	
	@IdLoaiTin int= 4,
	@IdChiNhanhs nvarchar(max)= 'c4fbe44f-c26e-499f-9033-af9c4e3c6fc3',
	@TextSearch nvarchar(max)= '',
	@FromDate datetime= '2023-11-03',
	@ToDate datetime= '2023-11-04',
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
								select  kh.Id, 
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
								select  kh.Id,
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
								where b.ThangSinhNhat = @birthday_monthFrom
								and b.NgaySinhNhat between (@birthday_dayFrom + 1) and @birthday_dayTo
							
					end	
		end

	if(@IdLoaiTin=3) ---- khách hàng có lịch hẹn từ - đến
		begin
			set @ToDate = DATEADD(day,1,@ToDate)

			select distinct
				kh.Id, 
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
				kh.Id, 
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
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetListCustomer_byIdLoaiTin]	
	@IdLoaiTin int= 3,
	@IdChiNhanhs nvarchar(max)= 'c4fbe44f-c26e-499f-9033-af9c4e3c6fc3',
	@TextSearch nvarchar(max)= '',
	@FromDate datetime= '2023-11-03',
	@ToDate datetime= '2023-11-04',
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
						; with data_cte
						as(
							select *
							from
							(
								select  kh.Id, 
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
							select count(Id) as TotalRow
							from data_cte
						)
						select *
						from data_cte
						cross join count_cte
						order by MaKhachHang desc
						OFFSET (@CurrentPage* @PageSize) ROWS
						FETCH NEXT @PageSize ROWS ONLY
							
					end
				else
					begin
							----- lọc ngày sinh cung thang  ------		
						; with data_cte
						as(							
							select *
							from
							(
								select  kh.Id,
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
								and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
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
								select count(Id) as TotalRow
								from data_cte
							)
							select *
							from data_cte
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
						kh.Id, 
						kh.MaKhachHang,
						kh.TenKhachHang,
						kh.SoDienThoai,
						bk.Id as IdBooking,
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
				select *
				from data_cte				
				cross join count_cte
				order by MaKhachHang desc
				OFFSET (@CurrentPage* @PageSize) ROWS
				FETCH NEXT @PageSize ROWS ONLY
		end

		if(@IdLoaiTin=4)
		begin
			---- tin giao dich ---
			; with data_cte
			as(
					select 
						kh.Id, 
						kh.MaKhachHang,
						kh.TenKhachHang,
						kh.SoDienThoai,
						hd.Id as IdHoaDon,
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
				select *
				from data_cte
				cross join count_cte
				order by MaKhachHang desc
				OFFSET (@CurrentPage* @PageSize) ROWS
				FETCH NEXT @PageSize ROWS ONLY
		end
    
END");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetListSMS]
	@IdChiNhanhs nvarchar(max)='',
	@FromDate datetime= '2023-11-01',
	@ToDate datetime = '2023-11-30',
	@TrangThais varchar(100) ='',
	@TextSearch nvarchar(max)='',
	@CurrentPage int = 0,
	@PageSize int = 50
AS
BEGIN

	SET NOCOUNT ON;
	if(ISNULL(@ToDate,'')!='')
		set @ToDate = DATEADD(day,1,@ToDate)
	declare @tblChiNhanh table (Id uniqueidentifier)
	if isnull(@IdChiNhanhs,'') !=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)

		;with data_cte
		as(
		select *
		from
		(
		select 
			sms.Id,
			sms.ThoiGianGui,
			sms.IdLoaiTin,
			sms.TrangThai,
			sms.NoiDungTin,
			kh.MaKhachHang,
			kh.TenKhachHang,
			kh.TenKhachHang_KhongDau,
			kh.SoDienThoai,

			case sms.IdLoaiTin
				when 1 then N'Tin thường'
				when 2 then N'Tin sinh nhật'
				when 3 then N'Tin lịch hẹn'
				when 4 then N'Tin giao dịch'
			end as LoaiTin,

			case sms.TrangThai
				when 99 then N'Lỗi không xác định'
				when 100 then N'Thành công'
				when 103 then N'Không đủ số dư'
				when 104 then N'Brandname không tồn tại'
				when 104 then N'Tin nhắn không hợp lệ'			
			end as sTrangThaiGuiTinNhan

		from HeThong_SMS sms
		left join DM_KhachHang kh on sms.IdKhachHang = kh.Id
		where (@IdChiNhanhs ='' or exists (select Id from @tblChiNhanh cn where sms.IdChiNhanh = cn.Id))
		and (@TrangThais ='' or sms.TrangThai in (select GiaTri from dbo.fnSplitstring(@TrangThais)))
		and sms.ThoiGianGui between @FromDate and @ToDate
		) tbl where (@TextSearch =''
			or tbl.TenKhachHang like '%N' + @TextSearch +'%'
			or tbl.TenKhachHang_KhongDau like '%N' + @TextSearch +'%'
			or tbl.SoDienThoai like '%N' + @TextSearch +'%'
			or tbl.MaKhachHang like '%N' + @TextSearch +'%'
			or tbl.LoaiTin like '%N' + @TextSearch +'%')
	),
	count_cte
	as
	(
	select count(ID) as TotalRow --,
		---ceiling(count(ID) /@PageSize) as TotalPage
		from data_cte
	)
	select *
	from data_cte
	cross join count_cte
	order by ThoiGianGui desc
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY

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
	where  br.IsDeleted='0'
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
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListCustomerChecking]
	@TenantId int= 7,
	@IdChiNhanh uniqueidentifier = null,
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
	and (@IdChiNhanh is null or checkin.IdChiNhanh= @IdChiNhanh)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[spJqAutoCustomer_byIdLoaiTin]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[spGetListCustomer_byIdLoaiTin]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[spGetListSMS]");
        }
    }
}
