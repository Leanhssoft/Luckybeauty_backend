using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class UpdateStoreSql20240129 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_booking_getAll]
    @TenantId INT,
    @IdChiNhanh UNIQUEIDENTIFIER,
    @IdNhanVien UNIQUEIDENTIFIER = NULL,
	@IdDichVu UNIQUEIDENTIFIER = NULL,
    @TimeFrom DATETIME,
    @TimeTo DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Result TABLE
    (   
        Id UNIQUEIDENTIFIER,
        SourceId UNIQUEIDENTIFIER,
		Employee NVARCHAR(50),
        StartTime NVARCHAR(5),
        EndTime NVARCHAR(5),
        Customer NVARCHAR(100),
        Services NVARCHAR(MAX),
        DayOfWeek NVARCHAR(10),
        Color NVARCHAR(10),
		BookingDate DateTime2
    );

    INSERT INTO @Result
    SELECT
        b.Id,
        bnv.IdNhanVien AS SourceId,
		nv.TenNhanVien AS Employee,
        CONVERT(NVARCHAR(5), b.StartTime, 108) AS StartTime,
        CONVERT(NVARCHAR(5), b.EndTime, 108) AS EndTime,
        b.TenKhachHang AS Customer,
        hh.TenHangHoa AS Services,
        CASE DATEPART(WEEKDAY, b.BookingDate)
            WHEN 1 THEN N'Chủ nhật'
            WHEN 2 THEN N'Thứ hai'
            WHEN 3 THEN N'Thứ ba'
            WHEN 4 THEN N'Thứ tư'
            WHEN 5 THEN N'Thứ năm'
            WHEN 6 THEN N'Thứ sáu'
            WHEN 7 THEN N'Thứ bảy'
            ELSE 'Thứ hai'
        END AS DayOfWeek,
        CASE
            WHEN b.TrangThai = 0 THEN '#a19f9c'
			WHEN B.TrangThai = 1 THEN '#FF9900'
			wHEN b.TrangThai = 2 THEN '#7DC1FF'
			WHEN b.TrangThai = 3 THEN '#009EF7'
			WHEN b.TrangThai = 4 THEN '#50CD89'
            ELSE '#009EF7'
        END AS Color,
		b.BookingDate
    FROM
        Booking b
        left JOIN BookingNhanVien bnv ON bnv.IdBooking = b.Id and bnv.IsDeleted = 0
		LEFT JOIN NS_NhanVien nv on nv.id = bnv.IdNhanVien
        LEFT JOIN BookingService bs ON bs.IdBooking = b.Id and bs.IsDeleted = 0
        LEFT JOIN DM_DonViQuiDoi dv ON dv.Id = bs.IdDonViQuiDoi
        LEFT JOIN DM_HangHoa hh ON hh.id = dv.IdHangHoa
    WHERE
        (b.TenantId = ISNULL(@TenantId, 1))
        AND (b.IsDeleted = 0)
        AND (b.IdChiNhanh = @IdChiNhanh)
        AND (b.BookingDate BETWEEN @TimeFrom AND @TimeTo)
        AND (@IdNhanVien IS NULL OR (bnv.IdNhanVien = @IdNhanVien AND @IdNhanVien IS NOT NULL))
		AND (@IdDichVu IS NULL OR (bs.IdDonViQuiDoi = @IdDichVu AND @IdDichVu IS NOT NULL))
   
    SELECT * FROM @Result ORDER BY BookingDate DESC,StartTime ASC;
END;");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_getBookingInfo]
	@IdBooking UNIQUEIDENTIFIER,
	@TenantId INT
AS 
begin
SELECT
		b.Id,
		b.IdKhachHang,
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
            WHEN b.TrangThai = 0 THEN '#a19f9c'
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
        left JOIN BookingNhanVien bnv ON bnv.IdBooking = b.Id and bnv.IsDeleted = 0
		LEFT JOIN NS_NhanVien nv on nv.id = bnv.IdNhanVien
        LEFT JOIN BookingService bs ON bs.IdBooking = b.Id and bs.IsDeleted = 0
        LEFT JOIN DM_DonViQuiDoi dv ON dv.Id = bs.IdDonViQuiDoi
        LEFT JOIN DM_HangHoa hh ON hh.id = dv.IdHangHoa
    WHERE
        b.TenantId = ISNULL(@TenantId, 1)
		AND b.Id = @IdBooking
        AND b.IsDeleted = 0;
end;");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_getKhachHang_Booking]
	@TenantId int= 7,
	@IdChiNhanhs nvarchar(max)= null,
	@TextSearch nvarchar(max) =null,
	@CurrentPage int =0,
	@PageSize int = 10,
	@TrangThaiBook int =3  ---0.xoa, 3.all
AS
BEGIN
	
	SET NOCOUNT ON;

	if(isnull(@TextSearch,'')!='') set @TextSearch = CONCAT(N'%', @TextSearch, '%')
	else set @TextSearch=''

	declare @tblChiNhanh table (ID uniqueidentifier)
	if(isnull(@IdChiNhanhs,'') !='')
		insert into @tblChiNhanh
		select GiaTri from  dbo.fnSplitstring(@IdChiNhanhs)
	else
		set @IdChiNhanhs=''
		

	; with data_cte
	as
	(
    select 
		bk.id as IdBooking,
		bk.IdKhachHang,
		bk.TrangThai,
		bk.StartTime,
		bk.EndTime,
		bk.BookingDate,
		case bk.TrangThai
			when 1 then N'Chưa xác nhận'
			when 2 then N'Đã xác nhận'
			when 3 then N'Đã check in'
		else '' end as TxtTrangThaiBook,
		kh.MaKhachHang,
		kh.TenKhachHang,
		kh.SoDienThoai,
		isnull(kh.Avatar,'') as Avatar,
		qd.MaHangHoa,
		qd.GiaBan,
		hh.TenHangHoa,
		qd.Id as IdDonViQuyDoi,
		qd.IdHangHoa,
		hh.IdNhomHangHoa,
		hh.IdLoaiHangHoa
	from Booking bk
	left join DM_KhachHang kh on bk.IdKhachHang = kh.Id
	left join BookingService bkS on bk.Id= bkS.IdBooking
	left join DM_DonViQuiDoi qd on bkS.IdDonViQuiDoi= qd.Id
	left join DM_HangHoa hh on qd.IdHangHoa= hh.Id
	left join BookingNhanVien bkN on bk.Id = bkN.IdNhanVien
	left join NS_NhanVien nv on bkN.IdNhanVien = nv.Id
	where bk.TenantId = @TenantId
	and bk.IsDeleted='0'
	and (@IdChiNhanhs ='' or exists (select ID from @tblChiNhanh cn where bk.IdChiNhanh= cn.ID))
	and (@TrangThaiBook = 3 or  bk.TrangThai = @TrangThaiBook)
	and not exists (select id from Booking_CheckIn_HoaDon bkHD where bk.Id = bkHD.IdBooking and bkHD.IsDeleted='0') ---khong lay khach dang checkin 
	and (@TextSearch =''
	 or kh.MaKhachHang like @TextSearch
	 or kh.TenKhachHang  like @TextSearch
	 or kh.TenKhachHang_KhongDau  like @TextSearch
	 or kh.SoDienThoai like @TextSearch
	 or qd.MaHangHoa like @TextSearch
	 or hh.TenHangHoa like @TextSearch
	 or hh.TenHangHoa_KhongDau like @TextSearch
	 )
	 ),
	 count_cte
	 as(
	 select count(IdBooking) as TotalRow
	 from data_cte
	 )
	 select *
	 from data_cte 
	 cross join count_cte
	 order by BookingDate
	OFFSET (@CurrentPage* @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY

END");
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[prc_lichSuDatLich]
	@TenantId INT = 1,
	@IdKhachHang UNIQUEIDENTIFIER ='66a7c340-8a01-4766-8791-4e071f9d884f',
	@SortBy nvarchar(50)='',
	@SortType nvarchar(4)='',
	@SkipCount INT = 0,
	@MaxResultCount INT = 10
AS 
BEGIN

	select
		b.Id,		
		b.BookingDate, 
		b.TrangThai,		
		b.CreationTime,
		bs.IdDonViQuiDoi,
		hh.TenHangHoa,
		hh.SoPhutThucHien,	
		dvqd.GiaBan,
		nv.TenNhanVien, ----- NV này là NV khách chọn lúc book ---		
		concat(CONVERT(NVARCHAR(5), b.StartTime, 108) , ' - ', CONVERT(NVARCHAR(5), b.EndTime, 108)) as ThoiGianHen,
		CASE b.TrangThai
            WHEN 0 THEN N'Hủy lịch'
			WHEN 1 THEN N'Đặt lịch'
			wHEN 2 THEN N'Đã xác nhận'
			wHEN 3 THEN N'Checkin'
			wHEN 4 THEN N'Hoàn thành'
            ELSE ''
        END AS TxtTrangThai	
	into #tempBook
	from booking b
	join BookingService bs on bs.IdBooking = b.id
	join DM_DonViQuiDoi dvqd on dvqd.id = bs.IdDonViQuiDoi
	join DM_HangHoa hh on hh.id = dvqd.IdHangHoa
	left join BookingNhanVien bns on bns.IdBooking = b.id
	left join NS_NhanVien nv on nv.id = bns.IdNhanVien	
	WHERE b.IsDeleted = 0 
		AND b.TenantId= @TenantId AND b.IdKhachHang = @IdKhachHang
	order by
	CASE WHEN @SortBy = 'bookingDate' AND @SortType = 'desc' THEN BookingDate END DESC,
		CASE WHEN @SortBy = 'tenDichVu' AND @SortType = 'desc' THEN hh.TenHangHoa END DESC,
		CASE WHEN @SortBy = 'thoiGianThucHien' AND @SortType = 'desc' THEN hh.SoPhutThucHien END DESC,
		CASE WHEN @SortBy = 'thoiGianBatDau' AND @SortType = 'desc' THEN b.StartTime END DESC,
		CASE WHEN @SortBy = 'gia' AND @SortType = 'desc' THEN dvqd.GiaBan END DESC,
		CASE WHEN @SortBy = 'nhanVienThucHien' AND @SortType = 'desc' THEN nv.TenNhanVien END DESC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'desc' THEN b.TrangThai END DESC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'desc' THEN b.CreationTime END DESC,
		CASE WHEN @SortBy = 'bookingDate' AND @SortType = 'asc' THEN BookingDate END ASC,
		CASE WHEN @SortBy = 'tenDichVu' AND @SortType = 'asc' THEN hh.TenHangHoa END ASC,
		CASE WHEN @SortBy = 'thoiGianThucHien' AND @SortType = 'asc' THEN hh.SoPhutThucHien END ASC,
		CASE WHEN @SortBy = 'thoiGianBatDau' AND @SortType = 'asc' THEN b.StartTime END ASC,
		CASE WHEN @SortBy = 'gia' AND @SortType = 'asc' THEN dvqd.GiaBan END ASC,
		CASE WHEN @SortBy = 'nhanVienThucHien' AND @SortType = 'asc' THEN nv.TenNhanVien END ASC,
		CASE WHEN @SortBy = 'trangThai' AND @SortType = 'asc' THEN b.TrangThai END ASC,
		CASE WHEN @SortBy = 'creationTime' AND @SortType = 'asc' THEN b.CreationTime END ASC,
		CASE WHEN ISNULL(@SortBy,'') = '' THEN b.CreationTime END DESC	
	OFFSET @SkipCount ROWS FETCH NEXT @MaxResultCount ROWS ONLY;

	; with data_cte
	as
	(
		select 
			nvth.Idboooking, 
			nv.TenNhanVien
		from(
			----- get NV thực tế đã làm cho khách----
			select 
				bk.Id as Idboooking,
				th.IdNhanVien,
				ct.IdDonViQuyDoi
			from #tempBook bk
			left join Booking_CheckIn_HoaDon ckHD on bk.Id = ckHD.IdBooking
			left join BH_HoaDon hd on ckHD.IdHoaDon = hd.Id and hd.IsDeleted='0'
			left join BH_HoaDon_ChiTiet ct on hd.Id= ct.IdHoaDon and bk.IdDonViQuiDoi = ct.IdDonViQuyDoi and ct.IsDeleted='0'
			left join BH_NhanVienThucHien th on ct.Id = th.IdHoaDonChiTiet and th.IsDeleted='0'
		) nvth
		join NS_NhanVien nv on nvth.IdNhanVien = nv.Id
		
	),
	tblNVTH
	as(
		select distinct Idboooking,
			(select TenNhanVien + ', ' as [text()]
			from data_cte dtIn
			where dtIn.Idboooking = dtOut.Idboooking
			For XML PATH ('')
			) as NVThucHiens
		from data_cte dtOut
	),
	tblBookNvth
	as
	(
		select distinct dt.Idboooking, nvth.NVThucHiens
		from data_cte dt
		left join tblNVTH nvth on dt.Idboooking = nvth.Idboooking 
	)
	select bk.*, nv.NVThucHiens
	from #tempBook bk
	left join tblBookNvth nv on bk.Id= nv.Idboooking

	SELECT COUNT(Id) as TotalCount FROM #tempBook
	drop table #tempBook
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
