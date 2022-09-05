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

    public double ComputeMaleBodyFat(float navel, float neck, float height)
    {
        return 86.010 * Math.Log10(navel - neck) - 70.041 * Math.Log10(height) + 36.76;
    }
    
    public double ComputeFemaleBodyFat(float navel, float hip, float neck, float height)
    {
        return 163.205 * Math.Log10(navel + hip - neck) - 97.684 * Math.Log10(height) - 78.387;
    }

    public async Task<double?> GenerateBodyFat(Users user)
    {
        var userBodies = await _bodyService.GetAllUserBodies(user.Id);
        var userHeights = await _bodyService.GetAllUserHeights(user.Id);

        var currentBody = userBodies.FirstOrDefault();
        var currentHeight = userHeights.FirstOrDefault();

        if (currentBody == null) return null;
        if (currentHeight == null) return null;
        
        if (user.Sex == "Male")
        {
            return ComputeMaleBodyFat(currentBody.Navel, currentBody.Neck, currentHeight.Height);
        }

        return ComputeFemaleBodyFat(currentBody.Navel, currentBody.Hip, currentBody.Neck, currentHeight.Height);
    }

    public async Task<IEnumerable<Macros>> GenerateMacros(Guid userId)
    {
        var user = await _userService.GetUserById(userId);
        
        if (user == null) return new List<Macros>();

        var userWeights = await _bodyService.GetAllUserWeights(userId);
        var bodyFat = await GenerateBodyFat(user);
        var macros = new List<Macros>();

        foreach (var userWeight in userWeights)
        {
            var calories = 0.0;

            if (user.Sex == "Male")
            {
                calories = bodyFat > 15 ? userWeight.Weight * 13 : userWeight.Weight * 13 + 500;
            }
            else
            {
                calories = bodyFat > 22 ? userWeight.Weight * 13 : userWeight.Weight * 13 + 500;
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