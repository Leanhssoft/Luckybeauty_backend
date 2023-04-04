using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TenLoaiHangHoa",
                table: "DM_LoaiHangHoa",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaLoaiHangHoa",
                table: "DM_LoaiHangHoa",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetDMHangHoa]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetProductCode]");

            migrationBuilder.Sql(@"
CREATE PROCEDURE [dbo].[spGetDMHangHoa]
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
CREATE PROCEDURE [dbo].[spGetProductCode]
(
	@TenantId int,
	@LoaiHangHoa int
)
AS
BEGIN
	DECLARE @MaLoai varchar(5) =(select top 1 MaLoaiHangHoa from DM_LoaiHangHoa where Id= @LoaiHangHoa)
    DECLARE @Return float = 0
	
	SELECT @Return = MAX(CAST (dbo.fnGetNumeric(MaHangHoa) AS float))
    FROM DM_DonViQuiDoi
	WHERE TenantId= @TenantId
	and MaHangHoa like @MaLoai +'%'
	and CHARINDEX('_',MaHangHoa)= 0
	
    SELECT @MaLoai as FirstStr, isnull(@Return,0) + 1 as MaxVal

END ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TenLoaiHangHoa",
                table: "DM_LoaiHangHoa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaLoaiHangHoa",
                table: "DM_LoaiHangHoa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetDMHangHoa]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetProductCode]");
        }
    }
}
