using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addprcsuggestNhanVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_SuggestNhanVien
	@TenantId INT,
	@IdChiNhanh UNIQUEIDENTIFIER
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
	AND qtct.IdChiNhanh = @IdChiNhanh;
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
