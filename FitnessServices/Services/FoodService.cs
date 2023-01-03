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
    private readonly IFatSecretApi _fatSecretApi;
    private readonly IFoodRepository _foodRepository;

    public FoodService(IUserService userService, IBodyService bodyService, IFoodApi foodApi, IFatSecretApi fatSecretApi,
        IFoodRepository foodRepository)
    {
        _userService = userService;
        _bodyService = bodyService;
        _foodApi = foodApi;
        _fatSecretApi = fatSecretApi;
        _foodRepository = foodRepository;
    }

    private static float GramToFluidOunce(float gram)
    {
        return gram * 0.035274f;
    }

    public async Task AddUserCustomMacros(Guid userId, Macros macros)
    {
        var userCustomMacros = new UserCustomMacros()
        {
            Id = macros.Id,
            UserId = userId,
            Created = DateTime.UtcNow,
            Calories = macros.Calories,
            Protein = macros.Protein,
            Carbs = macros.Carbs,
            Fat = macros.Fat,
            Fiber = macros.Fiber,
            Alcohol = macros.Alcohol,
            Water = macros.Water
        };

        if (userCustomMacros.Id == null)
        {
            await _foodRepository.AddUserCustomMacros(userCustomMacros);
        }
        else
        {
            await _foodRepository.UpdateUserCustomMacros(userCustomMacros);
        }
    }

    public async Task<Macros?> GetUserMacros(Guid userId)
    {
        var userCustomMacros = await _foodRepository.GetUserCustomMacros(userId);

        if (userCustomMacros == null)
        {
            return (await GenerateMacros(userId)).LastOrDefault();
        }

        return new Macros()
        {
            Calories = userCustomMacros.Calories,
            Carbs = userCustomMacros.Carbs,
            Fat = userCustomMacros.Fat,
            Protein = userCustomMacros.Protein,
            Fiber = userCustomMacros.Fiber,
            Alcohol = userCustomMacros.Alcohol,
            Water = userCustomMacros.Water
        };
    }
    
    private static float FluidOunceToMilliliter(float fluidOunce)
    {
        return fluidOunce * 29.5735f;
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
            const int
                alcohol = 24; // 2 standard american drinks, 3 standard european/asian drinks, 1 standard russian drink
            var water = user.Unit == UserUnits.Imperial ? userWeight.Weight * 0.6 : FluidOunceToMilliliter(userWeight.Weight * 0.6f);

            calories += alcohol * 7;

            macros.Add(new Macros()
            {
                Calories = (int) Math.Floor(calories),
                Protein = (int) Math.Floor(protein),
                Fat = (int) Math.Floor(fat),
                Carbs = (int) Math.Floor(carbs),
                Fiber = (int) Math.Floor(fiber),
                Alcohol = alcohol,
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

    public async Task<IEnumerable<FatSecretSearchItem>> SearchFood(string query, int page)
    {
        return await _fatSecretApi.SearchFoods(query, page);
    }

    private (FoodV2, IEnumerable<FoodV2Servings>) MapFatSecretFoodToFoodV2(FatSecretItem newFood)
    {
        var foodV2 = new FoodV2()
            {
                Brand = newFood.BrandName,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                FoodType = newFood.FoodType,
                Id = long.Parse(newFood.FoodId),
                Name = newFood.FoodName
            };
            
            var servings = newFood.Servings.Serving.Select(s => new FoodV2Servings()
            {
                Id = long.Parse(s.ServingId ?? "0"),
                FoodV2Id = long.Parse(newFood.FoodId),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Calories = float.Parse(s.Calories ?? "0"),
                AddedSugar = float.Parse(s.AddedSugar ?? "0"),
                Calcium = float.Parse(s.Calcium ?? "0"),
                Carbohydrate = float.Parse(s.Carbohydrate ?? "0"),
                Cholesterol = float.Parse(s.Cholesterol ?? "0"),
                Fat = float.Parse(s.Fat ?? "0"),
                Fiber = float.Parse(s.Fiber ?? "0"),
                Iron = float.Parse(s.Iron ?? "0"),
                MeasurementDescription = s.MeasurementDescription,
                MetricServingAmount = float.Parse(s.MetricServingAmount ?? "0"),
                MetricServingUnit = s.MetricServingUnit,
                ServingDescription = s.ServingDescription,
                NumberOfUnits = float.Parse(s.NumberOfUnits ?? "0"),
                MonounsaturatedFat = float.Parse(s.MonounsaturatedFat ?? "0"),
                PolyunsaturatedFat = float.Parse(s.PolyunsaturatedFat ?? "0"),
                TransFat = float.Parse(s.TransFat ?? "0"),
                Potassium = float.Parse(s.Potassium ?? "0"),
                Protein = float.Parse(s.Protein ?? "0"),
                SaturatedFat = float.Parse(s.SaturatedFat ?? "0"),
                Sodium = float.Parse(s.Sodium ?? "0"),
                Sugar = float.Parse(s.Sugar ?? "0"),
                VitaminA = float.Parse(s.VitaminA ?? "0"),
                VitaminC = float.Parse(s.VitaminC ?? "0"),
                VitaminD = float.Parse(s.VitaminD ?? "0"),
            }).ToList();

            return (foodV2, servings);
    }
    
    public async Task<FoodV2> GetFoodById(long foodId)
    {
        var food = await _foodRepository.GetFoodV2ById(foodId);
        
        if (food == null || !food.Servings.Any())
        {
            var newFood = await _fatSecretApi.GetFood(foodId);

            var (foodV2, servings) = MapFatSecretFoodToFoodV2(newFood);

            if (food == null)
            {
                await _foodRepository.AddFoodV2(foodV2);
            }

            if (!food.Servings.Any())
            {
                await _foodRepository.AddFoodV2Servings(servings);
            }

            foodV2.Servings = servings;
            return foodV2;
        }

        return food;
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
                var enumerable = edamamFoods as EdamamFoodHint[] ?? edamamFoods?.ToArray();

                var servingSize = (int) (enumerable?.FirstOrDefault()?.Measures
                    .FirstOrDefault(e => e.Label == "Serving")
                    ?.Weight ?? 28);

                var edamamFood = await _foodApi.Nutrients(userFood.EdamamFoodId, servingSize);
                
                newFood = new Food()
                {
                    EdamamFoodId = userFood.EdamamFoodId,
                    Name = enumerable?.FirstOrDefault()?.Food.Label ?? "",
                    Brand = enumerable?.FirstOrDefault()?.Food.CategoryLabel ?? "Generic",
                    ServingSize = servingSize,
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
                    Water =
                        GramToFluidOunce(GetValueFromDictionary(edamamFood?.TotalNutrients, "WATER")?.Quantity ?? 0),
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
    
    public async Task<UserFoodV2?> GetUserFoodV2(long userFoodId)
    {
        return await _foodRepository.GetUserFoodV2(userFoodId);
    }

    public async Task<IEnumerable<UserFoodV2>> GetAllUserFoodsV2ByDate(Guid userId, DateTime date)
    {
        return await _foodRepository.GetAllUserFoodsV2ByDate(userId, date);
    }

    public async Task<long> AddUserFoodV2(UserFoodV2 userFoodV2, DateTime date)
    {
        userFoodV2.Created = date;
        userFoodV2.Updated = DateTime.UtcNow;
        return await _foodRepository.AddUserFoodV2(userFoodV2);
    }

    public async Task UpdateUserFoodV2(UserFoodV2 userFood)
    {
        userFood.Updated = DateTime.UtcNow;
        await _foodRepository.UpdateUserFoodV2(userFood);
    }

    public async Task DeleteUserFoodV2(long userFoodId)
    {
        await _foodRepository.DeleteUserFoodV2(userFoodId);
    }
    
    public async Task<Macros> GetUserCurrentMacosV2(Guid userId, DateTime date)
    {
        var userFoods = await _foodRepository.GetAllUserFoodsV2ByDate(userId, date);
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
            var servings = userFood.ServingAmount;

            macros.Calories += (userFood.Serving?.Calories ?? 0) * servings;
            macros.Protein += (userFood.Serving?.Protein ?? 0) * servings;
            macros.Fat += (userFood.Serving?.Fat ?? 0) * servings;
            macros.Carbs += (userFood.Serving?.Carbohydrate ?? 0) * servings;
            macros.Fiber += (userFood.Serving?.Fiber ?? 0) * servings;
            macros.Alcohol += 0;
            macros.Water += 0;
        }

        return macros;
    }

    public async Task RefreshCashedFoodDb()
    {
        var foods = await _foodRepository.GetAllFoods();

        foreach (var food in foods)
        {
            var updatedFood = await _fatSecretApi.GetFood(food.Id);
            var (foodV2, servings) = MapFatSecretFoodToFoodV2(updatedFood);
            
            await _foodRepository.UpdateFoodV2(foodV2);
            await _foodRepository.UpdateFoodV2Servings(servings);
        }
    }

    public async Task<FoodV2> GetFoodByBarcode(string barcode)
    {
        var id = await _fatSecretApi.GetIdFromBarcode(barcode);
        
        if (id == null)
        {
            throw new Exception("Food not found");
        }

        return await GetFoodById(id);
    }
}