using FitnessRepository.Models;
using FitnessRepository.Repositories;

namespace FitnessServices.Services;

public class BodyService: IBodyService
{
    private readonly IBodyRepository _bodyRepository;

    public BodyService(IBodyRepository bodyRepository)
    {
        _bodyRepository = bodyRepository;
    }
    
    public async Task<IEnumerable<UserWeight>> GetAllUserWeights(Guid userId)
    {
        return await _bodyRepository.GetAllUserWeights(userId);
    }
    
    public async Task AddUserWeight(UserWeight userWeight)
    {
        await _bodyRepository.AddUserWeight(userWeight);
    }
    
    public async Task DeleteUserWeight(UserWeight userWeight)
    {
        await _bodyRepository.DeleteUserWeight(userWeight);
    }
    
    public async Task<IEnumerable<UserBody>> GetAllUserBodies(Guid userId)
    {
        return await _bodyRepository.GetAllUserBodies(userId);
    }
    
    public async Task AddUserBody(UserBody userBody)
    {
        await _bodyRepository.AddUserBody(userBody);
    }
    
    public async Task DeleteUserBody(UserBody userBody)
    {
        await _bodyRepository.DeleteUserBody(userBody);
    }
    
    public async Task<IEnumerable<UserBloodPressure>> GetAllUserBloodPressures(Guid userId)
    {
        return await _bodyRepository.GetAllUserBloodPressures(userId);
    }
    
    public async Task AddUserBloodPressure(UserBloodPressure userBloodPressure)
    {
        await _bodyRepository.AddUserBloodPressure(userBloodPressure);
    }
    
    public async Task DeleteUserBloodPressure(UserBloodPressure userBloodPressure)
    {
        await _bodyRepository.DeleteUserBloodPressure(userBloodPressure);
    }

    public async Task AddUserHeight(UserHeight userHeight)
    {
        await _bodyRepository.AddUserHeight(userHeight);
    }
    
    public async Task<IEnumerable<UserHeight>> GetAllUserHeights(Guid userId)
    {
        return await _bodyRepository.GetUserHeights(userId);
    }
}