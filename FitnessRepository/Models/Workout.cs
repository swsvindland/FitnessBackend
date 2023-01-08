using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessRepository.Models;

public sealed class Workout
{
    public long Id { get; set; }
    public Guid? UserId { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Days { get; set; }
    public int Duration { get; set; }
    public bool Premium { get; set; }
    [Column(TypeName = "varchar(255)")]
    public WorkoutType Type { get; set; }
    public IEnumerable<WorkoutExercise> WorkoutExercise { get; set; }
}