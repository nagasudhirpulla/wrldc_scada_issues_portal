using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ScadaIssuesPortal.App
{
    public class EmailSender : IEmailSender
    {
        EmailConfiguration EmailConfig { get; }
        public EmailSender(EmailConfiguration emailConfig)
        {
            EmailConfig = emailConfig;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine("Sending mail...");

            MailMessage message = new MailMessage
            {
                From = new MailAddress(EmailConfig.MailAddress),
                Subject = subject,
                IsBodyHtml = true,
                Body = htmlMessage
            };
            message.To.Add(email);

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = EmailConfig.HostName;
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(EmailConfig.Username, EmailConfig.Password, EmailConfig.Domain);
                smtpClient.Timeout = (60 * 5 * 1000);
                await smtpClient.SendMailAsync(message);
            }


            Console.WriteLine("Done sending mail...");
        }
    }
}
