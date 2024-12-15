using SkinCa.DataAccess.RepoContracts;

namespace SkinCa.DataAccess.Repositories;

public class DiseaseRepository:IDiseaseRepository
{
    public Task<List<Disease>> GetAllAsyncAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Disease> EditAsync(Disease disease)
    {
        throw new NotImplementedException();
    }

    public Task<List<Disease>> SearchAsync(string searchString)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(Disease disease)
    {
        throw new NotImplementedException();
    }
}