using Microsoft.EntityFrameworkCore.Migrations;

namespace training_api.Data.Migrations
{
    public partial class AddedPostAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdAuthor",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IdAuthor",
                table: "Posts",
                column: "IdAuthor");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_IdAuthor",
                table: "Posts",
                column: "IdAuthor",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_IdAuthor",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_IdAuthor",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IdAuthor",
                table: "Posts");
        }
    }
}
