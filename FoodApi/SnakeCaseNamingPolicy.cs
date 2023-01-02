using System.Text.Json;

namespace FoodApi
{
    public sealed class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToSnakeCase();
        }
    }
    
    public static class ExtensionMethods {

        public static string ToSnakeCase(this string str)
        {
            return string.Concat(
                str.Select(
                    (x, i) => i > 0 && char.IsUpper(x)
                        ? "_" + x
                        : x.ToString()
                )
            ).ToLower();
        }
    }
}