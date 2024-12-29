namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IDiseaseRepository
{
    Task<List<Disease>> GetAllAsyncAsync();
    Task<bool?> EditAsync(Disease disease);
    Task<List<Disease>> SearchAsync(string searchString);
    Task<bool>  CreateAsync(Disease disease);
    
    Task<bool?>  DeleteAsync(int id);
    
}