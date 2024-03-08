using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreFuncSql20240308 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop procedure if exists [dbo].[GetThuChi_DauKyCuoiKy]");
            migrationBuilder.Sql(@"drop function if exists [dbo].[FnCheckPermission_ByUserId]");
            migrationBuilder.Sql(@"create PROCEDURE [dbo].[GetThuChi_DauKyCuoiKy]
    @IdChiNhanhs nvarchar(max)='2324f320-30f3-4182-be92-e6d11b107601',
	@IdLoaiChungTus varchar(20)= null, ---- 11.thu.12.chi
	@IdLoaiChungTuLienQuan int = 0, ---- 0.all, -1.khong lienquan hoadon, 1. hoadonban, 4.nhaphang
	@HinhThucThanhToans nvarchar(20)='',
	@IdKhoanThuChi uniqueidentifier = null,
	@IdTaiKhoanNganHang uniqueidentifier = null,
	@TrangThais varchar(20)= null, ---0.huy, 1.hoanthanh
	@FromDate datetime = null,
	@ToDate datetime = null
AS
BEGIN
	
	SET NOCOUNT ON;
	declare @tblChiNhanh table (Id uniqueidentifier)
	declare @tblLoaiChungTu table (Id int)
	declare @tblTrangThai table(TrangThai tinyint)
	declare @tblHinhThucThanhToan table (Id int)

	if isnull(@IdChiNhanhs,'')!=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
		else set @IdChiNhanhs =''

	if isnull(@IdLoaiChungTus,'')!=''
		insert into @tblLoaiChungTu
		select GiaTri from dbo.fnSplitstring(@IdLoaiChungTus)
	else set @IdLoaiChungTus =''

	if isnull(@HinhThucThanhToans,'')!=''
		insert into @tblHinhThucThanhToan
		select GiaTri from dbo.fnSplitstring(@HinhThucThanhToans)
	else set @HinhThucThanhToans =''

	if isnull(@TrangThais,'') ='' set @TrangThais =''
		else 
		insert into @tblTrangThai
		select * from dbo.fnSplitstring(@TrangThais)


				select 
					sum(sq.TonDauKy) as TonDauKy,
					sum(sq.ThuTrongKy) as ThuTrongKy,
					sum(sq.ChiTrongKy) as ChiTrongKy,
					sum(sq.TonCuoiKy) as TonCuoiKy
				from
				(
					select tblHDLienQuan.*
					from
					(
						select 
							sqIn.*,
							iif(sqIn.IdHoaDonLienQuan is null, -1, hd.IdLoaiChungTu) as IdLoaiChungTuLienQuan
						from
						(
							select 
								qct.IdHoaDonLienQuan,
								iif(qhd.NgayLapHoaDon < @FromDate, iif(qhd.IdLoaiChungTu = 11, qct.TienThu,- qct.TienThu),0) as TonDauKy,
								iif(qhd.NgayLapHoaDon between @FromDate and @ToDate, 
									iif(qhd.IdLoaiChungTu = 11, qct.TienThu,0),0) as ThuTrongKy,
								iif(qhd.NgayLapHoaDon between @FromDate and @ToDate, 
									iif(qhd.IdLoaiChungTu = 12, qct.TienThu,0),0) as ChiTrongKy,
								iif(qhd.NgayLapHoaDon < @ToDate, iif(qhd.IdLoaiChungTu = 11, qct.TienThu,- qct.TienThu),0) as TonCuoiKy
						
							from QuyHoaDon qhd
							join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
							where qct.IsDeleted='0'
							and (@IdKhoanThuChi is null or qct.IdKhoanThuChi = @IdKhoanThuChi)
							and (@IdTaiKhoanNganHang is null or qct.IdTaiKhoanNganHang = @IdTaiKhoanNganHang)
							and (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where qhd.IdChiNhanh = cn.Id))
							and (@HinhThucThanhToans = '' or exists (select id from @tblHinhThucThanhToan ht where qct.HinhThucThanhToan= ht.Id))
							and (@TrangThais ='' or exists (select TrangThai from @tblTrangThai tt where qhd.TrangThai = tt.TrangThai))
							and (@IdLoaiChungTus ='' or exists (select * from @tblLoaiChungTu ct where qhd.IdLoaiChungTu = ct.ID))	
						) sqIn
						left join BH_HoaDon hd on sqIn.IdHoaDonLienQuan = hd.Id and hd.IsDeleted='0'		
					)tblHDLienQuan
					where (@IdLoaiChungTuLienQuan = 0 or tblHDLienQuan.IdLoaiChungTuLienQuan = @IdLoaiChungTuLienQuan)
				)sq
				
END");
            migrationBuilder.Sql(@"create FUNCTION [dbo].[FnCheckPermission_ByUserId]
(
	@UserId bigint,
	@PermissionName nvarchar(128)
)
RETURNS bit
AS
BEGIN
	
	DECLARE @ReturnValue bit = '1'

	---- neu admin: full quyen ---
	select @ReturnValue = IsAdmin from AbpUsers where Id = @UserId 
	if @ReturnValue = '0'
		begin
			declare @countRole int
			---- nếu không phải là admnin -- check tiep quyen ---
			set @countRole = (select count(permis.Id)
				from AbpPermissions permis
				join AbpUserRoles urole on permis.RoleId = urole.RoleId
				where urole.UserId = @UserId and Name = @PermissionName)

			---- nếu tên quyền chứa chữ Not ---
			if @PermissionName like N'Not%' ---- không có quyền --
				set @ReturnValue = iif(@countRole > 1, '0','1')
			else 
				set @ReturnValue = iif(@countRole > 1, '1', '0')					
		end

	RETURN @ReturnValue
END");

			migrationBuilder.Sql(@"ALTER FUNCTION [dbo].[fnGetMaPhieuThuChi]
(
	@TenantId int,
	@IdChiNhanh uniqueidentifier= null,
	@IdLoaiChungTu int= 11,
	@NgayLapHoaDon datetime= null
)
RETURNS nvarchar(100)
AS
BEGIN
	-- Declare the return variable here
	declare @strReturn nvarchar(100)
	DECLARE @maxMaHoaDon float = 1
	declare @kihieuchungtu varchar(10),  @lenMaChungTu int =0

	set @kihieuchungtu = (select top 1 MaLoaiChungTu from DM_LoaiChungTu where ID= @IdLoaiChungTu)
	set @lenMaChungTu  = LEN(@kihieuchungtu)


	SET @maxMaHoaDon = 
		(
			select  
				max(CAST(dbo.fnGetNumeric(RIGHT(MaHoaDon,LEN(MaHoaDon)- @lenMaChungTu))AS float)) as MaxNumber
			from
			(
				select MaHoaDon
				from dbo.QuyHoaDon	hd						
				where hd.TenantId = @TenantId
				and  MaHoaDon like @kihieuchungtu +'%'
				---- không check loại chứng từ: vì có thể update từ chi --> thu
			) a
			where ISNUMERIC(RIGHT(MaHoaDon,LEN(MaHoaDon)- @lenMaChungTu)) = 1
		)

	 if @maxMaHoaDon is null
		  RETURN  CONCAT(@kihieuchungtu, '001')		
	else
		begin
			set @maxMaHoaDon = FORMAT(@maxMaHoaDon + 1, 'F0') --- convert dạng mũ 10 về string
			declare @lenMaMax int = len(@maxMaHoaDon)

			if @maxMaHoaDon < 10  
				set @strReturn= CONCAT(@kihieuchungtu,'00',@maxMaHoaDon)
			else
				begin
					if @maxMaHoaDon < 100  
						set @strReturn=  CONCAT(@kihieuchungtu,'0',@maxMaHoaDon)
					else
						set @strReturn=  CONCAT(@kihieuchungtu,@maxMaHoaDon)
				end

		end

	return @strReturn
END");
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetAllSoQuy]
	@TenantId INT = 1,
    @IdChiNhanhs nvarchar(max)='2324f320-30f3-4182-be92-e6d11b107601',
	@IdLoaiChungTus varchar(20)= null, ---- 11.thu.12.chi
	@IdLoaiChungTuLienQuan int = 0, ---- 0.all, -1.khong lienquan hoadon, 1. hoadonban, 4.nhaphang
	@HinhThucThanhToans nvarchar(20)='',
	@IdKhoanThuChi uniqueidentifier = null,
	@IdTaiKhoanNganHang uniqueidentifier = null,
	@TrangThais varchar(20)= null, ---0.huy, 1.hoanthanh
	@FromDate datetime = null,
	@ToDate datetime = null,
    @Filter NVARCHAR(MAX) ='',
    @SortBy VARCHAR(50) ='MaHoaDonLienQuans', 
    @SortType VARCHAR(4)='DESC', 
    @SkipCount INT = 1,
    @MaxResultCount INT = 100
AS
BEGIN
	
	SET NOCOUNT ON;

	declare @tblChiNhanh table (Id uniqueidentifier)
	declare @tblLoaiChungTu table(ID varchar(40))
	declare @tblTrangThai table(TrangThai tinyint)
	declare @tblHinhThucTT table(HinhThucTT tinyint)

	if @SkipCount > 0 set @SkipCount = @SkipCount - 1

	if isnull(@SortBy,'')=''
		set @SortBy = 'NgayLapPhieu'
	

	if isnull(@ToDate,'')!='' 
		set @ToDate = DATEADD(day,1,@ToDate)

	if isnull(@Filter,'')!=''
		set @Filter = CONCAT(N'%',@Filter, '%')
	else set @Filter ='%%'


	if isnull(@IdChiNhanhs,'')!=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
		else set @IdChiNhanhs =''

	if isnull(@IdLoaiChungTus,'') ='' set @IdLoaiChungTus =''
	else 
		insert into @tblLoaiChungTu
		select * from dbo.fnSplitstring(@IdLoaiChungTus)

	if isnull(@TrangThais,'') ='' set @TrangThais =''
		else 
			insert into @tblTrangThai
			select * from dbo.fnSplitstring(@TrangThais)

	if isnull(@HinhThucThanhToans,'') ='' set @HinhThucThanhToans =''
	else 
		insert into @tblHinhThucTT
		select * from dbo.fnSplitstring(@HinhThucThanhToans)


		;with data_cte
		as(
		select tblWhere.*
		from
		(
			select 
				sq.Id,
				sq.IdLoaiChungTu,
				sq.IdChiNhanh,
				sq.IdBrandname,
				sq.MaHoaDon,
				sq.NgayLapHoaDon,
				sq.NoiDungThu,
				sq.TrangThai,
				sq.Thu_TienMat,
				sq.Thu_TienChuyenKhoan,
				sq.Thu_TienQuyetThe,

				sq.Chi_TienMat,
				sq.Chi_TienChuyenKhoan,
				sq.Chi_TienQuyetThe,

				sq.Thu_TienMat - sq.Chi_TienMat as TienMat,
				sq.Thu_TienChuyenKhoan - sq.Chi_TienChuyenKhoan as TienChuyenKhoan,
				sq.Thu_TienQuyetThe - sq.Chi_TienQuyetThe as TienQuyetThe,

				sq.IdHoaDonLienQuan,
				iif(sq.IdHoaDonLienQuan is null, -1, hd.IdLoaiChungTu) as IdLoaiChungTuLienQuan,
				hd.MaHoaDon as MaHoaDonLienQuan,	
				iif(sq.IdNhanVien is null, isnull(kh.MaKhachHang, 'KL'), nv.MaNhanVien) as MaNguoiNopTien,
				iif(sq.IdNhanVien is null, isnull(kh.TenKhachHang, N'Khách lẻ'), nv.TenNhanVien) as TenNguoiNop,
				iif(sq.IdNhanVien is null, isnull(kh.TenKhachHang_KhongDau, 'Khach le'), nv.TenNhanVien) as TenNguoiNop_KhongDau,

				TienThu,
				TienChi,
				TienThu + TienChi as TongTienThu
			from
			(
				select 
					qhd.Id,
					qhd.IdChiNhanh,
					qhd.IdBrandname,
					qhd.MaHoaDon,
					qhd.NgayLapHoaDon,
					qhd.NoiDungThu,
					qhd.IdLoaiChungTu,
					qhd.TrangThai,
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
				where qct.IsDeleted ='0'
				and (@FromDate is null or qhd.NgayLapHoaDon >= @FromDate)
				and (@ToDate is null or qhd.NgayLapHoaDon < @ToDate)
				and (@IdTaiKhoanNganHang is null or qct.IdTaiKhoanNganHang = @IdTaiKhoanNganHang)
				and (@IdKhoanThuChi is null or qct.IdKhoanThuChi = @IdKhoanThuChi)
				and (@TrangThais ='' or exists (select TrangThai from @tblTrangThai tt where qhd.TrangThai = tt.TrangThai))
				and (@IdLoaiChungTus ='' or exists (select * from @tblLoaiChungTu ct where qhd.IdLoaiChungTu = ct.ID))			
				and (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where qhd.IdChiNhanh = cn.Id))
				and (@HinhThucThanhToans ='' or exists (select pt.HinhThucTT from @tblHinhThucTT pt where pt.HinhThucTT = qct.HinhThucThanhToan))
			)sq
			left join BH_HoaDon hd on sq.IdHoaDonLienQuan = hd.Id and hd.IsDeleted='0'							
			left join DM_KhachHang kh on sq.IdKhachHang = kh.Id
			left join NS_NhanVien nv on sq.IdNhanVien = nv.Id		
		)tblWhere
		where (@Filter =''
			or MaHoaDon like @Filter
			or MaHoaDonLienQuan like @Filter
			or TenNguoiNop like @Filter or MaNguoiNopTien like @Filter
			or TenNguoiNop_KhongDau like @Filter
			)
			and (@IdLoaiChungTuLienQuan = 0 or tblWhere.IdLoaiChungTuLienQuan = @IdLoaiChungTuLienQuan)
		),
		tblGr
		as
		(
		select 
			dt.Id,
			dt.IdLoaiChungTu,
			dt.IdChiNhanh,
			dt.IdBrandname,
			dt.MaHoaDon, 
			dt.NgayLapHoaDon,
			dt.NoiDungThu,
			dt.MaNguoiNopTien,
			dt.TenNguoiNop,
			dt.TrangThai,
			max(dt.IdHoaDonLienQuan) as IdHoaDonLienQuan,
			max(MaHoaDonLienQuans) as MaHoaDonLienQuans,
			sum(Thu_TienMat) as Thu_TienMat,
			sum(Thu_TienChuyenKhoan) as Thu_TienChuyenKhoan,
			sum(Thu_TienQuyetThe) as Thu_TienQuyetThe,
			sum(Chi_TienMat) as Chi_TienMat,
			sum(Chi_TienChuyenKhoan) as Chi_TienChuyenKhoan,
			sum(Chi_TienQuyetThe) as Chi_TienQuyetThe,
			sum(TienThu) as TienThu,
			sum(TienChi) as TienChi,
			sum(TienThu - TienChi) as TongTienThu,

			sum(TienMat) as TienMat,
			sum(TienChuyenKhoan) as TienChuyenKhoan,
			sum(TienQuyetThe) as TienQuyetThe
		from data_cte dt
		left join
		(
			select dtIn.Id as IdPhieuThuChi,
			(
				select distinct MaHoaDonLienQuan + ', ' as [text()]
				from data_cte tblXML
				where tblXML.Id = dtIn.Id
				for xml path('')
			) MaHoaDonLienQuans
			from data_cte dtIn
			group by dtIn.Id
		)tblHD on dt.Id = tblHD.IdPhieuThuChi
		group by dt.Id,
			dt.IdLoaiChungTu,
			dt.IdChiNhanh,
			dt.IdBrandname,
			dt.MaHoaDon, 
			dt.NgayLapHoaDon, 
			dt.NoiDungThu,
			dt.MaNguoiNopTien,
			dt.TenNguoiNop,
			dt.TrangThai
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
				sum(TongTienThu) as SumTongThuChi,

				sum(TienMat) as SumTienMat,
				sum(TienChuyenKhoan) as SumTienChuyenKhoan,
				sum(TienQuyetThe) as SumTienQuyetThe
			from tblGr
		)
		select tblGr.*, 
			tblSum.*,
			iif(tblGr.IdLoaiChungTu =11, N'Phiếu thu',N'Phiếu chi') as LoaiPhieu,
			iif(tblGr.TrangThai =1, N'Đã thanh toán',N'Đã hủy') as TxtTrangThai
		from tblGr
		cross join tblSum
		order by 
		case when @SortType <> 'ASC' then ''
		when @SortBy='MaHoaDon' then MaHoaDon end ASC,
		case when @SortType <> 'DESC' then ''
		when @SortBy='MaHoaDon' then MaHoaDon end DESC,
		case when @SortType <> 'ASC' then ''
		when @SortBy='MaHoaDonLienQuans' then MaHoaDonLienQuans end ASC,
		case when @SortType <> 'DESC' then ''
		when @SortBy='MaHoaDonLienQuans' then MaHoaDonLienQuans end DESC,
		case when @SortType <> 'ASC' then ''
		when @SortBy='NgayLapHoaDon' then NgayLapHoaDon end ASC,
		case when @SortType <> 'DESC' then ''
		when @SortBy='NgayLapHoaDon' then NgayLapHoaDon end DESC,
		case when @SortType <> 'ASC' then ''
		when @SortBy='MaNguoiNopTien' then MaNguoiNopTien end ASC,
		case when @SortType <> 'DESC' then ''
		when @SortBy='MaNguoiNopTien' then MaNguoiNopTien end DESC,
		case when @SortType <> 'ASC' then ''
		when @SortBy='TenNguoiNop' then TenNguoiNop end ASC,
		case when @SortType <> 'DESC' then ''
		when @SortBy='TenNguoiNop' then TenNguoiNop end DESC,		
		case when @SortType <> 'ASC' then 0
		when @SortBy='TienMat' then TienMat end ASC,
		case when @SortType <> 'DESC' then 0
		when @SortBy='TienMat' then TienMat end DESC,
		case when @SortType <> 'ASC' then 0
		when @SortBy='TienChuyenKhoan' then TienChuyenKhoan end ASC,
		case when @SortType <> 'DESC' then 0
		when @SortBy='TienChuyenKhoan' then TienChuyenKhoan end DESC,
		case when @SortType <> 'ASC' then 0
		when @SortBy='TienQuyetThe' then TienQuyetThe end ASC,
		case when @SortType <> 'DESC' then 0
		when @SortBy='TienQuyetThe' then TienQuyetThe end DESC,
		case when @SortType <> 'ASC' then 0
		when @SortBy='TongTienThu' then TongTienThu end ASC,
		case when @SortType <> 'DESC' then 0
		when @SortBy='TongTienThu' then TongTienThu end DESC
		OFFSET (@SkipCount* @MaxResultCount) ROWS
		FETCH NEXT @MaxResultCount ROWS ONLY
END");
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetChiTietHoaDon_byIdHoaDon]
	@IdHoaDon uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON;
	declare @hdIsDelete bit, @lastTimeDelete varchar(20)
		select top 1 @hdIsDelete = IsDeleted, @lastTimeDelete = format(DeletionTime, 'yyyy-MM-dd HH:mm:ss')
			from BH_HoaDon where id= @IdHoaDon

			
		select ctLast.*
		from
		(
		  select 
				cthd.*,
				format(cthd.DeletionTime, 'yyyy-MM-dd HH:mm:ss') as LastTimeDelete,
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
		)ctLast
		where (@hdIsDelete = '0' and ctLast.IsDeleted ='0') ---- hd chuaxoa: chi get cthd chua xoa
		or (@hdIsDelete ='1' and ctLast.LastTimeDelete = @lastTimeDelete) --- nguoclai: get cthd xoa cuoicung
END");
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetInforHoaDon_byId]
	@Id uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON;

   select 
			hd.*,
			kh.MaKhachHang,
			isnull(kh.TenKhachHang,N'Khách lẻ') as TenKhachHang,
			kh.SoDienThoai,
			isnull(kh.Avatar,'') as Avatar,
			nv.TenNhanVien,
			cn.TenChiNhanh,
			hd.TongTienHDSauVAT - isnull(sq.DaThanhToan,0)  as ConNo,
			isnull(sq.DaThanhToan,0) as DaThanhToan,
			case hd.TrangThai
				when 1 then N'Tạm lưu'
				when 2 then N'Đang xử lý'
				when 3 then N'Hoàn thành'
			else N'Đã hủy' end as TxtTrangThaiHD
		from BH_HoaDon hd
		left join DM_KhachHang kh on hd.IdKhachHang = kh.Id
		left join DM_ChiNhanh cn on hd.IdChiNhanh = cn.Id
		left join NS_NhanVien nv on hd.IdNhanVien= nv.id
		left join
		(		
			select qct.IdHoaDonLienQuan,
					sum(iif(qhd.IdLoaiChungTu=11, qct.TienThu, - qct.TienThu)) as DaThanhToan
				from QuyHoaDon_ChiTiet qct
				join QuyHoaDon qhd on qct.IdQuyHoaDon = qhd.Id
				where qhd.TrangThai= 1
				and qct.IdHoaDonLienQuan = @Id
				and qct.IsDeleted = '0'
				group by qct.IdHoaDonLienQuan
				
		) sq on hd.id= sq.IdHoaDonLienQuan
		where hd.Id= @Id
END");
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListHoaDon]
			@TenantId int =7,
			@IdChiNhanhs nvarchar(max)=null,
			@IdUserLogin bigint = 1,
			@IdLoaiChungTus varchar(20)= null,
			@TrangThaiHDs varchar(20)= null, ---- 1.tamluu, 2.dangxuly, 3.hoanthanh, 0.huy
			@TrangThaiNos varchar(10)= null, ---- 0.hetno, 1.conno		
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
		declare @tblChiNhanh table(ID varchar(40))
		declare @tblLoaiChungTu table(ID varchar(40))
		declare @tblTrangThaiHD table(TrangThai tinyint)
		declare @tblTrangThaiNo table(TrangThai tinyint)

		---- check quyen khong duoc xem hoadon cua NV # ---
		declare @roleView bit = (select dbo.FnCheckPermission_ByUserId(@IdUserLogin,'Pages.HoaDon.Not_XemDanhSach_OtherUser'))	

		set @CurrentPage= @CurrentPage - 1
		if isnull(@DateTo,'')!='' set @DateTo = DATEADD(day,1,@DateTo)	
		
		if isnull(@IdLoaiChungTus,'') ='' set @IdLoaiChungTus =''
		else 
			insert into @tblLoaiChungTu
			select * from dbo.fnSplitstring(@IdLoaiChungTus)
		
		if isnull(@IdChiNhanhs,'') ='' set @IdChiNhanhs =''
		else 
			insert into @tblChiNhanh
			select * from dbo.fnSplitstring(@IdChiNhanhs)

		if isnull(@TrangThaiHDs,'') ='' set @TrangThaiHDs =''
		else 
			insert into @tblTrangThaiHD
			select * from dbo.fnSplitstring(@TrangThaiHDs)

		if isnull(@TrangThaiNos,'') ='' set @TrangThaiNos =''
		else 
			insert into @tblTrangThaiNo
			select * from dbo.fnSplitstring(@TrangThaiNos)

			
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
				select *
				from
				(
					select 
						tblHD.*,				
						isnull(tblSq.TienMat,0) as TienMat,
						isnull(tblSq.TienChuyenKhoan,0) as TienChuyenKhoan,
						isnull(tblSq.TienQuetThe,0) as TienQuetThe,
						isnull(tblSq.DaThanhToan,0) as DaThanhToan,
						tblHD.TongThanhToan - isnull(tblSq.DaThanhToan,0) as ConNo,
						iif(tblHD.TongThanhToan - isnull(tblSq.DaThanhToan,0) > 0,1,0) as TrangThaiNo
					from
					(
						select 
							hd.*,									
							kh.SoDienThoai,
							isnull(kh.Avatar,'') as Avatar,
							isnull(kh.MaKhachHang,N'KL') as MaKhachHang,		
							isnull(kh.TenKhachHang,N'Khách lẻ') as TenKhachHang,
							isnull(kh.TenKhachHang_KhongDau,N'Khach le') as TenKhachHang_KhongDau,
							case hd.TrangThai
								when 1 then N'Tạm lưu'
								when 2 then N'Đang xử lý'
								when 3 then N'Hoàn thành'
							else N'Đã hủy' end as TxtTrangThaiHD
						from BH_HoaDon hd
						left join DM_KhachHang kh on hd.IdKhachHang = kh.Id		
						where hd.TenantId = @TenantId
						and (@IdChiNhanhs =''  or exists (select * from @tblChiNhanh cn where hd.IdChiNhanh = cn.ID))
						and (@IdLoaiChungTus ='' or exists (select * from @tblLoaiChungTu ct where hd.IdLoaiChungTu = ct.ID))
						and (@TrangThaiHDs ='' or exists (select TrangThai from @tblTrangThaiHD tt where hd.TrangThai = tt.TrangThai))
						and (@roleView = 1  or hd.CreatorUserId = @IdUserLogin)
						and (@DateFrom is null or hd.NgayLapHoaDon > @DateFrom)
						and (@DateTo is null or hd.NgayLapHoaDon < @DateTo)
					)tblHD
					left join
					(
						select qct.IdHoaDonLienQuan,
							sum(iif(qhd.IdLoaiChungTu = 11, qct.TienThu, - qct.TienThu)) as DaThanhToan,
							sum(iif(qct.HinhThucThanhToan = 1, iif(qhd.IdLoaiChungTu = 11, qct.TienThu, - qct.TienThu), 0)) as TienMat,
							sum(iif(qct.HinhThucThanhToan = 2, iif(qhd.IdLoaiChungTu = 11, qct.TienThu, - qct.TienThu), 0)) as TienChuyenKhoan,
							sum(iif(qct.HinhThucThanhToan = 3, iif(qhd.IdLoaiChungTu = 11, qct.TienThu, - qct.TienThu), 0)) as TienQuetThe
						from QuyHoaDon_ChiTiet qct
						join QuyHoaDon qhd on qct.IdQuyHoaDon = qhd.Id
						where qhd.TrangThai= 1
						and qct.IsDeleted='0'
						and qhd.IsDeleted='0'
						group by qct.IdHoaDonLienQuan
					)tblSq on tblHD.Id = tblSq.IdHoaDonLienQuan
					where (@TextSearch ='' or  
						(MaHoaDon like @TextSearch or GhiChuHD like @TextSearch 
						 or MaKhachHang like @TextSearch or TenKhachHang like @TextSearch
						 or SoDienThoai like @TextSearch or TenKhachHang_KhongDau like @TextSearch))
				)tblConNo
				where (@TrangThaiNos ='' or exists (select ttNo.TrangThai from @tblTrangThaiNo ttNo where ttNo.TrangThai = tblConNo.TrangThaiNo ))
			),
			tblSum 
			as
			(
				select 
					COUNT(Id) as TotalRow,
					sum(TongTienHang) as SumTongTienHang,
					sum(TongGiamGiaHD) as SumTongGiamGiaHD,
					sum(TongThanhToan) as SumTongThanhToan,
					sum(DaThanhToan) as SumDaThanhToan,
					sum(ConNo) as SumConNo
				from data_cte
			)
			select dt.*,
				tblSum.*,
				nvlap.UserName,
				nv.TenNhanVien,
				cn.TenChiNhanh
			from data_cte dt
			cross join tblSum 
			left join DM_ChiNhanh cn on dt.IdChiNhanh = cn.Id
			left join NS_NhanVien nv on dt.IdNhanVien= nv.id
			left join AbpUsers nvlap on dt.CreatorUserId = nvlap.id
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop procedure if exists [dbo].[GetThuChi_DauKyCuoiKy]");
            migrationBuilder.Sql(@"drop function if exists [dbo].[FnCheckPermission_ByUserId]");
        }
    }
}
