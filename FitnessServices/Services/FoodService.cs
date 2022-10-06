using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;
using FoodApi;
using FoodApi.Models;

namespace FitnessServices.Services;

public sealed class FoodService : IFoodService
{
    private readonly IUserService _userService;
    private readonly IBodyService _bodyService;
    private readonly IFoodApi _foodApi;
    private readonly IFoodRepository _foodRepository;

    public FoodService(IUserService userService, IBodyService bodyService, IFoodApi foodApi, IFoodRepository foodRepository)
    {
        _userService = userService;
        _bodyService = bodyService;
        _foodApi = foodApi;
        _foodRepository = foodRepository;
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
                calories = currentBodyFat > 15 ? userWeight.Weight * 11 : userWeight.Weight * 13 + 500;
            }
            else
            {
                calories = currentBodyFat > 22 ? userWeight.Weight * 11 : userWeight.Weight * 13 + 500;
            }
            
            var protein = userWeight.Weight * 0.8;
            var fat = userWeight.Weight * 0.35;
            var carbs = (calories - protein * 4 - fat * 9) / 4;
            var fiber = calories * 0.015;
            var alcohol = calories * 0.10 / 7;
            var water = userWeight.Weight * 0.6;

            macros.Add(new Macros()
            {
                Calories = (int) Math.Floor(calories),
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
    
    public async Task<IEnumerable<string>?> AutocompleteFood(string query)
    {
        return await _foodApi.AutocompleteFood(query);
    }
    
    public async Task<IEnumerable<EdamamFood>?> ParseFood(string foodQuery, string? barcode)
    {
        return await _foodApi.ParseFood(foodQuery, barcode);
    }
    
    public async Task<EdamamNutrients?> GetFoodDetails(string foodId )
    {
        return await _foodApi.Nutrients(foodId);
    }

    public async Task<Macros> GetUserCurrentMacos(Guid userId)
    {
        var userFoods = await _foodRepository.GetUserFoods(userId);
        var macros = new Macros()
        {
            Alcohol = 0,
            Calories = 0,
            Carbs = 0,
            Fiber = 0,
            Fat = 0,
            Protein = 0,
            Water = 0
        };

        foreach (var userFood in userFoods)
        {
            var servings = userFood.Amount / userFood.Food?.ServingSize ?? 1;
            
            macros.Calories += userFood.Food?.Calories ?? 0 * servings;
            macros.Protein += userFood.Food?.Protein ?? 0 * servings;
            macros.Fat += userFood.Food?.TotalFat ?? 0 * servings;
            macros.Carbs += userFood.Food?.Carbohydrates ?? 0 * servings;
            macros.Fiber += userFood.Food?.Fiber ?? 0 * servings;
            macros.Alcohol += userFood.Food?.Alcohol ?? 0 * servings;
            macros.Water += userFood.Food?.Water ?? 0 * servings;
        }
        
        return macros;
    }
    
    public async Task<IEnumerable<UserFood>> GetUserFoods(Guid userId)
    {
        return await _foodRepository.GetUserFoods(userId);
    }
    
    private static EdamamNutrient? GetValueFromDictionary(Dictionary<string, EdamamNutrient>? dictionary, string key)
    {
        if (dictionary == null) return null;

        dictionary.TryGetValue(key, out var value);
        return value;
    }

    public async Task AddUserFood(UserFood userFood)
    {
        var newFood = new Food();
        var newUserFood = new UserFood();
        
        if (userFood.FoodId == null && userFood.EdamamFoodId != null)
        {
            var food = await _foodRepository.GetFoodByEdamamId(userFood.EdamamFoodId);
            newUserFood = userFood;

            if (food == null)
            {
                var edamamFoods = await _foodApi.ParseFood(userFood.EdamamFoodId, null);
                var edamamFood = await _foodApi.Nutrients(userFood.EdamamFoodId);
                var enumerable = edamamFoods as EdamamFood[] ?? edamamFoods?.ToArray();
                newFood = new Food()
                {
                    EdamamFoodId = userFood.EdamamFoodId,
                    Name = enumerable?.FirstOrDefault()?.Label ?? "",
                    Brand = enumerable?.FirstOrDefault()?.CategoryLabel ?? "Generic",
                    ServingSize = 100,
                    ServingSizeUnit = Units.Gram,
                    Calories = GetValueFromDictionary(edamamFood?.TotalNutrients, "ENERC_KCAL")?.Quantity ?? 0,
                    TotalFat = GetValueFromDictionary(edamamFood?.TotalNutrients, "FAT")?.Quantity ?? 0,
                    SaturatedFat = GetValueFromDictionary(edamamFood?.TotalNutrients, "FASAT")?.Quantity ?? 0,
                    TransFat = GetValueFromDictionary(edamamFood?.TotalNutrients, "FATRN")?.Quantity ?? 0,
                    MonounsaturatedFat = GetValueFromDictionary(edamamFood?.TotalNutrients, "FAMS")?.Quantity ?? 0,
                    PolyunsaturatedFat = GetValueFromDictionary(edamamFood?.TotalNutrients, "FAPU")?.Quantity ?? 0,
                    Cholesterol = GetValueFromDictionary(edamamFood?.TotalNutrients, "CHOLE")?.Quantity ?? 0,
                    Sodium = GetValueFromDictionary(edamamFood?.TotalNutrients, "NA")?.Quantity ?? 0,
                    Potassium = GetValueFromDictionary(edamamFood?.TotalNutrients, "K")?.Quantity ?? 0,
                    Carbohydrates = GetValueFromDictionary(edamamFood?.TotalNutrients, "CHOCDF")?.Quantity ?? 0,
                    Fiber = GetValueFromDictionary(edamamFood?.TotalNutrients, "FIBTG")?.Quantity ?? 0,
                    Sugar = GetValueFromDictionary(edamamFood?.TotalNutrients, "SUGAR")?.Quantity ?? 0,
                    Protein = GetValueFromDictionary(edamamFood?.TotalNutrients, "PROCNT")?.Quantity ?? 0,
                    Magnesium = GetValueFromDictionary(edamamFood?.TotalNutrients, "MG")?.Quantity ?? 0,
                    Calcium = GetValueFromDictionary(edamamFood?.TotalNutrients, "CA")?.Quantity ?? 0,
                    Iron = GetValueFromDictionary(edamamFood?.TotalNutrients, "FE")?.Quantity ?? 0,
                    Zinc = GetValueFromDictionary(edamamFood?.TotalNutrients, "ZN")?.Quantity ?? 0,
                    Phosphorus = GetValueFromDictionary(edamamFood?.TotalNutrients, "P")?.Quantity ?? 0,
                    VitaminA = GetValueFromDictionary(edamamFood?.TotalNutrients, "VITA_RAE")?.Quantity ?? 0,
                    VitaminC = GetValueFromDictionary(edamamFood?.TotalNutrients, "VITC")?.Quantity ?? 0,
                    VitaminD = GetValueFromDictionary(edamamFood?.TotalNutrients, "VITD")?.Quantity ?? 0,
                    VitaminE = GetValueFromDictionary(edamamFood?.TotalNutrients, "TOCPHA")?.Quantity ?? 0,
                    VitaminK = GetValueFromDictionary(edamamFood?.TotalNutrients, "VITK1")?.Quantity ?? 0,
                    Thiamin = GetValueFromDictionary(edamamFood?.TotalNutrients, "THIA")?.Quantity ?? 0,
                    Riboflavin = GetValueFromDictionary(edamamFood?.TotalNutrients, "RIBF")?.Quantity ?? 0,
                    Niacin = GetValueFromDictionary(edamamFood?.TotalNutrients, "NIA")?.Quantity ?? 0,
                    VitaminB6 = GetValueFromDictionary(edamamFood?.TotalNutrients, "VITB6A")?.Quantity ?? 0,
                    Folate = GetValueFromDictionary(edamamFood?.TotalNutrients, "FOLDFE")?.Quantity ?? 0,
                    VitaminB12 = GetValueFromDictionary(edamamFood?.TotalNutrients, "VITB12")?.Quantity ?? 0,
                    Water = GetValueFromDictionary(edamamFood?.TotalNutrients, "WATER")?.Quantity ?? 0,
                };

                var id = await _foodRepository.AddFood(newFood);
                newUserFood.FoodId = id;
            }
            else
            {
                newUserFood.FoodId = food.Id;
            }
        }
        else if (userFood.FoodId != null && userFood.EdamamFoodId == null)
        {
            var food = await _foodRepository.GetFood(userFood.FoodId.Value);
            newUserFood = userFood;
        }
        else
        {
            throw new Exception("Invalid food");
        }
        
        newUserFood.Created = DateTime.UtcNow.Date;
        await _foodRepository.AddUserFood(newUserFood);
    }
}