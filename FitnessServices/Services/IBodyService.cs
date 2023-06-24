using FitnessRepository.Models;
using FitnessServices.Models;
using Microsoft.AspNetCore.Http;

namespace FitnessServices.Services;

public interface IBodyService
{
    Task<IEnumerable<UserWeight>> GetAllUserWeights(Guid userId);
    Task<UserWeight?> GetCurrentUserWeight(Guid userId);
    Task AddUserWeight(UserWeight userWeight);
    Task UpdateUserWeight(UserWeight userWeight);
    Task DeleteUserWeight(long id);
    Task<IEnumerable<UserBody>> GetAllUserBodies(Guid userId);
    Task AddUserBody(UserBody userBody);
    Task UpdateUserBody(UserBody userBody);
    Task DeleteUserBody(long id);
    Task<IEnumerable<UserBloodPressure>> GetAllUserBloodPressures(Guid userId);
    Task AddUserBloodPressure(UserBloodPressure userBloodPressure);
    Task UpdateUserBloodPressure(UserBloodPressure userBloodPressure);
    Task DeleteUserBloodPressure(long id);
    Task AddUserHeight(UserHeight userHeight);
    Task<IEnumerable<UserHeight>> GetAllUserHeights(Guid userId);
    Task<IEnumerable<UserBodyFat>> GenerateBodyFats(Guid userId);

    Task<string> UploadProgressPhoto(Guid userId, DateTime date, IFormFile file, string connection,
        string containerName);

    Task<IEnumerable<ProgressPhoto>> GetProgressPhotos(Guid userId);
}