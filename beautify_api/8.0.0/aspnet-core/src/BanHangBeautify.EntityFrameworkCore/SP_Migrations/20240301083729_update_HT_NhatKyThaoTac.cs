using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class updateHTNhatKyThaoTac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HT_NhatKyThaoTac_DM_ChiNhanh_IdChiNhanh",
                table: "HT_NhatKyThaoTac");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdChiNhanh",
                table: "HT_NhatKyThaoTac",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4435), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4439), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4441), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4443), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4444), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4491), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4496), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4498), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4499), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4501), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4502), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4504), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4505), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4506), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4169), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4190), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4192), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4396), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 3, 1, 15, 37, 27, 722, DateTimeKind.Local).AddTicks(4414), null, null });

            migrationBuilder.AddForeignKey(
                name: "FK_HT_NhatKyThaoTac_DM_ChiNhanh_IdChiNhanh",
                table: "HT_NhatKyThaoTac",
                column: "IdChiNhanh",
                principalTable: "DM_ChiNhanh",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HT_NhatKyThaoTac_DM_ChiNhanh_IdChiNhanh",
                table: "HT_NhatKyThaoTac");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdChiNhanh",
                table: "HT_NhatKyThaoTac",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6790), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6795), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6797), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6798), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6800), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6801), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6808), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6809), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6811), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6812), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6814), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6815), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6817), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6818), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6479), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6500), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6502), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6700), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2024, 2, 28, 16, 48, 4, 775, DateTimeKind.Local).AddTicks(6716), null, null });

            migrationBuilder.AddForeignKey(
                name: "FK_HT_NhatKyThaoTac_DM_ChiNhanh_IdChiNhanh",
                table: "HT_NhatKyThaoTac",
                column: "IdChiNhanh",
                principalTable: "DM_ChiNhanh",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
