using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20231218 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[GetAllSetup_HoaHongDichVu]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[AddMultiple_ChietKhauDichVu_toMultipleNhanVien]");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetAllSetup_HoaHongDichVu]
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
										GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien,
										[1] as HoaHongThucHien,
										[2] as HoaHongYeuCauThucHien,
										[3] as HoaHongTuVan,
										0 as LaPhanTram_HoaHongThucHien,
										0 as LaPhanTram_HoaHongYeuCauThucHien,
										0 as LaPhanTram_HoaHongTuVan
									from
									(
									  select  GiaTri,   IdNhanVien, IdDonViQuiDoi, LoaiChietKhau,
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
										GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien,
										0 as HoaHongThucHien,
										0 as HoaHongYeuCauThucHien,
										0 as HoaHongTuVan,
										[1] as LaPhanTram_HoaHongThucHien,
										[2] as LaPhanTram_HoaHongYeuCauThucHien,
										[3] as LaPhanTram_HoaHongTuVan
									from
									(
									  select   IdNhanVien, IdDonViQuiDoi,LoaiChietKhau, iif(LaPhanTram ='1',1,0) as A,
										  GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien
									  from #tblWhere
									) d
									pivot
									(
										max(A)
									  for LoaiChietKhau in ([1],[2],[3])
									) piv
							)tbl
							group by IdNhanVien,IdDonViQuiDoi, GiaDichVu, TenDichVu, TenNhomDichVu, TenNhanVien
									
					
					SELECT 
						IdNhanVien,
						IdDonViQuiDoi,
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
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[AddMultiple_ChietKhauDichVu_toMultipleNhanVien]
	@TenantId int = 1,
	@IdChiNhanh uniqueidentifier = 'C4FBE44F-C26E-499F-9033-AF9C4E3C6FC3',
	@IdNhanViens varchar(max) ='64A16837-26C6-42FF-B52B-B0FD1F6CB228',
	@IdDonViQuyDois varchar(max) ='3E3232FC-73AB-4BE4-842C-CF2F06D337E1',
	@LoaiChietKhau tinyint = 1,
	@GiaTriChietKhau float =0,
	@LaPhanTram bit = '1',
	@CreatorUserId bigint = 1
AS
BEGIN

	SET NOCOUNT ON;

	declare @tblNhanVien table (IdNhanVien uniqueidentifier primary key)
	insert into @tblNhanVien
	select cast (GiaTri as uniqueidentifier) from dbo.fnSplitstring(@IdNhanViens) where GiaTri !=''

	declare @tblDonViQuyDoi table (IdDonViQuiDoi uniqueidentifier primary key)
	insert into @tblDonViQuyDoi
	select cast (GiaTri as uniqueidentifier) from dbo.fnSplitstring(@IdDonViQuyDois) where GiaTri !=''

	------ delete if exists same nvien same dichvu ---
	delete ck
	from NS_ChietKhauDichVu ck
	where exists (select IdNhanVien from @tblNhanVien nv where ck.IdNhanVien = nv.IdNhanVien)
	and exists (select IdDonViQuiDoi from @tblDonViQuyDoi qd where ck.IdDonViQuiDoi = qd.IdDonViQuiDoi)
	and ck.IdChiNhanh = @IdChiNhanh
	and ck.LoaiChietKhau = @LoaiChietKhau


	declare @idNhanVien uniqueidentifier
	declare _cur cursor for
	select IdNhanVien from @tblNhanVien
	open _cur
	FETCH NEXT FROM _cur
	INTO @idNhanVien
	WHILE @@FETCH_STATUS = 0
	BEGIN   
		 insert into NS_ChietKhauDichVu (Id, TenantId, IdChiNhanh, IdNhanVien, IdDonViQuiDoi, LoaiChietKhau, GiaTri, LaPhanTram, TrangThai, IsDeleted, CreationTime, CreatorUserId)
		 select NEWID(), @TenantId, @IdChiNhanh, @idNhanVien, qd.IdDonViQuiDoi, @LoaiChietKhau, @GiaTriChietKhau, @LaPhanTram, 1, '0', GETDATE(), @CreatorUserId
		 from @tblDonViQuyDoi qd		

		FETCH NEXT FROM _cur
	INTO @idNhanVien
	END
	CLOSE _cur;
	DEALLOCATE _cur;

END
");

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
		checkin.TrangThai as TrangThaiCheckIn,
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

		case checkin.TrangThai
			when 1 then N'Đang chờ'
			when 2 then N'Đang thực hiện'
			when 3 then N'Hoàn thành'
			when 4 then N'Hủy' ---- Khách có check in - nhưng đợi lâu quá ra về
			when 0 then N'Xóa' ---- Thêm sai và xóa
		 end as TxtTrangThaiCheckIn
	from KH_CheckIn checkin
	join DM_KhachHang kh on checkin.IdKhachHang= kh.Id
	left join DM_NhomKhachHang nhom on kh.IdNhomKhach= nhom.Id	
	where checkin.TrangThai in (1,2)
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
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetDMHangHoa]
			@TenantId int =2,
			@TextSearch nvarchar(max)=null,
			@IdNhomHangHoas nvarchar(max)=null,
			@IdLoaiHangHoa int = 0,
			@Where nvarchar(max) =null,
			@CurrentPage int = 1,
			@PageSize int = 1000,
			@ColumnSort varchar(100) ='tenNhomHang',
			@TypeSort varchar(20) = 'DESC'
		AS
		BEGIN
	
		SET NOCOUNT ON;
		if @CurrentPage > 0  set @CurrentPage = @CurrentPage- 1 else set @CurrentPage =0

		--- filter nhomhang ---
		declare @tblNhomHang table(ID varchar(40))
		if isnull(@IdNhomHangHoas,'') ='' set @IdNhomHangHoas =''
		else 
			insert into @tblNhomHang
			select * from dbo.fnGetListNhomHangHoa(@IdNhomHangHoas)

			
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
			hh.Id,		
			qd.Id as IdDonViQuyDoi,	
			hh.IdNhomHangHoa,
			qd.MaHangHoa,
			hh.TenHangHoa,
			isnull(qd.GiaBan,cast (0 as float)) as GiaBan,
			isnull(hh.SoPhutThucHien,cast (0 as float)) as SoPhutThucHien,
			hh.TrangThai,
			hh.MoTa,
			hh.NguoiTao,
			loai.TenLoaiHangHoa,
			isnull(nhom.Color,'') as Color,
			isnull(nhom.TenNhomHang,'') as TenNhomHang,
			case hh.trangthai
				when 1 then N'Đang kinh doanh'
				when 0 then N'Ngừng kinh doanh'
			else '' end as TxtTrangThaiHang
		from DM_HangHoa hh
		left join DM_LoaiHangHoa loai on hh.IdLoaiHangHoa = loai.Id
		left join DM_NhomHangHoa nhom on hh.IdNhomHangHoa = nhom.Id
		left join DM_DonViQuiDoi qd on hh.Id= qd.IdHangHoa
		where hh.TenantId = @TenantId
		and LaDonViTinhChuan =1
		and (@IdLoaiHangHoa = 0 or hh.IdLoaiHangHoa = @IdLoaiHangHoa)
		and (@IdNhomHangHoas='' or exists (select * from @tblNhomHang nhomh where hh.IdNhomHangHoa= nhomh.ID))
		and (@TextSearch ='' or  
			(hh.TenHangHoa like @TextSearch or hh.TenHangHoa_KhongDau like @TextSearch 
			or qd.MaHangHoa like @TextSearch 
			or hh.MoTa like @TextSearch
			or nhom.TenNhomHang  like @TextSearch
			or nhom.TenNhomHang_KhongDau like @TextSearch))
			),
			count_cte
			as
			(
				select  
				count(Id) as TotalRow,
				ceiling(count(Id)/ CAST(@PageSize as float)) as TotalPage
				from data_cte
			)
			select *
			from data_cte
			cross join count_cte
			order by 
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='MaHangHoa' then MaHangHoa end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='MaHangHoa' then MaHangHoa end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='TenHangHoa' then TenHangHoa end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='TenHangHoa' then TenHangHoa end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='GiaBan' then GiaBan end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='GiaBan' then GiaBan end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='TenNhomHang' then TenNhomHang end ASC,
			case when @TypeSort <> 'DESC' then ''
			when @ColumnSort='TenNhomHang' then TenNhomHang end DESC
		OFFSET (@CurrentPage* @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY;

 END;");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetChiTietHoaDon_byIdHoaDon]
	@IdHoaDon uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON;
	  select 
			cthd.*,
			qd.IdHangHoa,
			qd.MaHangHoa,
			hh.TenHangHoa,
			hh.TrangThai as TrangThaiHang,
			isnull(nv.TenNhanViens,'') as TenNVThucHiens
		from BH_HoaDon_ChiTiet cthd
		join DM_DonViQuiDoi qd on cthd.IdDonViQuyDoi = qd.Id
		join DM_HangHoa hh on qd.IdHangHoa= hh.Id
		left join
		(
			select  distinct nvthOut.IdHoaDonChiTiet,
			(
				
				select  nv.TenNhanVien +' ,' as [text()]
				from BH_NhanVienThucHien nvth
				join NS_NhanVien nv on nvth.IdNhanVien = nv.Id
				where nvth.IdHoaDonChiTiet = nvthOut.IdHoaDonChiTiet
				and nvth.IsDeleted='0'
				For XML PATH ('')
			)TenNhanViens
			from BH_NhanVienThucHien nvthOut
		) nv on cthd.Id = nv.IdHoaDonChiTiet
		where cthd.IdHoaDon= @IdHoaDon
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
				and qct.IsDeleted='0'
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
			and qctIn.IsDeleted='0'
			For XML PATH ('') 
			) sPhuongThucTT 
		from QuyHoaDon_ChiTiet qct
		where IdHoaDonLienQuan= @IdHoaDonLienQuan
		group by qct.IdQuyHoaDon
	) qct on qhd.Id= qct.IdQuyHoaDon
END");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetAllSoQuy]
                @TenantId INT = 1,
                @IdChiNhanh NVARCHAR(MAX) ='2324F320-30F3-4182-BE92-E6D11B107601',
				@FromDate datetime = null,
				@ToDate datetime = null,
                @Filter NVARCHAR(MAX) ='',
                @SortBy VARCHAR(50) ='ngayLapHoaDon', 
                @SortType VARCHAR(4)='desc', 
                @SkipCount INT = 2,
                @MaxResultCount INT = 2
            AS
            BEGIN
			if(ISNULL(@ToDate,'')!='') set @ToDate = DATEADD(DAY,1,@ToDate)
			if(@SkipCount > 0) set @SkipCount = @SkipCount -1

			; with data_cte
			as
			(
				select 
					qhd.Id,
                    qhd.IdChiNhanh,
					qhd.IdBrandname,
					qhd.IdLoaiChungTu,
					qhd.NgayLapHoaDon,
					qhd.MaHoaDon,                      
                    qhd.CreationTime,
					qhd.NoiDungThu,  
					qhd.TongTienThu,  
					qhd.TrangThai, 
					qct.IdKhachHang,
					qct.IdNhanVien,
					qct.IdHoaDonLienQuan,
					 CASE
                        WHEN qct.HinhThucThanhToan = 1 THEN N'Tiền mặt'
                        WHEN qct.HinhThucThanhToan = 2 THEN N'Chuyển khoản'
                        WHEN qct.HinhThucThanhToan = 3 THEN N'Quẹt thẻ'
                        WHEN qct.HinhThucThanhToan = 4 THEN N'Thẻ giá trị'
                        WHEN qct.HinhThucThanhToan = 5 THEN N'Sử dụng điểm'
                        ELSE N''
                    END AS HinhThucThanhToan
				from QuyHoaDon qhd
				join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
				where qhd.TenantId = @TenantId
				AND (@IdChiNhanh ='' OR exists (select * from dbo.fnSplitstring(@IdChiNhanh) cn where qhd.IdChiNhanh= cn.GiaTri))
				and (@FromDate is null or qhd.NgayLapHoaDon > @FromDate)
				and (@ToDate is null or qhd.NgayLapHoaDon < @ToDate)
			    AND qhd.IsDeleted = 0
				and qct.IsDeleted='0'
			),
			tbl_Where
			as
			(
				select cte.*,
					iif(cte.IdBrandname is not null, br.Brandname,
						iif(cte.IdKhachHang is not null, kh.TenKhachHang, iif(cte.IdNhanVien is null, N'Khách lẻ', nv.TenNhanVien))
						) as TenNguoiNop,
					iif(cte.IdBrandname is not null, br.SDtCuaHang,
						iif(cte.IdKhachHang is not null, kh.TenKhachHang, iif(cte.IdNhanVien is null, N'Khách lẻ', nv.TenNhanVien))
						) as SDTNguoiNop
				from data_cte cte
				left join DM_KhachHang kh on cte.IdKhachHang = kh.Id
				left join NS_NhanVien nv on cte.IdNhanVien = nv.Id
				left join HT_SMSBrandname br on cte.IdBrandname = br.Id ---- sử dụng cho host: khi khách nạp tiền brandname ---
				left join AbpTenants tn on br.TenantId = tn.Id
				where (ISNULL(@Filter, '') = ''
						----- tim kiem doi tuong nop tien ----
							OR LOWER(cte.HinhThucThanhToan) LIKE N'%'+LOWER(@Filter) +'%'
							OR LOWER(kh.TenKhachHang) LIKE N'%'+LOWER(@Filter) +'%'
							OR LOWER(kh.TenKhachHang_KhongDau) LIKE N'%'+LOWER(@Filter) +'%'
							OR LOWER(nv.TenNhanVien) LIKE N'%'+LOWER(@Filter) +'%'
							OR LOWER(br.Brandname) LIKE N'%'+LOWER(@Filter) +'%'
							OR LOWER(tn.TenancyName) LIKE N'%'+LOWER(@Filter) +'%'
							OR LOWER(tn.Name) LIKE N'%'+LOWER(@Filter) +'%'                          
						)
			),
			tblPThuc
			as 
			(
			select dtOut.Id,
				(
				select 											
					HinhThucThanhToan + ', ' AS [text()]
				from tbl_Where dtInt 
				where dtOut.Id = dtInt.Id
				for xml path('')
				) sPhuongThuc
			from tbl_Where dtOut
			group by dtOut.Id
			),
			tblGroup
			as
			(
			select 
					dtOut.Id,
					dtOut.IdChiNhanh,
					dtOut.IdBrandname,
					dtOut.MaHoaDon,
					dtOut.NgayLapHoaDon,
					dtOut.TongTienThu,
					dtOut.TrangThai,
					dtOut.IdKhachHang,
					dtOut.IdNhanVien,
					dtOut.IdLoaiChungTu,
					dtOut.NoiDungThu,
					dtOut.TenNguoiNop,
					dtOut.SDTNguoiNop,
					dtOut.CreationTime,
					max(dtOut.IdHoaDonLienQuan) as IdHoaDonLienQuan
			from tbl_Where dtOut
			group by dtOut.Id,
					dtOut.IdChiNhanh,
					dtOut.IdBrandname,
					dtOut.MaHoaDon,
					dtOut.NgayLapHoaDon,
					dtOut.TongTienThu,
					dtOut.TrangThai,
					dtOut.IdKhachHang,
					dtOut.IdNhanVien,
					dtOut.IdLoaiChungTu,
					dtOut.NoiDungThu,
					dtOut.TenNguoiNop,
					dtOut.SDTNguoiNop,
					dtOut.CreationTime

			),
			tblCount
			as(
				select count(*) as TotalCount
				from tblGroup gr
			)
			select gr.*,
				tblCount.TotalCount,
				lct.TenLoaiChungTu as LoaiPhieu,
				STUFF(pt.sPhuongThuc,len(pt.sPhuongThuc),1,'') as SHinhThucThanhToan,
				CASE WHEN gr.TrangThai = 1 THEN N'Đã thanh toán' ELSE N'Đã hủy' END AS TxtTrangThai
			from tblGroup gr
			left Join DM_LoaiChungTu lct on gr.IdLoaiChungTu = lct.Id
			left join tblPThuc pt on gr.Id= pt.Id
			cross join tblCount			
            ORDER BY
                CASE WHEN @SortBy = 'loaiPhieu' AND LOWER(@SortType) = 'asc' THEN lct.TenLoaiChungTu END ASC,
                CASE WHEN @SortBy = 'tongTienThu' AND LOWER(@SortType) = 'asc' THEN TongTienThu END ASC,
                CASE WHEN @SortBy = 'maHoaDon' AND LOWER(@SortType) = 'asc' THEN MaHoaDon END ASC,
                CASE WHEN @SortBy = 'trangThai' AND LOWER(@SortType) = 'asc' THEN gr.TrangThai END ASC,                     
				CASE WHEN @SortBy = 'ngayLapHoaDon' AND LOWER(@SortType) = 'asc' THEN NgayLapHoaDon END asc,
				CASE WHEN @SortBy = 'tenNguoiNop' AND LOWER(@SortType) = 'asc' THEN TenNguoiNop END asc,

                CASE WHEN @SortBy = 'loaiPhieu' AND LOWER(@SortType) = 'desc' THEN lct.TenLoaiChungTu END DESC,
                CASE WHEN @SortBy = 'tongTienThu' AND LOWER(@SortType) = 'desc' THEN TongTienThu END DESC,
                CASE WHEN @SortBy = 'maHoaDon' AND LOWER(@SortType) = 'desc' THEN MaHoaDon END DESC,
                CASE WHEN @SortBy = 'trangThai' AND LOWER(@SortType) = 'desc' THEN gr.TrangThai END DESC,
                CASE WHEN @SortBy = 'ngayLapHoaDon' AND LOWER(@SortType) = 'desc' THEN NgayLapHoaDon END DESC,
				CASE WHEN @SortBy = 'tenNguoiNop' AND LOWER(@SortType) = 'desc' THEN TenNguoiNop END DESC

				OFFSET (@SkipCount* @MaxResultCount) ROWS
				FETCH NEXT @MaxResultCount ROWS ONLY
				
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[GetAllSetup_HoaHongDichVu]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[AddMultiple_ChietKhauDichVu_toMultipleNhanVien]");
        }
    }
}
