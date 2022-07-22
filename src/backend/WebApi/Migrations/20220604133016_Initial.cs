using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeeControl.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Essential");

            migrationBuilder.CreateTable(
                name: "TerritoryDbo",
                schema: "Essential",
                columns: table => new
                {
                    TerritoryId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ReportToId = table.Column<string>(type: "character varying(10)", nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    TerritoryName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerritoryDbo", x => x.TerritoryId);
                    table.ForeignKey(
                        name: "FK_TerritoryDbo_TerritoryDbo_ReportToId",
                        column: x => x.ReportToId,
                        principalSchema: "Essential",
                        principalTable: "TerritoryDbo",
                        principalColumn: "TerritoryId",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Territory of corporate.");

            migrationBuilder.CreateTable(
                name: "UserDbo",
                schema: "Essential",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    Password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    MobileNo = table.Column<string>(type: "text", nullable: true),
                    TerritoryId = table.Column<string>(type: "character varying(10)", nullable: true),
                    Nationality = table.Column<string>(type: "text", nullable: true),
                    SuspendArgs = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TempPassword = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    TempPasswordTs = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDbo", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserDbo_TerritoryDbo_TerritoryId",
                        column: x => x.TerritoryId,
                        principalSchema: "Essential",
                        principalTable: "TerritoryDbo",
                        principalColumn: "TerritoryId");
                });

            migrationBuilder.CreateTable(
                name: "ClaimDbo",
                schema: "Essential",
                columns: table => new
                {
                    ClaimId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClaimType = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    GrantedTs = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2022, 6, 4, 13, 30, 16, 563, DateTimeKind.Utc).AddTicks(9280)),
                    GrantedById = table.Column<Guid>(type: "uuid", nullable: false),
                    RevokedTs = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedById = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimDbo", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_ClaimDbo_UserDbo_GrantedById",
                        column: x => x.GrantedById,
                        principalSchema: "Essential",
                        principalTable: "UserDbo",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimDbo_UserDbo_RevokedById",
                        column: x => x.RevokedById,
                        principalSchema: "Essential",
                        principalTable: "UserDbo",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_ClaimDbo_UserDbo_UserId",
                        column: x => x.UserId,
                        principalSchema: "Essential",
                        principalTable: "UserDbo",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "SessionDbo",
                schema: "Essential",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
                    CreatedTs = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2022, 6, 4, 13, 30, 16, 564, DateTimeKind.Utc).AddTicks(420)),
                    TerminationTs = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionDbo", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_SessionDbo_UserDbo_UserId",
                        column: x => x.UserId,
                        principalSchema: "Essential",
                        principalTable: "UserDbo",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserIdentities",
                columns: table => new
                {
                    IdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    UserDboUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentities", x => x.IdentityId);
                    table.ForeignKey(
                        name: "FK_UserIdentities_UserDbo_UserDboUserId",
                        column: x => x.UserDboUserId,
                        principalSchema: "Essential",
                        principalTable: "UserDbo",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "SessionLogDbo",
                schema: "Essential",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    LogTs = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2022, 6, 4, 13, 30, 16, 564, DateTimeKind.Utc).AddTicks(1090)),
                    Context = table.Column<string>(type: "text", nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionLogDbo", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_SessionLogDbo_SessionDbo_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "Essential",
                        principalTable: "SessionDbo",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDbo_ClaimType_ClaimValue",
                schema: "Essential",
                table: "ClaimDbo",
                columns: new[] { "ClaimType", "ClaimValue" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDbo_GrantedById",
                schema: "Essential",
                table: "ClaimDbo",
                column: "GrantedById");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDbo_RevokedById",
                schema: "Essential",
                table: "ClaimDbo",
                column: "RevokedById");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDbo_UserId",
                schema: "Essential",
                table: "ClaimDbo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionDbo_UserId",
                schema: "Essential",
                table: "SessionDbo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionLogDbo_SessionId",
                schema: "Essential",
                table: "SessionLogDbo",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryDbo_CountryCode_TerritoryName",
                schema: "Essential",
                table: "TerritoryDbo",
                columns: new[] { "CountryCode", "TerritoryName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryDbo_ReportToId",
                schema: "Essential",
                table: "TerritoryDbo",
                column: "ReportToId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDbo_TerritoryId",
                schema: "Essential",
                table: "UserDbo",
                column: "TerritoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentities_UserDboUserId",
                table: "UserIdentities",
                column: "UserDboUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimDbo",
                schema: "Essential");

            migrationBuilder.DropTable(
                name: "SessionLogDbo",
                schema: "Essential");

            migrationBuilder.DropTable(
                name: "UserIdentities");

            migrationBuilder.DropTable(
                name: "SessionDbo",
                schema: "Essential");

            migrationBuilder.DropTable(
                name: "UserDbo",
                schema: "Essential");

            migrationBuilder.DropTable(
                name: "TerritoryDbo",
                schema: "Essential");
        }
    }
}
