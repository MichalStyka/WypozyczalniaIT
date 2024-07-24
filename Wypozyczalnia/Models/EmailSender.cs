using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Wypozyczalnia.Models
{
    public class EmailSender:IEmailSender
    {

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "Projekt2024MLS@outlook.com";
            var pw = "Projekt2024!";

            var client = new SmtpClient("smtp.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(
                new MailMessage(from: mail,
                                to: email,
                                subject,
                                message
                                ));


        }
        }
}
