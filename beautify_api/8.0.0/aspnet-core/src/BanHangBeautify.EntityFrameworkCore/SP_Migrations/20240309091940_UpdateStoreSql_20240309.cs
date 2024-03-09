using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20240309 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROC [dbo].[prc_baoCao_BanHangTongHop]
	@TenantId int = 1,
	@IdChiNhanhs nvarchar(max) ='',
	@FromDate datetime = '2023-03-01',
	@ToDate datetime = '2024-03-31',
	@IdNhomHangHoa nvarchar(40) ='b02adbdf-4688-48c8-8dc2-3f0083f786e2',
	@TextSearch nvarchar(max) ='',
	@ColumnSort nvarchar(50)='tenNhomHang',
	@TypeSort nvarchar(4) ='asc',
	@CurrentPage int = 1,
	@PageSize int = 50
AS
	BEGIN
	SET NOCOUNT ON;
	declare @tblChiNhanh table (Id uniqueidentifier)
	declare @tblNhomHangHoa table (Id uniqueidentifier)

	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1

	if isnull(@ColumnSort,'')=''
		set @ColumnSort = 'tenNhomHang'

	if isnull(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%',@TextSearch, '%')
	else set @TextSearch ='%%'

	if isnull(@ToDate,'')!='' set @ToDate = DATEADD(day,1, @ToDate)

	if isnull(@IdChiNhanhs,'')!=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
		else set @IdChiNhanhs =''

	if isnull(@IdNhomHangHoa, '')!=''
		begin
		insert into @tblNhomHangHoa
		select ID from dbo.fnGetListNhomHangHoa(@IdNhomHangHoa)
		end
	else set @IdNhomHangHoa = ''

	
		;with data_cte
		as
		(
			SELECT 
				dvqd.Id,
				dvqd.MaHangHoa,
				hh.TenHangHoa,		
				ISNULL(nhh.TenNhomHang,N'Nhóm mặc định') as TenNhomHang,			
				SUM(hdct.SoLuong) as SoLuong,
				SUM(hdct.SoLuong * hdct.TienChietKhau) as TienChietKhau,
				SUM(hdct.SoLuong * hdct.DonGiaTruocCK) as ThanhTienTruocCK,
				SUM(hdct.SoLuong * hdct.DonGiaSauCK)  as ThanhTienSauCK
			FROM BH_HoaDon hd
			JOIN BH_HoaDon_ChiTiet hdct on hdct.IdHoaDon = hd.Id and hdct.IsDeleted = 0
			JOIN DM_DonViQuiDoi dvqd on hdct.IdDonViQuyDoi= dvqd.Id
			JOIN DM_HangHoa hh on dvqd.IdHangHoa = hh.Id
			LEFT JOIN DM_NhomHangHoa nhh on hh.IdNhomHangHoa = nhh.Id
			WHERE hd.TenantId = @TenantId and hd.IsDeleted = 0
			AND (@FromDate is null or hd.NgayLapHoaDon >= @FromDate)
			AND (@ToDate is null or hd.NgayLapHoaDon <  @ToDate)
			AND (@IdNhomHangHoa = '' or exists (select id from @tblNhomHangHoa nhom where nhom.Id = hh.IdNhomHangHoa ))
			AND (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where cn.Id = hd.IdChiNhanh ))
			AND (@TextSearch ='%%'
				or hd.MaHoaDon like @TextSearch
				or hh.TenHangHoa like @TextSearch
				or hh.TenHangHoa_KhongDau like @TextSearch
				or dvqd.MaHangHoa like @TextSearch
				or nhh.TenNhomHang like @TextSearch
				or nhh.TenNhomHang_KhongDau like @TextSearch)		
			GROUP BY
				dvqd.Id,
				dvqd.MaHangHoa,
				hh.TenHangHoa,	
				nhh.TenNhomHang
		),
		tblSum
		as
		(
			select count(Id) as TotalCount,
			sum(SoLuong) as SumSoLuong,
			sum(TienChietKhau) as SumTienChietKhau,
			sum(ThanhTienTruocCK) as SumThanhTienTruocCK,
			sum(ThanhTienSauCK) as SumThanhTienSauCK
			from data_cte
		)

		SELECT * 
		FROM data_cte
		cross join tblSum
		ORDER BY
			CASE WHEN LOWER(@TypeSort) = 'asc' AND @ColumnSort = 'maHangHoa' THEN MaHangHoa END ASC,
			CASE WHEN LOWER(@TypeSort) = 'asc' AND @ColumnSort = 'tenHangHoa' THEN TenHangHoa END ASC,
			CASE WHEN LOWER(@TypeSort) = 'asc' AND @ColumnSort = 'TenNhomHang' THEN TenNhomHang END ASC,
			CASE WHEN LOWER(@TypeSort) = 'asc' AND @ColumnSort = 'soLuong' THEN SoLuong END ASC,
			CASE WHEN LOWER(@TypeSort) = 'asc' AND @ColumnSort = 'TienChietKhau' THEN TienChietKhau END ASC,
			CASE WHEN LOWER(@TypeSort) = 'asc' AND @ColumnSort = 'ThanhTienTruocCK' THEN ThanhTienTruocCK END ASC,
			CASE WHEN LOWER(@TypeSort) = 'asc' AND @ColumnSort = 'ThanhTienSauCK' THEN ThanhTienSauCK END ASC,

			CASE WHEN LOWER(@TypeSort) = 'desc' AND @ColumnSort = 'maHangHoa' THEN MaHangHoa END desc,
			CASE WHEN LOWER(@TypeSort) = 'desc' AND @ColumnSort = 'tenHangHoa' THEN TenHangHoa END desc,
			CASE WHEN LOWER(@TypeSort) = 'desc' AND @ColumnSort = 'TenNhomHang' THEN TenNhomHang END desc,
			CASE WHEN LOWER(@TypeSort) = 'desc' AND @ColumnSort = 'soLuong' THEN SoLuong END desc,
			CASE WHEN LOWER(@TypeSort) = 'desc' AND @ColumnSort = 'TienChietKhau' THEN TienChietKhau END desc,
			CASE WHEN LOWER(@TypeSort) = 'desc' AND @ColumnSort = 'ThanhTienTruocCK' THEN ThanhTienTruocCK END desc,
			CASE WHEN LOWER(@TypeSort) = 'desc' AND @ColumnSort = 'ThanhTienSauCK' THEN ThanhTienSauCK END desc
		OFFSET (@CurrentPage* @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY

END;");
            migrationBuilder.Sql(@"ALTER PROC [dbo].[prc_baoCao_BanHangChiTiet]
	@TenantId int = 1,
	@IdChiNhanhs nvarchar(max) ='',
	@FromDate datetime = '2024-03-01',
	@ToDate datetime = '2024-03-31',
	@IdNhomHangHoa nvarchar(40) ='',
	@TextSearch nvarchar(max) ='',
	@ColumnSort nvarchar(50)='ngayLapHoaDon',
	@TypeSort nvarchar(4) ='desc',
	@CurrentPage int = 1,
	@PageSize int = 50
AS
BEGIN
	declare @tblChiNhanh table (Id uniqueidentifier)
	declare @tblNhomHangHoa table (Id uniqueidentifier)

	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1

	if isnull(@ColumnSort,'')=''
		set @ColumnSort = 'ngayLapHoaDon'
	if isnull(@TypeSort,'')=''
		set @TypeSort = 'desc'

	if isnull(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%',@TextSearch, '%')
	else set @TextSearch ='%%'

	if isnull(@ToDate,'')!='' set @ToDate = DATEADD(day,1, @ToDate)


	if isnull(@IdChiNhanhs,'')!=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
		else set @IdChiNhanhs =''

	if isnull(@IdNhomHangHoa, '')!=''
		begin
		insert into @tblNhomHangHoa
		select ID from dbo.fnGetListNhomHangHoa(@IdNhomHangHoa)
		end
	else set @IdNhomHangHoa = ''


	;with data_cte
		as
		(
			SELECT 
				hd.Id,
				hd.MaHoaDon,
				hd.NgayLapHoaDon,
				kh.SoDienThoai,
				ISNULL(kh.TenKhachHang,N'Khách lẻ') as TenKhachHang,	
				dvqd.MaHangHoa,
				hh.TenHangHoa,		
				ISNULL(nhh.TenNhomHang,N'Nhóm mặc định') as TenNhomHang,			
				hdct.SoLuong,
				hdct.DonGiaTruocCK,
				hdct.SoLuong * hdct.TienChietKhau as TienChietKhau,
				hdct.SoLuong * hdct.DonGiaTruocCK as ThanhTienTruocCK,
				hdct.SoLuong * hdct.DonGiaSauCK  as ThanhTienSauCK
			FROM BH_HoaDon hd
			JOIN BH_HoaDon_ChiTiet hdct on hdct.IdHoaDon = hd.Id and hdct.IsDeleted = 0
			left join DM_KhachHang kh on hd.IdKhachHang = kh.Id
			JOIN DM_DonViQuiDoi dvqd on hdct.IdDonViQuyDoi= dvqd.Id
			JOIN DM_HangHoa hh on dvqd.IdHangHoa = hh.Id
			LEFT JOIN DM_NhomHangHoa nhh on hh.IdNhomHangHoa = nhh.Id
			WHERE hd.TenantId = @TenantId and hd.IsDeleted = 0
			AND (@FromDate is null or hd.NgayLapHoaDon >= @FromDate)
			AND (@ToDate is null or hd.NgayLapHoaDon <  @ToDate)
			AND (@IdNhomHangHoa = '' or exists (select id from @tblNhomHangHoa nhom where nhom.Id = hh.IdNhomHangHoa ))
			AND (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where cn.Id = hd.IdChiNhanh ))
			AND (@TextSearch ='%%'
				or hd.MaHoaDon like @TextSearch
				or kh.TenKhachHang like @TextSearch
				or kh.TenKhachHang_KhongDau like @TextSearch
				or kh.SoDienThoai like @TextSearch
				or hh.TenHangHoa like @TextSearch
				or hh.TenHangHoa_KhongDau like @TextSearch
				or dvqd.MaHangHoa like @TextSearch
				or nhh.TenNhomHang like @TextSearch
				or nhh.TenNhomHang_KhongDau like @TextSearch)				
		),
		tblSum
		as
		(
			select count(Id) as TotalCount,
			sum(SoLuong) as SumSoLuong,
			sum(TienChietKhau) as SumTienChietKhau,
			sum(ThanhTienTruocCK) as SumThanhTienTruocCK,
			sum(ThanhTienSauCK) as SumThanhTienSauCK
			from data_cte
		)
		SELECT * 
		FROM data_cte
		cross join tblSum
		ORDER BY
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'MaHoaDon' THEN MaHoaDon END ASC,
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'NgayLapHoaDon' THEN NgayLapHoaDon END ASC,
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'maHangHoa' THEN MaHangHoa END ASC,
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'tenHangHoa' THEN TenHangHoa END ASC,
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'TenNhomHang' THEN TenNhomHang END ASC,
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'soLuong' THEN SoLuong END ASC,
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'DonGiaTruocCK' THEN DonGiaTruocCK END ASC,
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'TienChietKhau' THEN TienChietKhau END ASC,
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'ThanhTienTruocCK' THEN ThanhTienTruocCK END ASC,
			CASE WHEN @TypeSort = 'asc' AND @ColumnSort = 'ThanhTienSauCK' THEN ThanhTienSauCK END ASC,

			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'MaHoaDon' THEN MaHoaDon END desc,
			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'NgayLapHoaDon' THEN NgayLapHoaDon END desc,
			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'maHangHoa' THEN MaHangHoa END desc,
			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'tenHangHoa' THEN TenHangHoa END desc,
			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'TenNhomHang' THEN TenNhomHang END desc,
			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'soLuong' THEN SoLuong END desc,
			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'DonGiaTruocCK' THEN DonGiaTruocCK END desc,
			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'TienChietKhau' THEN TienChietKhau END desc,
			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'ThanhTienTruocCK' THEN ThanhTienTruocCK END desc,
			CASE WHEN @TypeSort = 'desc' AND @ColumnSort = 'ThanhTienSauCK' THEN ThanhTienSauCK END desc
		OFFSET (@CurrentPage* @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
