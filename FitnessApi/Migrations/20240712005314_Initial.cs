using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodV2",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "ProgressPhoto",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressPhoto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBloodPressure",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Systolic = table.Column<int>(type: "int", nullable: false),
                    Diastolic = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBloodPressure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBody",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Neck = table.Column<float>(type: "real", nullable: false),
                    Shoulders = table.Column<float>(type: "real", nullable: false),
                    Chest = table.Column<float>(type: "real", nullable: false),
                    LeftBicep = table.Column<float>(type: "real", nullable: false),
                    RightBicep = table.Column<float>(type: "real", nullable: false),
                    Navel = table.Column<float>(type: "real", nullable: false),
                    Waist = table.Column<float>(type: "real", nullable: false),
                    Hip = table.Column<float>(type: "real", nullable: false),
                    LeftThigh = table.Column<float>(type: "real", nullable: false),
                    RightThigh = table.Column<float>(type: "real", nullable: false),
                    LeftCalf = table.Column<float>(type: "real", nullable: false),
                    RightCalf = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBody", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCustomMacros",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Calories = table.Column<float>(type: "real", nullable: false),
                    Protein = table.Column<float>(type: "real", nullable: false),
                    Fat = table.Column<float>(type: "real", nullable: false),
                    Carbs = table.Column<float>(type: "real", nullable: false),
                    Fiber = table.Column<float>(type: "real", nullable: false),
                    CaloriesHigh = table.Column<float>(type: "real", nullable: true),
                    ProteinHigh = table.Column<float>(type: "real", nullable: true),
                    FatHigh = table.Column<float>(type: "real", nullable: true),
                    CarbsHigh = table.Column<float>(type: "real", nullable: true),
                    FiberHigh = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCustomMacros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserHeight",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHeight", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOneRepMaxEstimates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<long>(type: "bigint", nullable: false),
                    Estimate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOneRepMaxEstimates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserRole = table.Column<string>(type: "varchar(255)", nullable: false),
                    Unit = table.Column<string>(type: "varchar(255)", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    PaidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWeight",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWeight", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkoutActivity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutExerciseId = table.Column<long>(type: "bigint", nullable: false),
                    Set = table.Column<int>(type: "int", nullable: false),
                    Reps = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    Time = table.Column<float>(type: "real", nullable: true),
                    Week = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkoutActivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkoutsCompleted",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutId = table.Column<long>(type: "bigint", nullable: false),
                    Week = table.Column<short>(type: "smallint", nullable: false),
                    Day = table.Column<short>(type: "smallint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkoutsCompleted", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workout",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Days = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Premium = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkoutSubstitution",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutExerciseId = table.Column<long>(type: "bigint", nullable: false),
                    ExerciseId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkoutSubstitution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWorkoutSubstitution_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodV2Servings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "UserSupplements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplementId = table.Column<long>(type: "bigint", nullable: false),
                    Times = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSupplements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSupplements_Supplements_SupplementId",
                        column: x => x.SupplementId,
                        principalTable: "Supplements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkout",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWorkout_Workout_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercise",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkoutId = table.Column<long>(type: "bigint", nullable: false),
                    ExerciseId = table.Column<long>(type: "bigint", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false),
                    MinReps = table.Column<int>(type: "int", nullable: true),
                    MaxReps = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    RestTime = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutExercise_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutExercise_Workout_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workout",
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

            migrationBuilder.CreateTable(
                name: "UserSupplementActivity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserSupplementId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSupplementActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSupplementActivity_UserSupplements_UserSupplementId",
                        column: x => x.UserSupplementId,
                        principalTable: "UserSupplements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_UserSupplementActivity_UserSupplementId",
                table: "UserSupplementActivity",
                column: "UserSupplementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSupplements_SupplementId",
                table: "UserSupplements",
                column: "SupplementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkout_WorkoutId",
                table: "UserWorkout",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkoutSubstitution_ExerciseId",
                table: "UserWorkoutSubstitution",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercise_ExerciseId",
                table: "WorkoutExercise",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercise_WorkoutId",
                table: "WorkoutExercise",
                column: "WorkoutId");
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
