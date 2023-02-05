using FitnessRepository.Models;

namespace FitnessServices.Models;

public sealed class UserWorkoutExercise
{
    public long WorkoutId { get; set; }
    public long ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    public int? MinReps { get; set; }
    public int? MaxReps { get; set; }
    public int? Time { get; set; }
    public int Sets { get; set; }
    public int? RestTime { get; set; }
    public IEnumerable<UserWorkoutActivityModel> UserWorkoutActivities { get; set; }
}