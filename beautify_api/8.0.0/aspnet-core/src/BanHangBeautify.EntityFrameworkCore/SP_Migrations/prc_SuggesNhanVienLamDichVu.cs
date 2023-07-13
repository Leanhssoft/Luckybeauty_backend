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
    [Migration("prc_SuggesNhanVienLamDichVu")]
    partial class prc_SuggesNhanVienLamDichVu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_SuggestNhanVienDichVu
	@TenantId INT,
	@IdChiNhanh UNIQUEIDENTIFIER = NULL
AS
BEGIN
	SELECT nv.Id,nv.Avatar,nv.TenNhanVien,nv.SoDienThoai,cv.TenChucVu as ChucVu
	FROM NS_NhanVien nv
	JOIN (
		SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
		WHERE IsDeleted = 0 AND TenantId= @TenantId 
		GROUP BY IdNhanVien,IdChiNhanh
	) AS qtct ON qtct.IdNhanVien = nv.Id
	LEFT JOIN NS_ChucVu cv on cv.Id = nv.IdChucVu
	WHERE nv.TenantId = @TenantId
	AND nv.IsDeleted = 0
	AND (qtct.IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
	;
END;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP PROCEDURE prc_SuggestNhanVienDichVu");
        }
    }

	
}
