using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class adddescriptionHTCuaHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TongNgayNghi",
                table: "NS_NhanVien_TimeOff",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "IdNhanVien",
                table: "NS_NhanVien_TimeOff",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "LoaiNghi",
                table: "NS_NhanVien_TimeOff",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "HT_CongTy",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "HT_CongTy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "HT_CongTy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "HT_CongTy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "HT_CongTy",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 13, 13, 40, 20, 770, DateTimeKind.Local).AddTicks(8645), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 13, 13, 40, 20, 770, DateTimeKind.Local).AddTicks(8668), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 13, 13, 40, 20, 770, DateTimeKind.Local).AddTicks(8670), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 13, 13, 40, 20, 770, DateTimeKind.Local).AddTicks(8969), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 13, 13, 40, 20, 770, DateTimeKind.Local).AddTicks(8976), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_NS_NhanVien_TimeOff_IdNhanVien",
                table: "NS_NhanVien_TimeOff",
                column: "IdNhanVien");

            migrationBuilder.AddForeignKey(
                name: "FK_NS_NhanVien_TimeOff_NS_NhanViens_IdNhanVien",
                table: "NS_NhanVien_TimeOff",
                column: "IdNhanVien",
                principalTable: "NS_NhanViens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NS_NhanVien_TimeOff_NS_NhanViens_IdNhanVien",
                table: "NS_NhanVien_TimeOff");

            migrationBuilder.DropIndex(
                name: "IX_NS_NhanVien_TimeOff_IdNhanVien",
                table: "NS_NhanVien_TimeOff");

            migrationBuilder.DropColumn(
                name: "IdNhanVien",
                table: "NS_NhanVien_TimeOff");

            migrationBuilder.DropColumn(
                name: "LoaiNghi",
                table: "NS_NhanVien_TimeOff");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "HT_CongTy");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "HT_CongTy");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "HT_CongTy");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "HT_CongTy");

            migrationBuilder.AlterColumn<int>(
                name: "TongNgayNghi",
                table: "NS_NhanVien_TimeOff",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "HT_CongTy",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 12, 17, 11, 41, 521, DateTimeKind.Local).AddTicks(3345), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 12, 17, 11, 41, 521, DateTimeKind.Local).AddTicks(3373), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 12, 17, 11, 41, 521, DateTimeKind.Local).AddTicks(3375), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 12, 17, 11, 41, 521, DateTimeKind.Local).AddTicks(3703), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 12, 17, 11, 41, 521, DateTimeKind.Local).AddTicks(3714), null, null });
        }
    }
}
