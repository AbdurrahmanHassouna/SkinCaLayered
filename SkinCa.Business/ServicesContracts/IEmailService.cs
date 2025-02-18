namespace SkinCa.Business.ServicesContracts
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(string email,string token);
        Task SendForgotPasswordEmail(string email,string token);
    }
}
