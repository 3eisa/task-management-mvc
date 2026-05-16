using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByAppUserId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_CreatedByAppUserId",
                table: "TaskItems",
                column: "CreatedByAppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_AppUsers_CreatedByAppUserId",
                table: "TaskItems",
                column: "CreatedByAppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_AppUsers_CreatedByAppUserId",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_CreatedByAppUserId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "CreatedByAppUserId",
                table: "TaskItems");
        }
    }
}
