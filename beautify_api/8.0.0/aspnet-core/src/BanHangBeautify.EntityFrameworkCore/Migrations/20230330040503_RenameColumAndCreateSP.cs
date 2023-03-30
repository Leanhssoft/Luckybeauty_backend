using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumAndCreateSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenLoai",
                table: "DM_LoaiKhach",
                newName: "TenLoaiKhachHang");

            migrationBuilder.RenameColumn(
                name: "MaLoai",
                table: "DM_LoaiKhach",
                newName: "MaLoaiKhachHang");

            migrationBuilder.RenameColumn(
                name: "TenLoai",
                table: "DM_LoaiHangHoa",
                newName: "TenLoaiHangHoa");

            migrationBuilder.RenameColumn(
                name: "MaLoai",
                table: "DM_LoaiHangHoa",
                newName: "MaLoaiHangHoa");

            migrationBuilder.CreateTable(
                name: "DM_LoaiChungTu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaLoaiChungTu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenLoaiChungTu = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM_LoaiChungTu", x => x.Id);
                });


            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetNumeric]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnSplitstring]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetProductCode]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetDMHangHoa]");

            migrationBuilder.Sql(@" CREATE FUNCTION [dbo].[fnGetNumeric]
                    (@strAlphaNumeric VARCHAR(256))
                RETURNS VARCHAR(256)
                AS
                BEGIN
                    DECLARE @intAlpha INT
                    SET @intAlpha = PATINDEX('%[^0-9]%', @strAlphaNumeric)
                    BEGIN
                    WHILE @intAlpha > 0
                    BEGIN
                    SET @strAlphaNumeric = STUFF(@strAlphaNumeric, @intAlpha, 1, '' )
                    SET @intAlpha = PATINDEX('%[^0-9]%', @strAlphaNumeric )
                    END
                    END
                    RETURN ISNULL(@strAlphaNumeric,0)
                END ");
            _ = migrationBuilder.Sql(@"
                    CREATE FUNCTION [dbo].[fnSplitstring] ( @stringToSplit NVARCHAR(MAX) )
                    RETURNS
                     @returnList TABLE ([Name] [nvarchar] (500))
                    AS
                    BEGIN
                         DECLARE @name NVARCHAR(255)
                         DECLARE @pos INT

                         WHILE CHARINDEX(',', @stringToSplit) > 0
                         BEGIN
                          SELECT @pos  = CHARINDEX(',', @stringToSplit)  
                          SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

                          INSERT INTO @returnList 
                          SELECT @name

                          SELECT @stringToSplit = rtrim(ltrim(SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)))
                         END

                         INSERT INTO @returnList
                         SELECT @stringToSplit

                         RETURN
                    END ");
            _ = migrationBuilder.Sql(@"
                    CREATE FUNCTION [dbo].[fnGetProductCode]
                    (
	                    @TenantId int,
	                    @LoaiHangHoa int
                    )
                    RETURNS nvarchar(50)
                    AS
                    BEGIN
	                    DECLARE @MaLoai varchar(5) =(select top 1 MaLoaiHangHoa from DM_LoaiHangHoa where Id= @LoaiHangHoa)
                        DECLARE @Return float = 0
	
	                    SELECT @Return = MAX(CAST (dbo.fnGetNumeric(MaHangHoa) AS float))
                        FROM DM_DonViQuiDoi
	                    WHERE TenantId= @TenantId
	                    and MaHangHoa like @MaLoai +'%'
	                    and CHARINDEX('_',MaHangHoa)= 0

	                    RETURN concat(@MaLoai,FORMAT(isnull(@Return,0) + 1, 'F0'))
                    END ");
            _ = migrationBuilder.Sql(@"
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
		                qd.GiaBan,
		                hh.TrangThai,
		                hh.MoTa,
		                hh.NguoiTao,
		                loai.TenLoaiHangHoa,
		                isnull(nhom.TenNhomHang,'') as TenNhomHang,
		                case hh.trangthai
			                when 1 then N'Đang kinh doanh'
			                when 0 then N'Ngừng kinh doanh'
		                else '' end as TrangThaiText
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
                END ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DM_LoaiChungTu");

            migrationBuilder.RenameColumn(
                name: "TenLoaiKhachHang",
                table: "DM_LoaiKhach",
                newName: "TenLoai");

            migrationBuilder.RenameColumn(
                name: "MaLoaiKhachHang",
                table: "DM_LoaiKhach",
                newName: "MaLoai");

            migrationBuilder.RenameColumn(
                name: "TenLoaiHangHoa",
                table: "DM_LoaiHangHoa",
                newName: "TenLoai");

            migrationBuilder.RenameColumn(
                name: "MaLoaiHangHoa",
                table: "DM_LoaiHangHoa",
                newName: "MaLoai");


            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetNumeric]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnSplitstring]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetProductCode]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetDMHangHoa]");
        }
    }
}
