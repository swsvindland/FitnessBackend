using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IBodyRepository
{
    Task<IEnumerable<UserWeight>> GetAllUserWeights(Guid userId);
    Task AddUserWeight(UserWeight userWeight);
    Task DeleteUserWeight(UserWeight userWeight);
    Task<IEnumerable<UserBody>> GetAllUserBodies(Guid userId);
    Task AddUserBody(UserBody userBody);
    Task DeleteUserBody(UserBody userBody);
    Task<IEnumerable<UserBloodPressure>> GetAllUserBloodPressures(Guid userId);
    Task AddUserBloodPressure(UserBloodPressure userBloodPressure);
    Task DeleteUserBloodPressure(UserBloodPressure userBloodPressure);
}