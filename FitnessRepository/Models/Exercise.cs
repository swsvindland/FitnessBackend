using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessRepository.Models;

public sealed class Exercise
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public int Fatigue { get; set; }
    [Column(TypeName = "varchar(255)")]
    public ExerciseIcon? Icon { get; set; }
}

public enum ExerciseIcon
{
    Unknown,
    Barbell,
    Dumbbell,
    Cable,
    Machine,
    Bodyweight,
    Band,
    Cardio,
}