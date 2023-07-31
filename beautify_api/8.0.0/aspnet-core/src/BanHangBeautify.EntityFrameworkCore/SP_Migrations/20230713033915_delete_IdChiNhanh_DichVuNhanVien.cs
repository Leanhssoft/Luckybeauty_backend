using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class deleteIdChiNhanhDichVuNhanVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DichVu_NhanVien_DM_ChiNhanh_IdChiNhanh",
                table: "DichVu_NhanVien");

            migrationBuilder.DropIndex(
                name: "IX_DichVu_NhanVien_IdChiNhanh",
                table: "DichVu_NhanVien");

            migrationBuilder.DropColumn(
                name: "IdChiNhanh",
                table: "DichVu_NhanVien");

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6108), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6111), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6113), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6114), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6116), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6117), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6122), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6124), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6125), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6127), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6128), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6129), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6131), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6140), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(5800), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(5822), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(5824), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6069), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 13, 10, 39, 13, 583, DateTimeKind.Local).AddTicks(6080), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdChiNhanh",
                table: "DichVu_NhanVien",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7321), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7326), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7332), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7337), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7339), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7341), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7351), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7352), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7356), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7360), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7364), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7366), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7367), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7371), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(6346), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(6380), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(6385), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7239), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 12, 9, 58, 23, 834, DateTimeKind.Local).AddTicks(7270), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_DichVu_NhanVien_IdChiNhanh",
                table: "DichVu_NhanVien",
                column: "IdChiNhanh");

            migrationBuilder.AddForeignKey(
                name: "FK_DichVu_NhanVien_DM_ChiNhanh_IdChiNhanh",
                table: "DichVu_NhanVien",
                column: "IdChiNhanh",
                principalTable: "DM_ChiNhanh",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
