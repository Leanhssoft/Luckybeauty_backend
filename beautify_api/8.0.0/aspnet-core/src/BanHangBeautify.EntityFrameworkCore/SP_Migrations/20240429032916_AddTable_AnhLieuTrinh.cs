using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanHangBeautify.SPMigrations
{
    /// <inheritdoc />
    public partial class AddTableAnhLieuTrinh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
             table: "DM_LoaiChungTu",
             columns: new[] { "Id", "MaLoaiChungTu", "TenLoaiChungTu", "TrangThai", "IsDeleted", "CreationTime" },
             values: new object[,]
             {
                    { 15, "BK", "BookingCode" ,1,false, "2024-04-29"}
             });

            migrationBuilder.AddColumn<string>(
                name: "BookingCode",
                table: "Booking",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KhachHang_Anh_LieuTrinh",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AlbumName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang_Anh_LieuTrinh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KhachHang_Anh_LieuTrinh_DM_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "DM_KhachHang",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KhachHang_Anh_LieuTrinh_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageIndex = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang_Anh_LieuTrinh_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KhachHang_Anh_LieuTrinh_ChiTiet_KhachHang_Anh_LieuTrinh_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "KhachHang_Anh_LieuTrinh",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Anh_LieuTrinh_IdKhachHang",
                table: "KhachHang_Anh_LieuTrinh",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Anh_LieuTrinh_ChiTiet_AlbumId",
                table: "KhachHang_Anh_LieuTrinh_ChiTiet",
                column: "AlbumId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KhachHang_Anh_LieuTrinh_ChiTiet");

            migrationBuilder.DropTable(
                name: "KhachHang_Anh_LieuTrinh");

            migrationBuilder.DropColumn(
                name: "BookingCode",
                table: "Booking");
        }
    }
}
