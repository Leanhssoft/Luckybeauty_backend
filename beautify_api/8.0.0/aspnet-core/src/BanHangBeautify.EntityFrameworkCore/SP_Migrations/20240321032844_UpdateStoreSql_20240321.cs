using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20240321 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spJqAutoCustomer_byIdLoaiTin]	
	@IdLoaiTin int= 2,
	@IdChiNhanhs nvarchar(max)= 'c4fbe44f-c26e-499f-9033-af9c4e3c6fc3',
	@TextSearch nvarchar(max)= '09',
	@FromDate datetime= '2023-11-06',
	@ToDate datetime= '2023-11-12',
	@IsUserZalo bit ='0',
	@CurrentPage int=0,
	@PageSize int = 50
AS
BEGIN
	
	SET NOCOUNT ON;

	if ISNULL(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch='%%'

	declare @tblChiNhanh table(ID uniqueidentifier primary key)
	if(isnull(@IdChiNhanhs,'')!='')
		insert into @tblChiNhanh
		select distinct GiaTri from dbo.fnSplitstring(@IdChiNhanhs)

	if(@IdLoaiTin=2) ---- khách hàng có sinh nhật từ - đến
		begin
			declare @birthday_dayFrom int = datepart(day,@FromDate) - 1, ---- trừ 1: để tránh >= dateFrom
					@birthday_dayTo int = datepart(day,@ToDate) + 1, ---- cộng 1: để tránh <= dateTo
					@birthday_monthFrom  int = datepart(month,@FromDate),
					@birthday_monthTo  int = datepart(month,@ToDate)
		
			if(@birthday_monthFrom!= @birthday_monthTo)
					begin
						----- lọc ngày sinh # tháng (vd: 28/09 -->22/11)	 -----

							select b.*,
								customerZalo.ZOAUserId
							from
							(
								select  kh.Id as IdKhachHang, 
										kh.MaKhachHang, 
										kh.TenKhachHang, 
										kh.SoDienThoai,
										kh.TongTichDiem,
										kh.Email,
										kh.IdKhachHangZOA,
										datepart(day,NgaySinh) as NgaySinhNhat,
										datepart(month,NgaySinh) as ThangSinhNhat
								from DM_KhachHang kh								
								where kh.TrangThai in (0,1)
								and kh.NgaySinh is not null
								and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
									or TenKhachHang_KhongDau like @TextSearch
									or MaKhachHang like @TextSearch
									or TenKhachHang like @TextSearch
									or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
									or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
								) b
								left join Zalo_KhachHangThanhVien customerZalo on b.IdKhachHangZOA = customerZalo.Id
								where ((b.ThangSinhNhat > @birthday_monthFrom and b.ThangSinhNhat < @birthday_monthTo)
								or (b.ThangSinhNhat = @birthday_monthFrom and b.NgaySinhNhat > @birthday_dayFrom)
								or (b.ThangSinhNhat = @birthday_monthTo and b.NgaySinhNhat < @birthday_dayTo))
								and (@IsUserZalo ='0' or (@IsUserZalo ='1' and b.IdKhachHangZOA is not null))
							
					end
				else
					begin
							----- lọc ngày sinh cung  ------		
							select b.*,
								customerZalo.ZOAUserId
							from
							(
								select  kh.Id as IdKhachHang,
										kh.MaKhachHang, 
										kh.TenKhachHang,
										kh.SoDienThoai,
										kh.TongTichDiem,		
										kh.Email,
										kh.IdKhachHangZOA,
										datepart(day,NgaySinh) as NgaySinhNhat,
										datepart(month,NgaySinh) as ThangSinhNhat
								from DM_KhachHang kh													
								where kh.TrangThai in (0,1)
								and kh.NgaySinh is not null
								and (SoDienThoai like @TextSearch 
									or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
									or TenKhachHang_KhongDau like @TextSearch
									or MaKhachHang like @TextSearch
									or TenKhachHang like @TextSearch
									or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
									or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
								) b
								left join Zalo_KhachHangThanhVien customerZalo on b.IdKhachHangZOA = customerZalo.Id
								where b.ThangSinhNhat = @birthday_monthFrom
								and b.NgaySinhNhat > @birthday_dayFrom
								and b.NgaySinhNhat < @birthday_dayTo
								and (@IsUserZalo ='0' or (@IsUserZalo ='1' and b.IdKhachHangZOA is not null))
							
					end	
		end

	if(@IdLoaiTin=3) ---- khách hàng có lịch hẹn từ - đến
		begin
			set @ToDate = DATEADD(day,1,@ToDate)

			select distinct
				kh.Id as IdKhachHang, 
				kh.MaKhachHang,
				kh.TenKhachHang,
				kh.SoDienThoai,
				kh.Email,
				customerZalo.ZOAUserId,
				bk.IdBooking,
				isnull(kh.TongTichDiem,0) as TongTichDiem
			from
			(
			select bk.Id as IdBooking,
				bk.IdKhachHang
			from Booking bk
			where bk.TrangThai in (1,2) --- datlich/daxacnhan
			and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where bk.IdChiNhanh= cn.ID))
			and bk.BookingDate >= @FromDate and bk.BookingDate < @ToDate
			)bk
			join DM_KhachHang kh on bk.IdKhachHang= kh.Id
			left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHangZOA = customerZalo.Id
			where kh.TrangThai in (0,1)
				and (@IsUserZalo ='0' or (@IsUserZalo ='1' and kh.IdKhachHangZOA is not null))
				and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
					or TenKhachHang_KhongDau like @TextSearch
					or MaKhachHang like @TextSearch
					or TenKhachHang like @TextSearch
					or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
					or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
		end

		if(@IdLoaiTin=4)
		begin
			---- tin giao dich ---
			set @ToDate = DATEADD(day,1,@ToDate)
			select distinct
				kh.Id as IdKhachHang, 
				kh.MaKhachHang,
				kh.TenKhachHang,
				kh.SoDienThoai,
				kh.Email,
				hd.IdHoaDon,
				customerZalo.ZOAUserId,
				isnull(kh.TongTichDiem,0) as TongTichDiem
			from
			(
			select 
				hd.Id as IdHoaDon,
				hd.IdKhachHang
			from BH_HoaDon hd
			where hd.TrangThai=3 -- hoanthanh
			and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where hd.IdChiNhanh= cn.ID))
			and hd.NgayLapHoaDon between @FromDate and @ToDate
			) hd 
			join DM_KhachHang kh on hd.IdKhachHang= kh.Id
			left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHangZOA = customerZalo.Id
			where kh.TrangThai in (0,1)
				and (@IsUserZalo ='0' or (@IsUserZalo ='1' and kh.IdKhachHangZOA is not null))
				and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
					or TenKhachHang_KhongDau like @TextSearch
					or MaKhachHang like @TextSearch
					or TenKhachHang like @TextSearch
					or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
					or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
		end
    
END");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spJqAutoCustomer]
	@TenantId int= 3,
	@LoaiDoiTuong int= 1,
	@IsUserZalo int = 0,
	@TextSearch nvarchar(max)='',
	@CurrentPage int=0,
	@PageSize int = 50
AS
BEGIN
	
	SET NOCOUNT ON;



	if ISNULL(@TextSearch,'')!=''
		set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch='%%'

	;with data_cte
	as(
	select  kh.Id, kh.TenantId, kh.MaKhachHang, kh.TenKhachHang, kh.SoDienThoai,
		 kh.TongTichDiem,
		 kh.GioiTinhNam,
		 kh.IdKhachHangZOA,
		 nhom.TenNhomKhach
	from DM_KhachHang kh
	left join DM_NhomKhachHang nhom on kh.IdNhomKhach= nhom.Id	
	where kh.TrangThai in (0,1)
	and kh.TenantId= @TenantId
	and IdLoaiKhach in ( @LoaiDoiTuong,0)
	and (@IsUserZalo = 0 or (@IsUserZalo = 1 and kh.IdKhachHangZOA is not null))
	and (SoDienThoai like @TextSearch or TenKhachHang like @TextSearch COLLATE Vietnamese_CI_AI 
		or TenKhachHang_KhongDau like @TextSearch
		or MaKhachHang like @TextSearch
		or TenKhachHang like @TextSearch
		or TenKhachHang_KhongDau like @TextSearch COLLATE Vietnamese_CI_AI 	
		or MaKhachHang like @TextSearch COLLATE Vietnamese_CI_AI)
	)
	select kh.*,
		customerZalo.ZOAUserId
	from data_cte kh
	left join Zalo_KhachHangThanhVien customerZalo on kh.IdKhachHangZOA = customerZalo.Id
	order by MaKhachHang
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
