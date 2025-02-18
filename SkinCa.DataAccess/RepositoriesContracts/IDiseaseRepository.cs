namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IDiseaseRepository
{
    Task<Disease?> GetByIdAsync(int id);
    Task<List<Disease>> GetAllAsync();
    Task EditAsync(Disease disease);
    Task<List<Disease>> SearchAsync(string searchString);
    Task CreateAsync(Disease disease);
    
    Task DeleteAsync(int id);
    
}