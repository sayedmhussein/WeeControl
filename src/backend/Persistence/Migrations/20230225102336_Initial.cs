using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeeControl.ApiApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BuildingDbo",
                columns: table => new
                {
                    BuildingId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CountryId = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    BuildingAddress = table.Column<string>(type: "longtext", nullable: true),
                    BuildingName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Latitude = table.Column<double>(type: "double", maxLength: 90, nullable: true),
                    Longitude = table.Column<double>(type: "double", maxLength: 180, nullable: true),
                    BuildingType = table.Column<int>(type: "int", nullable: false),
                    OfficeId = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingDbo", x => x.BuildingId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PersonDbo",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    SecondName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    ThirdName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    LastName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    NationalityCode = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonDbo", x => x.PersonId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserFeedsDbo",
                columns: table => new
                {
                    FeedId = table.Column<Guid>(type: "char(36)", nullable: false),
                    HideTs = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FeedSubject = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    FeedBody = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    FeedUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    FeedTs = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 68, DateTimeKind.Utc).AddTicks(8468))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeedsDbo", x => x.FeedId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitDbo",
                columns: table => new
                {
                    UnitNumber = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    UnitBrand = table.Column<int>(type: "int", nullable: false),
                    UnitState = table.Column<int>(type: "int", nullable: false),
                    BuildingId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UnitType = table.Column<int>(type: "int", nullable: false),
                    UnitIdentification = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitDbo", x => x.UnitNumber);
                    table.ForeignKey(
                        name: "FK_UnitDbo_BuildingDbo_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "BuildingDbo",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AddressDbo",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PersonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Line1 = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Line2 = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    ZipCode = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    City = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    CountryCode = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    Latitude = table.Column<double>(type: "double", nullable: true),
                    Longitude = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressDbo", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_AddressDbo_PersonDbo_PersonId",
                        column: x => x.PersonId,
                        principalTable: "PersonDbo",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmployeeDbo",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SupervisorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    PersonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmployeeNo = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDbo", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_EmployeeDbo_EmployeeDbo_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "EmployeeDbo",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeDbo_PersonDbo_PersonId",
                        column: x => x.PersonId,
                        principalTable: "PersonDbo",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PersonContactDbo",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PersonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ContactType = table.Column<string>(type: "longtext", nullable: false),
                    ContactValue = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonContactDbo", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_PersonContactDbo_PersonDbo_PersonId",
                        column: x => x.PersonId,
                        principalTable: "PersonDbo",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PersonIdentityDbo",
                columns: table => new
                {
                    IdentityId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PersonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Number = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CountryId = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonIdentityDbo", x => x.IdentityId);
                    table.ForeignKey(
                        name: "FK_PersonIdentityDbo_PersonDbo_PersonId",
                        column: x => x.PersonId,
                        principalTable: "PersonDbo",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserDbo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PersonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SuspendArgs = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    TempPassword = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    TempPasswordTs = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    MobileNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Username = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDbo", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserDbo_PersonDbo_PersonId",
                        column: x => x.PersonId,
                        principalTable: "PersonDbo",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomerDbo",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CustomerName = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    CustomerLocalName = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    CountryCode = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    CustomerAddress = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    InvoiceAddress = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDbo", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_CustomerDbo_UserDbo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDbo",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserClaimDbo",
                columns: table => new
                {
                    ClaimId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ClaimType = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    ClaimValue = table.Column<string>(type: "varchar(255)", nullable: true),
                    GrantedTs = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 64, DateTimeKind.Utc).AddTicks(1160)),
                    GrantedById = table.Column<Guid>(type: "char(36)", nullable: false),
                    RevokedTs = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RevokedById = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaimDbo", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_UserClaimDbo_EmployeeDbo_GrantedById",
                        column: x => x.GrantedById,
                        principalTable: "EmployeeDbo",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserClaimDbo_EmployeeDbo_RevokedById",
                        column: x => x.RevokedById,
                        principalTable: "EmployeeDbo",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserClaimDbo_UserDbo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDbo",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserNotificationDbo",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserDboUserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Subject = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Body = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    NotificationUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    PublishedTs = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 71, DateTimeKind.Utc).AddTicks(2658)),
                    ReadTs = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotificationDbo", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_UserNotificationDbo_UserDbo_UserDboUserId",
                        column: x => x.UserDboUserId,
                        principalTable: "UserDbo",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserNotificationDbo_UserDbo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDbo",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserSessionDbo",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    DeviceId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    OneTimePassword = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true),
                    UserDboUserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedTs = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 72, DateTimeKind.Utc).AddTicks(1441)),
                    TerminationTs = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessionDbo", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_UserSessionDbo_UserDbo_UserDboUserId",
                        column: x => x.UserDboUserId,
                        principalTable: "UserDbo",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserSessionDbo_UserDbo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDbo",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomerDboPersonDbo",
                columns: table => new
                {
                    CustomerDboCustomerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PersonsPersonId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDboPersonDbo", x => new { x.CustomerDboCustomerId, x.PersonsPersonId });
                    table.ForeignKey(
                        name: "FK_CustomerDboPersonDbo_CustomerDbo_CustomerDboCustomerId",
                        column: x => x.CustomerDboCustomerId,
                        principalTable: "CustomerDbo",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerDboPersonDbo_PersonDbo_PersonsPersonId",
                        column: x => x.PersonsPersonId,
                        principalTable: "PersonDbo",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserSessionLogDbo",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SessionId = table.Column<Guid>(type: "char(36)", nullable: false),
                    LogTs = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2023, 2, 25, 10, 23, 36, 73, DateTimeKind.Utc).AddTicks(6147)),
                    Context = table.Column<string>(type: "longtext", nullable: true),
                    Details = table.Column<string>(type: "longtext", nullable: true),
                    UserSessionDboSessionId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessionLogDbo", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_UserSessionLogDbo_UserSessionDbo_SessionId",
                        column: x => x.SessionId,
                        principalTable: "UserSessionDbo",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSessionLogDbo_UserSessionDbo_UserSessionDboSessionId",
                        column: x => x.UserSessionDboSessionId,
                        principalTable: "UserSessionDbo",
                        principalColumn: "SessionId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AddressDbo_PersonId",
                table: "AddressDbo",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingDbo_CountryId_BuildingName",
                table: "BuildingDbo",
                columns: new[] { "CountryId", "BuildingName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDbo_UserId",
                table: "CustomerDbo",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDboPersonDbo_PersonsPersonId",
                table: "CustomerDboPersonDbo",
                column: "PersonsPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDbo_EmployeeId",
                table: "EmployeeDbo",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDbo_PersonId",
                table: "EmployeeDbo",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDbo_SupervisorId",
                table: "EmployeeDbo",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonContactDbo_PersonId",
                table: "PersonContactDbo",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonIdentityDbo_PersonId",
                table: "PersonIdentityDbo",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitDbo_BuildingId",
                table: "UnitDbo",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaimDbo_ClaimType_ClaimValue",
                table: "UserClaimDbo",
                columns: new[] { "ClaimType", "ClaimValue" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaimDbo_GrantedById",
                table: "UserClaimDbo",
                column: "GrantedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaimDbo_RevokedById",
                table: "UserClaimDbo",
                column: "RevokedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaimDbo_UserId",
                table: "UserClaimDbo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDbo_Email",
                table: "UserDbo",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDbo_PersonId",
                table: "UserDbo",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDbo_Username",
                table: "UserDbo",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDbo_Username_Password",
                table: "UserDbo",
                columns: new[] { "Username", "Password" });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationDbo_UserDboUserId",
                table: "UserNotificationDbo",
                column: "UserDboUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationDbo_UserId",
                table: "UserNotificationDbo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessionDbo_UserDboUserId",
                table: "UserSessionDbo",
                column: "UserDboUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessionDbo_UserId",
                table: "UserSessionDbo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessionLogDbo_SessionId",
                table: "UserSessionLogDbo",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessionLogDbo_UserSessionDboSessionId",
                table: "UserSessionLogDbo",
                column: "UserSessionDboSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressDbo");

            migrationBuilder.DropTable(
                name: "CustomerDboPersonDbo");

            migrationBuilder.DropTable(
                name: "PersonContactDbo");

            migrationBuilder.DropTable(
                name: "PersonIdentityDbo");

            migrationBuilder.DropTable(
                name: "UnitDbo");

            migrationBuilder.DropTable(
                name: "UserClaimDbo");

            migrationBuilder.DropTable(
                name: "UserFeedsDbo");

            migrationBuilder.DropTable(
                name: "UserNotificationDbo");

            migrationBuilder.DropTable(
                name: "UserSessionLogDbo");

            migrationBuilder.DropTable(
                name: "CustomerDbo");

            migrationBuilder.DropTable(
                name: "BuildingDbo");

            migrationBuilder.DropTable(
                name: "EmployeeDbo");

            migrationBuilder.DropTable(
                name: "UserSessionDbo");

            migrationBuilder.DropTable(
                name: "UserDbo");

            migrationBuilder.DropTable(
                name: "PersonDbo");
        }
    }
}
