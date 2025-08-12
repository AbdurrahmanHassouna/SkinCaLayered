using SkinCa.Business.Services;
using SkinCa.Business.ServicesContracts;
using SkinCa.DataAccess.Repositories;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.Presentation;

public static class DI
{
    public static IServiceCollection RegesterBussinessDI(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
        serviceCollection.AddScoped<IBannerService,BannerService>();
        serviceCollection.AddScoped<IBookmarkService, BookmarkService>();
        serviceCollection.AddScoped<IDiseaseService,DiseaseService>();
        serviceCollection.AddScoped<IDoctorInfoService,DoctorInfoService>();
        serviceCollection.AddScoped<IEmailService, EmailService>();
        serviceCollection.AddScoped<IScanResultService, ScanResultService>();
        return serviceCollection;
    }
    public static IServiceCollection RegesterRepositoriesDI(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBannerRepository,BannerRepository>();
        serviceCollection.AddScoped<IBookmarkRepository, BookmarkRepository>();
        serviceCollection.AddScoped<IDiseaseRepository,DiseaseRepository>();
        serviceCollection.AddScoped<IDoctorInfoRepository,DoctorInfoRepository>();
        serviceCollection.AddScoped<IScanResultRepository, ScanResultRepository>();
        serviceCollection.AddScoped<IChatRepository, ChatRepository>();
        serviceCollection.AddScoped<IMessageRepository, MessageRepository>();
        return serviceCollection;
    }
}