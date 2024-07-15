using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApi.Migrations
{
    /// <inheritdoc />
    public partial class UserFoodDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFoodV2_FoodV2Servings_ServingId",
                table: "UserFoodV2");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFoodV2_FoodV2Servings_ServingId",
                table: "UserFoodV2",
                column: "ServingId",
                principalTable: "FoodV2Servings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFoodV2_FoodV2Servings_ServingId",
                table: "UserFoodV2");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFoodV2_FoodV2Servings_ServingId",
                table: "UserFoodV2",
                column: "ServingId",
                principalTable: "FoodV2Servings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
