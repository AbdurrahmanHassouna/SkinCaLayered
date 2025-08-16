namespace SkinCa.DataAccess.RepositoriesContracts;

public interface IApplicationUserRepository
{
    Task UpdateAsync(ApplicationUserChat userChat);
}