using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SP_Migrations
{
    /// <inheritdoc />
    public partial class addprocgetAllSoQuy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE prc_SoQuy_GetAll
                @TenantId INT,
                @IdChiNhanh UNIQUEIDENTIFIER = NULL,
                @Filter NVARCHAR(MAX),
                @SortBy VARCHAR(50), -- Specify the column to sort by
                @SortType VARCHAR(4), -- Specify the sort order type (ASC or DESC)
                @SkipCount INT,
                @MaxResultCount INT
            AS
            BEGIN
                SELECT *
                FROM (
		
                    SELECT qhd.Id,
                        qhd.IdChiNhanh,
                        lct.TenLoaiChungTu as LoaiPhieu,
                        CASE
                            WHEN qhd_ct.IdKhoanThuChi IS NULL THEN qhd.MaHoaDon
                            ELSE ktc.MaKhoanThuChi + ' ' + qhd.MaHoaDon
                        END AS MaPhieu,
                        qhd.CreationTime AS ThoiGianTao,
                        ktc.TenKhoanThuChi AS LoaiThuChi,
                        qhd.TongTienThu,
                        CASE
                            WHEN qhd_ct.HinhThucThanhToan = 1 THEN N'Tiền mặt'
                            WHEN qhd_ct.HinhThucThanhToan = 2 THEN N'Pos'
                            WHEN qhd_ct.HinhThucThanhToan = 3 THEN N'Chuyển khoản'
                            WHEN qhd_ct.HinhThucThanhToan = 4 THEN N'Thẻ giá trị'
                            WHEN qhd_ct.HinhThucThanhToan = 5 THEN N'Sử dụng điểm'
                            ELSE N''
                        END AS HinhThucThanhToan,
                        CASE WHEN qhd.TrangThai = 1 THEN N'Đã thanh toán' ELSE N'Đã hủy' END AS TrangThai
                    FROM QuyHoaDon qhd
		            JOIN QuyHoaDon_ChiTiet qhd_ct ON qhd_ct.IdQuyHoaDon = qhd.id
                    JOIN DM_LoaiChungTu lct ON lct.id = qhd.IdLoaiChungTu
                    LEFT JOIN DM_KhoanThuChi ktc ON ktc.id = qhd_ct.IdKhoanThuChi
                    WHERE qhd.TenantId = @TenantId
                        AND (qhd.IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
			            AND qhd.IsDeleted = 0
                        AND (ISNULL(@Filter, '') = ''
                            OR LOWER(CASE WHEN qhd_ct.IdKhoanThuChi IS NULL THEN qhd.MaHoaDon ELSE ktc.MaKhoanThuChi + qhd.MaHoaDon END) LIKE N'%' + LOWER(@Filter) + '%'
                            OR LOWER(ktc.TenKhoanThuChi) LIKE N'%'+LOWER(@Filter)+'%'
                            OR LOWER(CASE WHEN qhd_ct.HinhThucThanhToan = 1 THEN N'Tiền mặt'
                                            WHEN qhd_ct.HinhThucThanhToan = 2 THEN N'Pos'
                                            WHEN qhd_ct.HinhThucThanhToan = 3 THEN N'Chuyển khoản'
                                            WHEN qhd_ct.HinhThucThanhToan = 4 THEN N'Thẻ giá trị'
                                            WHEN qhd_ct.HinhThucThanhToan = 5 THEN N'Sử dụng điểm'
                                    ELSE N'' END) LIKE N'%'+LOWER(@Filter) +'%'
                            OR LOWER(CASE WHEN qhd.TrangThai = 1 THEN N'Đã thanh toán' ELSE N'Đã hủy' END) LIKE N'%' + LOWER(@Filter) + '%'
                            OR LOWER(ktc.TenKhoanThuChi) LIKE N'%' + LOWER(@Filter) + '%'
                            OR LOWER(lct.TenLoaiChungTu) LIKE N'%' + LOWER(@Filter) + '%'
                        )
                    ) AS Results
                    ORDER BY
                        CASE WHEN @SortBy = 'loaiPhieu' AND LOWER(@SortType) = 'asc' THEN LoaiPhieu END ASC,
                        CASE WHEN @SortBy = 'tongTienThu' AND LOWER(@SortType) = 'asc' THEN TongTienThu END ASC,
                        CASE WHEN @SortBy = 'maPhieu' AND LOWER(@SortType) = 'asc' THEN MaPhieu END ASC,
                        CASE WHEN @SortBy = 'hinhThucThanhToan' AND LOWER(@SortType) = 'asc' THEN HinhThucThanhToan END ASC,
                        CASE WHEN @SortBy = 'trangThai' AND LOWER(@SortType) = 'asc' THEN TrangThai END ASC,
                        CASE WHEN @SortBy = 'thoiGianTao' AND LOWER(@SortType) = 'asc' THEN ThoiGianTao END ASC,
                        CASE WHEN @SortBy = 'loaiPhieu' AND LOWER(@SortType) = 'desc' THEN LoaiPhieu END DESC,
                        CASE WHEN @SortBy = 'tongTienThu' AND LOWER(@SortType) = 'desc' THEN TongTienThu END DESC,
                        CASE WHEN @SortBy = 'maPhieu' AND LOWER(@SortType) = 'desc' THEN MaPhieu END DESC,
                        CASE WHEN @SortBy = 'hinhThucThanhToan' AND LOWER(@SortType) = 'desc' THEN HinhThucThanhToan END DESC,
                        CASE WHEN @SortBy = 'trangThai' AND LOWER(@SortType) = 'desc' THEN TrangThai END DESC,
                        CASE WHEN @SortBy = 'thoiGianTao' AND LOWER(@SortType) = 'desc' THEN ThoiGianTao END DESC
                    OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

                    SELECT COUNT(*) AS TotalCount
                    FROM QuyHoaDon qhd
		            JOIN QuyHoaDon_ChiTiet qhd_ct ON qhd_ct.IdQuyHoaDon = qhd.id and qhd_ct.IsDeleted = 0
                    JOIN DM_LoaiChungTu lct ON lct.id = qhd.IdLoaiChungTu
                    LEFT JOIN DM_KhoanThuChi ktc ON ktc.id = qhd_ct.IdKhoanThuChi
                    WHERE qhd.TenantId = @TenantId
                        AND (qhd.IdChiNhanh = @IdChiNhanh OR @IdChiNhanh IS NULL)
			            AND qhd.IsDeleted = 0
                        AND (ISNULL(@Filter, '') = ''
                            OR LOWER(CASE WHEN qhd_ct.IdKhoanThuChi IS NULL THEN qhd.MaHoaDon ELSE ktc.MaKhoanThuChi + qhd.MaHoaDon END) LIKE N'%' + LOWER(@Filter) + '%'
                            OR LOWER(ktc.TenKhoanThuChi) LIKE N'%'+LOWER(@Filter)+'%'
                            OR LOWER(CASE WHEN qhd_ct.HinhThucThanhToan = 1 THEN N'Tiền mặt'
                                            WHEN qhd_ct.HinhThucThanhToan = 2 THEN N'Pos'
                                            WHEN qhd_ct.HinhThucThanhToan = 3 THEN N'Chuyển khoản'
                                            WHEN qhd_ct.HinhThucThanhToan = 4 THEN N'Thẻ giá trị'
                                            WHEN qhd_ct.HinhThucThanhToan = 5 THEN N'Sử dụng điểm'
                                    ELSE N'' END) LIKE N'%'+LOWER(@Filter) +'%'
                            OR LOWER(CASE WHEN qhd.TrangThai = 1 THEN N'Đã thanh toán' ELSE N'Đã hủy' END) LIKE N'%' + LOWER(@Filter) + '%'
                            OR LOWER(ktc.TenKhoanThuChi) LIKE N'%' + LOWER(@Filter) + '%'
                            OR LOWER(lct.TenLoaiChungTu) LIKE N'%' + LOWER(@Filter) + '%'
                        );
                    END;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE prc_SoQuy_GetAll");
        }
    }
}
