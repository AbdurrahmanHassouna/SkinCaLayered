namespace SkinCa.Business.ServicesContracts
{
    public interface IEmailService
    {
        Task<bool> SendConfirmationEmail(string email,string token);
        Task<bool> SendForgotPasswordEmail(string email,string token);
    }
}
