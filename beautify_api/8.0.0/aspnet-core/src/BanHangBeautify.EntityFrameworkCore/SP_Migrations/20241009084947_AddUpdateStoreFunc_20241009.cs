using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreFunc20241009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "DM_LoaiChungTu",
            columns: new[] { "Id", "MaLoaiChungTu", "TenLoaiChungTu", "TrangThai", "IsDeleted", "CreationTime" },
            values: new object[,]
            {
                    { 16, "DCTGT", "Điều chỉnh số dư thẻ giá trị" ,1,false, "2024-10-09"}
            });

            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetMaxNumber_ofMaHoaDon]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[FnCheckTheGiaTri_DaSuDung]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[FnGetSoDuTheGiaTri_ofKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetNhatKySuDungTGT_ChiTiet]");

			migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[fnGetMaxNumber_ofMaHoaDon]
(
	@TenantId int,
	@IdChiNhanh uniqueidentifier= null,
	@IdLoaiChungTu int= 1,
	@NgayLapHoaDon datetime= null
)
RETURNS float
AS
BEGIN
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
				from dbo.BH_HoaDon	hd						
				where hd.TenantId = @TenantId
				and  MaHoaDon like @kihieuchungtu +'%'
				and hd.IdLoaiChungTu= @IdLoaiChungTu
			) a
			where ISNUMERIC(RIGHT(MaHoaDon,LEN(MaHoaDon)- @lenMaChungTu)) = 1
		)

	 if @maxMaHoaDon is null			
		  set @maxMaHoaDon = 1	
	else
		set @maxMaHoaDon = FORMAT(@maxMaHoaDon + 1, 'F0') --- convert dạng mũ 10 về string

	return @maxMaHoaDon		
END");
            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[FnCheckTheGiaTri_DaSuDung]
(
	@IdTheGiaTri uniqueidentifier
)
RETURNS bit
AS
BEGIN

	declare @isDaSuDung bit ='0'
	declare @IdKhachHang uniqueidentifier, @ngayNapTheThis datetime, @tongNapThis float
	declare @gtriDieuChinhGanNhat float, @ngayDieuChinhGanNhat datetime
	declare @tblNapTien table(Id uniqueidentifier, TongTienHang float, TongGiamGiaHD float)
	DECLARE @tongThanhToan float, @tongNap float, @tongGiamGiaHD float, @tongSuDung float

		--- get thegiatri this --
		select top 1 @IdKhachHang = IdKhachHang , 
			@ngayNapTheThis = format( NgayLapHoaDon,'yyyy-MM-dd HH:mm:ss'),
			@tongNapThis = TongTienHang
		from BH_HoaDon
		where id= @IdTheGiaTri
		
		----- get phieudieuchinh gan nhat ---
		select top 1 @gtriDieuChinhGanNhat = TongTienHang,
			@ngayDieuChinhGanNhat= NgayLapHoaDon 
		from BH_HoaDon
		where TrangThai = 3 and IsDeleted='0'
		and IdKhachHang = @IdKhachHang
		and IdLoaiChungTu = 16
		and NgayLapHoaDon < @ngayNapTheThis
		order by NgayLapHoaDon desc

		if isnull(@gtriDieuChinhGanNhat,0) =0 set @gtriDieuChinhGanNhat = 0
		if @ngayDieuChinhGanNhat is null set @ngayDieuChinhGanNhat = '2021-01-01' -- ngày bắt đầu sử dụng PM (mặc định là ngày bất kỳ này)
		

		--- tongnap: chỉ tính sau thời điểm điều chỉnh, và trước thời điểm nạp thẻ (this ngayLapHD) ---	
		insert into @tblNapTien
		select id, TongTienHang, TongGiamGiaHD
		from BH_HoaDon
		where TrangThai = 3 and IsDeleted='0'
		and IdKhachHang = @IdKhachHang
		and IdLoaiChungTu = 8
		and NgayLapHoaDon > @ngayDieuChinhGanNhat and NgayLapHoaDon < @ngayNapTheThis

		select @tongThanhToan = 
			sum(qct.TienThu) 
		from QuyHoaDon qhd
		join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
		where qct.IdKhachHang = @IdKhachHang
		and qhd.TrangThai = 1
		and qct.IsDeleted ='0'
		and qct.HinhThucThanhToan in (1,2,3)
		and NgayLapHoaDon > @ngayDieuChinhGanNhat
		and exists (select id from @tblNapTien hd where qct.IdHoaDonLienQuan = hd.Id)

		----- tongsudung: lay full thoigian (sau dieuchinh) ---
		select @tongSuDung = 
			sum(qct.TienThu) 
		from QuyHoaDon qhd
		join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
		where qct.IdKhachHang = @IdKhachHang
		and qhd.TrangThai = 1
		and qct.IsDeleted ='0'
		and qct.HinhThucThanhToan = 4 -- THEGIATRI
		and NgayLapHoaDon > @ngayDieuChinhGanNhat

		select @tongNap = sum(TongTienHang), @tongGiamGiaHD = sum(@tongGiamGiaHD) from @tblNapTien

		if @gtriDieuChinhGanNhat  + isnull(@tongThanhToan,0) + isnull(@tongGiamGiaHD,0)   < isnull(@tongSuDung,0)
			set @isDaSuDung ='1'

		return cast (@isDaSuDung as bit)

END");
            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[FnGetSoDuTheGiaTri_ofKhachHang]
(
	@IdKhachHang uniqueidentifier,
	@ToDate datetime = null --- get số dư đến ngày
)
RETURNS float 
AS
BEGIN

	DECLARE @soDuTheGiaTri float, @tongNap float, @tongGiamGiaHD float, @tongThanhToan float, @tongSuDung float
	declare @gtriDieuChinhGanNhat float, @ngayDieuChinhGanNhat datetime

	declare @tblNapTien table(Id uniqueidentifier, TongTienHang float, TongGiamGiaHD float)

		if @ToDate is null set @ToDate = FORMAT(getdate(),'yyyy-MM-dd HH:mm:ss')

	---- get phieudieuchinh gần nhất (trước @toDate) ---
		select top 1 @gtriDieuChinhGanNhat = TongTienHang, @ngayDieuChinhGanNhat= NgayLapHoaDon 
		from BH_HoaDon
		where TrangThai = 3 and IsDeleted='0'
		and IdKhachHang = @IdKhachHang
		and IdLoaiChungTu = 16
		and NgayLapHoaDon < @ToDate
		order by NgayLapHoaDon desc
		
		if isnull(@gtriDieuChinhGanNhat,0) =0 set @gtriDieuChinhGanNhat = 0
		if @ngayDieuChinhGanNhat is null set @ngayDieuChinhGanNhat = '2021-01-01'	

		---- tongnap, thanh toán, sử dụng: chỉ tính sau thời điểm điều chỉnh ---	
		insert into @tblNapTien
		select id, TongTienHang, TongGiamGiaHD
		from BH_HoaDon
		where TrangThai = 3 and IsDeleted='0'
		and IdKhachHang = @IdKhachHang
		and IdLoaiChungTu = 8
		and NgayLapHoaDon > @ngayDieuChinhGanNhat

		select @tongNap = sum(TongTienHang), @tongGiamGiaHD= sum(TongGiamGiaHD) from @tblNapTien
		

		select @tongThanhToan = 
			sum(qct.TienThu) 
		from QuyHoaDon qhd
		join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
		where qct.IdKhachHang = @IdKhachHang
		and qhd.TrangThai = 1
		and qct.IsDeleted ='0'
		and qct.HinhThucThanhToan in (1,2,3)
		and NgayLapHoaDon > @ngayDieuChinhGanNhat
		and exists (select id from @tblNapTien hd where qct.IdHoaDonLienQuan = hd.Id)

		select @tongSuDung = 
			sum(qct.TienThu) 
		from QuyHoaDon qhd
		join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
		where qct.IdKhachHang = @IdKhachHang
		and qhd.TrangThai = 1
		and qct.IsDeleted ='0'
		and qct.HinhThucThanhToan = 4 -- THEGIATRI
		and NgayLapHoaDon > @ngayDieuChinhGanNhat
	
	----- sodu: chỉ lấy số tiền đã thanh toán, cộng cả phần giảm giá, cộng khuyến mại (todo)--
	set @soDuTheGiaTri = @gtriDieuChinhGanNhat +  isnull(@tongThanhToan,0) + isnull(@tongGiamGiaHD,0)  -  isnull(@tongSuDung,0)
	return @soDuTheGiaTri
END");
			migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetNhatKySuDungTGT_ChiTiet]
	@IdChiNhanhs  nvarchar(max)= '71313109-267A-4D84-B2A3-6A1AF5451597',
	@IdKhachHang uniqueidentifier = null,
	@IdLoaiChungTus varchar(20) ='1,2,8,16',
	@FromDate datetime = '2024-09-01',
	@ToDate datetime= '2024-12-30', 
	@TextSearch nvarchar(max)='',	
	@CurrentPage int =1,
	@PageSize int= 20
AS
BEGIN

	SET NOCOUNT ON;
	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1
	--- vì bên ngoài: toDate chưa cộng thệm 1 ngày
	if @ToDate is not null set @ToDate = FORMAT(DATEADD(day,1,@ToDate),'yyyy-MM-dd')
	declare @tblChiNhanh table(Id uniqueidentifier)
	if isnull(@IdChiNhanhs,'') ='' set @IdChiNhanhs =''
	else 
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
		where GiaTri !=''

	declare @tblLoaiChungTu table(IdLoaiChungTu tinyint)
	if isnull(@IdLoaiChungTus,'') ='' set @IdLoaiChungTus =''
	else 
		insert into @tblLoaiChungTu
		select GiaTri from dbo.fnSplitstring(@IdLoaiChungTus)
		where GiaTri !=''

	if isnull(@TextSearch,'') ='' set @TextSearch =''
	else set @TextSearch = concat(N'%',@TextSearch, '%')

	; with tblLast
	as
	(
	select 
		tblUnion.*,
		case tblUnion.IdLoaiChungTu
			when 1 then N'Hóa đơn'
			when 2 then N'Gói dịch vụ'
			when 8 then N'Nạp thẻ'
			when 16 then N'Phiếu điều chỉnh'
		else '' end as sLoaiChungTu,
		kh.MaKhachHang,
		kh.TenKhachHang,
		kh.SoDienThoai
	from
	(
	select 
		hd.Id,
		hd.IdKhachHang,
		hd.IdLoaiChungTu,
		hd.MaHoaDon,
		hd.NgayLapHoaDon,
		iif(hd.IdLoaiChungTu = 16, hd.TongTienHang,0) as GtriDieuChinh,
		iif(hd.IdLoaiChungTu = 8, hd.TongTienHang,0) as PhatSinhTang,		
		0 PhatSinhGiam 
	from BH_HoaDon hd
	where hd.IsDeleted='0'
	and hd.TrangThai = 3
	and hd.IdLoaiChungTu in (8,16)
	and hd.NgayLapHoaDon between @FromDate and @ToDate
	and (@IdChiNhanhs = '' or exists (select id from @tblChiNhanh cn where hd.IdChiNhanh = cn.Id))
	and (@IdKhachHang is null or hd.IdKhachHang = @IdKhachHang)
	
	union all

	select 
		hdsd.Id,
		hdsd.IdKhachHang,
		hdsd.IdLoaiChungTu,
		hdsd.MaHoaDon,
		hdsd.NgayLapHoaDon,
		0 as GtriDieuChinh,
		0 as PhatSinhTang,		
		qct.TienThu as PhatSinhGiam
	from QuyHoaDon qhd
	join QuyHoaDon_ChiTiet qct on qhd.Id = qct.IdQuyHoaDon
	join BH_HoaDon hdsd on qct.IdHoaDonLienQuan = hdsd.Id
	where qhd.IsDeleted='0'
	and qhd.TrangThai = 1
	and qct.HinhThucThanhToan = 4
	and qhd.NgayLapHoaDon between @FromDate and @ToDate
	and (@IdChiNhanhs = '' or exists (select id from @tblChiNhanh cn where qhd.IdChiNhanh = cn.Id))
	and (@IdKhachHang is null or qct.IdKhachHang = @IdKhachHang)
	)tblUnion
	join DM_KhachHang kh on tblUnion.IdKhachHang = kh.Id
	where (@IdLoaiChungTus ='' or exists (select IdLoaiChungTu from @tblLoaiChungTu ct where tblUnion.IdLoaiChungTu = ct.IdLoaiChungTu))
	and (@TextSearch =''
		or tblUnion.MaHoaDon like @TextSearch 
		or kh.MaKhachHang like @TextSearch or kh.TenKhachHang like @TextSearch
		or kh.TenKhachHang_KhongDau like @TextSearch or kh.SoDienThoai like @TextSearch)
	),		
	count_cte
	as(	
		select count(*) as TotalRow
		from tblLast
	)
	select *
	from tblLast dt
	cross join count_cte
	order by NgayLapHoaDon desc  
    OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
END");

            migrationBuilder.Sql(@"ALTER PROC [dbo].[prc_dashboard_hotService]
	@TenantId int = 1,
	@UserId int = NULL,
	@ThoiGianTu DateTime = '2024/09/01',
	@ThoiGianDen DateTime = '2024/10/08',
	@IdChiNhanh uniqueidentifier = '71313109-267a-4d84-b2a3-6a1af5451597'
AS
BEGIN
	SET NOCOUNT ON;
	declare @fromDate datetime= CAST(@ThoiGianTu AS DATE)
	declare @toDate datetime= CAST(dateadd(day,1,@ThoiGianDen) AS DATE)
	declare @tongSoLuongBan float

	select 		
		ct.IdDonViQuyDoi,
		sum(ct.ThanhTienSauCK) as TongDoanhThu,
		sum(ct.SoLuong) as TongSLMua		
	into #tbl
	from BH_HoaDon hd
	join BH_HoaDon_ChiTiet ct on hd.Id = ct.IdHoaDon
	where hd.NgayLapHoaDon between @fromDate and @toDate
	and hd.TenantId = @TenantId 
	and hd.IdChiNhanh = @IdChiNhanh AND hd.IsDeleted = 0
	and ct.IsDeleted= 0
	and ct.IdChiTietHoaDon is null --- không lấy DV sử dụng từ GDV ---
	group by ct.IdDonViQuyDoi
	
	select @tongSoLuongBan = sum(TongSLMua) from #tbl

	select top 5 
		hh.TenHangHoa as TenDichVu,
		hh.Color,
		TongDoanhThu,
		TongSLMua,
		TongSLMua / @tongSoLuongBan * 100 as PhanTram
	from #tbl tbl
	join DM_DonViQuiDoi dvqd on dvqd.Id = tbl.IdDonViQuyDoi
	JOIN DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	order by TongSLMua desc

END;");
			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[BaoCaoSuDungGDV_ChiTiet]
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
	--- vì bên ngoài: toDate chưa cộng thệm 1 ngày
	if @ToDate is not null set @ToDate = FORMAT(DATEADD(day,1,@ToDate),'yyyy-MM-dd')

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
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM DM_LoaiChungTu WHERE Id= 16");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetMaxNumber_ofMaHoaDon]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[FnCheckTheGiaTri_DaSuDung]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[FnGetSoDuTheGiaTri_ofKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetNhatKySuDungTGT_ChiTiet]");
        }
    }
}
