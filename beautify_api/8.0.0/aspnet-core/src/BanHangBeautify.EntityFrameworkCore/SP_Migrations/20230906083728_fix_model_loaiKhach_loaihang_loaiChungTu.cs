using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class fixmodelloaiKhachloaihangloaiChungTu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "DM_LoaiKhach",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "DM_LoaiHangHoa",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "DM_LoaiChungTu",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1371), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1375), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1378), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1379), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1381), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1382), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1387), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1435), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1437), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1439), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1440), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1441), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1443), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1445), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1105), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1126), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1131), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1333), null, null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 9, 6, 15, 37, 26, 564, DateTimeKind.Local).AddTicks(1344), null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "DM_LoaiKhach",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "DM_LoaiHangHoa",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "DM_LoaiChungTu",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1130), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1133), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1135), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1137), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1138), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1139), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1146), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1148), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1149), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1151), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1152), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1154), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1156), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1157), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(876), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(899), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(901), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1093), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime", "TenantId" },
                values: new object[] { new DateTime(2023, 8, 30, 14, 50, 56, 326, DateTimeKind.Local).AddTicks(1104), null, null, 0 });
        }
    }
}
