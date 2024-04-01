using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddStoreSql20230915 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetCustomerCode]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spImportDanhMucKhachHang]");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spGetCustomerCode]
(
	@TenantId int,
	@IdLoaiKhachHang int
)
AS
BEGIN
	DECLARE @MaLoai varchar(5) =(select top 1 MaLoaiKhachHang from DM_LoaiKhach where Id= @IdLoaiKhachHang)
    DECLARE @Return float = 0
	
	SELECT @Return = MAX(CAST (dbo.fnGetNumeric(MaKhachHang) AS float))
    FROM DM_KhachHang
	WHERE TenantId= @TenantId
	and IdLoaiKhach = @IdLoaiKhachHang
	and MaKhachHang like @MaLoai +'%'
	and CHARINDEX('_',MaKhachHang)= 0
	
    SELECT @MaLoai as FirstStr, isnull(@Return,0) + 1 as MaxVal

END ");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[spImportDanhMucKhachHang]
	@TenantId int = 1,
	@CreatorUserId int,
	@TenNhomKhachHang nvarchar(max)='',
	@IdLoaiKhach int =1, ---- nếu sau mở rộng: dùng cho cả Nhà cung cấp (1.KH, 2.NCC)
	@MaKhachHang nvarchar(max)='',
	@TenKhachHang nvarchar(max)='',
	@SoDienThoai nvarchar(max)='',
	@NgaySinh datetime = null,
	@GioiTinhNam bit = '0',
	@DiaChi nvarchar(max) ='',
	@MoTa nvarchar(max)=''
AS
BEGIN
	
	SET NOCOUNT ON;

	declare @idNhomKhachHang uniqueidentifier = null

	if (ISNULL(@TenNhomKhachHang,'')!='') --- tên nhóm != rỗng
		begin
			select top 1 @idNhomKhachHang = Id from DM_NhomKhachHang where TenNhomKhach like @TenNhomKhachHang and IsDeleted='0'
			if @idNhomKhachHang is null
				begin
					--- thêm nhóm mới nếu chưa tồn tại ---
					set @idNhomKhachHang = NEWID()
					insert into DM_NhomKhachHang (TenantId,Id, MaNhomKhach, TenNhomKhach, CreationTime, CreatorUserId, IsDeleted)
					values(@TenantId, @idNhomKhachHang, 
						(select dbo.fnGetFirstCharOfString(@TenNhomKhachHang)), --- manhomhang: lay ki tu dau --
						@TenNhomKhachHang, 					
						getdate(),
						@CreatorUserId,
						'0')
				end
		end

	------ DmKhachHang ---
	declare @tenKhachHang_KhongDau nvarchar(max) = (select dbo.fnConvertStringToUnsign(@TenKhachHang))
	declare @idKhachHang uniqueidentifier = newid()
	declare @isUpdate bit ='0'
	if (ISNULL(@MaKhachHang,'')='') ---- nếu mã rỗng: phát sinh mã mới --
		begin
			declare @tblMaKhachHang table(MaLoaiKhachHang varchar(5), MaxVal float)
			insert into @tblMaKhachHang
			exec dbo.spGetCustomerCode @TenantId, @IdLoaiKhach
			
			declare @max float, @maloaihang varchar(5)
			select top 1 @maloaihang = MaLoaiKhachHang, @max = MaxVal from @tblMaKhachHang

			declare @maxFormat NVARCHAR(max)= FORMAT(@max, 'F0');
			set @MaKhachHang = concat(@maloaihang, iif(@max < 10, '0'+ @maxFormat, @maxFormat)) --- nếu mã <10: thêm số 0 ở đầu		
		end
	else
		begin		
			Set @MaKhachHang = UPPER(@MaKhachHang)
			select top 1 @idKhachHang = Id from DM_KhachHang where MaKhachHang like @MaKhachHang and IsDeleted='0'
			if @idKhachHang is not null set @isUpdate ='1'
		end

	if  @isUpdate ='0'
		begin
				insert into DM_KhachHang (TenantId, Id, IdLoaiKhach,  IdNhomKhach,MaKhachHang, TenKhachHang, TenKhachHang_KhongDau, SoDienThoai, NgaySinh, GioiTinhNam, DiaChi, MoTa, TrangThai, IsDeleted, CreatorUserId, CreationTime)
				values (@TenantId, @idKhachHang, @IdLoaiKhach, @idNhomKhachHang, @MaKhachHang, @TenKhachHang, @tenKhachHang_KhongDau, @SoDienThoai,	@NgaySinh,	@GioiTinhNam, @DiaChi, @MoTa,		
					1,'0',@CreatorUserId, GETDATE())						
		end
	else
		begin
			update DM_KhachHang set 
								IdNhomKhach = @idNhomKhachHang,
								MaKhachHang = @MaKhachHang, 
								TenKhachHang = @MaKhachHang, 
								TenKhachHang_KhongDau = @tenKhachHang_KhongDau,
								SoDienThoai = @SoDienThoai,
								NgaySinh = @NgaySinh,
								GioiTinhNam = @GioiTinhNam,
								DiaChi = @DiaChi,
								MoTa = @MoTa,
								LastModifierUserId = @CreatorUserId,
								LastModificationTime =  GETDATE()					
			where Id = @idKhachHang		
		end  
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spGetCustomerCode]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[spImportDanhMucKhachHang]");
        }
    }
}
