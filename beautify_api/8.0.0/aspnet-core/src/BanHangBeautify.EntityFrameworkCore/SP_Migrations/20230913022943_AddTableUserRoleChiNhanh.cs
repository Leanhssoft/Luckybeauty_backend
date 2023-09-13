using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddTableUserRoleChiNhanh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpUserRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "IdChiNhanh",
                table: "AbpUserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_IdChiNhanh",
                table: "AbpUserRoles",
                column: "IdChiNhanh");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUserRoles_DM_ChiNhanh_IdChiNhanh",
                table: "AbpUserRoles",
                column: "IdChiNhanh",
                principalTable: "DM_ChiNhanh",
                principalColumn: "Id");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spImportDanhMucHangHoa]
	@TenantId int = 1,
	@CreatorUserId int,
	@TenNhomHangHoa nvarchar(max)='',
	@MaHangHoa nvarchar(max)='',
	@TenHangHoa nvarchar(max)='',
	@IdLoaiHangHoa int = 2,
	@GiaBan float =0,
	@SoPhutThucHien float =0,
	@GhiChu nvarchar(max)=''
AS
BEGIN
	
	SET NOCOUNT ON;
	if(isnull(@GiaBan,'0')='') set @GiaBan ='0'
	declare @idNhomHangHoa uniqueidentifier = null

	if (ISNULL(@TenNhomHangHoa,'')!='') --- tên nhóm != rỗng
		begin
			select top 1 @idNhomHangHoa = Id from DM_NhomHangHoa where TenNhomHang like @TenNhomHangHoa and IsDeleted='0'
			if @idNhomHangHoa is null
				begin
					--- thêm nhóm mới nếu chưa tồn tại ---
					set @idNhomHangHoa = NEWID()
					insert into DM_NhomHangHoa (TenantId,Id, MaNhomHang, TenNhomHang, TenNhomHang_KhongDau, LaNhomHangHoa, CreationTime, CreatorUserId, IsDeleted)
					values(@TenantId, @idNhomHangHoa, 
						(select dbo.fnGetFirstCharOfString(@TenNhomHangHoa)), --- manhomhang: lay ki tu dau --
						@TenNhomHangHoa, 
						(select dbo.fnConvertStringToUnsign(@TenNhomHangHoa)), --- tenkhongdau --
						iif(@IdLoaiHangHoa !=1,'1','0'), 
						getdate(),
						@CreatorUserId,
						'0')
				end
		end

	------ DmHangHoa + Dm_DonViQuyDoi ---
	declare @tenHangHoa_KhongDau nvarchar(max) = (select dbo.fnConvertStringToUnsign(@TenHangHoa))
	declare @idHangHoa uniqueidentifier = newid(), @idQuyDoi uniqueidentifier = null
	declare @isUpdate bit ='0'
	if (ISNULL(@MaHangHoa,'')='') ---- nếu mã rỗng: phát sinh mã mới --
		begin
			declare @tblMaHangHoa table(MaLoaiHang varchar(5), MaxVal float)
			insert into @tblMaHangHoa
			exec dbo.SpGetProductCode @TenantId, @IdLoaiHangHoa
			
			declare @max float, @maloaihang varchar(5)
			select top 1 @maloaihang = MaLoaiHang, @max = MaxVal from @tblMaHangHoa

			declare @maxFormat NVARCHAR(max)= FORMAT(@max, 'F0');
			set @MaHangHoa = concat(@maloaihang, iif(@max < 10, '0'+ @maxFormat, @maxFormat)) --- nếu mã <10: thêm số 0 ở đầu		
		end
	else
		begin		
			Set @MaHangHoa = UPPER(@MaHangHoa)
			select top 1 @idHangHoa = IdHangHoa, @idQuyDoi = Id from DM_DonViQuiDoi where MaHangHoa like @MaHangHoa and IsDeleted='0'
			if @idQuyDoi is not null set @isUpdate ='1'
		end

	if  @isUpdate ='0'
		begin
				insert into DM_HangHoa (TenantId, Id, IdLoaiHangHoa, IdNhomHangHoa, TenHangHoa, TenHangHoa_KhongDau, SoPhutThucHien, MoTa, TrangThai, IsDeleted, CreatorUserId, CreationTime)
				values (@TenantId, @idHangHoa, @IdLoaiHangHoa, @idNhomHangHoa, @TenHangHoa, @tenHangHoa_KhongDau, @SoPhutThucHien,	@GhiChu,			
					1,'0',@CreatorUserId, GETDATE())

				insert into DM_DonViQuiDoi (TenantId, Id, IdHangHoa, MaHangHoa, TyLeChuyenDoi, LaDonViTinhChuan, TenDonViTinh, GiaBan, CreatorUserId, CreationTime, IsDeleted)
				values (@TenantId, NEWID(), @idHangHoa, @MaHangHoa,1,1,'',@GiaBan, @CreatorUserId, GETDATE(), '0')
			
		end
	else
		begin
			update DM_HangHoa set IdLoaiHangHoa = @IdLoaiHangHoa,
								IdNhomHangHoa = @idNhomHangHoa,
								TenHangHoa = @TenHangHoa, 
								TenHangHoa_KhongDau = @tenHangHoa_KhongDau,
								SoPhutThucHien = @SoPhutThucHien,
								MoTa = @GhiChu,
								LastModifierUserId = @CreatorUserId,
								LastModificationTime =  GETDATE()					
			where Id = @idHangHoa

			update DM_DonViQuiDoi set MaHangHoa = @MaHangHoa,
								GiaBan = @GiaBan,
								LastModifierUserId = @CreatorUserId,
								LastModificationTime =  GETDATE()	
			where Id = @idQuyDoi
		end

  
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUserRoles_DM_ChiNhanh_IdChiNhanh",
                table: "AbpUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AbpUserRoles_IdChiNhanh",
                table: "AbpUserRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpUserRoles");

            migrationBuilder.DropColumn(
                name: "IdChiNhanh",
                table: "AbpUserRoles");
        }
    }
}
