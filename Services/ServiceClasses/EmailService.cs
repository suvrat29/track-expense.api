using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using track_expense.api.Services.Interfaces;

namespace track_expense.api.Services.ServiceClasses
{
    public class EmailService : IEmailService
    {
        #region Variables
        private readonly IConfiguration _configuration;
        private readonly IApplogService _applogService;
        #endregion

        #region Constructor
        public EmailService(IConfiguration configuration , IApplogService applogService)
        {
            _configuration = configuration;
            _applogService = applogService;
        }
        #endregion

        #region Public Methods
        public async Task SendEmailAsync(string from, string to, string subject, string html)
        {
            try
            {
                //create mail
                MimeMessage _mail = new MimeMessage();
                _mail.From.Add(MailboxAddress.Parse(from));
                _mail.To.Add(MailboxAddress.Parse(to));
                _mail.Subject = subject;
                _mail.Body = new TextPart(TextFormat.Html) { Text = html };

                //send mail
                using (SmtpClient smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(_configuration["EmailSettings:SMTPHost"], Convert.ToInt16(_configuration["EmailSettings:SMTPPort"]), SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_configuration["EmailSettings:SMTPUser"], _configuration["EmailSettings:SMTPPass"]);
                    await smtp.SendAsync(_mail);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "EmailService.cs", "SendEmailAsync()");
                throw;
            }
        }
        #endregion
    }
}
