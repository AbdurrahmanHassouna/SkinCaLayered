using SkinCa.Common;
using SkinCa.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using SkinCa.Common.Exceptions;
using SkinCa.Presentation;
using SkinCa.Presentation.Hubs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var builderServices = builder.Services;

builderServices.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });

builderServices.Configure<JWT>(configuration.GetSection("JWT"));
builderServices.Configure<EmailSecrets>(configuration.GetSection("EmailSecrets"));

builderServices.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("SkinCa"));
});

builderServices.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Tokens.EmailConfirmationTokenProvider = "Custom";
        options.Tokens.PasswordResetTokenProvider = "Custom";
        options.Tokens.ChangeEmailTokenProvider = "Custom";
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<CustomTokenProvider<ApplicationUser>>("Custom");

builderServices.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["JWT:Issuer"],
        ValidAudience = configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
    };
    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hubs/chat")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builderServices.AddSignalR();

builderServices.RegisterBussinessDI();
builderServices.RegisterRepositoriesDI();
builderServices.AddTransient<ExceptionMiddleware>();


builderServices.AddEndpointsApiExplorer();
builderServices.AddSwaggerGen();

builderServices.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("https://www.postman.com", "https://web.postman.co")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});
var app = builder.Build();
app.UseCors("AllowPostman");
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");
app.Run();