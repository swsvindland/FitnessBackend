namespace FitnessServices.Models;

public sealed class ChangePassword
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}