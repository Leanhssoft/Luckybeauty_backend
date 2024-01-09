using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreSql20240109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[ApplyAll_SetupHoaHongDV]");
            migrationBuilder.Sql(@"create PROCEDURE [dbo].[ApplyAll_SetupHoaHongDV]
	@IdNhanVienChosed uniqueidentifier =null,
	@IdNhanVien varchar(40) ='',
	@IdDonViQuyDoi varchar(40) ='',
	@IdNhomHang uniqueidentifier =null,
	@LoaiApDung bigint = 0, --- 1.nhanvien, 2.dichvu, 3.nhom
	@LoaiChietKhau tinyint = 1,
	@GiaTriChietKhau float =0,
	@LaPhanTram bit = '1',
	@UpdateUserId bigint = 1
AS
BEGIN

	SET NOCOUNT ON;
	if @LoaiApDung =1 ---- apdung theo nhanvien ---
		begin
			----- áp dụng cho tất cả dịch vụ của nhân viên -----
			update ck set ck.GiaTri = @GiaTriChietKhau, ck.LaPhanTram = @LaPhanTram,
				ck.LastModifierUserId = @UpdateUserId,
				ck.LastModificationTime = GETDATE()
			from NS_ChietKhauDichVu ck
			where ck.IdNhanVien = @IdNhanVien and LoaiChietKhau = @LoaiChietKhau						
		end

	if @LoaiApDung =2 ---- apdung theo dichvu
		begin
			----- áp dụng cho dịch vụ xxx của tất cả nhân viên -----
			update ck set ck.GiaTri = @GiaTriChietKhau, ck.LaPhanTram = @LaPhanTram,
				ck.LastModifierUserId = @UpdateUserId,
				ck.LastModificationTime = GETDATE()
			from NS_ChietKhauDichVu ck
			where ck.IdDonViQuiDoi = @IdDonViQuyDoi	and LoaiChietKhau = @LoaiChietKhau			
		end

	
	if @LoaiApDung = 3 ---- apdung theo nhom ---
		begin		
			if @IdNhanVienChosed is null or @IdNhanVienChosed like N'%0000%'
				begin
						---- apply all nhan vien ---
						-----  áp dụng cho tất cả dịch vụ thuộc nhóm xxxx-----
					update ck set ck.GiaTri = @GiaTriChietKhau, ck.LaPhanTram = @LaPhanTram,
						ck.LastModifierUserId = @UpdateUserId,
						ck.LastModificationTime = GETDATE()
					from NS_ChietKhauDichVu ck
					where LoaiChietKhau = @LoaiChietKhau			
					and exists (
						select qd.id from DM_DonViQuiDoi qd
						join DM_HangHoa hh on qd.IdHangHoa = hh.Id
						where qd.Id = ck.IdDonViQuiDoi
						and hh.IdNhomHangHoa = @IdNhomHang
						)
				end
			else
				begin
						----- chỉ áp dụng cho NV chosed ----
						----- áp dụng cho tất cả dịch vụ thuộc nhóm xxxx ------
					update ck set ck.GiaTri = @GiaTriChietKhau, ck.LaPhanTram = @LaPhanTram,
					ck.LastModifierUserId = @UpdateUserId,
					ck.LastModificationTime = GETDATE()
					from NS_ChietKhauDichVu ck
					where ck.IdNhanVien = @IdNhanVienChosed and LoaiChietKhau = @LoaiChietKhau
					and exists (
						select qd.id from DM_DonViQuiDoi qd
						join DM_HangHoa hh on qd.IdHangHoa = hh.Id
						where qd.Id = ck.IdDonViQuiDoi
						and hh.IdNhomHangHoa = @IdNhomHang
						)
				end
		end
END");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[GetAllSetup_HoaHongDichVu]
	@TenantId INT = 2,
	@IdNhanVien UNIQUEIDENTIFIER = null,
	@IdChiNhanh UNIQUEIDENTIFIER ='C4FBE44F-C26E-499F-9033-AF9C4E3C6FC3', 
	@Filter NVARCHAR(200)='',
	@SkipCount INT = 0,
	@MaxResultCount INT = 50,
	@SortBy NVARCHAR(30) ='tenNhanVien',
	@SortType VARCHAR(4) = 'desc'
AS
BEGIN

	SET NOCOUNT ON;


					SELECT 
							
							ckdv.IdNhanVien,
							ckdv.IdDonViQuiDoi,
							hh.IdNhomHangHoa,
							dvqd.GiaBan AS GiaDichVu,
							hh.TenHangHoa AS TenDichVu,
							nhh.TenNhomHang AS TenNhomDichVu,
							ckdv.LoaiChietKhau,
							ckdv.GiaTri,
							ckdv.LaPhanTram,							
							nv.TenNhanVien						
						into #tblWhere
						FROM NS_ChietKhauDichVu ckdv
						JOIN DM_DonViQuiDoi dvqd ON dvqd.id = ckdv.IdDonViQuiDoi
						JOIN DM_HangHoa hh ON hh.Id = dvqd.IdHangHoa
						join NS_NhanVien nv on ckdv.IdNhanVien = nv.Id
						LEFT JOIN DM_NhomHangHoa nhh ON nhh.Id = hh.IdNhomHangHoa
						WHERE ckdv.TenantId = @TenantId
							AND ckdv.IsDeleted = 0
							AND ckdv.IdChiNhanh = @IdChiNhanh
							AND (@IdNhanVien is null or ckdv.IdNhanVien = @IdNhanVien)
							AND (ISNULL(@Filter,'') = ''
								OR LOWER(hh.TenHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
								OR LOWER(hh.TenHangHoa_KhongDau) LIKE N'%' + LOWER(@Filter) + N'%'
								OR LOWER(dvqd.MaHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
								OR LOWER(nhh.TenNhomHang) LIKE N'%' + LOWER(@Filter) + N'%'
								OR LOWER(nhh.TenNhomHang_KhongDau) LIKE N'%' + LOWER(@Filter) + N'%'
								OR LOWER(nv.TenNhanVien) LIKE N'%' + LOWER(@Filter) + N'%'
								OR LOWER(nv.MaNhanVien) LIKE N'%' + LOWER(@Filter) + N'%'
							)




							select 
									IdNhanVien,
									IdDonViQuiDoi,
									 IdNhomHangHoa,
									GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien,
									max(HoaHongThucHien) as HoaHongThucHien,
									max(HoaHongYeuCauThucHien) as HoaHongYeuCauThucHien,
									max(HoaHongTuVan) as HoaHongTuVan,
									max(LaPhanTram_HoaHongThucHien) as LaPhanTram_HoaHongThucHien,
									max(LaPhanTram_HoaHongYeuCauThucHien) as LaPhanTram_HoaHongYeuCauThucHien,
									max(LaPhanTram_HoaHongTuVan) as LaPhanTram_HoaHongTuVan
							into #temp
								from
								(

										------- pivot GiaTri ---
									select 
										IdNhanVien,
										IdDonViQuiDoi,
										 IdNhomHangHoa,
										GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien,
										[1] as HoaHongThucHien,
										[2] as HoaHongYeuCauThucHien,
										[3] as HoaHongTuVan,
										0 as LaPhanTram_HoaHongThucHien,
										0 as LaPhanTram_HoaHongYeuCauThucHien,
										0 as LaPhanTram_HoaHongTuVan
									from
									(
									  select  GiaTri,   IdNhanVien, IdNhomHangHoa, IdDonViQuiDoi, LoaiChietKhau,
										GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien
									  from #tblWhere
									) d
									pivot
									(
										max(GiaTri)
									  for LoaiChietKhau in ([1],[2],[3])
									) piv

									union all

									------- pivot LaPhanTram ---
									select 
										IdNhanVien, IdDonViQuiDoi,
										 IdNhomHangHoa,
										GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien,
										0 as HoaHongThucHien,
										0 as HoaHongYeuCauThucHien,
										0 as HoaHongTuVan,
										[1] as LaPhanTram_HoaHongThucHien,
										[2] as LaPhanTram_HoaHongYeuCauThucHien,
										[3] as LaPhanTram_HoaHongTuVan
									from
									(
									  select   IdNhanVien, IdDonViQuiDoi, IdNhomHangHoa, LoaiChietKhau, iif(LaPhanTram ='1',1,0) as A,
										  GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien
									  from #tblWhere
									) d
									pivot
									(
										max(A)
									  for LoaiChietKhau in ([1],[2],[3])
									) piv
							)tbl
							group by IdNhanVien,IdDonViQuiDoi, GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien, IdNhomHangHoa
									
					
					SELECT 
						IdNhanVien,
						IdDonViQuiDoi,
						 IdNhomHangHoa,
						TenNhanVien,
						TenDichVu,
						GiaDichVu,
						TenNhomDichVu,
						HoaHongThucHien,
						HoaHongYeuCauThucHien,
						HoaHongTuVan,					
						 ----thay đổi kiểu dữ liệu từ int --> bit,
						cast(iif(LaPhanTram_HoaHongThucHien =1,'1','0') as bit) as LaPhanTram_HoaHongThucHien, 
						cast(iif(LaPhanTram_HoaHongYeuCauThucHien =1,'1','0') as bit) as LaPhanTram_HoaHongYeuCauThucHien,
						cast(iif(LaPhanTram_HoaHongTuVan =1,'1','0') as bit) as LaPhanTram_HoaHongTuVan  
					FROM #temp
					ORDER BY
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenDichVu' THEN TenDichVu END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenNhomDichVu' THEN TenNhomDichVu END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'giaDichVu' THEN GiaDichVu END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'hoaHongThucHien' THEN HoaHongThucHien END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'hoaHongYeuCauThucHien' THEN HoaHongYeuCauThucHien END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'hoaHongTuVan' THEN HoaHongTuVan END ASC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenDichVu' THEN TenDichVu END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenNhomDichVu' THEN TenNhomDichVu END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'giaDichVu' THEN GiaDichVu END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'hoaHongThucHien' THEN HoaHongThucHien END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'hoaHongYeuCauThucHien' THEN HoaHongYeuCauThucHien END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'hoaHongTuVan' THEN HoaHongTuVan END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = '' THEN TenNhanVien END DESC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenNhanVien' THEN TenNhanVien END ASC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenNhanVien' THEN TenNhanVien END DESC
					OFFSET  @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
					
					-- Tổng số bản ghi 
					SELECT COUNT(*) AS TotalCount FROM #temp;

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
		iif(br.TrangThai=1,N'Kích hoạt',N'Chưa kích hoạt') as TxtTrangThai
	from HT_SMSBrandname br
	where br.IsDeleted='0'
	and (@TenantId = 1 or br.TenantId = @TenantId) --- get all brandname (if HOST) - or only get brandname by tenantId
	and (@Keyword ='' or br.Brandname like @Keyword)
	and (@TrangThais ='' or exists (select GiaTri from dbo.fnSplitstring(@TrangThais) tt where br.TrangThai = tt.GiaTri))
	),
	count_cte
	as 
	(
		select count(Id) as TotalRow
		from data_cte
	)
	select dt.*,
		tenant.TenancyName,
		tenant.Name as DisplayTenantName,
		count_cte.*,
		isnull(tblThuChi.TongTienNap,0) as TongTienNap,
		isnull(tblThuChi.TongTienNap,0) - DaSuDung as ConLai
	from data_cte dt
	Left join AbpTenants tenant on dt.TenantId = tenant.Id
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
		CASE WHEN @SortType = 'asc' AND @SortBy = 'tenancyName' THEN tenant.TenancyName END ASC,
		CASE WHEN @SortType = 'asc' AND @SortBy = 'displayTenantName' THEN tenant.Name END ASC,
		CASE WHEN @SortType = 'asc' AND @SortBy = 'tongTienNap' THEN tblThuChi.TongTienNap END ASC,

		CASE WHEN @SortType = 'desc' AND @SortBy = 'brandname' THEN Brandname END DESC,
		CASE WHEN @SortType = 'desc' AND @SortBy = 'sdtCuaHang' THEN SDTCuaHang END DESC,
		CASE WHEN @SortType = 'desc' AND @SortBy = 'tenancyName' THEN NgayKichHoat END DESC,
		CASE WHEN @SortType = 'desc' AND @SortBy = 'displayTenantName' THEN tenant.TenancyName END DESC,
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
						bk.StartTime
					from
					(
					select bk.Id,
						bk.IdKhachHang, 
						bk.BookingDate,
						bk.StartTime, ----todo lấy trường này để check gửi trước ? tiếng
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
								and b.NgaySinhNhat > @birthday_dayFrom
								and b.NgaySinhNhat < @birthday_dayTo
							
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
			and bk.BookingDate >= @FromDate and bk.BookingDate < @ToDate
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
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[ApplyAll_SetupHoaHongDV]");
        }
    }
}
