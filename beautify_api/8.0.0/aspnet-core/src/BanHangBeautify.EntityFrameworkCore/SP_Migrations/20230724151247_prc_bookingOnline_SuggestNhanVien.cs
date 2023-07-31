using BanHangBeautify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BanHangBeautify.SP_Migrations
{
    [DbContext(typeof(SPADbContext))]
    [Migration("20230724151247_prc_bookingOnline_SuggestNhanVien")]
    public class prc_bookingOnline_SuggestNhanVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_bookingOnline_SuggestNhanVien]
	@TenantId int,
	@IdChiNhanh UNIQUEIDENTIFIER,
	@IdDichVu UNIQUEIDENTIFIER
AS
BEGIN
	SELECT ns.Id,ns.Avatar,ns.TenNhanVien,ns.SoDienThoai FROM NS_NhanVien ns join 
	DichVu_NhanVien dvnv on ns.id = dvnv.IdNhanVien
	JOIN (
			SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
			WHERE IsDeleted = 0 AND TenantId= @TenantId 
			GROUP BY IdNhanVien,IdChiNhanh
		) AS qtct ON qtct.IdNhanVien = ns.Id
	WHERE qtct.IdChiNhanh = @IdChiNhanh 
	AND ns.TenantId = @TenantId
	AND dvnv.IdDonViQuyDoi = @IdDichVu
	GROUP BY ns.Id,ns.Avatar,ns.TenNhanVien,ns.SoDienThoai,qtct.IdChiNhanh
END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_bookingOnline_SuggestNhanVien");
        }
    }
}
