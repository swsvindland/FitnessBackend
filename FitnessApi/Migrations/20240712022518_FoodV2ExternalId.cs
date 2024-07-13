using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApi.Migrations
{
    /// <inheritdoc />
    public partial class FoodV2ExternalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ExternalId",
                table: "FoodV2",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "FoodV2");
        }
    }
}
