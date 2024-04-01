using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreSql20240301 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoChiTietCongNo]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoTaiChinh_ChiTietSoQuy]");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[BaoCaoChiTietCongNo]
	@IdChiNhanhs nvarchar(max)='2324f320-30f3-4182-be92-e6d11b107601',
	@IdLoaiChungTus nvarchar(max)='',
	@NgayLapHoaDon_FromDate datetime = null,
	@NgayLapHoaDon_ToDate datetime = null,
	@TextSearch nvarchar(max)='',
	@TextSearchDichVu nvarchar(max)='',
	@ColumnSort varchar(40)='',
	@TypeSort nvarchar(max)='DESC',
	@CurrentPage int =1,
	@PageSize int = 20
AS
BEGIN
	
	SET NOCOUNT ON;

	declare @tblChiNhanh table (Id uniqueidentifier)
	declare @tblLoaiChungTu table (Id int)

	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1

	if isnull(@ColumnSort,'')=''
		set @ColumnSort = 'TenKhachHang'

	if isnull(@NgayLapHoaDon_ToDate,'')!='' 
		set @NgayLapHoaDon_ToDate = DATEADD(day,1,@NgayLapHoaDon_ToDate)

	if isnull(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%',@TextSearch, '%')
	else set @TextSearch ='%%'

	if isnull(@TextSearchDichVu,'')!=''
		set @TextSearchDichVu = CONCAT(N'%',@TextSearchDichVu, '%')
	else set @TextSearchDichVu ='%%'


	if isnull(@IdChiNhanhs,'')!=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
		else set @IdChiNhanhs =''

	if isnull(@IdLoaiChungTus,'')!=''
		insert into @tblLoaiChungTu
		select GiaTri from dbo.fnSplitstring(@IdLoaiChungTus)
	else set @IdLoaiChungTus =''

	; with data_cte
	as
	(
	select *
	from
	(
		select 
			cthd.*,
			qd.MaHangHoa,
			hh.TenHangHoa,
			hh.TenHangHoa_KhongDau,

			kh.SoDienThoai,
			isnull(kh.MaKhachHang,'KL') as MaKhachHang,
			isnull(kh.TenKhachHang, N'Khách lẻ') as TenKhachHang,
			isnull(kh.TenKhachHang_KhongDau, N'Khach le') as TenKhachHang_KhongDau,
			row_number() over(partition by cthd.Id order by cthd.Id) as RN
		from
		(
		select		
			hd.Id,
			hd.IdKhachHang,
			hd.MaHoaDon,
			hd.NgayLapHoaDon,
			hd.TongThanhToan,
			ct.SoLuong,
			ct.DonGiaSauVAT,
			ct.ThanhTienSauVAT,
			ct.IdDonViQuyDoi
		from BH_HoaDon hd
		join BH_HoaDon_ChiTiet ct on hd.Id= ct.IdHoaDon
		where hd.IsDeleted='0'
			and ct.IsDeleted='0'
			and (@NgayLapHoaDon_FromDate is null or hd.NgayLapHoaDon >= @NgayLapHoaDon_FromDate)
			and (@NgayLapHoaDon_ToDate is null or hd.NgayLapHoaDon < @NgayLapHoaDon_ToDate)
			and (@IdLoaiChungTus ='' or exists (select id from @tblLoaiChungTu ct where hd.IdLoaiChungTu = ct.Id))
			and (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where hd.IdChiNhanh = cn.Id))
		)cthd
		left join DM_KhachHang kh on cthd.IdKhachHang = kh.Id
		left join DM_DonViQuiDoi qd on cthd.IdDonViQuyDoi = qd.Id
		left join DM_HangHoa hh on qd.IdHangHoa = hh.Id
	) tblwhere
	where (@TextSearch =''
			or MaHoaDon like @TextSearch
			or MaKhachHang like @TextSearch or SoDienThoai like @TextSearch
			or TenKhachHang like @TextSearch or TenKhachHang_KhongDau like @TextSearch			
			)
	and (@TextSearchDichVu =''
			or MaHangHoa like @TextSearchDichVu
			or TenHangHoa like @TextSearchDichVu or TenHangHoa_KhongDau like @TextSearchDichVu		
			)
	),
	tblSumHD
	as
	(
		select 
			sum(TongThanhToan) as SumTongThanhToan
		from data_cte
		where RN = 1
	),
	tblSumCT
	as
	(
		select 
			count(Id) as TotalRow,
			sum(SoLuong) as SumSoLuong,
			sum(ThanhTienSauVAT) as SumThanhTienSauVAT
		from data_cte
	),
	tblSq
	as
	(
		select 
			hd.id,			
			isnull(sq.KhachDaTra,0) as KhachDaTra,
			hd.TongThanhToan - isnull(sq.KhachDaTra,0) as ConNo
		from data_cte hd
		left join
		(
			select 
				qct.IdHoaDonLienQuan,
				sum(iif(qhd.IdLoaiChungTu = 11, qct.TienThu, - qct.TienThu)) as KhachDaTra
			from QuyHoaDon qhd
			join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
			where qhd.IsDeleted='0'
			and qct.IsDeleted='0'
			and qhd.TrangThai !=0
			group by qct.IdHoaDonLienQuan
		)sq on hd.Id = sq.IdHoaDonLienQuan
		where RN = 1
	),
	tblSumSq
	as
	(
		select sum(KhachDaTra) as SumKhachDaTra,
			sum(ConNo) as SumConNo
		from tblSq
	)
	select dt.*,
		sq.KhachDaTra,
		sq.ConNo,		
		TotalRow,
		SumSoLuong,
		SumThanhTienSauVAT,
		SumTongThanhToan,
		SumKhachDaTra,
		SumConNo
	from data_cte dt
	left join tblSq sq on dt.Id= sq.Id
	cross join tblSumHD
	cross join tblSumCT
	cross join tblSumSq
	order by 
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='MaPhieuThuChi' then MaKhachHang end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='MaPhieuThuChi' then MaKhachHang end DESC,
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='NgayLapPhieu' then TenKhachHang end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='NgayLapPhieu' then TenKhachHang end DESC,
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='MaNguoiNopTien' then MaHoaDon end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='MaNguoiNopTien' then MaHoaDon end DESC,
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='TenNguoiNopTien' then NgayLapHoaDon end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='TenNguoiNopTien' then NgayLapHoaDon end DESC,
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='Thu_TienMat' then MaHangHoa end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='Thu_TienMat' then MaHangHoa end DESC,
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='Thu_TienChuyenKhoan' then TenHangHoa end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='HoaHongTuVan_TienChietKhau' then TenHangHoa end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Thu_TienQuyetThe' then SoLuong end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='Thu_TienQuyetThe' then SoLuong end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Chi_TienMat' then ThanhTienSauVAT end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='Chi_TienMat' then ThanhTienSauVAT end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Chi_TienChuyenKhoan' then TongThanhToan end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='Chi_TienChuyenKhoan' then TongThanhToan end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Chi_TienQuyetThe' then DonGiaSauVAT end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='Chi_TienQuyetThe' then DonGiaSauVAT end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='TienThu' then KhachDaTra end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='TienThu' then KhachDaTra end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='TienChi' then ConNo end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='TienChi' then ConNo end DESC
		OFFSET (@CurrentPage* @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY
END");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[BaoCaoTaiChinh_ChiTietSoQuy]
	@IdChiNhanhs nvarchar(max)='2324f320-30f3-4182-be92-e6d11b107601',
	@IdLoaiChungTus nvarchar(max)='',
	@NgayLapPhieuThuChi_FromDate datetime = '2024-02-01',
	@NgayLapPhieuThuChi_ToDate datetime =  '2024-02-29',
	@NgayLapHoaDon_FromDate datetime = null,
	@NgayLapHoaDon_ToDate datetime = null,
	@TextSearch nvarchar(max)='',
	@ColumnSort varchar(40)='',
	@TypeSort nvarchar(max)='DESC',
	@CurrentPage int =1,
	@PageSize int = 20
AS
BEGIN
	
	SET NOCOUNT ON;

	declare @tblChiNhanh table (Id uniqueidentifier)
	declare @tblLoaiChungTu table (Id int)

	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1

	if isnull(@ColumnSort,'')=''
		set @ColumnSort = 'NgayLapPhieu'
	

	if isnull(@NgayLapPhieuThuChi_ToDate,'')!='' 
		set @NgayLapPhieuThuChi_ToDate = DATEADD(day,1,@NgayLapPhieuThuChi_ToDate)

	if isnull(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%',@TextSearch, '%')
	else set @TextSearch ='%%'


	if isnull(@IdChiNhanhs,'')!=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
		else set @IdChiNhanhs =''

	if isnull(@IdLoaiChungTus,'')!=''
		insert into @tblLoaiChungTu
		select GiaTri from dbo.fnSplitstring(@IdLoaiChungTus)
	else set @IdLoaiChungTus =''


		;with data_cte
		as(
		select *
		from
		(
			select 
				sq.Id,
				sq.MaHoaDon as MaPhieuThuChi,
				sq.NgayLapHoaDon as NgayLapPhieu,
				sq.NoiDungThu,
				sq.Thu_TienMat,
				sq.Thu_TienChuyenKhoan,
				sq.Thu_TienQuyetThe,

				sq.Chi_TienMat,
				sq.Chi_TienChuyenKhoan,
				sq.Chi_TienQuyetThe,

				hd.MaHoaDon,
				hd.NgayLapHoaDon,
				iif(sq.IdNhanVien is null, isnull(kh.MaKhachHang, 'KL'), nv.MaNhanVien) as MaNguoiNopTien,
				iif(sq.IdNhanVien is null, isnull(kh.TenKhachHang, N'Khách lẻ'), nv.TenNhanVien) as TenNguoiNopTien,
				iif(sq.IdNhanVien is null, isnull(kh.TenKhachHang_KhongDau, 'Khach le'), nv.TenNhanVien) as TenNguoiNopTien_KhongDau,

				TienThu,
				TienChi

			from
			(
				select 
					qhd.Id,
					qhd.MaHoaDon,
					qhd.NgayLapHoaDon,
					qhd.NoiDungThu,
					qhd.IdLoaiChungTu,
					qct.IdKhachHang,
					qct.IdNhanVien,
					qct.IdHoaDonLienQuan,

					iif(qhd.IdLoaiChungTu = 11, qct.TienThu,0) as TienThu,
					iif(qhd.IdLoaiChungTu = 11, iif(qct.HinhThucThanhToan = 1, qct.TienThu,0),0) as Thu_TienMat,
					iif(qhd.IdLoaiChungTu = 11, iif(qct.HinhThucThanhToan = 2, qct.TienThu,0),0) as Thu_TienChuyenKhoan,
					iif(qhd.IdLoaiChungTu = 11, iif(qct.HinhThucThanhToan = 3, qct.TienThu,0),0) as Thu_TienQuyetThe,

					iif(qhd.IdLoaiChungTu = 12, qct.TienThu,0) as TienChi,
					iif(qhd.IdLoaiChungTu = 12, iif(qct.HinhThucThanhToan = 1, qct.TienThu,0),0) as Chi_TienMat,
					iif(qhd.IdLoaiChungTu = 12, iif(qct.HinhThucThanhToan = 2, qct.TienThu,0),0) as Chi_TienChuyenKhoan,
					iif(qhd.IdLoaiChungTu = 12, iif(qct.HinhThucThanhToan = 3, qct.TienThu,0),0) as Chi_TienQuyetThe

				from QuyHoaDon qhd
				join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
				where qhd.IsDeleted='0'
				and qct.IsDeleted='0'
				and qhd.TrangThai = 1
				and (@NgayLapPhieuThuChi_FromDate is null or qhd.NgayLapHoaDon >= @NgayLapPhieuThuChi_FromDate)
				and (@NgayLapPhieuThuChi_ToDate is null or qhd.NgayLapHoaDon < @NgayLapPhieuThuChi_ToDate)
				and (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where qhd.IdChiNhanh = cn.Id))
			)sq
			left join BH_HoaDon hd on sq.IdHoaDonLienQuan = hd.Id and hd.IsDeleted='0'
			left join DM_KhachHang kh on sq.IdKhachHang = kh.Id
			left join NS_NhanVien nv on sq.IdNhanVien = nv.Id
			where (@NgayLapHoaDon_FromDate is null or hd.NgayLapHoaDon >= @NgayLapHoaDon_FromDate)
			and (@NgayLapHoaDon_ToDate is null or hd.NgayLapHoaDon < @NgayLapHoaDon_ToDate)
			and (@IdLoaiChungTus ='' or exists (select id from @tblLoaiChungTu ct where hd.IdLoaiChungTu = ct.Id))
		)tblWhere
		where (@TextSearch =''
			or MaHoaDon like @TextSearch
			or MaPhieuThuChi like @TextSearch
			or TenNguoiNopTien like @TextSearch or MaNguoiNopTien like @TextSearch
			or TenNguoiNopTien_KhongDau like @TextSearch
			)
		),
		tblGr
		as
		(
		select 
			dt.Id,
			dt.MaPhieuThuChi, 
			dt.NgayLapPhieu,
			dt.NoiDungThu,
			dt.MaNguoiNopTien,
			dt.TenNguoiNopTien,
			max(MaHoaDonLienQuans) as MaHoaDonLienQuans,
			sum(Thu_TienMat) as Thu_TienMat,
			sum(Thu_TienChuyenKhoan) as Thu_TienChuyenKhoan,
			sum(Thu_TienQuyetThe) as Thu_TienQuyetThe,
			sum(Chi_TienMat) as Chi_TienMat,
			sum(Chi_TienChuyenKhoan) as Chi_TienChuyenKhoan,
			sum(Chi_TienQuyetThe) as Chi_TienQuyetThe,
			sum(TienThu) as TienThu,
			sum(TienChi) as TienChi,
			sum(TienThu - TienChi) as TongThuChi
		from data_cte dt
		left join
		(
			select dtIn.Id as IdPhieuThuChi,
			(
				select distinct MaHoaDon + ', ' as [text()]
				from data_cte tblXML
				where tblXML.Id = dtIn.Id
				for xml path('')
			) MaHoaDonLienQuans
			from data_cte dtIn
			group by dtIn.Id
		)tblHD on dt.Id = tblHD.IdPhieuThuChi
		group by dt.Id,
			dt.MaPhieuThuChi, 
			dt.NgayLapPhieu, 
			dt.NoiDungThu,
			dt.MaNguoiNopTien,
			dt.TenNguoiNopTien
		),
		tblSum
		as
		(
			select COUNT(Id) as TotalRow,
				sum(Thu_TienMat) as Sum_ThuTienMat,
				sum(Thu_TienChuyenKhoan) as Sum_ThuTienChuyenKhoan,
				sum(Thu_TienQuyetThe) as Sum_ThuTienQuyetThe,

				sum(Chi_TienMat) as Sum_ChiTienMat,
				sum(Chi_TienChuyenKhoan) as Sum_ChiTienChuyenKhoan,
				sum(Chi_TienQuyetThe) as Sum_ChiTienQuyetThe,
				sum(TienThu) as SumTienThu,
				sum(TienChi) as SumTienChi,
				sum(TongThuChi) as SumTongThuChi
			from tblGr
		)
		select *
		from tblGr
		cross join tblSum
		order by 
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='MaPhieuThuChi' then MaPhieuThuChi end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='MaPhieuThuChi' then MaPhieuThuChi end DESC,
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='NgayLapPhieu' then NgayLapPhieu end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='NgayLapPhieu' then NgayLapPhieu end DESC,
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='MaNguoiNopTien' then MaNguoiNopTien end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='MaNguoiNopTien' then MaNguoiNopTien end DESC,
		case when @TypeSort <> 'ASC' then ''
		when @ColumnSort='TenNguoiNopTien' then TenNguoiNopTien end ASC,
		case when @TypeSort <> 'DESC' then ''
		when @ColumnSort='TenNguoiNopTien' then TenNguoiNopTien end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Thu_TienMat' then Thu_TienMat end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='Thu_TienMat' then Thu_TienMat end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Thu_TienChuyenKhoan' then Thu_TienChuyenKhoan end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='HoaHongTuVan_TienChietKhau' then Thu_TienChuyenKhoan end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Thu_TienQuyetThe' then Thu_TienQuyetThe end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='Thu_TienQuyetThe' then Thu_TienQuyetThe end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Chi_TienMat' then Chi_TienMat end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='Chi_TienMat' then Chi_TienMat end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Chi_TienChuyenKhoan' then Chi_TienChuyenKhoan end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='Chi_TienChuyenKhoan' then Chi_TienChuyenKhoan end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='Chi_TienQuyetThe' then Chi_TienQuyetThe end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='Chi_TienQuyetThe' then Chi_TienQuyetThe end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='TienThu' then TienThu end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='TienThu' then TienThu end DESC,
		case when @TypeSort <> 'ASC' then 0
		when @ColumnSort='TienChi' then TienChi end ASC,
		case when @TypeSort <> 'DESC' then 0
		when @ColumnSort='TienChi' then TienChi end DESC
		OFFSET (@CurrentPage* @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY
END");

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


			
			if isnull(@Filter,'') =''
			begin
				set @Filter=''
			end
			else 
				set @Filter = N'%' + @Filter + '%'

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
				and qhd.TrangThai != 0
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
				where (@Filter = ''
							OR MaHoaDon LIKE @Filter						
							OR cte.HinhThucThanhToan LIKE @Filter
							OR kh.TenKhachHang LIKE @Filter
							OR kh.TenKhachHang_KhongDau LIKE @Filter
							OR nv.TenNhanVien LIKE @Filter
							OR br.Brandname LIKE @Filter
							OR tn.TenancyName LIKE @Filter
							OR tn.Name LIKE @Filter         
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
	else
		set @ToDate =DATEADD(day,1,@ToDate)

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
				nv.LastModificationTime,
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
				

				select * 
				from #temp nv
				ORDER BY
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenNhanVien' THEN nv.TenNhanVien END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soDienThoai' THEN nv.SoDienThoai END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'diaChi' THEN nv.DiaChi END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngaySinh' THEN nv.NgaySinh END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'gioiTinh' THEN nv.GioiTinh END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'chucVu' THEN nv.TenChucVu END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngayThamGia' THEN nv.NgayVaoLam END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'trangThai' THEN DiaChi END,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenNhanVien' THEN nv.TenNhanVien END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soDienThoai' THEN nv.SoDienThoai END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'diaChi' THEN nv.DiaChi END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngaySinh' THEN nv.NgaySinh END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'gioiTinh' THEN nv.GioiTinh END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'chucVu' THEN nv.TenChucVu END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngayThamGia' THEN nv.NgayVaoLam END DESC,
					CASE WHEN ISNULL(@SortType,'') = '' AND ISNULL(@SortBy,'') = '' THEN nv.LastModificationTime END DESC
				OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

				SELECT COUNT(Id) AS TotalCount
				FROM #temp
				
			END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoChiTietCongNo]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[BaoCaoTaiChinh_ChiTietSoQuy]");
        }
    }
}
