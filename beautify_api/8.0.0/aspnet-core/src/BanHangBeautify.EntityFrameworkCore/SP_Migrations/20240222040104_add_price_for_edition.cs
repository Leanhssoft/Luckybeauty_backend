using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class addpriceforedition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpEditions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "AbpEditions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4962), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4967), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4969), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4970), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4972), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4973), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4978), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4980), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4981), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4983), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4984), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4985), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4987), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4989), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4611), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4629), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4631), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4924), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 22, 11, 1, 2, 617, DateTimeKind.Local).AddTicks(4938), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpEditions");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "AbpEditions");

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2080), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2085), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2088), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2090), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2092), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2094), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2101), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2103), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2105), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2107), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2109), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2111), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2113), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2115), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(1543), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(1568), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(1570), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2019), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 1, 9, 44, 6, 698, DateTimeKind.Local).AddTicks(2044), null, null });
        }
    }
}
