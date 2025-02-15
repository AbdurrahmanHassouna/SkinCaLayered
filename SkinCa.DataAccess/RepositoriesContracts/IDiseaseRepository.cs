namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IDiseaseRepository
{
    Task<Disease?> GetByIdAsync(int id);
    Task<List<Disease>> GetAllAsync();
    Task<bool> EditAsync(Disease disease);
    Task<List<Disease>> SearchAsync(string searchString);
    Task<bool>  CreateAsync(Disease disease);
    
    Task<bool>  DeleteAsync(int id);
    
}