using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IBodyRepository
{
    Task<IEnumerable<UserWeight>> GetAllUserWeights(Guid userId);
    Task AddUserWeight(UserWeight userWeight);
    Task UpdateUserWeight(UserWeight userWeight);
    Task DeleteUserWeight(UserWeight userWeight);
    Task DeleteUserWeight(long id);
    Task<IEnumerable<UserBody>> GetAllUserBodies(Guid userId);
    Task AddUserBody(UserBody userBody);
    Task UpdateUserBody(UserBody userBody);
    Task DeleteUserBody(long id);
    Task DeleteUserBody(UserBody userBody);
    Task<IEnumerable<UserBloodPressure>> GetAllUserBloodPressures(Guid userId);
    Task AddUserBloodPressure(UserBloodPressure userBloodPressure);
    Task UpdateUserBloodPressure(UserBloodPressure userBloodPressure);
    Task DeleteUserBloodPressure(long id);
    Task DeleteUserBloodPressure(UserBloodPressure userBloodPressure);
    Task AddUserHeight(UserHeight userHeight);
    Task<IEnumerable<UserHeight>> GetUserHeights(Guid userId);
    Task<IEnumerable<ProgressPhoto>> GetProgressPhotos(Guid userId);
    Task<long> AddProgressPhoto(ProgressPhoto progressPhoto);
    Task<long> UpdateProgressPhoto(ProgressPhoto progressPhoto);
    Task DeleteProgressPhoto(long id);
}