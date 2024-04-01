using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddStoreSql20240227 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoHoaHongTongHop]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoHoaHongChiTiet]");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[BaoCaoHoaHongTongHop]
	@IdChiNhanhs nvarchar(max)='',
	@IdLoaiChungTus nvarchar(max)='',
	@FromDate datetime = null,
	@ToDate datetime = null,
	@TextSearch nvarchar(max)='',
	@ColumnSort varchar(40)='TenNhanVien',
	@TypeSort nvarchar(max)='ASC',
	@CurrentPage int =1,
	@PageSize int = 10
AS
BEGIN

	SET NOCOUNT ON;

	
	declare @tblChiNhanh table (Id uniqueidentifier)
	declare @tblLoaiChungTu table (Id int)

	
	if @CurrentPage > 0 set @CurrentPage = @CurrentPage -1;
			

	if isnull(@FromDate,'')=''
		set @FromDate = '2023-01-01'
	if isnull(@ToDate,'')=''
		set @ToDate = GETDATE()

	if isnull(@IdChiNhanhs,'')!=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
	else set @IdChiNhanhs =''

	if isnull(@IdLoaiChungTus,'')!=''
		insert into @tblLoaiChungTu
		select GiaTri from dbo.fnSplitstring(@IdLoaiChungTus)
	else set @IdLoaiChungTus =''

	if isnull(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%',@TextSearch, '%')
	else set @TextSearch ='%%'

	

	;with data_cte
	as
	(
		select 
			tblHoaHong.IdNhanVien,
			nv.MaNhanVien,
			nv.TenNhanVien,
			HoaHongThucHien_TienChietKhau,
			HoaHongTuVan_TienChietKhau,
			HoaHongThucHien_TienChietKhau + HoaHongTuVan_TienChietKhau as TongHoaHong
		from
		(
			select 
				th.IdNhanVien,
				sum(iif(th.LoaiChietKhau = 1, th.TienChietKhau,0)) as HoaHongThucHien_TienChietKhau,
				sum(iif(th.LoaiChietKhau = 3, th.TienChietKhau,0)) as HoaHongTuVan_TienChietKhau
			from
			(
				--- hd from - to --
				select	
					hd.Id
				from BH_HoaDon hd	
				where hd.IsDeleted='0'
				and (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where hd.IdChiNhanh = cn.Id))
				and (@IdLoaiChungTus ='' or exists (select id from @tblLoaiChungTu ct where hd.IdLoaiChungTu = ct.Id))
				and hd.NgayLapHoaDon between @FromDate and @ToDate
			)hd
			join BH_HoaDon_ChiTiet ct on hd.Id = ct.IdHoaDon
			join BH_NhanVienThucHien th on ct.Id = th.IdHoaDonChiTiet
			where ct.IsDeleted='0'
			and th.IsDeleted='0'
			group by th.IdNhanVien
	   )tblHoaHong
	   join NS_NhanVien nv on tblHoaHong.IdNhanVien = nv.Id
	   where nv.IsDeleted='0'
		and  (@TextSearch =''
			  or nv.TenNhanVien like @TextSearch or nv.MaNhanVien like @TextSearch)
	),
	tbl_sum
	as
	(
		select count(IdNhanVien) as TotalRow,
			sum(HoaHongThucHien_TienChietKhau) as SumHoaHongThucHien,
			sum(HoaHongTuVan_TienChietKhau) as SumHoaHongTuVan,
			sum(TongHoaHong) as SumTongHoaHong
		from data_cte
	)
	select *
	from data_cte
	cross join tbl_sum
	order by 
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
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[BaoCaoHoaHongChiTiet]
	@IdChiNhanhs nvarchar(max)='',
	@IdLoaiChungTus nvarchar(max)='',
	@IdNhomHangs nvarchar(max)='',
	@FromDate datetime = null,
	@ToDate datetime = null,
	@TextSearch nvarchar(max)='',
	@ColumnSort varchar(40)='NgayLapHoaDon',
	@TypeSort nvarchar(max)='DESC',
	@CurrentPage int =1,
	@PageSize int = 50
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
				ct.ThanhTienSauCK,
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
	tbl_sum
	as
	(
		select count(IdNhanVien) as TotalRow,			
			sum(HoaHongThucHien_TienChietKhau) as SumHoaHongThucHien,
			sum(HoaHongTuVan_TienChietKhau) as SumHoaHongTuVan,
			sum(TongHoaHong) as SumTongHoaHong
		from data_cte
	)
	select *
	from data_cte
	cross join tbl_sum
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoHoaHongTongHop]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoHoaHongChiTiet]");
        }
    }
}
