using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository;

public sealed class FitnessContext: DbContext
{
    public FitnessContext(DbContextOptions<FitnessContext> options) : base(options)
    {
        
    }
    
    public DbSet<UserSupplementActivity> UserSupplementActivity { get; set; }
    public DbSet<UserSupplement> UserSupplements { get; set; }
    public DbSet<Supplements> Supplements { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<UserBody> UserBody { get; set; }
    public DbSet<UserBloodPressure> UserBloodPressure { get; set; }
    public DbSet<UserWeight> UserWeight { get; set; }
    public DbSet<UserHeight> UserHeight { get; set; }
    public DbSet<Exercise> Exercise { get; set; }
    public DbSet<Workout> Workout { get; set; }
    public DbSet<WorkoutExercise> WorkoutExercise { get; set; }
    public DbSet<UserWorkout> UserWorkout { get; set; }
    public DbSet<UserWorkoutActivity> UserWorkoutActivity { get; set; }
    public DbSet<UserOneRepMaxEstimates> UserOneRepMaxEstimates { get; set; }
    public DbSet<UserWorkoutsCompleted> UserWorkoutsCompleted { get; set; }
    public DbSet<Food> Food { get; set; }
    public DbSet<UserFood> UserFood { get; set; }
    public DbSet<UserToken> UserToken { get; set; }
    public DbSet<UserCustomMacros> UserCustomMacros { get; set; }
}