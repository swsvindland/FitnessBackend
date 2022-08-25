namespace FitnessRepository.Models;

public class UserBody
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public float Neck { get; set; }
    public float Shoulders { get; set; }
    public float Chest { get; set; }
    public float LeftBicep { get; set; }
    public float RightBicep { get; set; }
    public float Navel { get; set; }
    public float Waist { get; set; }
    public float Hip { get; set; }
    public float LeftThigh { get; set; }
    public float RightThigh { get; set; }
    public float LeftCalf { get; set; }
    public float RightCalf { get; set; }
}