using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class EditStorepProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NguoiSua",
                table: "NS_NhanVien");

            migrationBuilder.DropColumn(
                name: "NguoiTao",
                table: "NS_NhanVien");

            migrationBuilder.DropColumn(
                name: "NguoiXoa",
                table: "NS_NhanVien");

            migrationBuilder.DropColumn(
                name: "TenNhanVien_KhongDau",
                table: "NS_NhanVien");

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8636), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8639), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8641), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8642), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8644), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8645), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8652), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8653), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8654), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8656), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8657), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8659), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8661), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8663), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8337), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8360), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8362), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8594), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 27, 10, 37, 15, 534, DateTimeKind.Local).AddTicks(8610), null, null });

            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[spGetDMHangHoa]
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
		FETCH NEXT @PageSize ROWS ONLY

 END 
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NguoiSua",
                table: "NS_NhanVien",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiTao",
                table: "NS_NhanVien",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiXoa",
                table: "NS_NhanVien",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNhanVien_KhongDau",
                table: "NS_NhanVien",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1049), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1058), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1064), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1070), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1159), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1166), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1171), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1176), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1178), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1183), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1188), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1192), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1197), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(1202), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(383), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(413), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(419), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(984), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 23, 14, 33, 57, 164, DateTimeKind.Local).AddTicks(993), null, null });
        }
    }
}
