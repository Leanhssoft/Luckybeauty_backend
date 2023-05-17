using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreSql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 17, 18, 39, 380, DateTimeKind.Local).AddTicks(4948), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 17, 18, 39, 380, DateTimeKind.Local).AddTicks(5074), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 17, 18, 39, 380, DateTimeKind.Local).AddTicks(5077), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 17, 18, 39, 380, DateTimeKind.Local).AddTicks(5322), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 17, 18, 39, 380, DateTimeKind.Local).AddTicks(5326), null, null });

            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[fnSplitstring] ( @stringToSplit NVARCHAR(MAX) )
            RETURNS
             @returnList TABLE ([GiaTri] [nvarchar] (500))
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
            END

            ");

            migrationBuilder.Sql(@"CREATE FUNCTION fnConvertStringToUnsign(@str NVARCHAR(MAX) )
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

            migrationBuilder.Sql(@"create FUNCTION [dbo].[fnGetNumeric]
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
END");


            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[fnGetFirstCharOfString](@stringToSplit NVARCHAR(MAX) )
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


            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[fnGetMaHoaDon]
(
	@TenantId int,
	@IdChiNhanh uniqueidentifier= null,
	@IdLoaiChungTu int= 1,
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
				from dbo.BH_HoaDon	hd						
				where hd.TenantId = @TenantId
				and  MaHoaDon like @kihieuchungtu +'%'
				and hd.IdLoaiChungTu= @IdLoaiChungTu
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
            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[fnGetMaPhieuThuChi]
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
				and hd.IdLoaiChungTu= @IdLoaiChungTu
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
            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[fnGetListNhomHangHoa] ( @IDNhomHang UNIQUEIDENTIFIER )
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


            // ============= SP =============

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetProductCode]
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

END 
");


            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetDMHangHoa]
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

            migrationBuilder.Sql(@"CREATE PROCEDURE spJqAutoCustomer
	@TenantId int= 1,
	@LoaiDoiTuong int= 1,
	@TextSearch nvarchar(max)='',
	@CurrentPage int=0,
	@PageSize int = 50
AS
BEGIN
	
	SET NOCOUNT ON;


	--declare @TextSearch nvarchar(max)=N'Nguyễn', @LoaiDoiTuong int = 1, @TenantId int = 1

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
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetListCustomerChecking]
	@TenantId int= 1,
	@TextSearch nvarchar(max)='',
	@CurrentPage int=0,
	@PageSize int = 50
AS
BEGIN
	
	SET NOCOUNT ON;


	--declare @TextSearch nvarchar(max)=N'Nguyễn', @LoaiDoiTuong int = 1, @TenantId int = 1

	if ISNULL(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch='%%'

	;with data_cte
	as(
	select
		checkin.Id, 
		checkin.TenantId,
		checkin.IdBooking,
		checkin.IdKhachHang,
		checkin.IdChiNhanh,
		checkin.DateTimeCheckIn,
		upper((select dbo.fnGetFirstCharOfString(kh.TenKhachHang))) as TenKhach_KiTuDau,
		kh.MaKhachHang, 
		kh.TenKhachHang, 
		kh.SoDienThoai,
		kh.TongTichDiem,
		kh.GioiTinhNam,
		nhom.TenNhomKhach,
		kh.IdNhomKhach, 
		FORMAT(checkin.DateTimeCheckIn,'dd/MM/yyyy') as DateCheckIn,
		FORMAT(checkin.DateTimeCheckIn,'hh:mm tt') as TimeCheckIn
	from KH_CheckIn checkin
	join DM_KhachHang kh on checkin.IdKhachHang= kh.Id
	left join DM_NhomKhachHang nhom on kh.IdNhomKhach= nhom.Id	
	where checkin.TrangThai= 1
	and kh.TenantId= @TenantId	
	and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
		or TenKhachHang_KhongDau like @TextSearch
		or MaKhachHang like @TextSearch
		or TenKhachHang like @TextSearch
		or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
		or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
	)
	select *
	from data_cte
	order by DateCheckIn desc
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY 
END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetDetailProduct]
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 16, 40, 47, 651, DateTimeKind.Local).AddTicks(79), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 16, 40, 47, 651, DateTimeKind.Local).AddTicks(103), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 16, 40, 47, 651, DateTimeKind.Local).AddTicks(105), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 16, 40, 47, 651, DateTimeKind.Local).AddTicks(446), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 17, 16, 40, 47, 651, DateTimeKind.Local).AddTicks(454), null, null });

            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnSplitstring]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnConvertStringToUnsign]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetNumeric]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetFirstCharOfString]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetMaHoaDon]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetMaPhieuThuChi]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetListNhomHangHoa]");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetProductCode]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetDMHangHoa]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spJqAutoCustomer]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetListCustomerChecking]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetDetailProduct]");
        }
    }
}
