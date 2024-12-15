using SkinCa.Common;
using SkinCa.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var _configuration = builder.Configuration;
var builderServices = builder.Services;
builderServices.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builderServices.Configure<JWT>(_configuration.GetSection("JWT"));
builderServices.Configure<EmailSecrets>(_configuration.GetSection("EmailSecrets"));

builderServices.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(_configuration.GetConnectionString("SkinCa"));
}); 

builderServices.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Tokens.EmailConfirmationTokenProvider = "Custom";
        options.Tokens.PasswordResetTokenProvider = "Custom";
        options.Tokens.ChangeEmailTokenProvider = "Custom";
        options.Password.RequireNonAlphanumeric = false;
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
        ValidIssuer = _configuration["JWT:Issuer"],
        ValidAudience = _configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
    };
});


builderServices.AddEndpointsApiExplorer();
builderServices.AddSwaggerGen();

var app = builder.Build();

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

app.Run();
