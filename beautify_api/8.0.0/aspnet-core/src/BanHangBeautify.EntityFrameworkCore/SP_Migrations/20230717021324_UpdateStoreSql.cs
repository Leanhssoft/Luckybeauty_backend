using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NguoiSua",
                table: "DM_NhomHangHoa");

            migrationBuilder.DropColumn(
                name: "NguoiTao",
                table: "DM_NhomHangHoa");

            migrationBuilder.DropColumn(
                name: "NguoiXoa",
                table: "DM_NhomHangHoa");

            migrationBuilder.DropColumn(
                name: "NguoiSua",
                table: "DM_NguonKhach");

            migrationBuilder.DropColumn(
                name: "NguoiTao",
                table: "DM_NguonKhach");

            migrationBuilder.DropColumn(
                name: "NguoiXoa",
                table: "DM_NguonKhach");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_khachHang_getAll]
					@TenantId INT = 7,
					@IdChiNhanh UNIQUEIDENTIFIER = null,
					@Filter NVARCHAR(max) =N'',
					@SkipCount INT = 0,
					@MaxResultCount INT = 10,
					@SortBy NVARCHAR(50)='CreateTime',
					@SortType VARCHAR(4)='desc'
				AS
				BEGIN
				
					SELECT * FROM
						(SELECT 
						kh.Id,
						kh.MaKhachHang,
						kh.TenKhachHang,
						kh.Avatar,
						kh.SoDienThoai,
						kh.DiaChi,
						kh.NgaySinh,
						nkh.TenNhomKhach,
						CASE when kh.GioiTinhNam = 1 THEN N'Nam' ELSE N'Nữ' END as GioiTinh,
						'' AS NhanVienPhuTrach,
						kh.TongTichDiem,
						kh.CreationTime,
						hd.TongChiTieu,
						ISNULL(tblCheckIn.CuocHenGanNhat,kh.CreationTime) AS CuocHenGanNhat,
						isnull(tblCheckIn.SoLanCheckIn,0) as SoLanCheckIn,
						isnull(tblChecking.TrangThaiCheckIn,0) as TrangThaiCheckIn,
						ngkh.TenNguon as TenNguonKhach
						FROM DM_KhachHang kh
						LEFT JOIN DM_NhomKhachHang nkh on nkh.Id = kh.IdNhomKhach
						LEFT JOIN DM_NguonKhach ngkh on ngkh.Id = kh.IdNguonKhach
						LEFT JOIN (
							SELECT IdKhachHang, MAX(CreationTime) AS MaxCreationTime,SUM(TongTienHDSauVAT) as TongChiTieu
							FROM BH_HoaDon
							WHERE IsDeleted = 0
							GROUP BY IdKhachHang
						) hd ON hd.IdKhachHang = kh.Id
						LEFT JOIN (
							SELECT IdKhachHang, max(DateTimeCheckIn) as CuocHenGanNhat, count(id) as SoLanCheckIn
							FROM KH_CheckIn
							WHERE IsDeleted = 0
							GROUP BY IdKhachHang
						) tblCheckIn ON kh.Id = tblCheckIn.IdKhachHang
						left join (
							SELECT IdKhachHang, 1 as TrangThaiCheckIn
							FROM KH_CheckIn
							WHERE IsDeleted = 0 and TrangThai= 1
							GROUP BY IdKhachHang
						) tblChecking ON kh.Id = tblChecking.IdKhachHang --- khach dang check in
						WHERE kh.TenantId = @TenantId 
						AND kh.IsDeleted = 0
						AND (ISNULL(@Filter,'') = ''
							OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(kh.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(kh.DiaChi) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(nkh.TenNhomKhach) LIKE N'%' + LOWER(@Filter) + N'%'
							OR LOWER(ngkh.TenNguon) LIKE N'%' + LOWER(@Filter) + N'%'
						)
					) as Result
					ORDER BY
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'diaChi' THEN DiaChi END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngaySinh' THEN NgaySinh END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenNguonKhach' THEN TenNguonKhach END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'gioiTinh' THEN GioiTinh END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tongTchDiem' THEN TongTichDiem END,
						CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'cuocHenGanNhat' THEN CuocHenGanNhat END,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'diaChi' THEN DiaChi END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngaySinh' THEN NgaySinh END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenNguonKhach' THEN TenNguonKhach END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'gioiTinh' THEN GioiTinh END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tongTchDiem' THEN TongTichDiem END DESC,
						CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'cuocHenGanNhat' THEN CuocHenGanNhat END DESC,
						CASE WHEN LOWER(ISNULL(@SortType,'')) = '' THEN CreationTime END DESC
					OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

					SELECT COUNT(*) as TotalCount
					FROM DM_KhachHang kh
					LEFT JOIN DM_NhomKhachHang nkh on nkh.Id = kh.IdNhomKhach
					LEFT JOIN DM_NguonKhach ngkh on ngkh.Id = kh.IdNguonKhach
					LEFT JOIN (
						SELECT IdKhachHang, MAX(CreationTime) AS MaxCreationTime,SUM(TongTienHDSauVAT) as TongChiTieu
						FROM BH_HoaDon
						WHERE IsDeleted = 0
						GROUP BY IdKhachHang
					) hd ON hd.IdKhachHang = kh.Id
					WHERE kh.TenantId = @TenantId 
					AND kh.IsDeleted = 0
					AND (ISNULL(@Filter,'') = ''
						OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(kh.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(kh.DiaChi) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(nkh.TenNhomKhach) LIKE N'%' + LOWER(@Filter) + N'%'
						OR LOWER(ngkh.TenNguon) LIKE N'%' + LOWER(@Filter) + N'%'
					);
				END;");
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
				hd.TongThanhToan - isnull(sq.DaThanhToan,0) as ConNo,
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NguoiSua",
                table: "DM_NhomHangHoa",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiTao",
                table: "DM_NhomHangHoa",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiXoa",
                table: "DM_NhomHangHoa",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiSua",
                table: "DM_NguonKhach",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiTao",
                table: "DM_NguonKhach",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiXoa",
                table: "DM_NguonKhach",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
