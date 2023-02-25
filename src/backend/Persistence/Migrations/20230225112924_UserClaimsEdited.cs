using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeeControl.ApiApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserClaimsEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaimDbo_EmployeeDbo_GrantedById",
                table: "UserClaimDbo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaimDbo_EmployeeDbo_RevokedById",
                table: "UserClaimDbo");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaimDbo_UserDbo_GrantedById",
                table: "UserClaimDbo",
                column: "GrantedById",
                principalTable: "UserDbo",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaimDbo_UserDbo_RevokedById",
                table: "UserClaimDbo",
                column: "RevokedById",
                principalTable: "UserDbo",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaimDbo_UserDbo_GrantedById",
                table: "UserClaimDbo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaimDbo_UserDbo_RevokedById",
                table: "UserClaimDbo");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaimDbo_EmployeeDbo_GrantedById",
                table: "UserClaimDbo",
                column: "GrantedById",
                principalTable: "EmployeeDbo",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaimDbo_EmployeeDbo_RevokedById",
                table: "UserClaimDbo",
                column: "RevokedById",
                principalTable: "EmployeeDbo",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
