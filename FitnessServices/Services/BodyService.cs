using System.Collections;
using Azure.Storage.Blobs;
using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;
using Microsoft.AspNetCore.Http;

namespace FitnessServices.Services;

public sealed class BodyService : IBodyService
{
    private readonly IUserService _userService;
    private readonly IBodyRepository _bodyRepository;

    public BodyService(IUserService userService, IBodyRepository bodyRepository)
    {
        _userService = userService;
        _bodyRepository = bodyRepository;
    }

    public async Task<IEnumerable<UserWeight>> GetAllUserWeights(Guid userId)
    {
        return await _bodyRepository.GetAllUserWeights(userId);
    }

    public async Task<UserWeight?> GetCurrentUserWeight(Guid userId)
    {
        return await _bodyRepository.GetCurrentUserWeights(userId);
    }

    public async Task AddUserWeight(UserWeight userWeight)
    {
        await _bodyRepository.AddUserWeight(userWeight);
    }

    public async Task UpdateUserWeight(UserWeight userWeight)
    {
        await _bodyRepository.UpdateUserWeight(userWeight);
    }

    public async Task DeleteUserWeight(long id)
    {
        await _bodyRepository.DeleteUserWeight(id);
    }

    public async Task<IEnumerable<UserBody>> GetAllUserBodies(Guid userId)
    {
        return await _bodyRepository.GetAllUserBodies(userId);
    }

    public async Task AddUserBody(UserBody userBody)
    {
        await _bodyRepository.AddUserBody(userBody);
    }

    public async Task UpdateUserBody(UserBody userBody)
    {
        await _bodyRepository.UpdateUserBody(userBody);
    }

    public async Task DeleteUserBody(long id)
    {
        await _bodyRepository.DeleteUserBody(id);
    }

    public async Task<IEnumerable<UserBloodPressure>> GetAllUserBloodPressures(Guid userId)
    {
        return await _bodyRepository.GetAllUserBloodPressures(userId);
    }

    public async Task AddUserBloodPressure(UserBloodPressure userBloodPressure)
    {
        await _bodyRepository.AddUserBloodPressure(userBloodPressure);
    }

    public async Task UpdateUserBloodPressure(UserBloodPressure userBloodPressure)
    {
        await _bodyRepository.UpdateUserBloodPressure(userBloodPressure);
    }

    public async Task DeleteUserBloodPressure(long id)
    {
        await _bodyRepository.DeleteUserBloodPressure(id);
    }

    public async Task AddUserHeight(UserHeight userHeight)
    {
        userHeight.Created = DateTime.UtcNow;
        await _bodyRepository.AddUserHeight(userHeight);
    }

    public async Task<IEnumerable<UserHeight>> GetAllUserHeights(Guid userId)
    {
        return await _bodyRepository.GetUserHeights(userId);
    }

    private static double CentimetersToInches(double centimeters)
    {
        return centimeters * 0.393701;
    }

    private static double ComputeMaleBodyFat(UserUnits unit, float navel, float neck, float height)
    {
        if (unit == UserUnits.Metric)
        {
            return 86.010 * Math.Log10(CentimetersToInches(navel) - CentimetersToInches(neck)) -
                70.041 * Math.Log10(CentimetersToInches(height)) + 36.76;
        }

        return 86.010 * Math.Log10(navel - neck) - 70.041 * Math.Log10(height) + 36.76;
    }

    private static double ComputeFemaleBodyFat(UserUnits unit, float navel, float hip, float neck, float height)
    {
        if (unit == UserUnits.Metric)
        {
            return 163.205 *
                   Math.Log10(CentimetersToInches(navel) + CentimetersToInches(hip) - CentimetersToInches(neck)) -
                   97.684 * Math.Log10(CentimetersToInches(height)) - 78.387;
        }

        return 163.205 * Math.Log10(navel + hip - neck) - 97.684 * Math.Log10(height) - 78.387;
    }

    public async Task<IEnumerable<UserBodyFat>> GenerateBodyFats(Guid userId)
    {
        var user = await _userService.GetUserById(userId);

        var userBodies = await GetAllUserBodies(userId);
        var userHeights = await GetAllUserHeights(userId);

        var currentHeight = userHeights.FirstOrDefault();

        if (user == null) return new List<UserBodyFat>();
        if (currentHeight == null) return new List<UserBodyFat>();

        var userBodyFats = new List<UserBodyFat>();

        foreach (var userBody in userBodies)
        {
            if (user.Sex == Sex.Male)
            {
                userBodyFats.Add(new UserBodyFat()
                {
                    BodyFat = ComputeMaleBodyFat(user.Unit, userBody.Navel, userBody.Neck, currentHeight.Height),
                    Created = userBody.Created,
                    UserId = userId
                });
            }
            else
            {
                userBodyFats.Add(new UserBodyFat()
                {
                    BodyFat = ComputeFemaleBodyFat(user.Unit, userBody.Navel, userBody.Hip, userBody.Neck,
                        currentHeight.Height),
                    Created = userBody.Created,
                    UserId = userId
                });
            }
        }

        return userBodyFats;
    }

    public async Task<string> UploadProgressPhoto(Guid userId, DateTime date, IFormFile file, string connection,
        string containerName)
    {
        var fileName = Guid.NewGuid();
        var fileExtension = Path.GetExtension(file.FileName);
        Stream myBlob = new MemoryStream();
        myBlob = file.OpenReadStream();
        var blobClient = new BlobContainerClient(connection, containerName);
        var blob = blobClient.GetBlobClient(fileName + fileExtension);
        await blob.UploadAsync(myBlob, true);

        var progressPhoto = new ProgressPhoto()
        {
            FileId = fileName,
            Filename = fileName + fileExtension,
            UserId = userId,
            Created = date
        };

        await _bodyRepository.AddProgressPhoto(progressPhoto);

        return fileName + fileExtension;
    }

    public async Task<IEnumerable<ProgressPhoto>> GetProgressPhotos(Guid userId)
    {
        return await _bodyRepository.GetProgressPhotos(userId);
    }
    
    public async Task<IEnumerable<UserCheckIn>> GetUserCheckIns(Guid userId)
    {
        return await _bodyRepository.GetUserCheckIns(userId);
    }
    
    public async Task<UserCheckIn?> GetLastUserCheckIn(Guid userId)
    {
        return await _bodyRepository.GetLastUserCheckIns(userId);
    }
    
    public async Task AddUserCheckIn(Guid userId)
    {
        var userCheckIn = new UserCheckIn()
        {
            UserId = userId,
            Created = DateTime.UtcNow
        };
        
        await _bodyRepository.AddUserCheckIn(userCheckIn);
    }
}