using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20240413 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_getKhachHang_noBooking]
		@TenantId INT = 1,
		@Filter NVARCHAR(max) =N'',
		@SkipCount INT = 0,
		@MaxResultCount INT = 10
	AS
	BEGIN	

	set nocount on;
	if @SkipCount > 0 set @SkipCount = @SkipCount - 1;
	;with data_cte
	as
	(
	
		SELECT 
			kh.Id,
			kh.MaKhachHang,
			kh.TenKhachHang,
			kh.Avatar,
			kh.SoDienThoai,
			kh.DiaChi,
			kh.NgaySinh,
			kh.IdNguonKhach,
			kh.GioiTinhNam,
			kh.TongTichDiem,		
			ISNULL(tblCheckIn.CuocHenGanNhat,kh.CreationTime) AS CuocHenGanNhat,
			isnull(tblCheckIn.SoLanCheckIn,0) as SoLanCheckIn,
			isnull(tblChecking.TrangThaiCheckIn,0) as TrangThaiCheckIn			
		FROM DM_KhachHang kh			
		LEFT JOIN (
			SELECT IdKhachHang, max(DateTimeCheckIn) as CuocHenGanNhat, count(id) as SoLanCheckIn
			FROM KH_CheckIn
			WHERE IsDeleted = 0
			GROUP BY IdKhachHang
		) tblCheckIn ON kh.Id = tblCheckIn.IdKhachHang
		left join (
			SELECT IdKhachHang, 1 as TrangThaiCheckIn
			FROM KH_CheckIn
			WHERE IsDeleted = 0 and TrangThai= 1
			GROUP BY IdKhachHang
		) tblChecking ON kh.Id = tblChecking.IdKhachHang --- khach dang check in
		WHERE kh.TenantId = @TenantId 
		AND kh.IsDeleted = 0
		and not exists
			(
			---- khong lay khachhang da booking, nhung chua tontai trong bang bkHoadon
			select bk.Id from Booking bk where bk.IdKhachHang = kh.Id 
				and not exists (select id from Booking_CheckIn_HoaDon bkHD where bk.Id = bkHD.IdBooking)
			)
		AND (ISNULL(@Filter,'') = ''
			OR LOWER(kh.TenKhachHang) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(kh.TenKhachHang_KhongDau) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(kh.SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(kh.DiaChi) LIKE N'%' + LOWER(@Filter) + N'%'			
		)
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
	ORDER BY MaKhachHang desc		
	OFFSET (@SkipCount* @MaxResultCount) ROWS
	FETCH NEXT @MaxResultCount ROWS ONLY
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
