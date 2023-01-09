namespace FitnessRepository.Models;

public class ProgressPhoto
{
    public long Id { get; set; }
    public string Filename { get; set; }
    public Guid FileId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
}