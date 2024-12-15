using SkinCa.DataAccess;

namespace SkinCa.DataAccess.RepoContracts;

public interface IDiseaseRepository
{
    Task<List<Disease>> GetAllAsyncAsync();
    Task<Disease> EditAsync(Disease disease);
    Task<List<Disease>> SearchAsync(string searchString);
    Task CreateAsync(Disease disease);
}