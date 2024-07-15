using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdentityFromFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodV2",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodV2", x => x.Id);
                });

           
            migrationBuilder.CreateTable(
                name: "FoodV2Servings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FoodId = table.Column<long>(type: "bigint", nullable: false),
                    AddedSugar = table.Column<float>(type: "real", nullable: true),
                    Calcium = table.Column<float>(type: "real", nullable: true),
                    Calories = table.Column<float>(type: "real", nullable: true),
                    Carbohydrate = table.Column<float>(type: "real", nullable: true),
                    Cholesterol = table.Column<float>(type: "real", nullable: true),
                    Fat = table.Column<float>(type: "real", nullable: true),
                    Fiber = table.Column<float>(type: "real", nullable: true),
                    Iron = table.Column<float>(type: "real", nullable: true),
                    MeasurementDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetricServingAmount = table.Column<float>(type: "real", nullable: true),
                    MetricServingUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonounsaturatedFat = table.Column<float>(type: "real", nullable: true),
                    NumberOfUnits = table.Column<float>(type: "real", nullable: true),
                    PolyunsaturatedFat = table.Column<float>(type: "real", nullable: true),
                    Potassium = table.Column<float>(type: "real", nullable: true),
                    Protein = table.Column<float>(type: "real", nullable: true),
                    SaturatedFat = table.Column<float>(type: "real", nullable: true),
                    ServingDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sodium = table.Column<float>(type: "real", nullable: true),
                    Sugar = table.Column<float>(type: "real", nullable: true),
                    TransFat = table.Column<float>(type: "real", nullable: true),
                    VitaminA = table.Column<float>(type: "real", nullable: true),
                    VitaminC = table.Column<float>(type: "real", nullable: true),
                    VitaminD = table.Column<float>(type: "real", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodV2Servings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodV2Servings_FoodV2_FoodId",
                        column: x => x.FoodId,
                        principalTable: "FoodV2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.CreateTable(
                name: "UserFoodV2",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FoodId = table.Column<long>(type: "bigint", nullable: false),
                    ServingId = table.Column<long>(type: "bigint", nullable: false),
                    ServingAmount = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFoodV2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFoodV2_FoodV2Servings_ServingId",
                        column: x => x.ServingId,
                        principalTable: "FoodV2Servings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFoodV2_FoodV2_FoodId",
                        column: x => x.FoodId,
                        principalTable: "FoodV2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            
            migrationBuilder.CreateIndex(
                name: "IX_FoodV2Servings_FoodId",
                table: "FoodV2Servings",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFoodV2_FoodId",
                table: "UserFoodV2",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFoodV2_ServingId",
                table: "UserFoodV2",
                column: "ServingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgressPhoto");

            migrationBuilder.DropTable(
                name: "UserBloodPressure");

            migrationBuilder.DropTable(
                name: "UserBody");

            migrationBuilder.DropTable(
                name: "UserCustomMacros");

            migrationBuilder.DropTable(
                name: "UserFoodV2");

            migrationBuilder.DropTable(
                name: "UserHeight");

            migrationBuilder.DropTable(
                name: "UserOneRepMaxEstimates");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserSupplementActivity");

            migrationBuilder.DropTable(
                name: "UserWeight");

            migrationBuilder.DropTable(
                name: "UserWorkout");

            migrationBuilder.DropTable(
                name: "UserWorkoutActivity");

            migrationBuilder.DropTable(
                name: "UserWorkoutsCompleted");

            migrationBuilder.DropTable(
                name: "UserWorkoutSubstitution");

            migrationBuilder.DropTable(
                name: "WorkoutExercise");

            migrationBuilder.DropTable(
                name: "FoodV2Servings");

            migrationBuilder.DropTable(
                name: "UserSupplements");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "Workout");

            migrationBuilder.DropTable(
                name: "FoodV2");

            migrationBuilder.DropTable(
                name: "Supplements");
        }
    }
}
