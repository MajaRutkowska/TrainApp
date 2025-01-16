using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace TrainApp.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("TrainApp2025@gmail.com", "ixdf exct brns srbh"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("TrainApp2025@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            try
            {
                smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Wiadomość wysłana pomyślnie.");
            }

            catch (SmtpException ex)
            {
                // Wyświetl szczegóły błędu
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Szczegóły: {ex.InnerException}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas wysyłania wiadomości: {ex.Message}");
                throw; // Rzuć ponownie wyjątek, aby zobaczyć dokładny błąd
            }
            return Task.CompletedTask;
        }
    }
}
