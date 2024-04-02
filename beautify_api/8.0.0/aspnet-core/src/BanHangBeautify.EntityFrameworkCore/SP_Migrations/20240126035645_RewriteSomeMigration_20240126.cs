using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class RewriteSomeMigration20240126 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_caLamViec_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_chietKhauDichVu_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_chietKhauHoaDon_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_chiNhanh_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_lichLamViec_getAll_Week]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_nghiLe_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_NhanVien_GetAll]");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_caLamViec_getAll]
	@TenantId INT,
	@IdChiNhanh UNIQUEIDENTIFIER = NULL,
	@Filter NVARCHAR(200),
	@SortBy NVARCHAR(20),
	@SortType VARCHAR(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS
BEGIN
	SELECT 
		Id,
		MaCa,
		TenCa,
		GioVao,
		GioRa,
		TongGioCong
	FROM NS_CaLamViec clv
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(MaCa) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(TenCa) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(10), TongGioCong)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), GioVao)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), GioRa)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
	ORDER BY 
		CASE WHEN @SortBy = 'maCa' AND @SortType = 'desc' THEN MaCa END DESC,
		CASE WHEN @SortBy = 'tenCa' AND @SortType = 'desc' THEN TenCa END DESC,
		CASE WHEN @SortBy = 'gioVao' AND @SortType = 'desc' THEN GioVao END DESC,
		CASE WHEN @SortBy = 'gioRa' AND @SortType = 'desc' THEN GioRa END DESC,
		CASE WHEN @SortBy = 'tongGioCong' AND @SortType = 'desc' THEN TongGioCong END DESC,
		CASE WHEN @SortBy = 'maCa' AND @SortType = 'asc' THEN MaCa END ASC,
		CASE WHEN @SortBy = 'tenCa' AND @SortType = 'asc' THEN TenCa END ASC,
		CASE WHEN @SortBy = 'gioVao' AND @SortType = 'asc' THEN GioVao END ASC,
		CASE WHEN @SortBy = 'gioRa' AND @SortType = 'asc' THEN GioRa END ASC,
		CASE WHEN @SortBy = 'tongGioCong' AND @SortType = 'asc' THEN TongGioCong END ASC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
	SELECT COUNT(*) as TotalCount FROM NS_CaLamViec WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(MaCa) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(TenCa) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(10), TongGioCong)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), GioVao)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), GioRa)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
END;");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_chietKhauDichVu_getAll]
					@TenantId INT,
					@IdNhanVien UNIQUEIDENTIFIER,
					@IdChiNhanh UNIQUEIDENTIFIER, 
					@Filter NVARCHAR(200),
					@SkipCount INT,
					@MaxResultCount INT,
					@SortBy NVARCHAR(30),
					@SortType VARCHAR(4) = 'desc'
				AS
				BEGIN
					CREATE TABLE #DataTable(
						Id uniqueidentifier,
						TenDichVu nvarchar(80),
						TenNhomDichVu nvarchar(30),
						GiaTri float,
						LaPhanTram bit,
						GiaDichVu float,
						HoaHongThucHien float,
						HoaHongYeuCauThucHien float,
						HoaHongTuVan float,
						CreationTime datetime2
					)
					INSERT INTO #DataTable
						SELECT 
							ckdv.Id,
							hh.TenHangHoa AS TenDichVu,
							nhh.TenNhomHang AS TenNhomDichVu,
							ckdv.GiaTri,
							ckdv.LaPhanTram,
							dvqd.GiaBan AS GiaDichVu,
							CASE WHEN ckdv.LoaiChietKhau = 1 THEN ckdv.GiaTri ELSE 0 END AS HoaHongThucHien,
							CASE WHEN ckdv.LoaiChietKhau = 2 THEN ckdv.GiaTri ELSE 0 END AS HoaHongYeuCauThucHien,
							CASE WHEN ckdv.LoaiChietKhau = 3 THEN ckdv.GiaTri ELSE 0 END AS HoaHongTuVan,
							ckdv.CreationTime
						FROM NS_ChietKhauDichVu ckdv
						JOIN DM_DonViQuiDoi dvqd ON dvqd.id = ckdv.IdDonViQuiDoi
						JOIN DM_HangHoa hh ON hh.Id = dvqd.IdHangHoa
						LEFT JOIN DM_NhomHangHoa nhh ON nhh.Id = hh.IdNhomHangHoa
						WHERE ckdv.TenantId = @TenantId
							AND ckdv.IsDeleted = 0
							AND ckdv.IdChiNhanh = @IdChiNhanh
							AND ckdv.IdNhanVien = @IdNhanVien
							AND (
								ISNULL(@Filter,'') = ''
								OR LOWER(hh.TenHangHoa) LIKE N'%' + LOWER(@Filter) + N'%'
								OR LOWER(nhh.TenNhomHang) LIKE N'%' + LOWER(@Filter) + N'%'
								OR LOWER(CONVERT(NVARCHAR(MAX), ckdv.GiaTri)) LIKE N'%' + LOWER(@Filter) + N'%'
								OR LOWER(CONVERT(NVARCHAR(MAX), dvqd.GiaBan)) LIKE N'%' + LOWER(@Filter) + N'%'
							)
						;
					-- Danh sách dịch vụ chiết khấu theo nhân viên
					SELECT * FROM #DataTable
					ORDER BY
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenDichVu' THEN TenDichVu END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'tenNhomDichVu' THEN TenNhomDichVu END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'giaDichVu' THEN GiaDichVu END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'hoaHongThucHien' THEN HoaHongThucHien END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'hoaHongYeuCauThucHien' THEN HoaHongYeuCauThucHien END ASC,
						CASE WHEN @SortType = 'asc' AND @SortBy = 'hoaHongTuVan' THEN HoaHongTuVan END ASC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenDichVu' THEN TenDichVu END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'tenNhomDichVu' THEN TenNhomDichVu END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'giaDichVu' THEN GiaDichVu END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'hoaHongThucHien' THEN HoaHongThucHien END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'hoaHongYeuCauThucHien' THEN HoaHongYeuCauThucHien END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = 'hoaHongTuVan' THEN HoaHongTuVan END DESC,
						CASE WHEN @SortType = 'desc' AND @SortBy = '' THEN CreationTime END DESC
					OFFSET  @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
					
					-- Tổng số bản ghi 
					SELECT COUNT(*) AS TotalCount FROM #DataTable;
					
				END;");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_chietKhauHoaDon_getAll] 
	@TenantId INT,
	@IdChiNhanh UNIQUEIDENTIFIER,
	@Filter NVARCHAR(200),
	@SortBy NVARCHAR(20),
	@SortType VARCHAR(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS
BEGIN
	SELECT 
		Id,
		ChungTuApDung,
		CASE WHEN LoaiChietKhau = 1 THEN CONVERT(NVARCHAR(MAX),GiaTriChietKhau) + N' % thực thu'
			 WHEN LoaiChietKhau = 2 THEN CONVERT(NVARCHAR(MAX),GiaTriChietKhau) + N' % doanh thu'
			 WHEN LoaiChietKhau = 3 THEN CONVERT(NVARCHAR(MAX),REPLACE(REPlace(REPLACE(FORMAT(GiaTriChietKhau, '###,###.##'),'.','@'),',','.'),'@',',')) + N' VNĐ'
		ELSE '0' END AS GiaTriChietKhau,
		GhiChu
	FROM NS_ChietKhauHoaDon 
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(ChungTuApDung) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(MAX), GiaTriChietKhau)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
	ORDER BY 
		CASE WHEN @SortBy = 'chungTuApDung' AND @SortType = 'desc' THEN ChungTuApDung END DESC,
		CASE WHEN @SortBy = 'giaTriChietKhau' AND @SortType = 'desc' THEN GiaTriChietKhau END DESC,
		CASE WHEN @SortBy = 'chungTuApDung' AND @SortType = 'asc' THEN ChungTuApDung END ASC,
		CASE WHEN @SortBy = 'giaTriChietKhau' AND @SortType = 'asc' THEN GiaTriChietKhau END ASC,
		CASE WHEN ISNULL(@SortBy,'') = '' THEN CreationTime END DESC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
	SELECT COUNT(*) AS TotalCount FROM NS_ChietKhauHoaDon 
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(ChungTuApDung) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(MAX), GiaTriChietKhau)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
END;");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_chiNhanh_getAll]
	@TenantId INT,
	@Filter NVARCHAR(200),
	@SortBy NVARCHAR(20),
	@SortType VARCHAR(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS
BEGIN
	SELECT 
		Id,
		IdCongTy,
		MaChiNhanh,
		TenChiNhanh,
		SoDienThoai,
		DiaChi,
		MaSoThue,
		Logo,
		GhiChu,
		TrangThai,
		NgayApDung,
		NgayHetHan
	FROM DM_ChiNhanh 
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(MaChiNhanh) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(TenChiNhanh) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(DiaChi) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(MaSoThue) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(GhiChu) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(TrangThai) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), NgayApDung)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), NgayHetHan)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
	ORDER BY 
		CASE WHEN @SortBy = 'maChiNhanh' AND @SortType = 'desc' THEN MaChiNhanh END DESC,
		CASE WHEN @SortBy = 'tenChiNhanh' AND @SortType = 'desc' THEN TenChiNhanh END DESC,
		CASE WHEN @SortBy = 'soDienThoai' AND @SortType = 'desc' THEN SoDienThoai END DESC,
		CASE WHEN @SortBy = 'diaChi' AND @SortType = 'desc' THEN DiaChi END DESC,
		CASE WHEN @SortBy = 'maSoThue' AND @SortType = 'desc' THEN MaSoThue END DESC,
		CASE WHEN @SortBy = 'ghiChu' AND @SortType = 'desc' THEN GhiChu END DESC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'desc' THEN TrangThai END DESC,
		CASE WHEN @SortBy = 'ngayApDung' AND @SortType = 'desc' THEN NgayApDung END DESC,
		CASE WHEN @SortBy = 'ngayHetHan' AND @SortType = 'desc' THEN NgayHetHan END DESC,
		CASE WHEN @SortBy = 'maChiNhanh' AND @SortType = 'asc' THEN MaChiNhanh END ASC,
		CASE WHEN @SortBy = 'tenChiNhanh' AND @SortType = 'asc' THEN TenChiNhanh END ASC,
		CASE WHEN @SortBy = 'soDienThoai' AND @SortType = 'asc' THEN SoDienThoai END ASC,
		CASE WHEN @SortBy = 'diaChi' AND @SortType = 'asc' THEN DiaChi END ASC,
		CASE WHEN @SortBy = 'maSoThue' AND @SortType = 'asc' THEN MaSoThue END ASC,
		CASE WHEN @SortBy = 'ghiChu' AND @SortType = 'asc' THEN GhiChu END ASC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'asc' THEN TrangThai END ASC,
		CASE WHEN @SortBy = 'ngayApDung' AND @SortType = 'asc' THEN NgayApDung END ASC,
		CASE WHEN @SortBy = 'ngayHetHan' AND @SortType = 'asc' THEN NgayHetHan END ASC,
		CASE WHEN ISNULL(@SortBy,'') = '' THEN CreationTime END DESC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
	SELECT COUNT(*) AS TotalCOunt FROM DM_ChiNhanh 
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(MaChiNhanh) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(TenChiNhanh) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(SoDienThoai) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(DiaChi) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(MaSoThue) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(GhiChu) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(TrangThai) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), NgayApDung)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), NgayHetHan)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
END;");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_lichLamViec_getAll_Week]
	@TenantId INT,
	@IdChiNhanh uniqueidentifier,
    @IdNhanVien uniqueidentifier = null,
	@SkipCount INT = 0,
	@MaxResultCount INT = 10,
	@DateFrom Date,
	@DateTo Date
AS
BEGIN
    -- Declare necessary variables
    DECLARE @ListItems TABLE (
		Id uniqueidentifier,
        Avatar VARCHAR(MAX),
        IdNhanVien uniqueidentifier,
		IdCaLamViec uniqueidentifier,
        TenNhanVien NVARCHAR(MAX),
        TongThoiGian Float,
        Monday VARCHAR(MAX),
        Tuesday VARCHAR(MAX),
        Wednesday VARCHAR(MAX),
        Thursday VARCHAR(MAX),
        Friday VARCHAR(MAX),
        Saturday VARCHAR(MAX),
        Sunday VARCHAR(MAX)
    )
    -- Retrieve data from the database
    DECLARE @NhanVien TABLE (
        -- Define necessary columns from the NhanVien table
        -- Adjust the column data types as per your actual table structure
        Id uniqueidentifier,
        Avatar NVARCHAR(MAX),
        TenNhanVien NVARCHAR(MAX)
    )

    -- Retrieve the list of NhanVien
    INSERT INTO @NhanVien (Id, Avatar, TenNhanVien)
    SELECT nv.Id, nv.Avatar, nv.TenNhanVien
    FROM NS_NhanVien nv
	LEFT JOIN NS_ChucVu cv ON cv.Id = nv.IdChucVu
	JOIN (
		SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
		WHERE IsDeleted = 0 AND TenantId= @TenantId 
		GROUP BY IdNhanVien,IdChiNhanh
	) AS qtct ON qtct.IdNhanVien = nv.Id
    WHERE nv.IsDeleted = 0 -- Assuming IsDeleted is a column in the NhanVien table
        AND nv.TenantId = ISNULL(@TenantId, 1) -- Assuming TenantId is a column in the NhanVien table
		AND qtct.IdChiNhanh = @IdChiNhanh
        AND (@IdNhanVien IS NULL OR (nv.Id = @IdNhanVien AND @IdNhanVien IS NOT NULL))
	ORDER BY nv.TenNhanVien DESC

    -- Retrieve the list of LichLamViecNhanVien
    INSERT INTO @ListItems (Id,Avatar, IdNhanVien,IdCaLamViec, TenNhanVien, TongThoiGian, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday)
    
	SELECT
		L.Id,
        N.Avatar,
        N.Id,
		C.Id,
        N.TenNhanVien,
        CASE
            WHEN C.Id IS NOT NULL THEN  C.TongGioCong
            ELSE 0
        END AS TongThoiGian,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 2 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Monday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 3 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Tuesday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 4 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Wednesday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY,LC.NgayLamViec) = 5 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Thursday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 6 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Friday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 7 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Saturday,
        CASE
            WHEN LC.NgayLamViec IS NOT NULL AND DATEPART(WEEKDAY, LC.NgayLamViec) = 1 THEN CONVERT(VARCHAR(5), C.GioVao, 108) + ' - ' + CONVERT(VARCHAR(5), C.GioRa, 108)
            ELSE ''
        END AS Sunday
    FROM @NhanVien N
    LEFT JOIN NS_LichLamViec L ON L.IdNhanVien = N.Id AND L.IsDeleted = 0
	LEFT JOIN NS_LichLamViec_Ca LC ON LC.IdLichLamViec = l.Id AND LC.IsDeleted = 0 AND LC.NgayLamViec BETWEEN @DateFrom AND @DateTo AND L.IsDeleted = 0
    LEFT JOIN NS_CaLamViec C ON C.Id = LC.IdCaLamViec AND C.IsDeleted = 0
	--WHERE L.TuNgay BETWEEN @DateFrom AND @DateTo
    GROUP BY L.Id,N.Id, N.Avatar, N.TenNhanVien,C.Id,C.TongGioCong, LC.NgayLamViec, C.GioVao, C.GioRa
	ORDER BY N.TenNhanVien;

    -- Return the result
    SELECT
	Id,
	Avatar,
	IdNhanVien,
	TenNhanVien,
	CONVERT(NVARCHAR(Max), SUM(TongThoiGian)) as TongThoiGian,
	MAX(Monday) as Monday,
	MAX(Tuesday) as Tuesday,
	MAX(Wednesday) as Wednesday,
	MAX(Thursday) as Thursday,
	MAX(Friday) as Friday,
	MAX(Saturday) as Saturday,
	MAX(Sunday) as Sunday
    FROM @ListItems
	Group by Id,Avatar, IdNhanVien, TenNhanVien,IdCaLamViec
	ORDER BY TenNhanVien DESC OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;


	SELECT Count(*) as TotalCount FROM (SELECT
	Id,
	Avatar,
	IdNhanVien,
	TenNhanVien,
	CONVERT(NVARCHAR(Max), SUM(TongThoiGian)) as TongThoiGian,
	MAX(Monday) as Monday,
	MAX(Tuesday) as Tuesday,
	MAX(Wednesday) as Wednesday,
	MAX(Thursday) as Thursday,
	MAX(Friday) as Friday,
	MAX(Saturday) as Saturday,
	MAX(Sunday) as Sunday
    FROM @ListItems
	Group by Id,Avatar, IdNhanVien, TenNhanVien,IdCaLamViec) AS DataTable;
END;");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_nghiLe_getAll] 
	@TenantId INT,
	@Filter NVARCHAR(200),
	@SortBy NVARCHAR(20),
	@SortType VARCHAR(4),
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS
BEGIN
	SELECT 
		Id,
		TenNgayLe,
		TuNgay,
		DenNgay,
		TongSoNgay AS TongSoNgay 
	FROM DM_NgayNghiLe  
	WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(TenNgayLe) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(10), TongSoNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), TuNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), DenNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
	ORDER BY 
		CASE WHEN @SortBy = 'tenNgayLe' AND @SortType = 'desc' THEN TenNgayLe END DESC,
		CASE WHEN @SortBy = 'tuNgay' AND @SortType = 'desc' THEN TuNgay END DESC,
		CASE WHEN @SortBy = 'denNgay' AND @SortType = 'desc' THEN DenNgay END DESC,
		CASE WHEN @SortBy = 'tongSoNgay' AND @SortType = 'desc' THEN TongSoNgay END DESC,
		CASE WHEN @SortBy = 'tenNgayLe' AND @SortType = 'asc' THEN TenNgayLe END ASC,
		CASE WHEN @SortBy = 'tuNgay' AND @SortType = 'asc' THEN TuNgay END ASC,
		CASE WHEN @SortBy = 'denNgay' AND @SortType = 'asc' THEN DenNgay END ASC,
		CASE WHEN @SortBy = 'tongSoNgay' AND @SortType = 'asc' THEN TongSoNgay END ASC
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
	SELECT COUNT(*) as TotalCount FROM DM_NgayNghiLe WHERE TenantId = @TenantId
		AND IsDeleted = 0
		AND (
			ISNull(@Filter,'') = ''
			OR LOWER(TenNgayLe) LIKE N'%' + LOWER(@Filter) + N'%' 
			OR LOWER(CONVERT(NVARCHAR(10), TongSoNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), TuNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
			OR LOWER(CONVERT(NVARCHAR(16), DenNgay)) LIKE N'%' + LOWER(@Filter) + N'%'
		)
END;
");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[prc_nhanVien_getAll]
				@TenantId INT,
				@IdChiNhanh UNIQUEIDENTIFIER = NULL,
				@IdChucVu UNIQUEIDENTIFIER = NULL,
				@Filter NVARCHAR(MAX),
				@SortBy NVARCHAR(50),
				@SortType NVARCHAR(4),
				@SkipCount INT = 0,
				@MaxResultCount INT = 10
			AS
			BEGIN
			set nocount on;
			--if isnull(@IdChucVu,'')='' set @IdChucVu =''

				SELECT 
				nv.Id,
				nv.Avatar,
				nv.Ho,
				nv.TenLot,
				nv.TenNhanVien,
				nv.SoDienThoai,
				nv.CCCD,
				nv.NgaySinh,
				nv.KieuNgaySinh,
				nv.GioiTinh,
				nv.NgayCap,
				nv.NoiCap,
				nv.DiaChi,
				cv.TenChucVu,
				nv.CreationTime AS NgayVaoLam,
				CASE WHEN nv.IsDeleted = 0 THEN N'Hoạt động' ELSE N'Ngừng hoạt động' END AS TrangThai
				into #temp
				FROM NS_NhanVien nv 
				LEFT JOIN NS_ChucVu cv ON cv.Id = nv.IdChucVu
				JOIN (
					SELECT IdNhanVien,IdChiNhanh FROM NS_QuaTrinh_CongTac
					WHERE IsDeleted = 0 AND TenantId= @TenantId 
					GROUP BY IdNhanVien,IdChiNhanh
				) AS qtct ON qtct.IdNhanVien = nv.Id
				WHERE nv.TenantId = @TenantId
				AND (qtct.IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
				and (@IdChucVu is null or cv.Id = @IdChucVu)
				AND (
					ISNULL(@Filter,'') = ''
					OR LOWER(nv.TenNhanVien) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.SoDienThoai) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.CCCD) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.DiaChi) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(cv.TenChucVu) LIKE N'%'+LOWER(@Filter)+N'%'
					OR LOWER(nv.DiaChi) LIKE N'%'+LOWER(@Filter)+N'%'
				)
				ORDER BY
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'tenNhanVien' THEN nv.TenNhanVien END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'soDienThoai' THEN nv.SoDienThoai END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'diaChi' THEN nv.DiaChi END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngaySinh' THEN nv.NgaySinh END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'gioiTinh' THEN nv.GioiTinh END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'chucVu' THEN cv.TenChucVu END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'ngayThamGia' THEN nv.CreationTime END,
					CASE WHEN LOWER(@SortType) = 'asc' AND @SortBy = 'trangThai' THEN DiaChi END,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'tenNhanVien' THEN nv.TenNhanVien END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'soDienThoai' THEN nv.SoDienThoai END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'diaChi' THEN nv.DiaChi END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngaySinh' THEN nv.NgaySinh END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'gioiTinh' THEN nv.GioiTinh END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'chucVu' THEN cv.TenChucVu END DESC,
					CASE WHEN LOWER(@SortType) = 'desc' AND @SortBy = 'ngayThamGia' THEN nv.CreationTime END DESC,
					CASE WHEN ISNULL(@SortType,'') = '' AND ISNULL(@SortBy,'') = '' THEN nv.LastModificationTime END DESC
				OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

				select * from #temp

				SELECT COUNT(Id) AS TotalCount
				FROM #temp
				
			END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_caLamViec_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_chietKhauDichVu_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_chietKhauHoaDon_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_chiNhanh_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_lichLamViec_getAll_Week]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_nghiLe_getAll]");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[prc_NhanVien_GetAll]");
        }
    }
}
