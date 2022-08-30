using FitnessRepository.Models;
using FitnessServices.Models;

namespace FitnessServices.Services;

public class FoodService: IFoodService
{
    private readonly IBodyService _bodyService;
    
    public FoodService(IBodyService bodyService)
    {
        _bodyService = bodyService;
    }

    public async Task<IEnumerable<Macros>> GenerateMacros(Guid userId)
    {
        var userWeights =  await _bodyService.GetAllUserWeights(userId);
        var macros = new List<Macros>();

        foreach (var userWeight in userWeights)
        {
            var calories = userWeight.Weight * 13;
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