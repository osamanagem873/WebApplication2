using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace WebApplication2.Models
{
    public class SendMail : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (MailMessage mailMessage = new MailMessage()) 
            {
                mailMessage.From = new MailAddress("");
                mailMessage.Subject = subject;
                mailMessage.Body = email + htmlMessage;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(""));
                SmtpClient smtp= new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                System.Net.NetworkCredential networkCred = new System.Net.NetworkCredential();
                networkCred.UserName = "";
                networkCred.Password = "";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCred;
                smtp.Port = 25;
                await smtp.SendMailAsync(mailMessage);
            }
        }
    }
}
