using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class EditTblCheckin2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_KH_CheckIn_IdChiNhanh",
                table: "KH_CheckIn",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_KH_CheckIn_IdKhachHang",
                table: "KH_CheckIn",
                column: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_KH_CheckIn_DM_ChiNhanh_IdChiNhanh",
                table: "KH_CheckIn",
                column: "IdChiNhanh",
                principalTable: "DM_ChiNhanh",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KH_CheckIn_DM_KhachHang_IdKhachHang",
                table: "KH_CheckIn",
                column: "IdKhachHang",
                principalTable: "DM_KhachHang",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[spGetDMHangHoa]
	@TenantId int =1,
	@TextSearch nvarchar(max)=null,
	@IdNhomHangHoas nvarchar(max)=null,
	@Where nvarchar(max) =null,
	@CurrentPage int = 1,
	@PageSize int = 1000,
	@ColumnSort varchar(100) ='CreationTime',
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
		and (@IdNhomHangHoas='' or exists (select * from @tblNhomHang nhomh where hh.IdNhomHangHoa= nhomh.ID))
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

            // new function sql
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetListNhomHangHoa]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnConvertStringToUnsign]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetFirstCharOfString]");

            migrationBuilder.Sql(@"
CREATE FUNCTION [dbo].[fnGetListNhomHangHoa] ( @IDNhomHang UNIQUEIDENTIFIER )
RETURNS
 @tblNhomHang TABLE (ID UNIQUEIDENTIFIER)
AS
BEGIN
	IF(@IDNhomHang IS NOT NULL)
	BEGIN
		DECLARE @tblNhomHangTemp TABLE (ID UNIQUEIDENTIFIER);
		INSERT INTO @tblNhomHang VALUES (@IDNhomHang);
		INSERT INTO @tblNhomHangTemp VALUES (@IDNhomHang);
		DECLARE @intFlag INT;
		SET @intFlag = 1;
		WHILE (@intFlag != 0)
		BEGIN
			SELECT @intFlag = COUNT(ID) FROM DM_NhomHangHoa WHERE IdParent IN (SELECT ID FROM @tblNhomHangTemp);
			IF(@intFlag != 0)
			BEGIN
				INSERT INTO @tblNhomHangTemp
				SELECT ID FROM DM_NhomHangHoa WHERE IdParent IN (SELECT ID FROM @tblNhomHangTemp); 
				DELETE FROM @tblNhomHangTemp WHERE ID IN (SELECT ID FROM @tblNhomHang);
				INSERT INTO @tblNhomHang
				SELECT ID FROM @tblNhomHangTemp
			END
		END
	END
	ELSE
	BEGIN
		INSERT INTO @tblNhomHang
		SELECT ID FROM DM_NhomHangHoa
	END
	RETURN
END");
			migrationBuilder.Sql(@"
CREATE FUNCTION [dbo].[fnConvertStringToUnsign](@str NVARCHAR(MAX) )
RETURNS NVARCHAR(MAX)
AS
BEGIN    
    IF (@str IS NULL OR @str = '')  RETURN ''
   
    DECLARE @RT NVARCHAR(MAX)
    DECLARE @SIGN_CHARS NCHAR(256)
    DECLARE @UNSIGN_CHARS NCHAR (256)
 
    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' + NCHAR(272) + NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
 
    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
   
    SET @COUNTER = 1
    WHILE (@COUNTER <= LEN(@str))
    BEGIN  
        SET @COUNTER1 = 1
        WHILE (@COUNTER1 <= LEN(@SIGN_CHARS) + 1)
        BEGIN
            IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@str,@COUNTER ,1))
            BEGIN          
                IF @COUNTER = 1
                    SET @str = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@str, @COUNTER+1,LEN(@str)-1)      
                ELSE
                    SET @str = SUBSTRING(@str, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@str, @COUNTER+1,LEN(@str)- @COUNTER)
                BREAK
            END
            SET @COUNTER1 = @COUNTER1 +1
        END
        SET @COUNTER = @COUNTER +1
    END
    RETURN LOWER(@str)
END");
			migrationBuilder.Sql(@"
CREATE FUNCTION [dbo].[fnGetFirstCharOfString](@stringToSplit NVARCHAR(MAX) )
RETURNS NVARCHAR(MAX)
AS
BEGIN 
	set @stringToSplit= rtrim(ltrim(@stringToSplit))
	DECLARE @returnList TABLE ([Name] [nvarchar] (500))   
	DECLARE @name NVARCHAR(255)
	DECLARE @pos INT

	 WHILE CHARINDEX(' ', @stringToSplit) > 0
	 BEGIN
	  SELECT @pos  = CHARINDEX(' ', @stringToSplit)  
	  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

	  INSERT INTO @returnList  
	  SELECT @name

	  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
	 END

	 INSERT INTO @returnList
	 SELECT @stringToSplit

	 RETURN (Select dbo.fnConvertStringToUnsign(Left(Name, 1))AS [text()]
    								From @returnList
    								For XML PATH (''))
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KH_CheckIn_DM_ChiNhanh_IdChiNhanh",
                table: "KH_CheckIn");

            migrationBuilder.DropForeignKey(
                name: "FK_KH_CheckIn_DM_KhachHang_IdKhachHang",
                table: "KH_CheckIn");

            migrationBuilder.DropIndex(
                name: "IX_KH_CheckIn_IdChiNhanh",
                table: "KH_CheckIn");

            migrationBuilder.DropIndex(
                name: "IX_KH_CheckIn_IdKhachHang",
                table: "KH_CheckIn");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetListNhomHangHoa]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnConvertStringToUnsign]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetFirstCharOfString]");
        }
    }
}
