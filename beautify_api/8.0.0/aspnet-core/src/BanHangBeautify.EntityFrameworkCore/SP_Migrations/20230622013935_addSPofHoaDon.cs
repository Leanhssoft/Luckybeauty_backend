using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addSPofHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetListHoaDon]
			@TenantId int =1,
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
			kh.MaKhachHang,
			isnull(kh.TenKhachHang,N'Khách lẻ') as TenKhachHang,
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
				cast(isnull(sq.DaThanhToan,0) as real) as DaThanhToan,
				cast(hd.TongThanhToan - isnull(sq.DaThanhToan,0)as real) as ConNo,
				TotalRow,
				TotalPage,
				cast(SumTongTienHang as real) as SumTongTienHang,
				cast(SumTongGiamGiaHD as real) as SumTongGiamGiaHD,
				cast(SumTongThanhToan as real) as SumTongThanhToan,
				cast(SumDaThanhToan as real) as SumDaThanhToan
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

			migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetChiTietHoaDon_byIdHoaDon]
	@IdHoaDon uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON;
	  select 
			cthd.*,
			qd.IdHangHoa,
			qd.MaHangHoa,
			hh.TenHangHoa,
			hh.TrangThai as TrangThaiHang
		from BH_HoaDon_ChiTiet cthd
		join DM_DonViQuiDoi qd on cthd.IdDonViQuyDoi = qd.Id
		join DM_HangHoa hh on qd.IdHangHoa= hh.Id		
		where cthd.IdHoaDon= @IdHoaDon
END");

			migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetInforHoaDon_byId]
	@Id uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON;

   select 
			hd.*,
			kh.MaKhachHang,
			isnull(kh.TenKhachHang,N'Khách lẻ') as TenKhachHang,
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
		where hd.Id= @Id
END");

            migrationBuilder.Sql(@"CREATE PROCEDURE spGetNhatKyThanhToan_ofHoaDon
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
				(case qct.HinhThucThanhToan
					when 1 then N'Tiền mặt'
					when 2 then N'Pos'
					when 3 then N'Chuyển khoản'
					when 4 then N'Thẻ giá trị'
					when 5 then N'Sử dụng điểm'
				else ''
				end) + ', ' AS [text()]
			from QuyHoaDon_ChiTiet qct
			where IdHoaDonLienQuan= @IdHoaDonLienQuan
			For XML PATH ('') 
			) sPhuongThucTT 
		from QuyHoaDon_ChiTiet qct
		where IdHoaDonLienQuan= @IdHoaDonLienQuan
		group by qct.IdQuyHoaDon
	) qct on qhd.Id= qct.IdQuyHoaDon
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE spGetListHoaDon");
            migrationBuilder.Sql("DROP PROCEDURE spGetChiTietHoaDon_byIdHoaDon");
            migrationBuilder.Sql("DROP PROCEDURE spGetInforHoaDon_byId");
            migrationBuilder.Sql("DROP PROCEDURE spGetNhatKyThanhToan_ofHoaDon");
        }
    }
}
