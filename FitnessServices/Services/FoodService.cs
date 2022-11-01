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

    public FoodService(IUserService userService, IBodyService bodyService, IFoodApi foodApi,
        IFoodRepository foodRepository)
    {
        _userService = userService;
        _bodyService = bodyService;
        _foodApi = foodApi;
        _foodRepository = foodRepository;
    }
    
    private static float GramToFluidOunce(float gram)
    {
        return gram * 0.035274f;
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

            if (user.Sex == Sex.Male)
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

    public async Task<IEnumerable<EdamamFoodHint>?> ParseFood(string foodQuery, string? barcode)
    {
        return await _foodApi.ParseFood(foodQuery, barcode);
    }

    public async Task<EdamamNutrients?> GetFoodDetails(string foodId, float servingSizeInGrams)
    {
        return await _foodApi.Nutrients(foodId, servingSizeInGrams);
    }

    public async Task<Macros> GetUserCurrentMacos(Guid userId, DateTime date)
    {
        var userFoods = await _foodRepository.GetUserFoods(userId, date);
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

            macros.Calories += (userFood.Food?.Calories ?? 0) * servings;
            macros.Protein += (userFood.Food?.Protein ?? 0) * servings;
            macros.Fat += (userFood.Food?.TotalFat ?? 0) * servings;
            macros.Carbs += (userFood.Food?.Carbohydrates ?? 0) * servings;
            macros.Fiber += (userFood.Food?.Fiber ?? 0) * servings;
            macros.Alcohol += (userFood.Food?.Alcohol ?? 0) * servings;
            macros.Water += GramToFluidOunce((userFood.Food?.Water ?? 0) * servings);
        }

        return macros;
    }

    public async Task<IEnumerable<UserFood>> GetUserFoods(Guid userId, DateTime date)
    {
        return await _foodRepository.GetUserFoods(userId, date);
    }
    
    public async Task<UserFood?> GetUserFood(Guid userId, DateTime date, long foodId)
    {
        return await _foodRepository.GetUserFood(userId, date, foodId);
    }

    public async Task<Food?> GetFood(long foodId)
    {
        return await _foodRepository.GetFood(foodId);
    }
    
    public async Task<IEnumerable<UserFoodGridItem>> GetUserFoodsForGrid(Guid userId, DateTime date)
    {
        var userFoods = await _foodRepository.GetUserFoods(userId, date);
        var userFoodsGrid = new List<UserFoodGridItem>();

        foreach (var userFood in userFoods)
        {
            var servings = userFood.Amount / userFood.Food?.ServingSize ?? 1;

            userFoodsGrid.Add(new UserFoodGridItem()
            {
                Amount = userFood.Amount,
                Created = userFood.Created,
                FoodId = userFood.FoodId,
                EdamamFoodId = userFood.EdamamFoodId,
                Id = userFood.Id,
                Servings = servings,
                UserId = userFood.UserId,
                Food = new Food()
                {
                    Name = userFood.Food?.Name ?? string.Empty,
                    Id = userFood.Food?.Id ?? 0,
                    ServingSize = userFood.Food?.ServingSize ?? 0,
                    ServingSizeUnit = userFood.Food?.ServingSizeUnit ?? Units.Gram,
                    Alcohol = (userFood.Food?.Alcohol ?? 0) * servings,
                    Calories = (userFood.Food?.Calories ?? 0) * servings,
                    Carbohydrates = (userFood.Food?.Carbohydrates ?? 0) * servings,
                    Fiber = (userFood.Food?.Fiber ?? 0) * servings,
                    TotalFat = (userFood.Food?.TotalFat ?? 0) * servings,
                    Protein = (userFood.Food?.Protein ?? 0) * servings,
                    Water = GramToFluidOunce((userFood.Food?.Water ?? 0) * servings),
                    SaturatedFat = (userFood.Food?.SaturatedFat ?? 0) * servings,
                    TransFat = (userFood.Food?.TransFat ?? 0) * servings,
                    Cholesterol = (userFood.Food?.Cholesterol ?? 0) * servings,
                    Sodium = (userFood.Food?.Sodium ?? 0) * servings,
                    Potassium = (userFood.Food?.Potassium ?? 0) * servings,
                    Sugar = (userFood.Food?.Sugar ?? 0) * servings,
                    VitaminA = (userFood.Food?.VitaminA ?? 0) * servings,
                    VitaminC = (userFood.Food?.VitaminC ?? 0) * servings,
                    Calcium = (userFood.Food?.Calcium ?? 0) * servings,
                    Iron = (userFood.Food?.Iron ?? 0) * servings,
                    VitaminD = (userFood.Food?.VitaminD ?? 0) * servings,
                    VitaminB6 = (userFood.Food?.VitaminB6 ?? 0) * servings,
                    VitaminB12 = (userFood.Food?.VitaminB12 ?? 0) * servings,
                    Magnesium = (userFood.Food?.Magnesium ?? 0) * servings,
                    Zinc = (userFood.Food?.Zinc ?? 0) * servings,
                    Folate = (userFood.Food?.Folate ?? 0) * servings,
                    VitaminK = (userFood.Food?.VitaminK ?? 0) * servings,
                    Thiamin = (userFood.Food?.Thiamin ?? 0) * servings,
                    Riboflavin = (userFood.Food?.Riboflavin ?? 0) * servings,
                    Niacin = (userFood.Food?.Niacin ?? 0) * servings,
                    Phosphorus = (userFood.Food?.Phosphorus ?? 0) * servings,
                }
            });
        }

        return userFoodsGrid;
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
                var edamamFood = await _foodApi.Nutrients(userFood.EdamamFoodId, userFood.Amount);
                var enumerable = edamamFoods as EdamamFoodHint[] ?? edamamFoods?.ToArray();
                newFood = new Food()
                {
                    EdamamFoodId = userFood.EdamamFoodId,
                    Name = enumerable?.FirstOrDefault()?.Food.Label ?? "",
                    Brand = enumerable?.FirstOrDefault()?.Food.CategoryLabel ?? "Generic",
                    ServingSize = (int) (enumerable?.FirstOrDefault()?.Measures.FirstOrDefault(e => e.Label == "Serving")?.Weight ?? 0),
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
                    Water = GramToFluidOunce(GetValueFromDictionary(edamamFood?.TotalNutrients, "WATER")?.Quantity ?? 0),
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

        newUserFood.Created = userFood.Created.Date;
        await _foodRepository.AddUserFood(newUserFood);
    }
    
    public async Task UpdateUserFood(UserFood userFood)
    {
        await _foodRepository.UpdateUserFood(userFood);
    }
    
    public async Task DeleteUserFood(long userFoodId)
    {
        await _foodRepository.DeleteUserFood(userFoodId);
    }
}