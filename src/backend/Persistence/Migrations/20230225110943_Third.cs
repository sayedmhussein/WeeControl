using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace WeeControl.ApiApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LogTs",
                table: "UserSessionLogDbo",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 279, DateTimeKind.Utc).AddTicks(3261))
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTs",
                table: "UserSessionDbo",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 276, DateTimeKind.Utc).AddTicks(9928))
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedTs",
                table: "UserNotificationDbo",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 275, DateTimeKind.Utc).AddTicks(5325))
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FeedTs",
                table: "UserFeedsDbo",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 271, DateTimeKind.Utc).AddTicks(3268))
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "FeedSubject",
                table: "UserFeedsDbo",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "FeedBody",
                table: "UserFeedsDbo",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GrantedTs",
                table: "UserClaimDbo",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 263, DateTimeKind.Utc).AddTicks(6304))
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LogTs",
                table: "UserSessionLogDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 279, DateTimeKind.Utc).AddTicks(3261),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTs",
                table: "UserSessionDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 276, DateTimeKind.Utc).AddTicks(9928),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedTs",
                table: "UserNotificationDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 275, DateTimeKind.Utc).AddTicks(5325),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FeedTs",
                table: "UserFeedsDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 271, DateTimeKind.Utc).AddTicks(3268),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "FeedSubject",
                table: "UserFeedsDbo",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55);

            migrationBuilder.AlterColumn<string>(
                name: "FeedBody",
                table: "UserFeedsDbo",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GrantedTs",
                table: "UserClaimDbo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 25, 10, 44, 55, 263, DateTimeKind.Utc).AddTicks(6304),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);
        }
    }
}
