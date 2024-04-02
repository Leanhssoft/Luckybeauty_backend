using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreSql20231223 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[GetAllNhatKyChuyenTien]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[GetAllUser]");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetAllNhatKyChuyenTien]
	@TextSearch  nvarchar(max) ='',
	@CurrentPage int =0,
	@PageSize int = 10,
	@ColumnSort varchar(200)='creatTime',
	@TypeSort varchar(20) ='desc'

AS
BEGIN

	SET NOCOUNT ON;

	if @CurrentPage > 0 set @CurrentPage = @CurrentPage -1;

	if isnull(@TextSearch,'') !=''
		set @TextSearch = concat(N'%', @TextSearch, '%')
	else
		set @TextSearch=''
	

	select	
		his.Id,
		his.IdPhieuNapTien,
		his.IdNguoiChuyenTien,
		his.IdNguoiNhanTien,
		his.CreationTime,
		his.SoTienChuyen_Nhan,
		his.NoiDungChuyen_Nhan,
		iif(his.IdPhieuNapTien is not null,u3.UserName, u1.UserName) as UserChuyenTien,
		iif(his.IdPhieuNapTien is not null,u2.UserName, u2.UserName) as UserNhanTien,
		iif(his.IdPhieuNapTien is null, N'Chuyển tiền', N'Nộp tiền') as LoaiPhieu
	into #temp
	from SMS_LichSuNap_ChuyenTien his
	left join AbpUsers u1 on his.IdNguoiChuyenTien = u1.Id
	left join AbpUsers u2 on his.IdNguoiNhanTien = u2.Id
	left join AbpUsers u3 on his.CreatorUserId = u3.Id
	where his.IsDeleted='0'
	and (@TextSearch =''
		or u1.UserName like @TextSearch
		or u2.UserName like @TextSearch
		or his.SoTienChuyen_Nhan like @TextSearch
		or his.NoiDungChuyen_Nhan like @TextSearch
		)

	

	select *
	from #temp
	ORDER BY
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'soTienChuyen_Nhan' THEN SoTienChuyen_Nhan END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'userChuyenTien' THEN UserChuyenTien END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'userNhanTien' THEN UserNhanTien END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'loaiPhieu' THEN LoaiPhieu END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'creationTime' THEN CreationTime END ASC,

		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'soTienChuyen_Nhan' THEN SoTienChuyen_Nhan END DESC,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'userChuyenTien' THEN UserChuyenTien END DESC,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'userNhanTien' THEN UserNhanTien END DESC,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'loaiPhieu' THEN LoaiPhieu END DESC,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'creationTime' THEN CreationTime END DESC	
	OFFSET  @CurrentPage ROWS FETCH NEXT @PageSize ROWS ONLY;

	SELECT COUNT(Id) AS TotalCount FROM #temp;

    
END");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetAllUser]
	@TextSearch  nvarchar(max) ='',
	@CurrentPage int =0,
	@PageSize int = 10,
	@ColumnSort varchar(200)='creatTime',
	@TypeSort varchar(20) ='desc'
AS
BEGIN

	SET NOCOUNT ON;

	if @CurrentPage > 0 set @CurrentPage = @CurrentPage -1;

	if isnull(@TextSearch,'') !=''
		set @TextSearch = concat(N'%', @TextSearch, '%')
	else
		set @TextSearch=''
	

	select 
		u.Id,
		u.NhanSuId,
		u.IdChiNhanhMacDinh,
		u.UserName,
		u.Name,
		u.Surname,
		u.EmailAddress,
		u.PhoneNumber,
		u.IsActive,
		u.IsAdmin,
		u.CreationTime,
		nv.TenNhanVien,
		nv.Avatar,
		cn.TenChiNhanh
	into #temp
	from AbpUsers u
	left join NS_NhanVien nv on u.NhanSuId = nv.Id
	left join DM_ChiNhanh cn on u.IdChiNhanhMacDinh = cn.Id
	where u.IsDeleted='0'
	and (@TextSearch =''
		or u.UserName like @TextSearch
		or u.Name like @TextSearch
		or u.Surname like @TextSearch
		or nv.TenNhanVien like @TextSearch
		or cn.TenChiNhanh like @TextSearch	
		)


		select Count(Id) as TotalCount from  #temp

		select tbl.*,
			roles.RoleNames,
			iif(tbl.IsActive='1',N'Hoạt động', N'Ngừng hoạt động') as TxtTrangThai
		from #temp tbl
		left join (
			select distinct userRole.UserId,
				(
				select r.Name + ', ' AS [text()]
				from AbpUserRoles urole
				join AbpRoles r on urole.RoleId = r.Id				
				where urole.UserId = userRole.UserId
				For XML PATH ('')
				) as RoleNames
			from AbpUserRoles userRole
		) roles on roles.UserId = tbl.Id
		ORDER BY
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'name' THEN Name END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'surname' THEN Surname END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'emailAddress' THEN EmailAddress END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'phoneNumber' THEN PhoneNumber END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'tenNhanVien' THEN TenNhanVien END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'tenChiNhanh' THEN TenChiNhanh END ASC,
		CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'creationTime' THEN CreationTime END ASC,	

		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'name' THEN Name END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'surname' THEN Surname END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'emailAddress' THEN EmailAddress END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'phoneNumber' THEN PhoneNumber END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'tenNhanVien' THEN TenNhanVien END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'tenChiNhanh' THEN TenChiNhanh END desc,
		CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'creationTime' THEN CreationTime END DESC
		OFFSET  @CurrentPage ROWS FETCH NEXT @PageSize ROWS ONLY;
END");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetListBandname]
	@TenantId int = 1,
    @Keyword nvarchar(max) ='',
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
	where br.IsDeleted='0'
	and (@TenantId = 1 or br.TenantId = @TenantId) --- get all brandname (if HOST) - or only get brandname by tenantId
	and (@Keyword ='' or br.Brandname like @Keyword)
	),
	count_cte
	as 
	(
		select count(Id) as TotalRow
		from data_cte
	)
	select dt.*,
		tenant.TenancyName,
		tenant.Name as DisplayTenantName,
		count_cte.*,
		isnull(tblThuChi.TongTienNap,0) as TongTienNap,
		isnull(tblThuChi.TongTienNap,0) - DaSuDung as ConLai
	from data_cte dt
	Left join AbpTenants tenant on dt.TenantId = tenant.Id
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

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[GetAllNhatKyChuyenTien]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[GetAllUser]");
        }
    }
}
