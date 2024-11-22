using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql2024112202 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_booking_getAll]
    @TenantId INT = 1,
    @IdChiNhanhs nvarchar(max) ='71313109-267a-4d84-b2a3-6a1af5451597',
	@TextSearch nvarchar(max) ='',
    @TimeFrom DATETIME ='2024-11-01',
    @TimeTo DATETIME='2024-12-01',	
	@TrangThais nvarchar(max) ='3',
	@CurrentPage int = 1,
	@PageSize int = 10
AS
BEGIN
    SET NOCOUNT ON;

	if @CurrentPage > 0 set @CurrentPage = @CurrentPage - 1

	if isnull(@TextSearch,'') ='' set @TextSearch =''
	else set @TextSearch = concat('%',@TextSearch,'%')

	declare @tblChiNhanh table (Id uniqueidentifier)
	if isnull(@IdChiNhanhs,'') !=''
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
	else set @IdChiNhanhs =''

	declare @tblTrangThai table (Id tinyint)
	if isnull(@TrangThais,'') !=''
		insert into @tblTrangThai
		select GiaTri from dbo.fnSplitstring(@TrangThais)
	else set @TrangThais =''

	;with data_cte
	as
	(
    SELECT
        b.Id,
		b.BookingCode,
        isnull(bnv.IdNhanVien,'00000000-0000-0000-0000-000000000000') as IdNhanVien,
		nv.Avatar,
		isnull(nv.TenNhanVien,N'Chưa xác định') as NhanVienThucHien,
		cvu.TenChucVu,
        CONVERT(NVARCHAR(5), b.StartTime, 108) AS StartTime,
        CONVERT(NVARCHAR(5), b.EndTime, 108) AS EndTime,
        kh.TenKhachHang,
		kh.MaKhachHang,
		kh.SoDienThoai,
        hh.TenHangHoa AS TenDichVu,
        CASE DATEPART(WEEKDAY, b.BookingDate)
            WHEN 1 THEN N'Chủ nhật'
            WHEN 2 THEN N'Thứ hai'
            WHEN 3 THEN N'Thứ ba'
            WHEN 4 THEN N'Thứ tư'
            WHEN 5 THEN N'Thứ năm'
            WHEN 6 THEN N'Thứ sáu'
            WHEN 7 THEN N'Thứ bảy'
            ELSE 'Thứ hai'
        END AS DayOfWeek,
        CASE
            WHEN b.TrangThai = 0 THEN '#a19f9c'
			WHEN B.TrangThai = 1 THEN '#FF9900'
			wHEN b.TrangThai = 2 THEN '#7DC1FF'
			WHEN b.TrangThai = 3 THEN '#009EF7'
			WHEN b.TrangThai = 4 THEN '#50CD89'
            ELSE '#009EF7'
        END AS Color,
		b.BookingDate,
		b.GhiChu,
		b.TrangThai
    FROM Booking b
        left JOIN BookingNhanVien bnv ON bnv.IdBooking = b.Id and bnv.IsDeleted = 0
		LEFT JOIN NS_NhanVien nv on nv.id = bnv.IdNhanVien
        LEFT JOIN BookingService bs ON bs.IdBooking = b.Id and bs.IsDeleted = 0
        LEFT JOIN DM_DonViQuiDoi dv ON dv.Id = bs.IdDonViQuiDoi
        LEFT JOIN DM_HangHoa hh ON hh.id = dv.IdHangHoa
		left join DM_KhachHang kh on b.IdKhachHang = kh.Id
		left join NS_ChucVu cvu on nv.IdChucVu = cvu.Id
    WHERE
        (b.TenantId = ISNULL(@TenantId, 1))
        AND (b.IsDeleted = 0)
		and (@TrangThais ='' or exists (select id from @tblTrangThai tt where b.TrangThai = tt.Id)) 
		and (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where b.IdChiNhanh = cn.Id))
        AND (b.BookingDate BETWEEN @TimeFrom AND @TimeTo)
		and (@TextSearch ='' or 
			(hh.TenHangHoa like @TextSearch or hh.TenHangHoa_KhongDau like @TextSearch
			or nv.TenNhanVien like @TextSearch
			or kh.TenKhachHang like @TextSearch or kh.TenKhachHang_KhongDau like @TextSearch ))
	),
	count_cte
	as 
	(
		select count(Id) as TotalRow
		from data_cte
	)
	select *
	from data_cte
	cross join count_cte
	ORDER BY BookingDate DESC,StartTime ASC
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
