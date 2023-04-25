using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class EditTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ID_Parent",
                table: "DM_NhomHangHoa",
                type: "uniqueidentifier",
                nullable: true);
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetDetailProduct]");

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetDMHangHoa]
	                @TenantId int =1,
	                @TextSearch nvarchar(max)=null,
	                @IdNhomHangHoas nvarchar(max)=null,
	                @Where nvarchar(max) =null,
	                @CurrentPage int = 0,
                    @PageSize int = 1000,
                    @ColumnSort varchar(100) ='CreationTime',
                    @TypeSort varchar(20) = 'DESC'
                AS
                BEGIN
	
	                SET NOCOUNT ON;
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
		                hh.TrangThai,
		                hh.MoTa,
		                hh.NguoiTao,
		                loai.TenLoaiHangHoa,
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
	                and (@TextSearch ='' or  
		                (hh.TenHangHoa like @TextSearch or qd.MaHangHoa like @TextSearch or hh.MoTa like @TextSearch))
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
					FETCH NEXT @PageSize ROWS ONLY

                END ");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetDetailProduct]
	@IdDonViQuyDoi uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON;

	select 
		hh.Id,
		hh.IdLoaiHangHoa,
		hh.IdNhomHangHoa,
		qd.Id as IdDonViQuyDoi,
		qd.MaHangHoa,
		qd.GiaBan,
		hh.TenHangHoa,
		loai.TenLoaiHangHoa,
		hh.MoTa,
		hh.TrangThai,	
		qd.LaDonViTinhChuan,
		qd.TyLeChuyenDoi,	
		qd.TenDonViTinh,
		isnull(nhom.TenNhomHang,N'Nhóm mặc định') as TenNhomHang,
		iif(hh.TrangThai =1, N'Đang kinh doanh',N'Ngừng kinh doanh') as TxtTrangThaiHang		
	from DM_HangHoa hh
	join DM_LoaiHangHoa loai on hh.IdLoaiHangHoa= loai.Id
	join DM_DonViQuiDoi qd on hh.Id = qd.IdHangHoa
	left join DM_NhomHangHoa nhom on hh.IdNhomHangHoa= nhom.Id
	where qd.Id = @IdDonViQuyDoi 
END");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID_Parent",
                table: "DM_NhomHangHoa");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetDetailProduct]");
        }
    }
}
