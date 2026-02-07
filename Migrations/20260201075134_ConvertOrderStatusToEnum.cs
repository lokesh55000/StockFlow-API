using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SrockFlow_API.Migrations
{
    /// <inheritdoc />
    public partial class ConvertOrderStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add temp column
            migrationBuilder.AddColumn<int>(
                name: "StatusTemp",
                table: "Orders",
                nullable: false,
                defaultValue: 1);

            // Convert existing string values to enum values
            migrationBuilder.Sql(
                "UPDATE Orders SET StatusTemp = 1 WHERE Status = 'Placed'");
            migrationBuilder.Sql(
                "UPDATE Orders SET StatusTemp = 2 WHERE Status = 'Cancelled'");

            // Drop old column
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            // Rename temp column
            migrationBuilder.RenameColumn(
                name: "StatusTemp",
                table: "Orders",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
