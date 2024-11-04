using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddUpdateStoreFuntion20241104 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetMaxNumber_ofMaKhachHang]");

            migrationBuilder.Sql(@"create FUNCTION [dbo].[fnGetMaxNumber_ofMaKhachHang]
(
	@IdLoaiDoiTuong tinyint = 1
)
RETURNS float
AS
BEGIN
	DECLARE @maxCode float = 1
	declare @kihieu varchar(10),  @lenMaChungTu int =0

	set @kihieu = (select top 1 MaLoaiKhachHang from DM_LoaiKhach where ID= @IdLoaiDoiTuong)
	set @lenMaChungTu  = LEN(@kihieu)


	SET @maxCode = 
		(
			select  
				max(CAST(dbo.fnGetNumeric(RIGHT(MaKhachHang,LEN(MaKhachHang)- @lenMaChungTu))AS float)) as MaxNumber
			from
			(
				select MaKhachHang
				from dbo.DM_KhachHang					
				where  MaKhachHang like @kihieu +'%'
				and IdLoaiKhach = @IdLoaiDoiTuong
			) a
			where ISNUMERIC(RIGHT(MaKhachHang,LEN(MaKhachHang)- @lenMaChungTu)) = 1
		)

	 if @maxCode is null			
		  set @maxCode = 1	
	else
		set @maxCode = FORMAT(@maxCode + 1, 'F0') --- convert dạng mũ 10 về string

	return @maxCode		
END");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_khachHang_getAll]
			@TenantId INT,
			@IdChiNhanh UNIQUEIDENTIFIER = null,
			@Filter NVARCHAR(max) =N'',
			@SkipCount INT = 1,
			@MaxResultCount INT = 10,
			@SortBy NVARCHAR(50)='CreateTime',
			@SortType VARCHAR(4)='desc',
			@IdNhomKhach UNIQUEIDENTIFIER = NULL,
			@GioiTinh BIT = NULL,
			@TongChiTieuTu FLOAT = NULL,
			@TongChiTieuDen FLOAT = NULL,
			@TimeFrom DATE = NULL,
			@TimeTo DATE = NULL
			AS
			BEGIN
				set nocount on;

				if @SkipCount > 0 set @SkipCount = @SkipCount - 1;
				if isnull(@SortType,'')!='' set @SortType = LOWER(@SortType)
				if isnull(@Filter,'')!='' set @Filter = concat(N'%', @Filter, '%')

				
					SELECT  *
					into #tblCus
					FROM
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
							isnull(tblCheckIn.SoLanCheckIn,0) as SoLanCheckIn,					
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
								SELECT IdKhachHang, 
									count(id) as SoLanCheckIn
								FROM KH_CheckIn
								WHERE IsDeleted = 0
								GROUP BY IdKhachHang
							) tblCheckIn ON kh.Id = tblCheckIn.IdKhachHang						
							WHERE kh.TenantId = @TenantId 
							AND kh.IsDeleted = 0
							AND (ISNULL(@Filter,'') = ''
								OR kh.MaKhachHang LIKE @Filter
								OR kh.TenKhachHang LIKE @Filter
								OR kh.TenKhachHang_KhongDau LIKE @Filter
								OR kh.SoDienThoai LIKE @Filter
								OR kh.DiaChi LIKE @Filter
								OR nkh.TenNhomKhach LIKE @Filter
							)
							AND (@IdNhomKhach IS NULL OR (nkh.Id = @IdNhomKhach AND @IdNhomKhach IS NOT NULL))
							AND (@TimeFrom IS NULL OR @TimeTo IS NULL OR CAST(kh.CreationTime AS DATE) BETWEEN @TimeFrom AND @TimeTo)
							AND (@GioiTinh IS NULL OR kh.GioiTinhNam = @GioiTinh)
							AND (@TongChiTieuTu IS NULL OR @TongChiTieuDen IS NULL OR TongChiTieu BETWEEN @TongChiTieuTu AND @TongChiTieuDen)
						) as Result

					select cus.*,
						isnull(tblBooking.SoLanBooking,0) as SoLanBooking,
						tblBooking.CuocHenGanNhat,
						isnull(cus.TongChiTieu,0) - isnull(tblSoquy.TongThanhToan,0) as ConNo
					from #tblCus cus
					left join
					(
						------ get thu/chi of customer ---
						select 
							qct.IdKhachHang,
							----- 11.thu/12.chi
							sum(iif(qhd.IdLoaiChungTu =11, qct.TienThu, - qct.TienThu)) as TongThanhToan
						from QuyHoaDon qhd
						join QuyHoaDon_ChiTiet qct on qhd.Id= qct.IdQuyHoaDon
						where qhd.IsDeleted='0'
						and qct.IsDeleted ='0'
						and qhd.TenantId = @TenantId
						and exists (select id from #tblCus cusIn where cusIn.Id = qct.IdKhachHang)
						group by qct.IdKhachHang
					)tblSoquy on cus.Id = tblSoquy.IdKhachHang
					left join
					(
						select IdKhachHang,
							max(BookingDate) as CuocHenGanNhat, 
							count(IdKhachHang) as SoLanBooking
						from Booking bk
						where bk.IsDeleted=0 
						and bk.TrangThai !=0 ---- trangthai=0: datlich, nhung sau do huy
						and exists (select id from #tblCus cusIn where cusIn.Id = bk.IdKhachHang)
						group by bk.IdKhachHang
					)tblBooking on cus.id = tblBooking.IdKhachHang
					ORDER BY
						CASE WHEN @SortType = 'asc' AND @SortBy = 'createTime' THEN CreationTime END,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'createTime' THEN CreationTime END DESC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'diaChi' THEN DiaChi END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'ngaySinh' THEN NgaySinh END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenNguonKhach' THEN TenNguonKhach END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'gioiTinh' THEN GioiTinh END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tongTichDiem' THEN TongTichDiem END,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenKhachHang' THEN TenKhachHang END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'soDienThoai' THEN SoDienThoai END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'diaChi' THEN DiaChi END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'ngaySinh' THEN NgaySinh END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenNguonKhach' THEN TenNguonKhach END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'gioiTinh' THEN GioiTinh END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenNhomKhach' THEN TenNhomKhach END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tongChiTieu' THEN TongChiTieu END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tongTichDiem' THEN TongTichDiem END DESC,						
						CASE WHEN ISNULL(@SortType,'') = '' THEN CreationTime END DESC
						OFFSET (@SkipCount* @MaxResultCount) ROWS
						FETCH NEXT @MaxResultCount ROWS ONLY
				

					SELECT COUNT(Id) as TotalCount
					FROM #tblCus 
					END;");

			migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spImportDanhMucKhachHang]
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
	declare @idKhachHang uniqueidentifier = null
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
				set @idKhachHang = NEWID();
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
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fnGetMaxNumber_ofMaKhachHang]");
        }
    }
}
