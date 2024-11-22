using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20241122 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROC [dbo].[prc_dashboard_thongKeSoLuong]
	@TenantId int = 1,
	@ThoiGianTu DateTime = '2024-01-01',
	@ThoiGianDen DateTime = '2024-12-01',
	@IdChiNhanhs nvarchar(max) = '71313109-267A-4D84-B2A3-6A1AF5451597'
AS
BEGIN
	SET NOCOUNT ON

		declare @tblChiNhanh table (Id uniqueidentifier)
		if isnull(@IdChiNhanhs,'')='' set @IdChiNhanhs =''
		else
			insert into @tblChiNhanh
			select GiaTRi from dbo.fnSplitstring(@IdChiNhanhs)

		declare @dayFromFilter int = datepart(day,@ThoiGianTu)
		declare @monthFromFilter int = datepart(MONTH,@ThoiGianTu)
		declare @dayToFilter int = datepart(day,@ThoiGianDen)
		declare @monthToFilter int = datepart(MONTH,@ThoiGianDen)

	DECLARE @khachHangSinhNhat INT = 0,
			@tongDoanhThu FLOAT = 0,
			@tongThucThu FLOAT = 0,
			@tongLichHen INT = 0;



		SELECT @khachHangSinhNhat = Count(*) 
		from
		(
			select kh.Id, DATEPART(day,NgaySinh) as Ngay, DATEPART(month, NgaySinh) as Thang
			FROM DM_KhachHang kh	
			WHERE kh.IsDeleted = 0 
			AND kh.TenantId = @TenantId 
			and kh.NgaySinh is not null
			--AND datepart(DAY,kh.NgaySinh) between datepart(DAY,@ThoiGianTu) AND datepart(DAY,@ThoiGianDen)  
			--AND datepart(MONTH,kh.NgaySinh) between datepart(MONTH,@ThoiGianTu) AND datepart(MONTH,@ThoiGianDen)
		)kh
		where  (@monthFromFilter = @monthToFilter and kh.Thang = @monthToFilter 
				and kh.Ngay between @dayFromFilter and @dayToFilter)
			or (@monthFromFilter != @monthToFilter 
				and 
				((kh.Thang = @monthFromFilter and kh.Ngay between @dayFromFilter and 31)
				or (kh.Thang = @monthToFilter and kh.Ngay between 1 and @dayToFilter)
				or (kh.Thang > @monthFromFilter and kh.Thang < @monthToFilter)
				))

	SELECT @tongLichHen = COUNT(*) 
	FROM Booking bk
	where TenantId = @TenantId and IsDeleted = 0
	AND BookingDate BETWEEN @ThoiGianTu AND @ThoiGianDen
	AND (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where cn.Id = bk.IdChiNhanh))

	SELECT @tongDoanhThu = SUM(TongThanhToan) 
	FROM BH_HoaDon hd
	where TenantId = @TenantId and IsDeleted = 0 
	and hd.IdLoaiChungTu in (1,2) --- hd.gdv
	AND (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where cn.Id = hd.IdChiNhanh))
	AND hd.NgayLapHoaDon between @ThoiGianTu and @ThoiGianDen

	select @tongThucThu = sum(qct.TienThu)
	from QuyHoaDon qhd
	join QuyHoaDon_ChiTiet qct on qhd.Id = qct.IdQuyHoaDon
	where qhd.TenantId = @TenantId and qhd.IsDeleted = 0 
	and qhd.NgayLapHoaDon between @ThoiGianTu and @ThoiGianDen
	AND (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where cn.Id = qhd.IdChiNhanh))
	and qct.HinhThucThanhToan not in (5) --- 5.doidiem

	SELECT 
		ISNULL(@khachHangSinhNhat,0) as TongKhachHangSinhNhat,
		ISNULL(@tongLichHen,0) as TongLichHen,
		ISNULL(@tongThucThu,0) as TongThucThu,
		ISNULL(@tongDoanhThu,0) as TongDoanhThu
		
END;");
            migrationBuilder.Sql(@"ALTER PROC [dbo].[prc_dashboard_danhSachLichHen]
	@TenantId int,
	@UserId int,
	@ThoiGianTu DateTime = null,
	@ThoiGianDen DateTime = null,
	@IdChiNhanhs nvarchar(max) = null,
	@CurrentPage int = 1,
	@PageSize int = 3
AS
BEGIN
	SET NOCOUNT ON;

	if @CurrentPage is null set @CurrentPage = 1
	else set @CurrentPage = @CurrentPage -1;

	if @PageSize is null set @PageSize = 3


	declare @tblChiNhanh table (Id uniqueidentifier)
		if isnull(@IdChiNhanhs,'')='' set @IdChiNhanhs =''
		else
			insert into @tblChiNhanh
			select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)
	
	;with data_cte
	as (
	SELECT 
		b.Id, 
		b.BookingDate,
		b.StartTime,
		b.EndTime,
		kh.Avatar,
		b.TenKhachHang, 
		kh.SoDienThoai,
		hh.TenHangHoa,
		dvqd.GiaBan,
		b.TrangThai,
	CASE 
		WHEN b.TrangThai = 1 THEN N'Chưa xác nhận' 
		WHEN b.TrangThai = 2 THEN N'Đã xác nhận' 
		WHEN b.TrangThai = 3 THEN N'Checkin' 
		WHEN b.TrangThai = 4 THEN N'Hoàn thành' 
		ELSE 'Xóa'
	END as TxtTrangThai 
	FROM Booking b 
	JOIN BookingService bs on bs.IdBooking = b.Id 
	JOIN DM_DonViQuiDoi dvqd on dvqd.Id = bs.IdDonViQuiDoi
	JOIN DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	JOIN DM_KhachHang kh on kh.Id = b.IdKhachHang
	WHERE b.IsDeleted = 0 AND b.TenantId = @TenantId
	and b.TrangThai != 0
	AND b.BookingDate between @ThoiGianTu AND @ThoiGianDen
	AND (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where cn.Id = b.IdChiNhanh))
	),
	count_cte
	as 
	(
		select COUNT(Id) as TotalRow
		from data_cte
	)
	select *
	from data_cte
	cross join count_cte
	order by BookingDate desc
	offset @CurrentPage * @PageSize rows
	fetch next @PageSize rows only
END;");
            migrationBuilder.Sql(@"ALTER PROC [dbo].[prc_dashboard_hotService]
	@TenantId int = 1,
	@LoaiBaoCao int = 0,-- 0.doanhthu, 1.soluong
	@ThoiGianTu DateTime = '2024-11-01',
	@ThoiGianDen DateTime = '2024-11-30',
	@IdChiNhanhs nvarchar(max) = '71313109-267a-4d84-b2a3-6a1af5451597'
AS
BEGIN
	SET NOCOUNT ON;

	declare @tblChiNhanh table (Id uniqueidentifier)
	if isnull(@IdChiNhanhs,'') ='' set @IdChiNhanhs =''
	else
		insert into @tblChiNhanh
		select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)

	declare @tblCTHD table (IdDonViQuyDoi uniqueidentifier, GiaTri float)

	if @LoaiBaoCao = 0 ---- top dichvu theo DoanhThu --
	begin 
		insert into @tblCTHD
		select top 5 *
		from
		(
			select		
				ct.IdDonViQuyDoi,
				sum(ct.ThanhTienSauCK) as TongDoanhThu
			from BH_HoaDon hd
			join BH_HoaDon_ChiTiet ct on hd.Id = ct.IdHoaDon
			where hd.NgayLapHoaDon between @ThoiGianTu and @ThoiGianDen
			and hd.TenantId = @TenantId 
			and hd.IsDeleted ='0'
			and (@IdChiNhanhs ='' or exists (select * from @tblChiNhanh cn where hd.IdChiNhanh = cn.Id))
			and ct.IsDeleted= 0
			and ct.IdChiTietHoaDon is null --- không lấy DV sử dụng từ GDV ---
			and hd.LaHoaDonDauKy = '0'
			group by ct.IdDonViQuyDoi
		)cthd
		order by TongDoanhThu desc
	end
	else
	begin ---- top dichvu theo SoLuong --
		insert into @tblCTHD
		select top 5 *
		from
		(
			select		
				ct.IdDonViQuyDoi,
				sum(ct.SoLuong) as TongSoLuong
			from BH_HoaDon hd
			join BH_HoaDon_ChiTiet ct on hd.Id = ct.IdHoaDon
			where hd.NgayLapHoaDon between @ThoiGianTu and @ThoiGianDen
			and hd.TenantId = @TenantId 
			and hd.IsDeleted ='0'
			and (@IdChiNhanhs ='' or exists (select * from @tblChiNhanh cn where hd.IdChiNhanh = cn.Id))
			and ct.IsDeleted= 0
			and ct.IdChiTietHoaDon is null --- không lấy DV sử dụng từ GDV ---
			and hd.LaHoaDonDauKy = '0'
			group by ct.IdDonViQuyDoi
		)cthd
		order by TongSoLuong desc
	end
	
	select 
		dvqd.MaHangHoa,
		hh.TenHangHoa,
		tbl.GiaTri
	from @tblCTHD tbl
	join DM_DonViQuiDoi dvqd on dvqd.Id = tbl.IdDonViQuyDoi
	JOIN DM_HangHoa hh on hh.id = dvqd.IdHangHoa

END;");
            migrationBuilder.Sql(@"ALTER PROC [dbo].[prc_dashboard_thongKeDoanhThu]
	@TenantId int,
	@IdChiNhanhs nvarchar(max) = null
AS
BEGIN
	SET NOCOUNT ON;

	declare @tblChiNhanh table (Id uniqueidentifier)
		if isnull(@IdChiNhanhs,'')='' set @IdChiNhanhs =''
		else
			insert into @tblChiNhanh
			select GiaTri from dbo.fnSplitstring(@IdChiNhanhs)

    -- Create a temporary table to hold the month data
    CREATE TABLE #MonthlyData (
        MonthName NVARCHAR(20),
		MonthValue INT,
        ThangNay DECIMAL(18, 2),
        ThangTruoc DECIMAL(18, 2)
    );

    DECLARE @MonthCounter INT = 1;

    -- Loop through each month
    WHILE @MonthCounter <= 12
    BEGIN
        DECLARE @Month NVARCHAR(20) = CAST(@MonthCounter AS NVARCHAR(2));

        INSERT INTO #MonthlyData (MonthName,MonthValue,ThangNay, ThangTruoc)
        SELECT
            @Month,
			@MonthCounter,
            SUM(CASE WHEN YEAR(hd.NgayLapHoaDon) = YEAR(GETDATE()) AND MONTH(hd.NgayLapHoaDon) = @MonthCounter THEN hdct.ThanhTienSauVAT ELSE 0 END),
            SUM(CASE WHEN YEAR(hd.NgayLapHoaDon) = YEAR(DATEADD(YEAR, -1, GETDATE())) AND MONTH(hd.NgayLapHoaDon) = @MonthCounter THEN hdct.ThanhTienSauVAT ELSE 0 END)
        FROM BH_HoaDon hd
        JOIN BH_HoaDon_ChiTiet hdct ON hdct.IdHoaDon = hd.Id AND hdct.IsDeleted = 0
        WHERE hd.IsDeleted = 0 AND hd.TenantId = @TenantId 
		AND (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where cn.Id = hd.IdChiNhanh))
        GROUP BY DATEPART(MONTH, hd.NgayLapHoaDon);

        SET @MonthCounter = @MonthCounter + 1;
    END;

    -- Fetch data from the temporary table
    SELECT MonthName as Month ,SUM(ThangNay) AS ThangNay ,SUM(ThangTruoc) AS ThangTruoc 
	FROM #MonthlyData 
	GROUP BY MonthName,MonthValue ORDER BY MonthValue asc;

    -- Drop the temporary table
    DROP TABLE #MonthlyData;
    
END;");
            migrationBuilder.Sql(@"ALTER PROC [dbo].[prc_dashboard_thongKeLichHen]
	@TenantId int,
	@IdChiNhanhs nvarchar(max) = null
AS
BEGIN
	SET NOCOUNT ON;

	declare @tblChiNhanh table (Id uniqueidentifier)
		if isnull(@IdChiNhanhs,'')='' set @IdChiNhanhs =''
		else
			insert into @tblChiNhanh
			select GiaTRi from dbo.fnSplitstring(@IdChiNhanhs)

    -- Create a temporary table to hold the month data
    CREATE TABLE #WeeklyData (
        WeekName NVARCHAR(20),
		WeekValue INT,
        TuanNay INT,
        TuanTruoc INT
    );

    DECLARE @WeekCounter INT = 1;

    -- Loop through each month
    WHILE @WeekCounter <= 7
    BEGIN
        DECLARE @DayOfWeek NVARCHAR(20) = CASE WHEN @WeekCounter = 1 Then N'0'
		ELSE CAST(@WeekCounter AS NVARCHAR(10)) end;

        INSERT INTO #WeeklyData (WeekName,WeekValue,TuanNay, TuanTruoc)
        SELECT
            @DayOfWeek,
			@WeekCounter,
            SUM(CASE WHEN DATEPART(WEEK,BookingDate) =  DATEPART(WEEK, GETDATE()) AND DATEPART(WEEKDAY,BookingDate) = @WeekCounter THEN 1 ELSE 0 END),
            SUM(CASE WHEN DATEPART(WEEK,BookingDate) =  DATEPART(WEEK,DATEADD(WEEK, -1, GETDATE())) AND DATEPART(WEEKDAY,BookingDate)= @WeekCounter THEN 1 ELSE 0 END)
        FROM Booking b
        WHERE IsDeleted = 0 AND TenantId = @TenantId
		AND (@IdChiNhanhs ='' or exists (select id from @tblChiNhanh cn where cn.Id = b.IdChiNhanh))
        GROUP BY DATEPART(WEEK, BookingDate);

        SET @WeekCounter = @WeekCounter + 1;
    END;

    -- Fetch data from the temporary table
    SELECT WeekName as Tuan ,SUM(TuanNay) AS TuanNay ,SUM(TuanTruoc) AS TuanTruoc 
	FROM #WeeklyData 
	GROUP BY WeekName,WeekValue ORDER BY WeekName asc;

    -- Drop the temporary table
    DROP TABLE #WeeklyData;
    
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
