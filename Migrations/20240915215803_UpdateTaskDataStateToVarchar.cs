using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaskDataStateToVarchar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaskDataState",
                table: "TaskItems",
                type: "varchar(24)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaskDataState",
                table: "TaskItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(24)");
        }
    }
}
