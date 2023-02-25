using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeeControl.ApiApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LogTs",
                table: "UserSessionLogDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 279, DateTimeKind.Utc).AddTicks(3261),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 73, DateTimeKind.Utc).AddTicks(6147));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTs",
                table: "UserSessionDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 276, DateTimeKind.Utc).AddTicks(9928),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 72, DateTimeKind.Utc).AddTicks(1441));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedTs",
                table: "UserNotificationDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 275, DateTimeKind.Utc).AddTicks(5325),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 71, DateTimeKind.Utc).AddTicks(2658));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FeedTs",
                table: "UserFeedsDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 271, DateTimeKind.Utc).AddTicks(3268),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 68, DateTimeKind.Utc).AddTicks(8468));

            migrationBuilder.AlterColumn<DateTime>(
                name: "GrantedTs",
                table: "UserClaimDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 263, DateTimeKind.Utc).AddTicks(6304),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 64, DateTimeKind.Utc).AddTicks(1160));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LogTs",
                table: "UserSessionLogDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 73, DateTimeKind.Utc).AddTicks(6147),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 279, DateTimeKind.Utc).AddTicks(3261));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTs",
                table: "UserSessionDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 72, DateTimeKind.Utc).AddTicks(1441),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 276, DateTimeKind.Utc).AddTicks(9928));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedTs",
                table: "UserNotificationDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 71, DateTimeKind.Utc).AddTicks(2658),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 275, DateTimeKind.Utc).AddTicks(5325));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FeedTs",
                table: "UserFeedsDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 68, DateTimeKind.Utc).AddTicks(8468),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 271, DateTimeKind.Utc).AddTicks(3268));

            migrationBuilder.AlterColumn<DateTime>(
                name: "GrantedTs",
                table: "UserClaimDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 64, DateTimeKind.Utc).AddTicks(1160),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 263, DateTimeKind.Utc).AddTicks(6304));
        }
    }
}
