using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("20230803022001_add_prc_getInfoBooking")]
    public class add_prc_getInfoBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_getBookingInfo
	@IdBooking UNIQUEIDENTIFIER,
	@TenantId INT
AS 
SELECT
		b.Id,
		b.BookingDate,
        CONVERT(NVARCHAR(5), b.StartTime, 108) AS StartTime,
        CONVERT(NVARCHAR(5), b.EndTime, 108) AS EndTime,
        kh.Avatar as AvatarKhachHang,
        b.TenKhachHang,
		b.SoDienThoai,
        b.GhiChu,
		nv.TenNhanVien AS NhanVienThucHien,
        hh.TenHangHoa AS TenDichVu,
		ISNULL(dv.GiaBan,0) AS DonGia,
        CASE
            WHEN b.TrangThai = 0 THEN '#F1416C'
			WHEN B.TrangThai = 1 THEN '#FF9900'
			wHEN b.TrangThai = 2 THEN '#7DC1FF'
			WHEN b.TrangThai = 3 THEN '#009EF7'
			WHEN b.TrangThai = 4 THEN '#50CD89'
            ELSE '#009EF7'
        END AS Color,
        b.TrangThai
    FROM
        Booking b
        LEFT JOIN DM_KhachHang kh on kh.Id= b.IdKhachHang
        INNER JOIN BookingNhanVien bnv ON bnv.IdBooking = b.Id and bnv.IsDeleted = 0
		LEFT JOIN NS_NhanVien nv on nv.id = bnv.IdNhanVien
        LEFT JOIN BookingService bs ON bs.IdBooking = b.Id and bs.IsDeleted = 0
        LEFT JOIN DM_DonViQuiDoi dv ON dv.Id = bs.IdDonViQuiDoi
        LEFT JOIN DM_HangHoa hh ON hh.id = dv.IdHangHoa
    WHERE
        b.TenantId = ISNULL(@TenantId, 1)
		AND b.Id = @IdBooking
        AND b.IsDeleted = 0;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_getBookingInfo");
        }
    }
}
