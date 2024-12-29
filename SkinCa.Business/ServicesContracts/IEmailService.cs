namespace SkinCa.Business.ServicesContracts
{
    public interface IEmailService
    {
        Task SendEmailConfirmation(string email,string token);
        Task SendForgotPasswordEmail(string email,string token);
    }
}
