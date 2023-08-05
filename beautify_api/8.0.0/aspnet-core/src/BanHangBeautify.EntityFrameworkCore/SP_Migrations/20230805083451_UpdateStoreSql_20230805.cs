using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20230805 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_GetInforBooking_byID
	@IdBooking uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON;

   select 
		bk.id as IdBooking,
		bk.IdKhachHang,
		bk.TrangThai,
		bk.StartTime,
		bk.EndTime,
		bk.BookingDate,
		case bk.TrangThai
			when 1 then N'Chưa xác nhận'
			when 2 then N'Đã xác nhận'
			when 3 then N'Đã check in'
		else '' end as TxtTrangThaiBook,
		kh.MaKhachHang,
		kh.TenKhachHang,
		kh.SoDienThoai,
		qd.MaHangHoa,
		qd.GiaBan,
		hh.TenHangHoa,
		qd.Id as IdDonViQuyDoi,
		qd.IdHangHoa,
		hh.IdNhomHangHoa,
		hh.IdLoaiHangHoa
	from Booking bk
	left join DM_KhachHang kh on bk.IdKhachHang = kh.Id
	left join BookingService bkS on bk.Id= bkS.IdBooking
	left join DM_DonViQuiDoi qd on bkS.IdDonViQuiDoi= qd.Id
	left join DM_HangHoa hh on qd.IdHangHoa= hh.Id
	left join BookingNhanVien bkN on bk.Id = bkN.IdNhanVien
	left join NS_NhanVien nv on bkN.IdNhanVien = nv.Id
	where bk.Id = @IdBooking
END");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_getKhachHang_Booking]
	@TenantId int= 7,
	@IdChiNhanhs nvarchar(max)= null,
	@TextSearch nvarchar(max) =null,
	@CurrentPage int =0,
	@PageSize int = 10,
	@TrangThaiBook int =3  ---0.xoa, 3.all
AS
BEGIN
	
	SET NOCOUNT ON;

	if(isnull(@TextSearch,'')!='') set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch=''

	declare @tblChiNhanh table (ID uniqueidentifier)
	if(isnull(@IdChiNhanhs,'') !='')
		insert into @tblChiNhanh
		select GiaTri from  dbo.fnSplitstring(@IdChiNhanhs)
	else
		set @IdChiNhanhs=''
		

	; with data_cte
	as
	(
    select 
		bk.id as IdBooking,
		bk.IdKhachHang,
		bk.TrangThai,
		bk.StartTime,
		bk.EndTime,
		bk.BookingDate,
		case bk.TrangThai
			when 1 then N'Chưa xác nhận'
			when 2 then N'Đã xác nhận'
			when 3 then N'Đã check in'
		else '' end as TxtTrangThaiBook,
		kh.MaKhachHang,
		kh.TenKhachHang,
		kh.SoDienThoai,
		qd.MaHangHoa,
		qd.GiaBan,
		hh.TenHangHoa,
		qd.Id as IdDonViQuyDoi,
		qd.IdHangHoa,
		hh.IdNhomHangHoa,
		hh.IdLoaiHangHoa
	from Booking bk
	left join DM_KhachHang kh on bk.IdKhachHang = kh.Id
	left join BookingService bkS on bk.Id= bkS.IdBooking
	left join DM_DonViQuiDoi qd on bkS.IdDonViQuiDoi= qd.Id
	left join DM_HangHoa hh on qd.IdHangHoa= hh.Id
	left join BookingNhanVien bkN on bk.Id = bkN.IdNhanVien
	left join NS_NhanVien nv on bkN.IdNhanVien = nv.Id
	where bk.TenantId = @TenantId
	and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where bk.IdChiNhanh= cn.ID))
	and (@TrangThaiBook = 3 or  bk.TrangThai = @TrangThaiBook)
	and not exists (select id from Booking_CheckIn_HoaDon bkHD where bk.Id = bkHD.IdBooking) ---khong lay khach dang checkin 
	and (@TextSearch =''
	 or kh.MaKhachHang like @TextSearch
	 or kh.TenKhachHang  like @TextSearch
	 or kh.TenKhachHang_KhongDau  like @TextSearch
	 or kh.SoDienThoai like @TextSearch
	 or qd.MaHangHoa like @TextSearch
	 or hh.TenHangHoa like @TextSearch
	 or hh.TenHangHoa_KhongDau like @TextSearch
	 )
	 ),
	 count_cte
	 as(
	 select count(IdBooking) as TotalRow
	 from data_cte
	 )
	 select *
	 from data_cte 
	 cross join count_cte
	 order by BookingDate
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY

END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[prc_GetInforBooking_byID]");
        }
    }
}
