using System.Net;
using System.Net.Mail;
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
            using var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(network.Email.Trim(), network.Password);
            
            var senderEmail = network.Email.Trim();
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(senderEmail, "SkinCa"),
                To = { new MailAddress(email) },
                Subject = subject,
                IsBodyHtml = true,
                Body = message
            };
           
            await smtpClient.SendMailAsync(mail);
        }

        private string GetEmailTemplate(string title, string greeting, string content, string tokenValue, bool isPasswordReset = false)
        {
            var tokenClass = isPasswordReset ? "token" : "code";
            
            return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>{title}</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, Arial, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: #333333;
            line-height: 1.6;
            padding: 20px;
            margin: 0;
        }}
        
        .email-wrapper {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.15);
            overflow: hidden;
        }}
        
        .header {{
            background: linear-gradient(135deg, #4CAF50 0%, #45a049 100%);
            color: white;
            text-align: center;
            padding: 30px 20px;
        }}
        
        .header h1 {{
            font-size: 28px;
            font-weight: 300;
            margin: 0;
            text-shadow: 0 1px 3px rgba(0, 0, 0, 0.3);
        }}
        
        .content {{
            padding: 40px 30px;
            text-align: center;
        }}
        
        .greeting {{
            font-size: 18px;
            color: #555555;
            margin-bottom: 20px;
        }}
        
        .message {{
            font-size: 16px;
            color: #666666;
            margin-bottom: 30px;
            line-height: 1.8;
        }}
        
        .code, .token {{
            display: inline-block;
            background: linear-gradient(135deg, #007BFF 0%, #0056b3 100%);
            color: #ffffff;
            font-size: 32px;
            font-weight: 700;
            font-family: 'Courier New', monospace;
            padding: 20px 30px;
            margin: 25px 0;
            border-radius: 8px;
            letter-spacing: 4px;
            text-align: center;
            box-shadow: 0 4px 15px rgba(0, 123, 255, 0.3);
            border: 2px solid rgba(255, 255, 255, 0.2);
        }}
        
        .expiry {{
            font-size: 14px;
            color: #888888;
            margin-top: 20px;
            padding: 15px;
            background-color: #f8f9fa;
            border-left: 4px solid #007BFF;
            border-radius: 4px;
            text-align: left;
        }}
        
        .footer {{
            background-color: #f8f9fa;
            padding: 25px;
            text-align: center;
            border-top: 1px solid #e9ecef;
        }}
        
        .footer p {{
            font-size: 12px;
            color: #999999;
            margin: 0;
        }}
        
        .brand {{
            color: #4CAF50;
            font-weight: 600;
        }}
        
        /* Responsive styles */
        @media only screen and (max-width: 600px) {{
            body {{
                padding: 10px;
            }}
            
            .email-wrapper {{
                margin: 10px;
            }}
            
            .header {{
                padding: 25px 15px;
            }}
            
            .header h1 {{
                font-size: 24px;
            }}
            
            .content {{
                padding: 25px 20px;
            }}
            
            .code, .token {{
                font-size: 24px;
                padding: 15px 20px;
                letter-spacing: 2px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""email-wrapper"">
        <div class=""header"">
            <h1>{title}</h1>
        </div>
        
        <div class=""content"">
            <div class=""greeting"">{greeting}</div>
            <div class=""message"">{content}</div>
            <div class=""{tokenClass}"">{tokenValue}</div>
            <div class=""expiry"">
                <strong>⏱️ Important:</strong> This code expires in 2 minutes for security reasons.
            </div>
        </div>
        
        <div class=""footer"">
            <p>&copy; 2024 <span class=""brand"">SkinCa</span>. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
        }

        public async Task SendConfirmationEmail(string email, string token)
        {
            string message = GetEmailTemplate(
                title: "Email Verification",
                greeting: "Hello! 👋",
                content: "Thank you for signing up with SkinCa! Please use the verification code below to confirm your email address:",
                tokenValue: token,
                isPasswordReset: false
            );
            
            await SendEmailAsync(email, "Email Confirmation - SkinCa", message);
        }

        public async Task SendForgotPasswordEmail(string email, string token)
        {
            string message = GetEmailTemplate(
                title: "Password Reset",
                greeting: "Hello! 🔐",
                content: "We received a request to reset your password. Please use the verification code below to proceed with resetting your password:",
                tokenValue: token,
                isPasswordReset: true
            );
            
            await SendEmailAsync(email, "Reset Your Password - SkinCa", message);
        }
    }
}