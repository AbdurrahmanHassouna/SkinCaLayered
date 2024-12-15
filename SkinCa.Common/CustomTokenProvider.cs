using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OtpNet;

namespace SkinCa.Common
{
    public class CustomTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : class
    {
        private readonly ILogger<CustomTokenProvider<TUser>> _logger;

        public CustomTokenProvider(ILogger<CustomTokenProvider<TUser>> logger)
        {
            _logger = logger;
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var securityStamp = await manager.GetSecurityStampAsync(user);
            if (securityStamp == null)
            {
                _logger.LogWarning("Security stamp is null for user.");
                throw new InvalidOperationException("Security stamp is required to generate a token.");
            }

            var totp = new Totp(Base32Encoding.ToBytes(securityStamp), step: 60, totpSize: 4);
            return totp.ComputeTotp();
        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            var securityStamp = await manager.GetSecurityStampAsync(user);
            if (securityStamp == null)
            {
                _logger.LogWarning("Security stamp is null for user.");
                return false;
            }

            var totp = new Totp(Base32Encoding.ToBytes(securityStamp), step: 60, totpSize: 4);
            return totp.VerifyTotp(token, out _, new VerificationWindow(previous: 1, future: 1));
        }

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            
            return Task.FromResult(true);
        }
    }
}