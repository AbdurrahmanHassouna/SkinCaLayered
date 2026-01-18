using SkinCa.Business.Services;
using SkinCa.Business.ServicesContracts;
using SkinCa.DataAccess.Repositories;
using SkinCa.DataAccess.RepositoriesContracts;
using SkinCa.Services;

namespace SkinCa.Presentation;

public static class DI
{
    public static IServiceCollection RegisterBussinessDI(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
        serviceCollection.AddScoped<IBannerService,BannerService>();
        serviceCollection.AddScoped<IBookmarkService, BookmarkService>();
        serviceCollection.AddScoped<IDiseaseService,DiseaseService>();
        serviceCollection.AddScoped<IDoctorInfoService,DoctorInfoService>();
        serviceCollection.AddScoped<IChatService, ChatService>();
        serviceCollection.AddScoped<IMessageService, MessageService>();
        serviceCollection.AddTransient<IEmailService, EmailService>();
        serviceCollection.AddScoped<IScanResultService, ScanResultService>();
        return serviceCollection;
    }
    public static IServiceCollection RegisterRepositoriesDI(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBannerRepository,BannerRepository>();
        serviceCollection.AddScoped<IBookmarkRepository, BookmarkRepository>();
        serviceCollection.AddScoped<IDiseaseRepository,DiseaseRepository>();
        serviceCollection.AddScoped<IDoctorInfoRepository,DoctorInfoRepository>();
        serviceCollection.AddScoped<IScanResultRepository, ScanResultRepository>();
        serviceCollection.AddScoped<IChatRepository, ChatRepository>();
        serviceCollection.AddScoped<IMessageRepository, MessageRepository>();
        serviceCollection.AddScoped<IApplicationUserRepository, ApplicationUserChatRepository>();
        return serviceCollection;
    }
}