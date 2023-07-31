using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class addcheckuseradmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2673), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2677), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2678), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2680), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2682), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2683), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2688), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2690), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2691), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2693), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2743), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2745), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2747), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2749), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2430), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2453), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2455), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2629), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 2, 15, 57, 17, 382, DateTimeKind.Local).AddTicks(2647), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "AbpUsers");

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
        }
    }
}
