using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStore20240930 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetChiTiet_SuDungGDV_ofCustomer]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoSuDungGDV_ChiTiet]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetNhatKySuDungGDV_ofKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetListIdNhom_byParentId]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[FnCheckChiTietGDV_DaSuDung]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[FnCheckGDV_DaSuDung]");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetChiTiet_SuDungGDV_ofCustomer]
    @IdChiNhanhs [nvarchar](max) = null,
	@IdCustomer [nvarchar](max) = null,
	@TextSearch nvarchar(max) = null,
	@DateFrom datetime = null,
	@DateTo datetime = null,
	@TrangThaiConBuoi tinyint = 2 --- 2./all, 1.conbuoi,0.hetbuoi
AS
BEGIN
    SET NOCOUNT ON;

	declare @tblChiNhanh table (Id uniqueidentifier)
	if isnull(@IDChiNhanhs,'')!=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IDChiNhanhs)
	else set @IdChiNhanhs ='' 
	
	if isnull(@TextSearch,'')='' set @TextSearch =''
		else set @TextSearch = concat('%',@TextSearch,'%')

	if isnull(@IdCustomer,'')=''  set @IdCustomer ='00000000-0000-0000-0000-000000000000' 


	select 
		ctGDV.*,
		qd.IdHangHoa,
		qd.MaHangHoa,
		qd.GiaBan,
		qd.TenDonViTinh,
		hh.IdLoaiHangHoa, 
		hh.TenHangHoa,
		ISNULL(hh.IdNhomHangHoa,'00000000-0000-0000-0000-000000000001') as IdNhomHangHoa
	from
	(
		select 
			tbl.MaHoaDon,
			tbl.NgaylapHoaDon,		
			ctm.IdHoaDon,
			ctm.Id as IdChiTietHoaDon,
			ctm.IdDonViQuyDoi,
			ctm.GhiChu,
			ctm.DonGiaTruocCK,
			ctm.TienChietKhau,
			ctm.DonGiaSauCK,
			ctm.ThanhTienSauCK,
			ctm.SoLuong - ISNULL(ctt.SoLuongTra,0) as SoLuongMua,
    		ISNULL(ctt.SoLuongDung,0) as SoLuongDung,
    		round(ctm.SoLuong - ISNULL(ctt.SoLuongTra,0) - ISNULL(ctt.SoLuongDung,0),2) as SoLuongConLai
    	from
			(
			select 
					hd.ID,
					hd.MaHoaDon, 
					hd.IdChiNhanh,					
					hd.NgayLapHoaDon								
				from dbo.BH_HoaDon hd
				where hd.IdLoaiChungTu = 2 and hd.TrangThai = 3 
				and (@IDChiNhanhs ='' or exists (select id from @tblChiNhanh cn where hd.IdChiNhanh = cn.Id))
				and (@IdCustomer ='00000000-0000-0000-0000-000000000000' or hd.IdKhachHang = @IdCustomer)
			) tbl 
		join dbo.BH_HoaDon_ChiTiet ctm on tbl.ID = ctm.IdHoaDon 	   	
    	left join 
    	(
    		select a.IdChiTietHoaDon,
    			SUM(a.SoLuongTra) as SoLuongTra,
    			SUM(a.SoLuongDung) as SoLuongDung
    		from
    			(-- sum soluongtra
    			select ct.IdChiTietHoaDon,
    				SUM(ct.SoLuong) as SoLuongTra,
    				0 as SoLuongDung
    			from BH_HoaDon_ChiTiet ct 
    			join BH_HoaDon hd on ct.IdHoaDon = hd.ID
    			where hd.TrangThai= 3 and hd.IdLoaiChungTu = 6 
				and ct.IsDeleted='0'
				and (@IdCustomer ='00000000-0000-0000-0000-000000000000' or hd.IdKhachHang = @IdCustomer)
    			group by ct.IdChiTietHoaDon
    
    			union all
    			-- sum soluong sudung
    			select ct.IdChiTietHoaDon,
    				0 as SoLuongDung,
    				SUM(ct.SoLuong) as SoLuongDung
    			from BH_HoaDon_ChiTiet ct 
    			join BH_HoaDon hd on ct.IdHoaDon = hd.ID
    			where hd.TrangThai= 3 and hd.IdLoaiChungTu = 1   
				and ct.IsDeleted='0'
				and (@IdCustomer ='00000000-0000-0000-0000-000000000000' or hd.IdKhachHang = @IdCustomer)
    			group by ct.IdChiTietHoaDon
    			) a group by a.IdChiTietHoaDon
    	) ctt on ctm.ID = ctt.IdChiTietHoaDon 
		where ctm.IsDeleted ='0'
		)ctGDV
		join DM_DonViQuiDoi qd on ctGDV.IdDonViQuyDoi = qd.ID
    	join DM_HangHoa hh on qd.IdHangHoa = hh.ID
		where (@TrangThaiConBuoi = 2 or (SoLuongConLai = 0 and @TrangThaiConBuoi = 0)
			or (@TrangThaiConBuoi = 1 and SoLuongConLai > 0))
		and (@TextSearch ='' or ctGDV.MaHoaDon like @TextSearch or hh.TenHangHoa like @TextSearch
			or hh.TenHangHoa_KhongDau like @TextSearch or qd.MaHangHoa like @TextSearch)
		order by ctGDV.NgayLapHoaDon
END");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[BaoCaoSuDungGDV_ChiTiet]
	@IdChiNhanhs  nvarchar(max)= '',
	@FromDate datetime = '2024-09-01',
	@ToDate datetime= '2024-09-30', 
	@TextSearch nvarchar(max)='Laser',	
	@CurrentPage int =1,
	@PageSize int= 10
AS
BEGIN

	SET NOCOUNT ON;
	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1
	declare @tblChiNhanh table(Id uniqueidentifier)
	if isnull(@IdChiNhanhs,'') ='' set @IdChiNhanhs =''
	else 
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
		where GiaTri !=''

	if isnull(@TextSearch,'') ='' set @TextSearch =''
	else set @TextSearch = concat(N'%',@TextSearch, '%')
		
	;with ctsd as
	(
		select 		
			ct.Id as IdChitietSD,	
			hd.MaHoaDon as MaHoaDonSD,
			hd.NgayLapHoaDon as NgayLapHoaDonSD,
			ct.IdChiTietHoaDon,			
			ct.SoLuong as SoLuongSD
		from BH_HoaDon hd
		join BH_HoaDon_ChiTiet ct on hd.Id = ct.IdHoaDon
		where hd.IsDeleted='0'
		and hd.TrangThai = 3
		and hd.IdLoaiChungTu = 1
		and hd.NgayLapHoaDon between @FromDate and @ToDate
		and ct.IdChiTietHoaDon is not null
		and ct.IsDeleted='0'
		and (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where hd.IdChiNhanh= cn.Id))
	),	
	ctmua
	as
	(
		----- get ctgdv ---
		select 
			ct.IdHoaDon
		from BH_HoaDon_ChiTiet ct 
		where exists (select IdChitietSD from ctsd where ctsd.IdChiTietHoaDon = ct.Id)
		and ct.IsDeleted='0'				
	),
	allCTMua
	as
	(
		select 
			hd.Id as IDGoiDV,
			hd.IdKhachHang,
			hd.MaHoaDon as MaGoiDichVu,
			hd.NgayLapHoaDon as NgayMuaGDV,
			ct.Id as IdChiTietMua,
			ct.IdDonViQuyDoi,
			ct.SoLuong as SoLuongMua,
			ct.DonGiaSauCK,
			ct.ThanhTienSauCK
		from BH_HoaDon hd
		join BH_HoaDon_ChiTiet ct on hd.Id = ct.IdHoaDon
		where hd.IsDeleted='0'
		and hd.TrangThai = 3
		and hd.IdLoaiChungTu = 2
		and exists (select idhoadon from ctmua where ctmua.IdHoaDon = hd.Id)
		and ct.IsDeleted='0'	
	),
	ctAll
	as(
	select 
		tblJoin.MaKhachHang,
		tblJoin.TenKhachHang,
		tblJoin.SoDienThoai,
		tblJoin.IDGoiDV,
		tblJoin.IdKhachHang,
		tblJoin.MaGoiDichVu,
		tblJoin.NgayMuaGDV,
		tblJoin.IdChiTietMua,
		tblJoin.IdDonViQuyDoi,
		iif(tblJoin.RN >1 ,'',tblJoin.MaHangHoa) as MaHangHoa,
		iif(tblJoin.RN >1 ,'',tblJoin.TenHangHoa) as TenHangHoa,
		iif(tblJoin.RN >1 ,0,tblJoin.SoLuongMua) as SoLuongMua,
		iif(tblJoin.RN >1 ,0,tblJoin.DonGiaSauCK) as DonGiaSauCK,
		iif(tblJoin.RN >1 ,0,tblJoin.ThanhTienSauCK) as ThanhTienSauCK,

		tblJoin.IdChitietSD,
		tblJoin.MaHoaDonSD,
		tblJoin.NgayLapHoaDonSD,
		tblJoin.SoLuongSD,
		tblJoin.GiaTriSuDung
	from
	(
	select ctmua.*,
		ctsd.IdChitietSD,
		ctsd.IdChiTietHoaDon,
		ctsd.NgayLapHoaDonSD,		
		isnull(ctsd.SoLuongSD,0) as SoLuongSD,
		isnull(ctsd.MaHoaDonSD,'') as MaHoaDonSD,
		isnull(ctsd.SoLuongSD,0) * ctmua.DonGiaSauCK as GiaTriSuDung,
		qd.MaHangHoa,
		hh.TenHangHoa,
		hh.TenHangHoa_KhongDau,
		kh.MaKhachHang,
		kh.TenKhachHang,
		kh.TenKhachHang_KhongDau,
		kh.SoDienThoai,
		ROW_NUMBER() over (partition by ctmua.IdChiTietMua order by ctmua.NgayMuaGDV) as RN
	from allCTMua ctmua
	join DM_KhachHang kh on ctmua.IdKhachHang = kh.Id
	join DM_DonViQuiDoi qd on ctmua.IdDonViQuyDoi = qd.Id
	join DM_HangHoa hh on qd.IdHangHoa = hh.Id
	left join ctsd on ctmua.IdChiTietMua = ctsd.IdChiTietHoaDon
	)tblJoin
	where (@TextSearch ='' or tblJoin.MaGoiDichVu like @TextSearch or tblJoin.MaHoaDonSD like @TextSearch
			or tblJoin.MaKhachHang like @TextSearch or tblJoin.TenKhachHang like @TextSearch
			or tblJoin.TenKhachHang_KhongDau like @TextSearch or tblJoin.SoDienThoai like @TextSearch
			or tblJoin.MaHangHoa like @TextSearch or tblJoin.TenHangHoa like @TextSearch or tblJoin.TenHangHoa_KhongDau like @TextSearch)
	),
	count_cte
	as(	
		select count(*) as TotalRow
		from ctAll
	)
	select *
	from ctAll dt
	cross join count_cte
	order by  dt.IdChiTietMua   
    OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
");
			migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetNhatKySuDungGDV_ofKhachHang]
	@IdCustomer uniqueidentifier = null,
	@IdGoiDichVu uniqueidentifier = null,
	@TextSearch nvarchar(max)='',
	@CurrentPage int = 0,
	@PageSize int = 10
AS
BEGIN
	
	SET NOCOUNT ON;
	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1
	if ISNULL(@TextSearch,'') ='' set @TextSearch =''
	else set @TextSearch = concat(N'%',@TextSearch,'%')

	
	;with data_cte
	as(
		select 
		hd.*,
		ct.Id as IdChiTietSD,
		ct.IdDonViQuyDoi,
		ct.SoLuong as SoLuongSD,
		ct.DonGiaSauCK,
		ct.SoLuong * ct.DonGiaSauCK as GiaTriSuDung,
		qd.MaHangHoa,
		hh.TenHangHoa
	from
	(
		select hd.Id as IdHoaDonSD, hd.MaHoaDon as MaHoaDonSD, hd.NgayLapHoaDon as NgayLapHoaDonSD
		from BH_HoaDon hd
		where hd.IdKhachHang = @IdCustomer
		and hd.IdLoaiChungTu = 1
		and hd.IsDeleted='0' 
		and hd.TrangThai = 3
	)hd
	join BH_HoaDon_ChiTiet ct on hd.IdHoaDonSD= ct.IdHoaDon
	join DM_DonViQuiDoi qd on ct.IdDonViQuyDoi = qd.Id
	join DM_HangHoa hh on qd.IdHangHoa= hh.Id
	where ct.IdChiTietHoaDon is not null
	and ct.IsDeleted ='0'
	and (@TextSearch ='' or hd.MaHoaDonSD like @TextSearch
		or qd.MaHangHoa like @TextSearch or hh.TenHangHoa like @TextSearch or hh.TenHangHoa_KhongDau like @TextSearch)
	and (@IdGoiDichVu is null or exists (
		select gdv.Id from BH_HoaDon gdv
		join BH_HoaDon_ChiTiet ctm on gdv.Id = ctm.IdHoaDon
		where gdv.Id = @IdGoiDichVu and ctm.Id = ct.IdChiTietHoaDon
		))
	),
	count_cte
	as(	
		select count(IdChiTietSD) as TotalRow
		from data_cte
	)
	select *
	from data_cte dt
	cross join count_cte
	order by dt.NgayLapHoaDonSD
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
END");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetListIdNhom_byParentId]
	@IdNhomHangHoas nvarchar(max)=''
AS
BEGIN
	
	SET NOCOUNT ON;

		declare @tblNhomSearch table (ID uniqueidentifier)
		insert into @tblNhomSearch
		select * from dbo.fnSplitstring(@IdNhomHangHoas) where GiaTri!=''
	
			declare @tblNhomHang table(ID uniqueidentifier)
			if isnull(@IdNhomHangHoas,'') !='' 
			begin			
				declare @id uniqueidentifier
			
				declare _cur cursor for
				select ID from @tblNhomSearch
				open _cur
				fetch next from _cur
				into @id
				WHILE @@FETCH_STATUS = 0
					begin		
						insert into @tblNhomHang
						select * from dbo.fnGetListNhomHangHoa(@id)
						fetch next from _cur
						into @id
					end

				CLOSE _cur;
				DEALLOCATE _cur;
			end

			select * from @tblNhomHang
END");

            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[FnCheckChiTietGDV_DaSuDung]
(
	@IdChiTietGDV uniqueidentifier
)
RETURNS bit
AS
BEGIN
	declare @count int =0

	select @count = count (ct.Id)
	from BH_HoaDon_ChiTiet ct
	join BH_HoaDon hd on ct.IdHoaDon = hd.Id
	where ct.IdChiTietHoaDon = @IdChiTietGDV
	and hd.TrangThai = 3

	if isnull(@count,0) > 0 return '1'
	return '0';
END");
            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[FnCheckGDV_DaSuDung]
(
	@IdGoiDV uniqueidentifier
)
RETURNS bit
AS
BEGIN
	declare @count int =0
	declare @ctMuaGoc table(Id uniqueidentifier)
	insert into @ctMuaGoc 
	select Id from BH_HoaDon_ChiTiet where IdHoaDon= @IdGoiDV

	select @count = count (ct.Id)
	from BH_HoaDon_ChiTiet ct
	join BH_HoaDon hd on ct.IdHoaDon = hd.Id
	where exists (select * from @ctMuaGoc  ctg where ctg.Id = ct.IdChiTietHoaDon)
	and hd.TrangThai = 3

	if isnull(@count,0) > 0 return '1'
	return '0';
END");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[BaoCaoHoaHongChiTiet]
	@IdChiNhanhs nvarchar(max)='',
	@IdLoaiChungTus nvarchar(max)='',
	@IdNhomHangs nvarchar(max)='',
	@FromDate datetime = null,
	@ToDate datetime = null,
	@TextSearch nvarchar(max)='',
	@ColumnSort varchar(40)='',
	@TypeSort nvarchar(max)='DESC',
	@CurrentPage int =1,
	@PageSize int = 20
AS
BEGIN

	SET NOCOUNT ON;

	
	declare @tblChiNhanh table (Id uniqueidentifier)
	declare @tblNhomHang table (Id uniqueidentifier)
	declare @tblLoaiChungTu table (Id int)

	if @CurrentPage > 0 set @CurrentPage = @CurrentPage -1;
			
	if isnull(@FromDate,'')=''
		set @FromDate = '2023-01-01'
	if isnull(@ToDate,'')=''
		set @ToDate = GETDATE()
	else 
		set @ToDate =DATEADD(day,1,@ToDate)

	if isnull(@ColumnSort,'') ='' set @ColumnSort ='NgayLapHoaDon'
--	select @ColumnSort, @TypeSort


	if isnull(@IdChiNhanhs,'')!=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
	else set @IdChiNhanhs =''

	if isnull(@IdLoaiChungTus,'')!=''
		insert into @tblLoaiChungTu
		select GiaTri from dbo.fnSplitstring(@IdLoaiChungTus)
	else set @IdLoaiChungTus =''

	if isnull(@IdNhomHangs,'')!=''
		insert into @tblNhomHang
		select GiaTri from dbo.fnSplitstring(@IdNhomHangs)
	else set @IdNhomHangs =''


	if isnull(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%',@TextSearch, '%')
	else set @TextSearch ='%%'

	
	;with data_cte
	as
	(
		select 
			*,
			HoaHongThucHien_TienChietKhau + HoaHongTuVan_TienChietKhau as TongHoaHong
		from
		(
			select 
				th.IdHoaDonChiTiet, 
				hd.MaHoaDon, 
				hd.NgayLapHoaDon,
				nv.MaNhanVien,
				nv.TenNhanVien,
				kh.MaKhachHang,
				isnull(kh.TenKhachHang, N'Khách lẻ') as TenKhachHang,
				th.IdNhanVien,
				qd.MaHangHoa,
				hh.TenHangHoa,
				nhomhh.TenNhomHang,		
				ct.SoLuong,
				(ct.DonGiaSauCK * ct.SoLuong) as ThanhTienSauCK, -- vi sudung GDV, thanhtien = 0
				iif(th.LoaiChietKhau = 1, th.PTChietKhau,0) as HoaHongThucHien_PTChietKhau,
				iif(th.LoaiChietKhau = 1, th.TienChietKhau,0) as HoaHongThucHien_TienChietKhau,
				iif(th.LoaiChietKhau = 3, th.PTChietKhau,0) as HoaHongTuVan_PTChietKhau,
				iif(th.LoaiChietKhau = 3, th.TienChietKhau,0) as HoaHongTuVan_TienChietKhau
			from
			(
				--- hd from - to --
				select	
					hd.Id,
					hd.MaHoaDon, 
					hd.NgayLapHoaDon,
					hd.IdKhachHang
				from BH_HoaDon hd	
				where hd.IsDeleted='0'
				and (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where hd.IdChiNhanh = cn.Id))
				and (@IdLoaiChungTus ='' or exists (select id from @tblLoaiChungTu ct where hd.IdLoaiChungTu = ct.Id))
				and hd.NgayLapHoaDon between @FromDate and @ToDate
			)hd
			left join DM_KhachHang kh on hd.IdKhachHang = kh.Id
			join BH_HoaDon_ChiTiet ct on hd.Id = ct.IdHoaDon
			join BH_NhanVienThucHien th on ct.Id = th.IdHoaDonChiTiet
			left join NS_NhanVien nv on th.IdNhanVien = nv.Id
			join DM_DonViQuiDoi qd on ct.IdDonViQuyDoi = qd.Id
			join DM_HangHoa hh on qd.IdHangHoa= hh.Id
			left join DM_NhomHangHoa nhomhh on hh.IdNhomHangHoa =  nhomhh.Id			
			where ct.IsDeleted='0'
			and th.IsDeleted='0'
			and nv.IsDeleted='0'
			and	(@TextSearch =''
			or hd.MaHoaDon like @TextSearch
			  or nv.TenNhanVien like @TextSearch or nv.MaNhanVien like @TextSearch
			  or kh.MaKhachHang like @TextSearch or kh.TenKhachHang like @TextSearch or kh.TenKhachHang_KhongDau like @TextSearch
			  or qd.MaHangHoa like @TextSearch  or hh.TenHangHoa like @TextSearch or hh.TenHangHoa_KhongDau like @TextSearch
			  )
	   )tblHoaHong
	),
	tbl_sumChiTiet
	as(
		select 
			sum(SoLuong) as SumSoLuong,
			sum(ThanhTienSauCK) as SumThanhTienSauCK
		from
		(
			select 
				IdHoaDonChiTiet,
				SoLuong,
				ThanhTienSauCK,
				ROW_NUMBER() over (partition by IdHoaDonChiTiet order by IdHoaDonChiTiet) as RN
			from data_cte
		)tblCT 
		where RN = 1
	),
	tbl_sumHoaHong
	as
	(
		select count(IdNhanVien) as TotalRow,						
			sum(HoaHongThucHien_TienChietKhau) as SumHoaHongThucHien,
			sum(HoaHongTuVan_TienChietKhau) as SumHoaHongTuVan,
			sum(TongHoaHong) as SumTongHoaHong
		from data_cte
	)
	select *
	from data_cte dt
	cross join tbl_sumChiTiet sumct 
	cross join tbl_sumHoaHong sumck
	order by 
	case when @TypeSort <> 'ASC' then ''
	when @ColumnSort='MaHoaDon' then MaHoaDon end ASC,
	case when @TypeSort <> 'DESC' then ''
	when @ColumnSort='MaHoaDon' then MaHoaDon end DESC,
	case when @TypeSort <> 'ASC' then ''
	when @ColumnSort='NgayLapHoaDon' then NgayLapHoaDon end ASC,
	case when @TypeSort <> 'DESC' then ''
	when @ColumnSort='NgayLapHoaDon' then NgayLapHoaDon end DESC,
	case when @TypeSort <> 'ASC' then ''
	when @ColumnSort='MaNhanVien' then MaNhanVien end ASC,
	case when @TypeSort <> 'DESC' then ''
	when @ColumnSort='MaNhanVien' then MaNhanVien end DESC,
	case when @TypeSort <> 'ASC' then ''
	when @ColumnSort='TenNhanVien' then TenNhanVien end ASC,
	case when @TypeSort <> 'DESC' then ''
	when @ColumnSort='TenNhanVien' then TenNhanVien end DESC,
	case when @TypeSort <> 'ASC' then 0
	when @ColumnSort='HoaHongThucHien_TienChietKhau' then HoaHongThucHien_TienChietKhau end ASC,
	case when @TypeSort <> 'DESC' then 0
	when @ColumnSort='HoaHongThucHien_TienChietKhau' then HoaHongThucHien_TienChietKhau end DESC,
	case when @TypeSort <> 'ASC' then 0
	when @ColumnSort='HoaHongTuVan_TienChietKhau' then HoaHongTuVan_TienChietKhau end ASC,
	case when @TypeSort <> 'DESC' then 0
	when @ColumnSort='HoaHongTuVan_TienChietKhau' then HoaHongTuVan_TienChietKhau end DESC,
	case when @TypeSort <> 'ASC' then 0
	when @ColumnSort='TongHoaHong' then TongHoaHong end ASC,
	case when @TypeSort <> 'DESC' then 0
	when @ColumnSort='TongHoaHong' then TongHoaHong end DESC
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
		iif(qhd.IdLoaiChungTu=11,N'Phiếu thu',N'Phiếu chi') as LoaiPhieu,
		CASE WHEN qhd.TrangThai = 1 THEN N'Đã thanh toán' ELSE N'Đã hủy' END AS TxtTrangThai,
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
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetDMHangHoa]
			@TenantId int =1,
			@TextSearch nvarchar(max)=null,
			@IdNhomHangHoas nvarchar(max)='',
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
		insert into @tblNhomHang
		exec GetListIdNhom_byParentId @IdNhomHangHoas
	
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
			isnull(nhom.ThuTuHienThi,10) as ThuTuHienThi,
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
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_getKhachHang_Booking]
	@TenantId int= 1,
	@IdChiNhanhs nvarchar(max)= null,
	@TextSearch nvarchar(max) =null,
	@FromDate datetime = '2024-09-08',
	@ToDate datetime = '2024-09-20',
	@CurrentPage int =0,
	@PageSize int = 10,
	@TrangThaiBook int =3  ---0.xoa, 3.all
AS
BEGIN
	
	SET NOCOUNT ON;

	if(isnull(@TextSearch,'')!='') set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch=''

	declare @filterDate bit= '1'
	if @FromDate is null and @ToDate is null
		set @filterDate ='0'

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
	and bk.IsDeleted='0'
	and (@filterDate ='0' or bk.BookingDate between @FromDate and @ToDate)
	and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where bk.IdChiNhanh= cn.ID))
	and (@TrangThaiBook = 3 or  bk.TrangThai = @TrangThaiBook)
	and not exists (select id from Booking_CheckIn_HoaDon bkHD where bk.Id = bkHD.IdBooking and bkHD.IsDeleted='0') ---khong lay khach dang checkin 
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetChiTiet_SuDungGDV_ofCustomer]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoSuDungGDV_ChiTiet]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetNhatKySuDungGDV_ofKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetListIdNhom_byParentId]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[FnCheckChiTietGDV_DaSuDung]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[FnCheckGDV_DaSuDung]");
        }
    }
}
