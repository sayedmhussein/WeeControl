using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeeControl.ApiApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CustomerAltered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerDboPersonDbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDboPersonDbo_PersonsPersonId",
                table: "CustomerDboPersonDbo",
                column: "PersonsPersonId");
        }
    }
}
