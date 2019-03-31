using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RetreatSchedule.Config;
using RetreatSchedule.Data;

namespace RetreatSchedule.Areas.Identity.Services
{
    public class EmailSender : IRetreatEmailSender
    {
        private readonly RetreatDBContext _context;
        private readonly EmailConfig _emailConfig;

        public EmailSender(RetreatDBContext context, IOptions<EmailConfig> emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig?.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, bool sendAdmin)
        {
            using (var smtpClient = new SmtpClient
            {
                Host = _emailConfig.Host,
                Port = _emailConfig.Port,
                EnableSsl = _emailConfig.EnableTLS,
                Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password)
            })
            {
                using (var mail = new MailMessage(_emailConfig.Username, email)
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                })
                {
                    if (sendAdmin)
                    {
                        var emails = _context.Users.Where(x => !string.IsNullOrWhiteSpace(x.AspId)).Select(x => x.Email).ToList();
                        if (emails != null && emails.Count() != 0)
                        {
                            foreach (string adminEmail in emails)
                            {
                                mail.Bcc.Add(new MailAddress(adminEmail));
                            }
                        }
                    }                    

                    await smtpClient.SendMailAsync(mail);
                }
            }
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendEmailAsync(email, subject, htmlMessage, false);
        }
    }
}
