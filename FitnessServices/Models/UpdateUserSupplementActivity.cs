using System.Collections.Specialized;
using System.Text.Json.Serialization;
using FitnessRepository.Models;

namespace FitnessServices.Models;

public sealed class UpdateUserSupplementActivity
{
    public string Date { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public long UserSupplementId { get; set; }
    [JsonConverter(typeof(StringEnumerator))]  
    public SupplementTimes Time { get; set; }
}