using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20240228 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetAllSoQuy]
                @TenantId INT = 1,
                @IdChiNhanh NVARCHAR(MAX) ='2324F320-30F3-4182-BE92-E6D11B107601',
				@FromDate datetime = null,
				@ToDate datetime = null,
                @Filter NVARCHAR(MAX) ='',
                @SortBy VARCHAR(50) ='ngayLapHoaDon', 
                @SortType VARCHAR(4)='desc', 
                @SkipCount INT = 1,
                @MaxResultCount INT = 100
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
					iif(IdLoaiChungTu = 11, TongTienThu,0) as TienThu,
					iif(IdLoaiChungTu = 12, TongTienThu,0) as TienChi,
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
				select count(*) as TotalCount,
					sum(TienThu) as SumTongTienThu,
					sum(TienChi) as SumTongTienChi
				from tblGroup gr
			)
			select gr.*,
				tblCount.TotalCount,
				tblCount.SumTongTienThu,
				tblCount.SumTongTienChi,
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
		if isnull(@DateTo,'')!='' set @DateTo = DATEADD(day,1,@DateTo)
	
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
		and hd.IsDeleted='0'
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
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[BaoCaoHoaHongChiTiet]
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
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[BaoCaoHoaHongTongHop]
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

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spImportDanhMucHangHoa]
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
						iif(@IdLoaiHangHoa !=1,'0','1'), 
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

			declare @maxFormat NVARCHAR(max)= FORMAT(@max, 'F0');
			set @MaHangHoa = concat(@maloaihang, iif(@max < 10, '0'+ @maxFormat, @maxFormat)) --- nếu mã <10: thêm số 0 ở đầu		
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
