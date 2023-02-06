using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;
using FoodApi;
using FoodApi.Models;
using Microsoft.Extensions.Logging;

namespace FitnessServices.Services;

public sealed class FoodService : IFoodService
{
    private readonly IUserService _userService;
    private readonly IBodyService _bodyService;
    private readonly IFatSecretApi _fatSecretApi;
    private readonly IFoodRepository _foodRepository;

    public FoodService(IUserService userService, IBodyService bodyService, IFatSecretApi fatSecretApi,
        IFoodRepository foodRepository)
    {
        _userService = userService;
        _bodyService = bodyService;
        _fatSecretApi = fatSecretApi;
        _foodRepository = foodRepository;
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
            CaloriesHigh = macros.CaloriesHigh,
            ProteinHigh = macros.ProteinHigh,
            CarbsHigh = macros.CarbsHigh,
            FatHigh = macros.FatHigh,
            FiberHigh = macros.FiberHigh
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
            CaloriesHigh = userCustomMacros.CaloriesHigh,
            CarbsHigh = userCustomMacros.CarbsHigh,
            FatHigh = userCustomMacros.FatHigh,
            ProteinHigh = userCustomMacros.ProteinHigh,
            FiberHigh = userCustomMacros.FiberHigh
        };
    }

    public async Task<IEnumerable<Macros>> GenerateMacros(Guid userId)
    {
        var user = await _userService.GetUserById(userId);

        if (user == null) return new List<Macros>();

        var userWeights = (await _bodyService.GetAllUserWeights(userId)).ToArray();
        var bodyFat = await _bodyService.GenerateBodyFats(user.Id);
        var macros = new List<Macros>();
        var currentBodyFat = bodyFat.LastOrDefault()?.BodyFat ?? 10;

        if (!userWeights.Any())
        {
            return new[]
            {
                new Macros()
                {
                    Calories = 2000,
                    CaloriesHigh = 2500,
                    Carbs = 250,
                    CarbsHigh = 300,
                    Fat = 60,
                    FatHigh = 70,
                    Protein = 100,
                    ProteinHigh = 120,
                    Fiber = 20,
                    FiberHigh = 50
                }
            };
        }


        foreach (var userWeight in userWeights)
        {
            float calories;

            if (user.Sex == Sex.Male)
            {
                calories = currentBodyFat > 15 ? userWeight.Weight * 11 : userWeight.Weight * 14;
            }
            else
            {
                calories = currentBodyFat > 22 ? userWeight.Weight * 11 : userWeight.Weight * 14;
            }

            calories -= 250;
            var caloriesHigh = calories + 500;
            var protein = userWeight.Weight * 0.7f;
            var proteinHigh = userWeight.Weight * 1.2f;
            var fat = userWeight.Weight * 0.35f;
            var fatHigh = userWeight.Weight * 0.45f;
            var carbs = (calories - protein * 4 - fat * 9) / 4;
            var carbsHigh = (calories + 300 - proteinHigh * 4 - fatHigh * 9) / 4;
            var fiber = calories * 0.010f;
            var fiberHigh = caloriesHigh * 0.020f;

            macros.Add(new Macros()
            {
                Calories = calories,
                Protein = protein,
                Fat = fat,
                Carbs = carbsHigh > carbs ? carbs + fiber : carbsHigh + fiberHigh,
                Fiber = fiber,
                CaloriesHigh = caloriesHigh,
                ProteinHigh = proteinHigh,
                FatHigh = fatHigh,
                CarbsHigh = carbsHigh < carbs ? carbs + fiber : carbsHigh + fiberHigh,
                FiberHigh = fiberHigh,
            });
        }

        return macros;
    }

    public async Task<IEnumerable<string>?> AutocompleteFood(string query)
    {
        return await _fatSecretApi.Autocomplete(query);
    }

    public async Task<IEnumerable<FatSecretSearchItem>?> SearchFood(string query, int page)
    {
        return await _fatSecretApi.SearchFoods(query, page);
    }

    private static (FoodV2, IEnumerable<FoodV2Servings>) MapFatSecretFoodToFoodV2(FatSecretItem newFood)
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

    public async Task<FoodV2?> GetFoodById(long foodId)
    {
        var food = await _foodRepository.GetFoodV2ById(foodId);

        if (food != null && food.Servings.Any()) return food;

        var newFood = await _fatSecretApi.GetFood(foodId);

        if (newFood == null) return null;

        var (foodV2, servings) = MapFatSecretFoodToFoodV2(newFood);
        var foodV2ServingsEnumerable = servings as FoodV2Servings[] ?? servings.ToArray();
        
        if (food == null)
        {
            await _foodRepository.AddFoodV2(foodV2);
            await _foodRepository.AddFoodV2Servings(foodV2ServingsEnumerable);
        }

        if (food != null && !food.Servings.Any())
        {
            await _foodRepository.AddFoodV2Servings(foodV2ServingsEnumerable);
        }
        
        foodV2.Servings = foodV2ServingsEnumerable;
        return foodV2;
    }

    public async Task<IEnumerable<UserFoodV2>> GetRecentUserFoods(Guid userId, DateTime date)
    {
        var allFoods = await _foodRepository.GetAllUserFoodsV2(userId);
        var seen = new Dictionary<long, bool>();
        var recentFoods = new List<UserFoodV2>();

        foreach (var allFood in allFoods)
        {
            if (seen.ContainsKey(allFood.FoodV2Id))
            {
                continue;
            }

            if (allFood.Created.Date != date.Date)
            {
                allFood.ServingAmount = 0;
            }

            recentFoods.Add(allFood);
            seen.TryAdd(allFood.FoodV2Id, true);
        }

        return recentFoods.Take(20);
    }

    public async Task<UserFoodV2?> GetUserFoodV2(long userFoodId)
    {
        return await _foodRepository.GetUserFoodV2(userFoodId);
    }

    public async Task<IEnumerable<UserFoodV2>> GetAllUserFoodsV2ByDate(Guid userId, DateTime date)
    {
        return await _foodRepository.GetAllUserFoodsV2ByDate(userId, date);
    }

    public async Task<float> QuickAddUserFoodV2(Guid userId, long foodId, DateTime date)
    {
        var food = await GetFoodById(foodId);
        var userFoodV2 = (await GetAllUserFoodsV2ByDate(userId, date)).FirstOrDefault(e => e.FoodV2Id == foodId);

        if (userFoodV2 == null)
        {
            var newFood = new UserFoodV2()
            {
                UserId = userId,
                FoodV2Id = foodId,
                Created = date,
                ServingId = food?.Servings.FirstOrDefault()?.Id ?? 0,
                Updated = date,
                ServingAmount = 1
            };

            await _foodRepository.AddUserFoodV2(newFood);

            return 1;
        }

        userFoodV2.Updated = date;
        userFoodV2.ServingAmount += 1;
        await _foodRepository.UpdateUserFoodV2(userFoodV2);

        return userFoodV2.ServingAmount;
    }

    public async Task<float> QuickRemoveUserFoodV2(Guid userId, long foodId, DateTime date)
    {
        var food = await GetFoodById(foodId);
        var userFoodV2 = (await GetAllUserFoodsV2ByDate(userId, date)).FirstOrDefault(e => e.FoodV2Id == foodId);

        if (userFoodV2 == null)
        {
            var newFood = new UserFoodV2()
            {
                UserId = userId,
                FoodV2Id = foodId,
                Created = date,
                ServingId = food?.Servings.FirstOrDefault()?.Id ?? 0,
                Updated = date,
                ServingAmount = 0
            };

            await _foodRepository.AddUserFoodV2(newFood);

            return 1;
        }

        userFoodV2.Updated = date;
        userFoodV2.ServingAmount = userFoodV2.ServingAmount < 1 ? 0 : userFoodV2.ServingAmount - 1;
        await _foodRepository.UpdateUserFoodV2(userFoodV2);

        return userFoodV2.ServingAmount;
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
            Calories = 0,
            Carbs = 0,
            Fiber = 0,
            Fat = 0,
            Protein = 0,
        };

        foreach (var userFood in userFoods)
        {
            var servings = userFood.ServingAmount;

            macros.Calories += (userFood.Serving?.Calories ?? 0) * servings;
            macros.Protein += (userFood.Serving?.Protein ?? 0) * servings;
            macros.Fat += (userFood.Serving?.Fat ?? 0) * servings;
            macros.Carbs += (userFood.Serving?.Carbohydrate ?? 0) * servings;
            macros.Fiber += (userFood.Serving?.Fiber ?? 0) * servings;
        }

        return macros;
    }

    public async Task RefreshCashedFoodDb(ILogger logger)
    {
        var foods = await _foodRepository.RefreshAllFoodsV2();
        foreach (var food in foods)
        {
            var updatedFood = await _fatSecretApi.GetFood(food.Id);

            if (updatedFood == null) continue;

            var (foodV2, _) = MapFatSecretFoodToFoodV2(updatedFood);

            if (foodV2.Servings.Count() == food.Servings.Count()) continue;
            logger.LogInformation("Updating food {FoodId} {FoodName}", food.Id, food.Name);
            await _foodRepository.UpdateFoodV2(foodV2);
        }
    }

    public async Task<FoodV2?> GetFoodByBarcode(string barcode)
    {
        var id = await _fatSecretApi.GetIdFromBarcode(barcode);

        if (id != null) return await GetFoodById(id.Value);

        return null;
    }
}