using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class fixnamefieldThoiGianKetThucDMKhuyenMai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThoiGianKeThuc",
                table: "DM_KhuyenMai",
                newName: "ThoiGianKetThuc");

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 16, 11, 21, 19, 771, DateTimeKind.Local).AddTicks(4002), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 16, 11, 21, 19, 771, DateTimeKind.Local).AddTicks(4028), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 16, 11, 21, 19, 771, DateTimeKind.Local).AddTicks(4030), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 16, 11, 21, 19, 771, DateTimeKind.Local).AddTicks(4315), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 5, 16, 11, 21, 19, 771, DateTimeKind.Local).AddTicks(4327), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThoiGianKetThuc",
                table: "DM_KhuyenMai",
                newName: "ThoiGianKeThuc");

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
        }
    }
}
