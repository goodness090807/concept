using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concept.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveResourceEntityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceAuthorizations_Resources_ResourceEntityId",
                table: "ResourceAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_ResourceAuthorizations_ResourceEntityId",
                table: "ResourceAuthorizations");

            migrationBuilder.DropColumn(
                name: "ResourceEntityId",
                table: "ResourceAuthorizations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResourceEntityId",
                table: "ResourceAuthorizations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAuthorizations_ResourceEntityId",
                table: "ResourceAuthorizations",
                column: "ResourceEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceAuthorizations_Resources_ResourceEntityId",
                table: "ResourceAuthorizations",
                column: "ResourceEntityId",
                principalTable: "Resources",
                principalColumn: "Id");
        }
    }
}
