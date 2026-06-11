using System.Net;
using System.Net.Mail;
using AttendanceSystem.Application;
using Microsoft.Extensions.Configuration;

namespace AttendanceSystem.Infrastructure;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendAsync(string to, string subject, string body)
    {
        IConfiguration smtp = configuration.GetSection("Smtp");
        
        using SmtpClient client = new(smtp["Host"], int.Parse(smtp["Port"]!))
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(smtp["Username"], smtp["Password"])
        };

        MailMessage message = new (smtp["From"]!, to, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}