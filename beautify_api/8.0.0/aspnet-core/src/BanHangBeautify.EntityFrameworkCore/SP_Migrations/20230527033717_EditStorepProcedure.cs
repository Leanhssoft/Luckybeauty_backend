using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class EditStorepProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.Sql(@"ALTER PROCEDURE spGetDMHangHoa
			@TenantId int =1,
			@TextSearch nvarchar(max)=null,
			@IdNhomHangHoas nvarchar(max)=null,
			@IdLoaiHangHoa int = 0,
			@Where nvarchar(max) =null,
			@CurrentPage int = 1,
			@PageSize int = 1000,
			@ColumnSort varchar(100) ='CreationTime',
			@TypeSort varchar(20) = 'DESC'
		AS
		BEGIN
	
		SET NOCOUNT ON;
		if @CurrentPage > 0  set @CurrentPage = @CurrentPage- 1 else set @CurrentPage =0

		--- filter nhomhang ---
		declare @tblNhomHang table(ID varchar(40))
		if isnull(@IdNhomHangHoas,'') ='' set @IdNhomHangHoas =''
		else 
			insert into @tblNhomHang
			select * from dbo.fnGetListNhomHangHoa(@IdNhomHangHoas)

			
		declare @tblSearch table(Txt nvarchar(max))
	
		if isnull(@TextSearch,'') =''
			begin
				set @TextSearch=''
			end
			else 
				set @TextSearch = N'%' + @TextSearch + '%'

		;with data_cte
		as
		(
		select 
			hh.Id,		
			qd.Id as IdDonViQuyDoi,	
			hh.IdNhomHangHoa,
			qd.MaHangHoa,
			hh.TenHangHoa,
			isnull(qd.GiaBan,cast (0 as float)) as GiaBan,
			isnull(hh.SoPhutThucHien,cast (0 as float)) as SoPhutThucHien,
			hh.TrangThai,
			hh.MoTa,
			hh.NguoiTao,
			loai.TenLoaiHangHoa,
			isnull(nhom.Color,'') as Color,
			isnull(nhom.TenNhomHang,'') as TenNhomHang,
			case hh.trangthai
				when 1 then N'Đang kinh doanh'
				when 0 then N'Ngừng kinh doanh'
			else '' end as TxtTrangThaiHang
		from DM_HangHoa hh
		left join DM_LoaiHangHoa loai on hh.IdLoaiHangHoa = loai.Id
		left join DM_NhomHangHoa nhom on hh.IdNhomHangHoa = nhom.Id
		left join DM_DonViQuiDoi qd on hh.Id= qd.IdHangHoa
		where hh.TenantId = @TenantId
		and LaDonViTinhChuan =1
		and (@IdLoaiHangHoa = 0 or hh.IdLoaiHangHoa = @IdLoaiHangHoa)
		and (@IdNhomHangHoas='' or exists (select * from @tblNhomHang nhomh where hh.IdNhomHangHoa= nhomh.ID))
		and (@TextSearch ='' or  
			(hh.TenHangHoa like @TextSearch or hh.TenHangHoa_KhongDau like @TextSearch or qd.MaHangHoa like @TextSearch or hh.MoTa like @TextSearch))
			),
			count_cte
			as
			(
				select  
				count(Id) as TotalRow,
				ceiling(count(Id)/ CAST(@PageSize as float)) as TotalPage
				from data_cte
			)
			select *
			from data_cte
			cross join count_cte
			order by 
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='MaHangHoa' then MaHangHoa end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='MaHangHoa' then MaHangHoa end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='TenHangHoa' then TenHangHoa end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='TenHangHoa' then TenHangHoa end DESC,
			case when @TypeSort <> 'ASC' then 0
			when @ColumnSort='GiaBan' then GiaBan end ASC,
			case when @TypeSort <> 'DESC' then 0
			when @ColumnSort='GiaBan' then GiaBan end DESC
		OFFSET (@CurrentPage* @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY;

 END;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
