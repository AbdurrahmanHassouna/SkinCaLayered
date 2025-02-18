using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkinCa.Business.ServicesContracts;
using SkinCa.Common;

namespace SkinCa.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSecrets network;
        private ILogger<EmailService> _logger;
        public EmailService(IOptions<EmailSecrets> options, ILogger<EmailService> logger)
        {
            network = options.Value;
            _logger = logger;
        }

        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var stmpClient = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(network.Email, network.Password),
            };
            var senderEmail = network.Email.Trim();
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(senderEmail, "SkinCa Email"),
                To = { new MailAddress(email) },
                Subject = subject,
                IsBodyHtml = true,
                Body = message
            };
            
            await stmpClient.SendMailAsync(mail);
            
        }

        public async Task SendConfirmationEmail(string email, string token)
        {
            string message = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        color: #333;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border: 1px solid #ddd;
                        border-radius: 8px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    h1 {{
                        color: #4CAF50;
                        text-align: center;
                    }}
                    p {{
                        font-size: 16px;
                        margin: 10px 0;
                    }}
                    .code {{
                        display: inline-block;
                        background-color: #007BFF;
                        color: #ffffff;
                        font-size: 24px;
                        font-weight: bold;
                        padding: 10px 20px;
                        margin: 20px auto;
                        border-radius: 5px;
                        letter-spacing: 2px;
                        text-align: center;
                    }}
                    .footer {{
                        margin-top: 20px;
                        font-size: 12px;
                        text-align: center;
                        color: #777;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Verification Code</h1>
                    <p>Hello,</p>
                    <p>Your verification code is:</p>
                    <div class='code'>{token}</div>
                    <p>Please use this code to complete your verification. This code is valid for 10 minutes.</p>
                    <div class='footer'>
                        <p>&copy; 2024 SkinCa. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>
            ";
            await SendEmailAsync(
                email,
                "Email Confirmation",
                message);
        }

        public async Task SendForgotPasswordEmail(string email, string token)
        {
            string message = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        color: #333;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border: 1px solid #ddd;
                        border-radius: 8px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    h1 {{
                        color: #4CAF50;
                        text-align: center;
                    }}
                    p {{
                        font-size: 16px;
                        margin: 10px 0;
                    }}
                    .code {{
                        display: inline-block;
                        background-color: #007BFF;
                        color: #ffffff;
                        font-size: 24px;
                        font-weight: bold;
                        padding: 10px 20px;
                        margin: 20px auto;
                        border-radius: 5px;
                        letter-spacing: 2px;
                        text-align: center;
                    }}
                    .footer {{
                        margin-top: 20px;
                        font-size: 12px;
                        text-align: center;
                        color: #777;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Reset Password</h1>
                    <p>Hello,</p>
                    <p>Your verification token is:</p>
                    <div class='token'>{token}</div>
                    <p>Please use this code to complete your verification. This code is valid for 10 minutes.</p>
                    <div class='footer'>
                        <p>&copy; 2024 SkinCa. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>
        ";
            await SendEmailAsync(
                email,
                "Reset Password",
                message);
        }
    }
}