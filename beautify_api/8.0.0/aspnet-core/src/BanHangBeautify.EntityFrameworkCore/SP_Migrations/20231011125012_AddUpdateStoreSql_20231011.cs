using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreSql20231011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetListBandname]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetInforBrandname_byId]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetAllSoQuy]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetListUser_havePermission]");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetListBandname]
	@TenantId int = 1,
    @Keyword nvarchar(max) ='2',
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
	where  br.IsDeleted='0'
	and (@Keyword ='' or br.Brandname like @Keyword)
	),
	count_cte
	as 
	(
		select count(Id) as TotalRow
		from data_cte
	)
	select dt.*,
		count_cte.*,
		isnull(tblThuChi.TongTienNap,0) as TongTienNap,
		isnull(tblThuChi.TongTienNap,0) - DaSuDung as ConLai
	from data_cte dt
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

END");
            migrationBuilder.Sql(@"CREATE PROCEDURE spGetInforBrandname_byId
	@IdBrandname uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON;

    select 
		br.Id,
		br.TenantId,
		br.Brandname,
		br.SDTCuaHang,
		br.NgayKichHoat,
		br.TrangThai,
		iif(br.TrangThai=1,N'Kích hoạt',N'Chưa kích hoạt') as TxtTrangThai
	from HT_SMSBrandname br
	left join
	(
		----- tongnaptien ---
		select 
			 qhd.IdBrandname,
			 qhd.TenantId,
			 sum(iif(qhd.IdLoaiChungTu=11, qhd.TongTienThu, - qhd.TongTienThu)) as TongTienNap
		from QuyHoaDon qhd
		where qhd.TrangThai=1
		and qhd.IdBrandname = @IdBrandname
		group by qhd.IdBrandname, qhd.TenantId
	)tblThuChi
	on br.Id = tblThuChi.IdBrandname
	where br.Id=@IdBrandname
END");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetAllSoQuy]
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
				
END");
            migrationBuilder.Sql(@"CREATE PROCEDURE GetListUser_havePermission
	@TenantId int = 1,
	@IdChiNhanh uniqueidentifier = null,
	@PermissionsName nvarchar(128)=''
AS
BEGIN
	
	SET NOCOUNT ON;

   select tblUserRole.TenantId, UserId
   from AbpUserRoles tblUserRole
   join AbpUsers tblUser on tblUserRole.UserId = tblUser.Id
   where tblUserRole.TenantId= @TenantId
   and tblUser.IsActive='1' and tblUser.IsDeleted ='0'
   and (
		Discriminator='UserRole' ---- admin macdinh----
	   or (tblUserRole.IdChiNhanh= @IdChiNhanh
		   and exists (
			   select * from AbpPermissions tbl
			   where tbl.RoleId = tblUserRole.RoleId and tbl.Name = @PermissionsName
		   )
	  )
  )
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetListBandname]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetInforBrandname_byId]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetAllSoQuy]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetListUser_havePermission]");
        }
    }
}
