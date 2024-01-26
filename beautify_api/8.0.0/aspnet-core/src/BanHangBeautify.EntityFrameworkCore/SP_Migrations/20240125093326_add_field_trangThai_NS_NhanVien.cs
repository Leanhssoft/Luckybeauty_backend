using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addfieldtrangThaiNSNhanVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "NS_NhanVien",
                type: "int",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[prc_SuggestNhanVienDichVu]");
            migrationBuilder.Sql(@"CREATE PROCEDURE prc_SuggestNhanVienDichVu
	@TenantId INT,
	@IdChiNhanh UNIQUEIDENTIFIER,
	@IdNhanVien UNIQUEIDENTIFIER = NULL
AS
BEGIN
	SELECT nv.Id,nv.Avatar,nv.TenNhanVien,nv.SoDienThoai,nv.TrangThai,cv.TenChucVu as ChucVu
	FROM NS_NhanVien nv
	JOIN (
		SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
		WHERE IsDeleted = 0 AND TenantId= @TenantId 
		GROUP BY IdNhanVien,IdChiNhanh
	) AS qtct ON qtct.IdNhanVien = nv.Id
	LEFT JOIN NS_ChucVu cv on cv.Id = nv.IdChucVu
	WHERE nv.TenantId = @TenantId
	AND nv.IsDeleted = 0
	AND qtct.IdChiNhanh = @IdChiNhanh
	AND (@IdNhanVien IS NULL OR (nv.Id = @IdNhanVien AND @IdNhanVien IS NOT NULL))
	;
END;"); migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[prc_bookingOnline_SuggestNhanVien]");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_bookingOnline_SuggestNhanVien]
	@TenantId int,
	@IdChiNhanh UNIQUEIDENTIFIER,
	@IdDichVu UNIQUEIDENTIFIER,
	@TenNhanVien nvarchar(50)
AS
BEGIN
	SELECT ns.Id,ns.Avatar,ns.TenNhanVien,ns.SoDienThoai,ns.TrangThai FROM NS_NhanVien ns join 
	DichVu_NhanVien dvnv on ns.id = dvnv.IdNhanVien
	JOIN (
			SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
			WHERE IsDeleted = 0 AND TenantId= @TenantId 
			GROUP BY IdNhanVien,IdChiNhanh
		) AS qtct ON qtct.IdNhanVien = ns.Id
	WHERE qtct.IdChiNhanh = @IdChiNhanh 
	AND ns.TenantId = @TenantId
	AND ns.IsDeleted = false
	AND dvnv.IdDonViQuyDoi = @IdDichVu
	AND (
			ISNULL(@TenNhanVien, '') = ''
			OR LOWER(ns.TenNhanVien) LIKE N'%' + LOWER(@TenNhanVien) + '%' COLLATE SQL_Latin1_General_CP1_CI_AI
		)
	GROUP BY ns.Id,ns.Avatar,ns.TenNhanVien,ns.SoDienThoai,qtct.IdChiNhanh,ns.TrangThai
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "NS_NhanVien");

           
        }
    }
}
