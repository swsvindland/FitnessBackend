using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository;

public class FitnessContext: DbContext
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
    public DbSet<Exercise> Exercise { get; set; }
    public DbSet<Workout> Workout { get; set; }
    public DbSet<WorkoutBlock> WorkoutBlock { get; set; }
    public DbSet<WorkoutBlockExercise> WorkoutBlockExercise { get; set; }
    public DbSet<UserWorkout> UserWorkout { get; set; }
    public DbSet<UserWorkoutActivity> UserWorkoutActivity { get; set; }
}