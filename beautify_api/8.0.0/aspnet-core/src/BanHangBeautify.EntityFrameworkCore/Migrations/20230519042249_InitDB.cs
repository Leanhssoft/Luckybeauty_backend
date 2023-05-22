using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BanHangBeautify.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpAuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ReturnValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutionDuration = table.Column<int>(type: "int", nullable: false),
                    ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ImpersonatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    ImpersonatorTenantId = table.Column<int>(type: "int", nullable: true),
                    CustomData = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpBackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobType = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    JobArgs = table.Column<string>(type: "nvarchar(max)", maxLength: 1048576, nullable: false),
                    TryCount = table.Column<short>(type: "smallint", nullable: false),
                    NextTryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAbandoned = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpBackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InputType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpEditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_AbpEditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChangeSets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpersonatorTenantId = table.Column<int>(type: "int", nullable: true),
                    ImpersonatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChangeSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_AbpLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLanguageTexts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    LanguageName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 67108864, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLanguageTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(type: "tinyint", nullable: false),
                    UserIds = table.Column<string>(type: "nvarchar(max)", maxLength: 131072, nullable: true),
                    ExcludedUserIds = table.Column<string>(type: "nvarchar(max)", maxLength: 131072, nullable: true),
                    TenantIds = table.Column<string>(type: "nvarchar(max)", maxLength: 131072, nullable: true),
                    TargetNotifiers = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpNotificationSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    NotificationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    EntityTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpNotificationSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnitRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnitRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    table.PrimaryKey("PK_AbpOrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbpTenantNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    NotificationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenantNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserLinkId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_AbpUserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLoginAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenancyName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    UserNameOrEmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Result = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLoginAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TenantNotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetNotifiers = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserOrganizationUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NhanSuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthenticationSource = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EmailConfirmationCode = table.Column<string>(type: "nvarchar(328)", maxLength: 328, nullable: true),
                    PasswordResetCode = table.Column<string>(type: "nvarchar(328)", maxLength: 328, nullable: true),
                    LockoutEndDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    IsLockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsPhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsTwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedEmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebhookName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    WebhookUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Webhooks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Headers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Booking_Color",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaMau = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Booking_Color", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_KhoanThuChi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaKhoanThuChi = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenKhoanThuChi = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    LaKhoanThu = table.Column<bool>(type: "bit", nullable: false),
                    ChungTuApDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DM_KhoanThuChi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_KhuyenMai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiKhuyenMai = table.Column<byte>(type: "tinyint", nullable: false),
                    HinhThucKM = table.Column<byte>(type: "tinyint", nullable: false),
                    ThoiGianApDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TatCaKhachHang = table.Column<bool>(type: "bit", nullable: false),
                    TatCaChiNhanh = table.Column<bool>(type: "bit", nullable: false),
                    TatCaNhanVien = table.Column<bool>(type: "bit", nullable: false),
                    NgayApDung = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ThangApDung = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ThuApDung = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    GioApDung = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DM_KhuyenMai", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_LoaiChungTu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaLoaiChungTu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenLoaiChungTu = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_LoaiChungTu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_LoaiHangHoa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaLoaiHangHoa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenLoaiHangHoa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_LoaiHangHoa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_LoaiKhach",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaLoaiKhachHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenLoaiKhachHang = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_LoaiKhach", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_NganHang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaNganHang = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenNganHang = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ChiPhiThanhToan = table.Column<float>(type: "real", nullable: true),
                    TheoPhanTram = table.Column<bool>(type: "bit", nullable: true),
                    ThuPhiThanhToan = table.Column<bool>(type: "bit", nullable: true),
                    ChungTuApDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DM_NganHang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_NgayNghiLe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TenNgayLe = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongSoNgay = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DM_NgayNghiLe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_NguonKhach",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaNguon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenNguon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayXoa = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_DM_NguonKhach", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_NhomHangHoa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaNhomHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenNhomHang = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenNhomHangKhongDau = table.Column<string>(name: "TenNhomHang_KhongDau", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LaNhomHangHoa = table.Column<bool>(type: "bit", nullable: true),
                    IdParent = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_NhomHangHoa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_NhomKhachHang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaNhomKhach = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenNhomKhach = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    GiamGia = table.Column<float>(type: "real", nullable: true),
                    LaPhamTram = table.Column<bool>(type: "bit", nullable: true),
                    TuDongCapNhat = table.Column<bool>(type: "bit", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_NhomKhachHang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_ViTriPhong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaViTriPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenViTriPhong = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
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
                    table.PrimaryKey("PK_DM_ViTriPhong", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HT_CauHinhPhanMem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TichDiem = table.Column<bool>(type: "bit", nullable: false),
                    KhuyenMai = table.Column<bool>(type: "bit", nullable: false),
                    MauInMacDinh = table.Column<bool>(type: "bit", nullable: false),
                    SuDungMaChungTu = table.Column<bool>(type: "bit", nullable: false),
                    QLKhachHangTheoChiNhanh = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_HT_CauHinhPhanMem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HT_CongTy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TenCongTy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MaSoThue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_HT_CongTy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NS_CaLamViec",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaCa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenCa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    GioVao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioRa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongGioCong = table.Column<float>(type: "real", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_NS_CaLamViec", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NS_ChucVu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaChucVu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenChucVu = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_NS_ChucVu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicEntityProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityFullName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DynamicPropertyId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicEntityProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicEntityProperties_AbpDynamicProperties_DynamicPropertyId",
                        column: x => x.DynamicPropertyId,
                        principalTable: "AbpDynamicProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicPropertyValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DynamicPropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicPropertyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicPropertyValues_AbpDynamicProperties_DynamicPropertyId",
                        column: x => x.DynamicPropertyId,
                        principalTable: "AbpDynamicProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EditionId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpFeatures_AbpEditions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChanges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangeType = table.Column<byte>(type: "tinyint", nullable: false),
                    EntityChangeSetId = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    EntityTypeFullName = table.Column<string>(type: "nvarchar(192)", maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityChanges_AbpEntityChangeSets_EntityChangeSetId",
                        column: x => x.EntityChangeSetId,
                        principalTable: "AbpEntityChangeSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsStatic = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbpSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpSettings_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbpTenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenancyName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EditionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpEditions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbpUserClaims",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserClaims_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLogins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserLogins_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserRoles_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserTokens_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookSendAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebhookEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebhookSubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseStatusCode = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookSendAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpWebhookSendAttempts_AbpWebhookEvents_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalTable: "AbpWebhookEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_KhuyenMai_ApDung",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhomKhach = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_KhuyenMai_ApDung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_KhuyenMai_ApDung_DM_KhuyenMai_IdKhuyenMai",
                        column: x => x.IdKhuyenMai,
                        principalTable: "DM_KhuyenMai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_KhuyenMai_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    STT = table.Column<byte>(type: "tinyint", nullable: false),
                    TongTienHang = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiamGiaTheoPhanTram = table.Column<bool>(type: "bit", nullable: false),
                    IdNhomHangMua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdDonViQuiDoiMua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhomHangTang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdDonViQuiDoiTang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoLuongMua = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuongTang = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaKhuyenMai = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_DM_KhuyenMai_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_KhuyenMai_ChiTiet_DM_KhuyenMai_IdKhuyenMai",
                        column: x => x.IdKhuyenMai,
                        principalTable: "DM_KhuyenMai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_HangHoa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenHangHoa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenHangHoaKhongDau = table.Column<string>(name: "TenHangHoa_KhongDau", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    SoPhutThucHien = table.Column<float>(type: "real", nullable: true),
                    IdLoaiHangHoa = table.Column<int>(type: "int", nullable: false),
                    IdNhomHangHoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_DM_HangHoa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_HangHoa_DM_LoaiHangHoa_IdLoaiHangHoa",
                        column: x => x.IdLoaiHangHoa,
                        principalTable: "DM_LoaiHangHoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DM_HangHoa_DM_NhomHangHoa_IdNhomHangHoa",
                        column: x => x.IdNhomHangHoa,
                        principalTable: "DM_NhomHangHoa",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DM_KhachHang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaKhachHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenKhachHang = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenKhachHangKhongDau = table.Column<string>(name: "TenKhachHang_KhongDau", type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    GioiTinhNam = table.Column<bool>(type: "bit", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    TongTichDiem = table.Column<float>(type: "real", nullable: true),
                    MaSoThue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KieuNgaySinh = table.Column<int>(type: "int", nullable: true),
                    IdLoaiKhach = table.Column<int>(type: "int", nullable: false),
                    IdNhomKhach = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNguonKhach = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdTinhThanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdQuanHuyen = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_KhachHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_KhachHang_DM_LoaiKhach_IdLoaiKhach",
                        column: x => x.IdLoaiKhach,
                        principalTable: "DM_LoaiKhach",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DM_KhachHang_DM_NguonKhach_IdNguonKhach",
                        column: x => x.IdNguonKhach,
                        principalTable: "DM_NguonKhach",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DM_KhachHang_DM_NhomKhachHang_IdNhomKhach",
                        column: x => x.IdNhomKhach,
                        principalTable: "DM_NhomKhachHang",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DM_NhomKhach_DieuKien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    STT = table.Column<byte>(type: "tinyint", nullable: false),
                    IdNhomKhach = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiDieuKien = table.Column<byte>(type: "tinyint", nullable: false),
                    LoaiSoSanh = table.Column<byte>(type: "tinyint", nullable: false),
                    GiaTriSo = table.Column<float>(type: "real", nullable: false),
                    GiaTriBool = table.Column<bool>(type: "bit", nullable: false),
                    GiaTriThoiGian = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GiaTriKhuVuc = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_NhomKhach_DieuKien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_NhomKhach_DieuKien_DM_NhomKhachHang_IdNhomKhach",
                        column: x => x.IdNhomKhach,
                        principalTable: "DM_NhomKhachHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_Phong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenPhong = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IdViTri = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_Phong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_Phong_DM_ViTriPhong_IdViTri",
                        column: x => x.IdViTri,
                        principalTable: "DM_ViTriPhong",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HT_CauHinh_TichDiem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdCauHinh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HTCauHinhPhanMemId = table.Column<Guid>(name: "HT_CauHinhPhanMemId", type: "uniqueidentifier", nullable: true),
                    TyLeDoiDiem = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChoPhepThanhToanBangDiem = table.Column<bool>(type: "bit", nullable: false),
                    DiemThanhToan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienThanhToan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KhongTichDiemHDGiamGia = table.Column<bool>(type: "bit", nullable: false),
                    TichDiemHoaDonGiamGia = table.Column<bool>(type: "bit", nullable: false),
                    KhongTichDiemSPGiamGia = table.Column<bool>(type: "bit", nullable: false),
                    TatCaKhachHang = table.Column<bool>(type: "bit", nullable: false),
                    SoLanMua = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_HT_CauHinh_TichDiem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HT_CauHinh_TichDiem_HT_CauHinhPhanMem_HT_CauHinhPhanMemId",
                        column: x => x.HTCauHinhPhanMemId,
                        principalTable: "HT_CauHinhPhanMem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DM_ChiNhanh",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCongTy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaChiNhanh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenChiNhanh = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MaSoThue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayApDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_DM_ChiNhanh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_ChiNhanh_HT_CongTy_IdCongTy",
                        column: x => x.IdCongTy,
                        principalTable: "HT_CongTy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicEntityPropertyValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DynamicEntityPropertyId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicEntityPropertyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicEntityPropertyValues_AbpDynamicEntityProperties_DynamicEntityPropertyId",
                        column: x => x.DynamicEntityPropertyId,
                        principalTable: "AbpDynamicEntityProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityPropertyChanges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityChangeId = table.Column<long>(type: "bigint", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    PropertyTypeFullName = table.Column<string>(type: "nvarchar(192)", maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    NewValueHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalValueHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                        column: x => x.EntityChangeId,
                        principalTable: "AbpEntityChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpPermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpPermissions_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpPermissions_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoleClaims",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoleClaims_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_DonViQuiDoi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MaHangHoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenDonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TyLeChuyenDoi = table.Column<float>(type: "real", nullable: true),
                    GiaBan = table.Column<float>(type: "real", nullable: true),
                    LaDonViTinhChuan = table.Column<int>(type: "int", nullable: true),
                    IdHangHoa = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_DM_DonViQuiDoi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_DonViQuiDoi_DM_HangHoa_IdHangHoa",
                        column: x => x.IdHangHoa,
                        principalTable: "DM_HangHoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HT_CauHinh_TichDiemChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdTichDiem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNhomKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_HT_CauHinh_TichDiemChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HT_CauHinh_TichDiemChiTiet_HT_CauHinh_TichDiem_IdTichDiem",
                        column: x => x.IdTichDiem,
                        principalTable: "HT_CauHinh_TichDiem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiBooking = table.Column<byte>(type: "tinyint", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserXuLy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayXuLy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_DM_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "DM_KhachHang",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DM_MauIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiChungTu = table.Column<int>(type: "int", nullable: false),
                    TenMauIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaMacDinh = table.Column<bool>(type: "bit", nullable: false),
                    NoiDungMauIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DM_MauIn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_MauIn_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DM_MauIn_DM_LoaiChungTu_LoaiChungTu",
                        column: x => x.LoaiChungTu,
                        principalTable: "DM_LoaiChungTu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_PhongBan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaPhongBan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenPhongBan = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayXoa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DM_PhongBan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_PhongBan_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DM_TaiKhoanNganHang",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdNganHang = table.Column<Guid>(type: "uniqueidentifier", maxLength: 256, nullable: false),
                    SoTaiKhoan = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TenChuThe = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DM_TaiKhoanNganHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DM_TaiKhoanNganHang_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DM_TaiKhoanNganHang_DM_NganHang_IdNganHang",
                        column: x => x.IdNganHang,
                        principalTable: "DM_NganHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HT_CauHinh_ChungTu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdLoaiChungTu = table.Column<int>(type: "int", nullable: false),
                    MaLoaiChungTu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuDungMaChiNhanh = table.Column<bool>(type: "bit", nullable: false),
                    KiTuNganCach1 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    KiTuNganCach2 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    NgayThangNam = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    KiTuNganCach3 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    DoDaiSTT = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_HT_CauHinh_ChungTu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HT_CauHinh_ChungTu_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HT_CauHinh_ChungTu_DM_LoaiChungTu_IdLoaiChungTu",
                        column: x => x.IdLoaiChungTu,
                        principalTable: "DM_LoaiChungTu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HT_NhatKyThaoTac",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChucNang = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LoaiNhatKy = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoiDungChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_HT_NhatKyThaoTac", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HT_NhatKyThaoTac_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KH_CheckIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdBooking = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateTimeCheckIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_KH_CheckIn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KH_CheckIn_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KH_CheckIn_DM_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "DM_KhachHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_ChietKhauHoaDon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DMChiNhanhId = table.Column<Guid>(name: "DM_ChiNhanhId", type: "uniqueidentifier", nullable: true),
                    LoaiChietKhau = table.Column<byte>(type: "tinyint", nullable: false),
                    GiaTriChietKhau = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChungTuApDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_NS_ChietKhauHoaDon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauHoaDon_DM_ChiNhanh_DM_ChiNhanhId",
                        column: x => x.DMChiNhanhId,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuyHoaDon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdLoaiChungTu = table.Column<int>(type: "int", nullable: false),
                    MaHoaDon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NgayLapHoaDon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TongTienThu = table.Column<float>(type: "real", nullable: true),
                    NoiDungThu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    HachToanKinhDoanh = table.Column<bool>(type: "bit", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_QuyHoaDon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_DM_LoaiChungTu_IdLoaiChungTu",
                        column: x => x.IdLoaiChungTu,
                        principalTable: "DM_LoaiChungTu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdDonViQuiDoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdBooking = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_BookingService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingService_Booking_IdBooking",
                        column: x => x.IdBooking,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingService_DM_DonViQuiDoi_IdDonViQuiDoi",
                        column: x => x.IdDonViQuiDoi,
                        principalTable: "DM_DonViQuiDoi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_NhanVien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ho = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TenLot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenNhanVien = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KieuNgaySinh = table.Column<int>(type: "int", nullable: false),
                    GioiTinh = table.Column<int>(type: "int", nullable: false),
                    NgayCap = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NoiCap = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Avatar = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IdPhongBan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiXoa = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayXoa = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_NS_NhanVien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_NhanVien_DM_PhongBan_IdPhongBan",
                        column: x => x.IdPhongBan,
                        principalTable: "DM_PhongBan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NS_NhanVien_NS_ChucVu_IdChucVu",
                        column: x => x.IdChucVu,
                        principalTable: "NS_ChucVu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BH_HoaDon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdLoaiChungTu = table.Column<int>(type: "int", nullable: false),
                    MaHoaDon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NgayLapHoaDon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayApDung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdPhong = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TongTienHangChuaChietKhau = table.Column<float>(type: "real", nullable: true),
                    PTChietKhauHang = table.Column<float>(type: "real", nullable: true),
                    TongChietKhauHangHoa = table.Column<float>(type: "real", nullable: true),
                    TongTienHang = table.Column<float>(type: "real", nullable: true),
                    PTThueHD = table.Column<float>(type: "real", nullable: true),
                    TongTienThue = table.Column<float>(type: "real", nullable: true),
                    TongTienHDSauVAT = table.Column<float>(type: "real", nullable: true),
                    PTGiamGiaHD = table.Column<float>(type: "real", nullable: true),
                    TongGiamGiaHD = table.Column<float>(type: "real", nullable: true),
                    ChiPhiTraHang = table.Column<float>(type: "real", nullable: true),
                    TongThanhToan = table.Column<float>(type: "real", nullable: true),
                    ChiPhiHD = table.Column<float>(type: "real", nullable: true),
                    ChiPhiGhiChu = table.Column<string>(name: "ChiPhi_GhiChu", type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DiemGiaoDich = table.Column<float>(type: "real", nullable: true),
                    GhiChuHD = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_BH_HoaDon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_BH_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_DM_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "DM_KhachHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_DM_LoaiChungTu_IdLoaiChungTu",
                        column: x => x.IdLoaiChungTu,
                        principalTable: "DM_LoaiChungTu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_DM_Phong_IdPhong",
                        column: x => x.IdPhong,
                        principalTable: "DM_Phong",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingNhanVien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdBooking = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_BookingNhanVien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingNhanVien_Booking_IdBooking",
                        column: x => x.IdBooking,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingNhanVien_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DichVu_NhanVien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdDonViQuyDoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_DichVu_NhanVien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DichVu_NhanVien_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DichVu_NhanVien_DM_DonViQuiDoi_IdDonViQuyDoi",
                        column: x => x.IdDonViQuyDoi,
                        principalTable: "DM_DonViQuiDoi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DichVu_NhanVien_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeThong_SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNguoiGui = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdTinNhan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoTinGui = table.Column<int>(type: "int", nullable: false),
                    NoiDungTin = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ThoiGianGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiTin = table.Column<byte>(type: "tinyint", nullable: false),
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
                    table.PrimaryKey("PK_HeThong_SMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeThong_SMS_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeThong_SMS_DM_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "DM_KhachHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeThong_SMS_NS_NhanVien_IdNguoiGui",
                        column: x => x.IdNguoiGui,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_ChietKhauDichVu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDonViQuiDoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiChietKhau = table.Column<byte>(type: "tinyint", nullable: false),
                    GiaTri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LaPhanTram = table.Column<bool>(type: "bit", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_NS_ChietKhauDichVu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauDichVu_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauDichVu_DM_DonViQuiDoi_IdDonViQuiDoi",
                        column: x => x.IdDonViQuiDoi,
                        principalTable: "DM_DonViQuiDoi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauDichVu_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_ChietKhauHoaDon_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChietKhauHD = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_NS_ChietKhauHoaDon_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauHoaDon_ChiTiet_NS_ChietKhauHoaDon_IdChietKhauHD",
                        column: x => x.IdChietKhauHD,
                        principalTable: "NS_ChietKhauHoaDon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_ChietKhauHoaDon_ChiTiet_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_LichLamViec",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    LapLai = table.Column<bool>(type: "bit", nullable: false),
                    KieuLapLai = table.Column<int>(type: "int", nullable: false),
                    GiaTriLap = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_NS_LichLamViec", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_LichLamViec_DM_ChiNhanh_IdChiNhanh",
                        column: x => x.IdChiNhanh,
                        principalTable: "DM_ChiNhanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_LichLamViec_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_NhanVien_TimeOff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiNghi = table.Column<int>(type: "int", nullable: false),
                    TongNgayNghi = table.Column<double>(type: "float", nullable: false),
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
                    table.PrimaryKey("PK_NS_NhanVien_TimeOff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_NhanVien_TimeOff_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_QuaTrinh_CongTac",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPhongBan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiSua = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_NS_QuaTrinh_CongTac", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_QuaTrinh_CongTac_DM_PhongBan_IdPhongBan",
                        column: x => x.IdPhongBan,
                        principalTable: "DM_PhongBan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_QuaTrinh_CongTac_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BH_HoaDon_Anh",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    URLAnh = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_BH_HoaDon_Anh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_Anh_BH_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BH_HoaDon_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    STT = table.Column<int>(type: "int", nullable: false),
                    IdDonViQuyDoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiTietHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoLuong = table.Column<float>(type: "real", nullable: true),
                    DonGiaTruocCK = table.Column<float>(type: "real", nullable: true),
                    ThanhTienTruocCK = table.Column<float>(type: "real", nullable: true),
                    PTChietKhau = table.Column<float>(type: "real", nullable: true),
                    TienChietKhau = table.Column<float>(type: "real", nullable: true),
                    DonGiaSauCK = table.Column<float>(type: "real", nullable: true),
                    ThanhTienSauCK = table.Column<float>(type: "real", nullable: true),
                    PTThue = table.Column<float>(type: "real", nullable: true),
                    TienThue = table.Column<float>(type: "real", nullable: true),
                    DonGiaSauVAT = table.Column<float>(type: "real", nullable: true),
                    ThanhTienSauVAT = table.Column<float>(type: "real", nullable: true),
                    TonLuyKe = table.Column<float>(type: "real", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_BH_HoaDon_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_ChiTiet_BH_HoaDon_ChiTiet_IdChiTietHoaDon",
                        column: x => x.IdChiTietHoaDon,
                        principalTable: "BH_HoaDon_ChiTiet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_ChiTiet_BH_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BH_HoaDon_ChiTiet_DM_DonViQuiDoi_IdDonViQuyDoi",
                        column: x => x.IdDonViQuyDoi,
                        principalTable: "DM_DonViQuiDoi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuyHoaDon_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdQuyHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHoaDonLienQuan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdTaiKhoanNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdKhoanThuChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LaPTChiPhiNganHang = table.Column<float>(type: "real", nullable: true),
                    ChiPhiNganHang = table.Column<float>(type: "real", nullable: true),
                    ThuPhiTienGui = table.Column<float>(type: "real", nullable: true),
                    DiemThanhToan = table.Column<float>(type: "real", nullable: true),
                    HinhThucThanhToan = table.Column<byte>(type: "tinyint", nullable: false),
                    TienThu = table.Column<float>(type: "real", nullable: true),
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
                    table.PrimaryKey("PK_QuyHoaDon_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_BH_HoaDon_IdHoaDonLienQuan",
                        column: x => x.IdHoaDonLienQuan,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_DM_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "DM_KhachHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_DM_KhoanThuChi_IdKhoanThuChi",
                        column: x => x.IdKhoanThuChi,
                        principalTable: "DM_KhoanThuChi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_DM_TaiKhoanNganHang_IdTaiKhoanNganHang",
                        column: x => x.IdTaiKhoanNganHang,
                        principalTable: "DM_TaiKhoanNganHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuyHoaDon_ChiTiet_QuyHoaDon_IdQuyHoaDon",
                        column: x => x.IdQuyHoaDon,
                        principalTable: "QuyHoaDon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NS_LichLamViec_Ca",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdCaLamViec = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdLichLamViec = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GiaTri = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_NS_LichLamViec_Ca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NS_LichLamViec_Ca_NS_CaLamViec_IdCaLamViec",
                        column: x => x.IdCaLamViec,
                        principalTable: "NS_CaLamViec",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NS_LichLamViec_Ca_NS_LichLamViec_IdLichLamViec",
                        column: x => x.IdLichLamViec,
                        principalTable: "NS_LichLamViec",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BH_NhanVienThucHien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IdNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdHoaDonChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdQuyHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PTChietKhau = table.Column<float>(type: "real", nullable: true),
                    TienChietKhau = table.Column<float>(type: "real", nullable: true),
                    HeSo = table.Column<float>(type: "real", nullable: true),
                    ChiaDeuChietKhau = table.Column<bool>(type: "bit", nullable: true),
                    TinhHoaHongTruocCK = table.Column<bool>(type: "bit", nullable: true),
                    LoaiChietKhau = table.Column<byte>(type: "tinyint", nullable: true),
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
                    table.PrimaryKey("PK_BH_NhanVienThucHien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BH_NhanVienThucHien_BH_HoaDon_ChiTiet_IdHoaDonChiTiet",
                        column: x => x.IdHoaDonChiTiet,
                        principalTable: "BH_HoaDon_ChiTiet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_NhanVienThucHien_BH_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "BH_HoaDon",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BH_NhanVienThucHien_NS_NhanVien_IdNhanVien",
                        column: x => x.IdNhanVien,
                        principalTable: "NS_NhanVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BH_NhanVienThucHien_QuyHoaDon_IdQuyHoaDon",
                        column: x => x.IdQuyHoaDon,
                        principalTable: "QuyHoaDon",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "DM_LoaiHangHoa",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "MaLoaiHangHoa", "NguoiSua", "NguoiTao", "NguoiXoa", "TenLoaiHangHoa", "TenantId", "TrangThai" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7653), null, null, null, false, null, null, "HH", null, null, null, "Hàng Hóa", 0, 1 },
                    { 2, new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7677), null, null, null, false, null, null, "DV", null, null, null, "Dịch Vụ", 0, 1 },
                    { 3, new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7678), null, null, null, false, null, null, "CB", null, null, null, "Combo", 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "DM_LoaiKhach",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "MaLoaiKhachHang", "NguoiSua", "NguoiTao", "NguoiXoa", "TenLoaiKhachHang", "TenantId", "TrangThai" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7910), null, null, null, false, null, null, "KH", null, null, null, "Khách hàng", 0, 1 },
                    { 2, new DateTime(2023, 5, 19, 11, 22, 48, 632, DateTimeKind.Local).AddTicks(7914), null, null, null, false, null, null, "NCC", null, null, null, "Nhà cung cấp", 0, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionDuration",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionDuration" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_UserId",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicEntityProperties_DynamicPropertyId",
                table: "AbpDynamicEntityProperties",
                column: "DynamicPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicEntityProperties_EntityFullName_DynamicPropertyId_TenantId",
                table: "AbpDynamicEntityProperties",
                columns: new[] { "EntityFullName", "DynamicPropertyId", "TenantId" },
                unique: true,
                filter: "[EntityFullName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicEntityPropertyValues_DynamicEntityPropertyId",
                table: "AbpDynamicEntityPropertyValues",
                column: "DynamicEntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicProperties_PropertyName_TenantId",
                table: "AbpDynamicProperties",
                columns: new[] { "PropertyName", "TenantId" },
                unique: true,
                filter: "[PropertyName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicPropertyValues_DynamicPropertyId",
                table: "AbpDynamicPropertyValues",
                column: "DynamicPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_EntityChangeSetId",
                table: "AbpEntityChanges",
                column: "EntityChangeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_EntityTypeFullName_EntityId",
                table: "AbpEntityChanges",
                columns: new[] { "EntityTypeFullName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_CreationTime",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_Reason",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "Reason" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_UserId",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_EditionId_Name",
                table: "AbpFeatures",
                columns: new[] { "EditionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_TenantId_Name",
                table: "AbpFeatures",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLanguages_TenantId_Name",
                table: "AbpLanguages",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key",
                table: "AbpLanguageTexts",
                columns: new[] { "TenantId", "Source", "LanguageName", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName_EntityId_UserId",
                table: "AbpNotificationSubscriptions",
                columns: new[] { "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpNotificationSubscriptions_TenantId_NotificationName_EntityTypeName_EntityId_UserId",
                table: "AbpNotificationSubscriptions",
                columns: new[] { "TenantId", "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_TenantId_OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_TenantId_RoleId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ParentId",
                table: "AbpOrganizationUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits",
                columns: new[] { "TenantId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_RoleId",
                table: "AbpPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_TenantId_Name",
                table: "AbpPermissions",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_UserId",
                table: "AbpPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_RoleId",
                table: "AbpRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_TenantId_ClaimType",
                table: "AbpRoleClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_CreatorUserId",
                table: "AbpRoles",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_DeleterUserId",
                table: "AbpRoles",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_LastModifierUserId",
                table: "AbpRoles",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_TenantId_NormalizedName",
                table: "AbpRoles",
                columns: new[] { "TenantId", "NormalizedName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_TenantId_Name_UserId",
                table: "AbpSettings",
                columns: new[] { "TenantId", "Name", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_UserId",
                table: "AbpSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenantNotifications_TenantId",
                table: "AbpTenantNotifications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_CreatorUserId",
                table: "AbpTenants",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_DeleterUserId",
                table: "AbpTenants",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_EditionId",
                table: "AbpTenants",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_LastModifierUserId",
                table: "AbpTenants",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_TenancyName",
                table: "AbpTenants",
                column: "TenancyName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_EmailAddress",
                table: "AbpUserAccounts",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_EmailAddress",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "EmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_UserId",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_UserName",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "UserName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_UserName",
                table: "AbpUserAccounts",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_TenantId_ClaimType",
                table: "AbpUserClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_UserId",
                table: "AbpUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result",
                table: "AbpUserLoginAttempts",
                columns: new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLoginAttempts_UserId_TenantId",
                table: "AbpUserLoginAttempts",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_ProviderKey_TenantId",
                table: "AbpUserLogins",
                columns: new[] { "ProviderKey", "TenantId" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey",
                table: "AbpUserLogins",
                columns: new[] { "TenantId", "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_TenantId_UserId",
                table: "AbpUserLogins",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_UserId",
                table: "AbpUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserNotifications_UserId_State_CreationTime",
                table: "AbpUserNotifications",
                columns: new[] { "UserId", "State", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_TenantId_OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_TenantId_UserId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_TenantId_RoleId",
                table: "AbpUserRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_TenantId_UserId",
                table: "AbpUserRoles",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_UserId",
                table: "AbpUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_CreatorUserId",
                table: "AbpUsers",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_DeleterUserId",
                table: "AbpUsers",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_LastModifierUserId",
                table: "AbpUsers",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId_NormalizedEmailAddress",
                table: "AbpUsers",
                columns: new[] { "TenantId", "NormalizedEmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId_NormalizedUserName",
                table: "AbpUsers",
                columns: new[] { "TenantId", "NormalizedUserName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserTokens_TenantId_UserId",
                table: "AbpUserTokens",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserTokens_UserId",
                table: "AbpUserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpWebhookSendAttempts_WebhookEventId",
                table: "AbpWebhookSendAttempts",
                column: "WebhookEventId");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdChiNhanh",
                table: "BH_HoaDon",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdHoaDon",
                table: "BH_HoaDon",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdKhachHang",
                table: "BH_HoaDon",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdLoaiChungTu",
                table: "BH_HoaDon",
                column: "IdLoaiChungTu");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdNhanVien",
                table: "BH_HoaDon",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_IdPhong",
                table: "BH_HoaDon",
                column: "IdPhong");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_Anh_IdHoaDon",
                table: "BH_HoaDon_Anh",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_ChiTiet_IdChiTietHoaDon",
                table: "BH_HoaDon_ChiTiet",
                column: "IdChiTietHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_ChiTiet_IdDonViQuyDoi",
                table: "BH_HoaDon_ChiTiet",
                column: "IdDonViQuyDoi");

            migrationBuilder.CreateIndex(
                name: "IX_BH_HoaDon_ChiTiet_IdHoaDon",
                table: "BH_HoaDon_ChiTiet",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_NhanVienThucHien_IdHoaDon",
                table: "BH_NhanVienThucHien",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_BH_NhanVienThucHien_IdHoaDonChiTiet",
                table: "BH_NhanVienThucHien",
                column: "IdHoaDonChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_BH_NhanVienThucHien_IdNhanVien",
                table: "BH_NhanVienThucHien",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_BH_NhanVienThucHien_IdQuyHoaDon",
                table: "BH_NhanVienThucHien",
                column: "IdQuyHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_IdChiNhanh",
                table: "Booking",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_IdKhachHang",
                table: "Booking",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_BookingNhanVien_IdBooking",
                table: "BookingNhanVien",
                column: "IdBooking");

            migrationBuilder.CreateIndex(
                name: "IX_BookingNhanVien_IdNhanVien",
                table: "BookingNhanVien",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_BookingService_IdBooking",
                table: "BookingService",
                column: "IdBooking");

            migrationBuilder.CreateIndex(
                name: "IX_BookingService_IdDonViQuiDoi",
                table: "BookingService",
                column: "IdDonViQuiDoi");

            migrationBuilder.CreateIndex(
                name: "IX_DichVu_NhanVien_IdChiNhanh",
                table: "DichVu_NhanVien",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_DichVu_NhanVien_IdDonViQuyDoi",
                table: "DichVu_NhanVien",
                column: "IdDonViQuyDoi");

            migrationBuilder.CreateIndex(
                name: "IX_DichVu_NhanVien_IdNhanVien",
                table: "DichVu_NhanVien",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_DM_ChiNhanh_IdCongTy",
                table: "DM_ChiNhanh",
                column: "IdCongTy");

            migrationBuilder.CreateIndex(
                name: "IX_DM_DonViQuiDoi_IdHangHoa",
                table: "DM_DonViQuiDoi",
                column: "IdHangHoa");

            migrationBuilder.CreateIndex(
                name: "IX_DM_HangHoa_IdLoaiHangHoa",
                table: "DM_HangHoa",
                column: "IdLoaiHangHoa");

            migrationBuilder.CreateIndex(
                name: "IX_DM_HangHoa_IdNhomHangHoa",
                table: "DM_HangHoa",
                column: "IdNhomHangHoa");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdLoaiKhach",
                table: "DM_KhachHang",
                column: "IdLoaiKhach");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdNguonKhach",
                table: "DM_KhachHang",
                column: "IdNguonKhach");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhachHang_IdNhomKhach",
                table: "DM_KhachHang",
                column: "IdNhomKhach");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhuyenMai_ApDung_IdKhuyenMai",
                table: "DM_KhuyenMai_ApDung",
                column: "IdKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_DM_KhuyenMai_ChiTiet_IdKhuyenMai",
                table: "DM_KhuyenMai_ChiTiet",
                column: "IdKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_DM_MauIn_IdChiNhanh",
                table: "DM_MauIn",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_DM_MauIn_LoaiChungTu",
                table: "DM_MauIn",
                column: "LoaiChungTu");

            migrationBuilder.CreateIndex(
                name: "IX_DM_NhomKhach_DieuKien_IdNhomKhach",
                table: "DM_NhomKhach_DieuKien",
                column: "IdNhomKhach");

            migrationBuilder.CreateIndex(
                name: "IX_DM_Phong_IdViTri",
                table: "DM_Phong",
                column: "IdViTri");

            migrationBuilder.CreateIndex(
                name: "IX_DM_PhongBan_IdChiNhanh",
                table: "DM_PhongBan",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_DM_TaiKhoanNganHang_IdChiNhanh",
                table: "DM_TaiKhoanNganHang",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_DM_TaiKhoanNganHang_IdNganHang",
                table: "DM_TaiKhoanNganHang",
                column: "IdNganHang");

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdChiNhanh",
                table: "HeThong_SMS",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdKhachHang",
                table: "HeThong_SMS",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HeThong_SMS_IdNguoiGui",
                table: "HeThong_SMS",
                column: "IdNguoiGui");

            migrationBuilder.CreateIndex(
                name: "IX_HT_CauHinh_ChungTu_IdChiNhanh",
                table: "HT_CauHinh_ChungTu",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_HT_CauHinh_ChungTu_IdLoaiChungTu",
                table: "HT_CauHinh_ChungTu",
                column: "IdLoaiChungTu");

            migrationBuilder.CreateIndex(
                name: "IX_HT_CauHinh_TichDiem_HT_CauHinhPhanMemId",
                table: "HT_CauHinh_TichDiem",
                column: "HT_CauHinhPhanMemId");

            migrationBuilder.CreateIndex(
                name: "IX_HT_CauHinh_TichDiemChiTiet_IdTichDiem",
                table: "HT_CauHinh_TichDiemChiTiet",
                column: "IdTichDiem");

            migrationBuilder.CreateIndex(
                name: "IX_HT_NhatKyThaoTac_IdChiNhanh",
                table: "HT_NhatKyThaoTac",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_KH_CheckIn_IdChiNhanh",
                table: "KH_CheckIn",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_KH_CheckIn_IdKhachHang",
                table: "KH_CheckIn",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauDichVu_IdChiNhanh",
                table: "NS_ChietKhauDichVu",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauDichVu_IdDonViQuiDoi",
                table: "NS_ChietKhauDichVu",
                column: "IdDonViQuiDoi");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauDichVu_IdNhanVien",
                table: "NS_ChietKhauDichVu",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauHoaDon_DM_ChiNhanhId",
                table: "NS_ChietKhauHoaDon",
                column: "DM_ChiNhanhId");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauHoaDon_ChiTiet_IdChietKhauHD",
                table: "NS_ChietKhauHoaDon_ChiTiet",
                column: "IdChietKhauHD");

            migrationBuilder.CreateIndex(
                name: "IX_NS_ChietKhauHoaDon_ChiTiet_IdNhanVien",
                table: "NS_ChietKhauHoaDon_ChiTiet",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViec_IdChiNhanh",
                table: "NS_LichLamViec",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViec_IdNhanVien",
                table: "NS_LichLamViec",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViec_Ca_IdCaLamViec",
                table: "NS_LichLamViec_Ca",
                column: "IdCaLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_NS_LichLamViec_Ca_IdLichLamViec",
                table: "NS_LichLamViec_Ca",
                column: "IdLichLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_NS_NhanVien_IdChucVu",
                table: "NS_NhanVien",
                column: "IdChucVu");

            migrationBuilder.CreateIndex(
                name: "IX_NS_NhanVien_IdPhongBan",
                table: "NS_NhanVien",
                column: "IdPhongBan");

            migrationBuilder.CreateIndex(
                name: "IX_NS_NhanVien_TimeOff_IdNhanVien",
                table: "NS_NhanVien_TimeOff",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NS_QuaTrinh_CongTac_IdNhanVien",
                table: "NS_QuaTrinh_CongTac",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NS_QuaTrinh_CongTac_IdPhongBan",
                table: "NS_QuaTrinh_CongTac",
                column: "IdPhongBan");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_IdChiNhanh",
                table: "QuyHoaDon",
                column: "IdChiNhanh");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_IdLoaiChungTu",
                table: "QuyHoaDon",
                column: "IdLoaiChungTu");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdHoaDonLienQuan",
                table: "QuyHoaDon_ChiTiet",
                column: "IdHoaDonLienQuan");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdKhachHang",
                table: "QuyHoaDon_ChiTiet",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdKhoanThuChi",
                table: "QuyHoaDon_ChiTiet",
                column: "IdKhoanThuChi");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdNhanVien",
                table: "QuyHoaDon_ChiTiet",
                column: "IdNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdQuyHoaDon",
                table: "QuyHoaDon_ChiTiet",
                column: "IdQuyHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_QuyHoaDon_ChiTiet_IdTaiKhoanNganHang",
                table: "QuyHoaDon_ChiTiet",
                column: "IdTaiKhoanNganHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuditLogs");

            migrationBuilder.DropTable(
                name: "AbpBackgroundJobs");

            migrationBuilder.DropTable(
                name: "AbpDynamicEntityPropertyValues");

            migrationBuilder.DropTable(
                name: "AbpDynamicPropertyValues");

            migrationBuilder.DropTable(
                name: "AbpEntityPropertyChanges");

            migrationBuilder.DropTable(
                name: "AbpFeatures");

            migrationBuilder.DropTable(
                name: "AbpLanguages");

            migrationBuilder.DropTable(
                name: "AbpLanguageTexts");

            migrationBuilder.DropTable(
                name: "AbpNotifications");

            migrationBuilder.DropTable(
                name: "AbpNotificationSubscriptions");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnitRoles");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpPermissions");

            migrationBuilder.DropTable(
                name: "AbpRoleClaims");

            migrationBuilder.DropTable(
                name: "AbpSettings");

            migrationBuilder.DropTable(
                name: "AbpTenantNotifications");

            migrationBuilder.DropTable(
                name: "AbpTenants");

            migrationBuilder.DropTable(
                name: "AbpUserAccounts");

            migrationBuilder.DropTable(
                name: "AbpUserClaims");

            migrationBuilder.DropTable(
                name: "AbpUserLoginAttempts");

            migrationBuilder.DropTable(
                name: "AbpUserLogins");

            migrationBuilder.DropTable(
                name: "AbpUserNotifications");

            migrationBuilder.DropTable(
                name: "AbpUserOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpUserRoles");

            migrationBuilder.DropTable(
                name: "AbpUserTokens");

            migrationBuilder.DropTable(
                name: "AbpWebhookSendAttempts");

            migrationBuilder.DropTable(
                name: "AbpWebhookSubscriptions");

            migrationBuilder.DropTable(
                name: "BH_HoaDon_Anh");

            migrationBuilder.DropTable(
                name: "BH_NhanVienThucHien");

            migrationBuilder.DropTable(
                name: "Booking_Color");

            migrationBuilder.DropTable(
                name: "BookingNhanVien");

            migrationBuilder.DropTable(
                name: "BookingService");

            migrationBuilder.DropTable(
                name: "DichVu_NhanVien");

            migrationBuilder.DropTable(
                name: "DM_KhuyenMai_ApDung");

            migrationBuilder.DropTable(
                name: "DM_KhuyenMai_ChiTiet");

            migrationBuilder.DropTable(
                name: "DM_MauIn");

            migrationBuilder.DropTable(
                name: "DM_NgayNghiLe");

            migrationBuilder.DropTable(
                name: "DM_NhomKhach_DieuKien");

            migrationBuilder.DropTable(
                name: "HeThong_SMS");

            migrationBuilder.DropTable(
                name: "HT_CauHinh_ChungTu");

            migrationBuilder.DropTable(
                name: "HT_CauHinh_TichDiemChiTiet");

            migrationBuilder.DropTable(
                name: "HT_NhatKyThaoTac");

            migrationBuilder.DropTable(
                name: "KH_CheckIn");

            migrationBuilder.DropTable(
                name: "NS_ChietKhauDichVu");

            migrationBuilder.DropTable(
                name: "NS_ChietKhauHoaDon_ChiTiet");

            migrationBuilder.DropTable(
                name: "NS_LichLamViec_Ca");

            migrationBuilder.DropTable(
                name: "NS_NhanVien_TimeOff");

            migrationBuilder.DropTable(
                name: "NS_QuaTrinh_CongTac");

            migrationBuilder.DropTable(
                name: "QuyHoaDon_ChiTiet");

            migrationBuilder.DropTable(
                name: "AbpDynamicEntityProperties");

            migrationBuilder.DropTable(
                name: "AbpEntityChanges");

            migrationBuilder.DropTable(
                name: "AbpRoles");

            migrationBuilder.DropTable(
                name: "AbpEditions");

            migrationBuilder.DropTable(
                name: "AbpWebhookEvents");

            migrationBuilder.DropTable(
                name: "BH_HoaDon_ChiTiet");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "DM_KhuyenMai");

            migrationBuilder.DropTable(
                name: "HT_CauHinh_TichDiem");

            migrationBuilder.DropTable(
                name: "NS_ChietKhauHoaDon");

            migrationBuilder.DropTable(
                name: "NS_CaLamViec");

            migrationBuilder.DropTable(
                name: "NS_LichLamViec");

            migrationBuilder.DropTable(
                name: "DM_KhoanThuChi");

            migrationBuilder.DropTable(
                name: "DM_TaiKhoanNganHang");

            migrationBuilder.DropTable(
                name: "QuyHoaDon");

            migrationBuilder.DropTable(
                name: "AbpDynamicProperties");

            migrationBuilder.DropTable(
                name: "AbpEntityChangeSets");

            migrationBuilder.DropTable(
                name: "AbpUsers");

            migrationBuilder.DropTable(
                name: "BH_HoaDon");

            migrationBuilder.DropTable(
                name: "DM_DonViQuiDoi");

            migrationBuilder.DropTable(
                name: "HT_CauHinhPhanMem");

            migrationBuilder.DropTable(
                name: "DM_NganHang");

            migrationBuilder.DropTable(
                name: "DM_KhachHang");

            migrationBuilder.DropTable(
                name: "DM_LoaiChungTu");

            migrationBuilder.DropTable(
                name: "DM_Phong");

            migrationBuilder.DropTable(
                name: "NS_NhanVien");

            migrationBuilder.DropTable(
                name: "DM_HangHoa");

            migrationBuilder.DropTable(
                name: "DM_LoaiKhach");

            migrationBuilder.DropTable(
                name: "DM_NguonKhach");

            migrationBuilder.DropTable(
                name: "DM_NhomKhachHang");

            migrationBuilder.DropTable(
                name: "DM_ViTriPhong");

            migrationBuilder.DropTable(
                name: "DM_PhongBan");

            migrationBuilder.DropTable(
                name: "NS_ChucVu");

            migrationBuilder.DropTable(
                name: "DM_LoaiHangHoa");

            migrationBuilder.DropTable(
                name: "DM_NhomHangHoa");

            migrationBuilder.DropTable(
                name: "DM_ChiNhanh");

            migrationBuilder.DropTable(
                name: "HT_CongTy");
        }
    }
}
