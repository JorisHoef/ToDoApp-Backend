using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoAppBackend.Migrations
{
    public partial class SolveConstraintsForEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Alter column to change enum representation from int to string
            migrationBuilder.AlterColumn<string>(
                name: "TaskDataState",
                table: "TaskItems",
                type: "VARCHAR(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            // Alter column for ParentTaskId to ensure it's nullable if required
            migrationBuilder.AlterColumn<long?>(
                name: "ParentTaskId",
                table: "TaskItems",
                type: "BIGINT",
                nullable: true, // Ensure this matches your model
                oldClrType: typeof(long),
                oldType: "BIGINT");

            // Ensure TaskItemId or any other columns match the model definitions
            migrationBuilder.AlterColumn<long?>(
                name: "TaskItemId",
                table: "TaskItems",
                type: "BIGINT",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "BIGINT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the TaskDataState column back to int
            migrationBuilder.AlterColumn<int>(
                name: "TaskDataState",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)");

            // Revert ParentTaskId to non-nullable if required
            migrationBuilder.AlterColumn<long>(
                name: "ParentTaskId",
                table: "TaskItems",
                type: "BIGINT",
                nullable: false,
                oldClrType: typeof(long?),
                oldType: "BIGINT");

            // Revert TaskItemId to non-nullable if required
            migrationBuilder.AlterColumn<long>(
                name: "TaskItemId",
                table: "TaskItems",
                type: "BIGINT",
                nullable: false,
                oldClrType: typeof(long?),
                oldType: "BIGINT");
        }
    }
}
