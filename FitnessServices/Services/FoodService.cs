using FitnessRepository.Models;
using FitnessServices.Models;

namespace FitnessServices.Services;

public class FoodService : IFoodService
{
    private readonly IUserService _userService;
    private readonly IBodyService _bodyService;

    public FoodService(IUserService userService, IBodyService bodyService)
    {
        _userService = userService;
        _bodyService = bodyService;
    }
    
    public async Task<IEnumerable<Macros>> GenerateMacros(Guid userId)
    {
        var user = await _userService.GetUserById(userId);
        
        if (user == null) return new List<Macros>();

        var userWeights = await _bodyService.GetAllUserWeights(userId);
        var bodyFat = await _bodyService.GenerateBodyFats(user.Id);
        var macros = new List<Macros>();
        var currentBodyFat = bodyFat?.LastOrDefault()?.BodyFat ?? 10;
        
        
        foreach (var userWeight in userWeights)
        {
            var calories = 0.0;

            if (user.Sex == "Male")
            {
                calories = currentBodyFat > 15 ? userWeight.Weight * 13 : userWeight.Weight * 13 + 500;
            }
            else
            {
                calories = currentBodyFat > 22 ? userWeight.Weight * 13 : userWeight.Weight * 13 + 500;
            }
            
            var protein = userWeight.Weight * 0.8;
            var fat = userWeight.Weight * 0.35;
            var carbs = (calories - protein * 4 - fat * 9) / 4;
            var fiber = calories * 0.015;
            var alcohol = calories * 0.10 / 7;
            var water = userWeight.Weight * 0.6;

            macros.Add(new Macros()
            {
                Protein = (int) Math.Floor(protein),
                Fat = (int) Math.Floor(fat),
                Carbs = (int) Math.Floor(carbs),
                Fiber = (int) Math.Floor(fiber),
                Alcohol = (int) Math.Floor(alcohol),
                Water = (int) Math.Floor(water),
            });
        }

        return macros;
    }
}