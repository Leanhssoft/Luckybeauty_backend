using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class updatelichLamViecCa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_LichLamViec_IdLichLamViec",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.DropIndex(
                name: "IX_NS_LichLamViec_Ca_IdLichLamViec",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdLichLamViec",
                table: "NS_LichLamViec_Ca",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NS_LichLamViecId",
                table: "NS_LichLamViec_Ca",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Ngay",
                table: "NS_LichLamViec_Ca",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "GioNghiDen",
                table: "NS_CaLamViec",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GioNghiTu",
                table: "NS_CaLamViec",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7103), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7107), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7108), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7110), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7111), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7113), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7118), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7119), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7121), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7122), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7124), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7125), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7127), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7129), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(6662), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(6731), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(6733), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7062), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 7, 3, 12, 4, 37, 435, DateTimeKind.Local).AddTicks(7076), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViec_Ca_NS_LichLamViecId",
                table: "NS_LichLamViec_Ca",
                column: "NS_LichLamViecId");

            migrationBuilder.AddForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_LichLamViec_NS_LichLamViecId",
                table: "NS_LichLamViec_Ca",
                column: "NS_LichLamViecId",
                principalTable: "NS_LichLamViec",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_LichLamViec_NS_LichLamViecId",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.DropIndex(
                name: "IX_NS_LichLamViec_Ca_NS_LichLamViecId",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.DropColumn(
                name: "NS_LichLamViecId",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.DropColumn(
                name: "Ngay",
                table: "NS_LichLamViec_Ca");

            migrationBuilder.DropColumn(
                name: "GioNghiDen",
                table: "NS_CaLamViec");

            migrationBuilder.DropColumn(
                name: "GioNghiTu",
                table: "NS_CaLamViec");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdLichLamViec",
                table: "NS_LichLamViec_Ca",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6311), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6314), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6316), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6366), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6368), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6369), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6380), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6381), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6383), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6384), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 22, 8, 39, 33, 513, DateTimeKind.Local).AddTicks(7386), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6388), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6389), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiChungTu",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6391), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6083), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6109), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiHangHoa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6111), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6279), null, null });

            migrationBuilder.UpdateData(
                table: "DM_LoaiKhach",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "DeletionTime", "LastModificationTime" },
                values: new object[] { new DateTime(2023, 6, 20, 16, 33, 22, 832, DateTimeKind.Local).AddTicks(6290), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViec_Ca_IdLichLamViec",
                table: "NS_LichLamViec_Ca",
                column: "IdLichLamViec");

            migrationBuilder.AddForeignKey(
                name: "FK_NS_LichLamViec_Ca_NS_LichLamViec_IdLichLamViec",
                table: "NS_LichLamViec_Ca",
                column: "IdLichLamViec",
                principalTable: "NS_LichLamViec",
                principalColumn: "Id");
        }
    }
}
