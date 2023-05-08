using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID_Parent",
                table: "DM_NhomHangHoa",
                newName: "IdParent");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[spGetDMHangHoa]
	@TenantId int =1,
	@TextSearch nvarchar(max)=null,
	@IdNhomHangHoas nvarchar(max)=null,
	@Where nvarchar(max) =null,
	@CurrentPage int = 0,
    @PageSize int = 1000,
    @ColumnSort varchar(100) ='CreationTime',
    @TypeSort varchar(20) = 'DESC'
AS
BEGIN
	
	SET NOCOUNT ON;

	if @CurrentPage > 0  set @CurrentPage = @CurrentPage- 1;
	else set @CurrentPage =0

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
		hh.TrangThai,
		hh.MoTa,
		hh.NguoiTao,
		loai.TenLoaiHangHoa,
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
	and (@TextSearch ='' or  
		(hh.TenHangHoa like @TextSearch or qd.MaHangHoa like @TextSearch or hh.MoTa like @TextSearch))
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
		when @ColumnSort='GiaBan' then GiaBan end DESC
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY

END ");
			migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetDetailProduct]
	@IdDonViQuyDoi uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON;

	select 
		hh.Id,
		hh.IdLoaiHangHoa,
		hh.IdNhomHangHoa,
		isnull(hh.SoPhutThucHien,0) as SoPhutThucHien,
		qd.Id as IdDonViQuyDoi,
		qd.MaHangHoa,
		qd.GiaBan,
		hh.TenHangHoa,
		loai.TenLoaiHangHoa,
		hh.MoTa,
		hh.TrangThai,	
		qd.LaDonViTinhChuan,
		qd.TyLeChuyenDoi,	
		qd.TenDonViTinh,
		isnull(nhom.TenNhomHang,N'Nhóm mặc định') as TenNhomHang,
		iif(hh.TrangThai =1, N'Đang kinh doanh',N'Ngừng kinh doanh') as TxtTrangThaiHang		
	from DM_HangHoa hh
	join DM_LoaiHangHoa loai on hh.IdLoaiHangHoa= loai.Id
	join DM_DonViQuiDoi qd on hh.Id = qd.IdHangHoa
	left join DM_NhomHangHoa nhom on hh.IdNhomHangHoa= nhom.Id
	where qd.Id = @IdDonViQuyDoi 
END");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spJqAutoCustomer]");
            migrationBuilder.Sql(@"
CREATE PROCEDURE [dbo].[spJqAutoCustomer]
	@TenantId int= 1,
	@LoaiDoiTuong int= 1,
	@TextSearch nvarchar(max)='',
	@CurrentPage int=0,
	@PageSize int = 50
AS
BEGIN
	
	SET NOCOUNT ON;

	if ISNULL(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch='%%'

	;with data_cte
	as(
	select  kh.Id, kh.TenantId, kh.MaKhachHang, kh.TenKhachHang, kh.SoDienThoai,
		 kh.TongTichDiem,
		 kh.GioiTinhNam,
		nhom.TenNhomKhach
	from DM_KhachHang kh
	left join DM_NhomKhachHang nhom on kh.IdNhomKhach= nhom.Id	
	where kh.TrangThai in (0,1)
	and kh.TenantId= @TenantId
	and IdLoaiKhach= @LoaiDoiTuong
	and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
		or TenKhachHang_KhongDau like @TextSearch
		or MaKhachHang like @TextSearch
		or TenKhachHang like @TextSearch
		or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
		or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
	)
	select *
	from data_cte
	order by MaKhachHang
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
    
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdParent",
                table: "DM_NhomHangHoa",
                newName: "ID_Parent");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spJqAutoCustomer]");
        }
    }
}
