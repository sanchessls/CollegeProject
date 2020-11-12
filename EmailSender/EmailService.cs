using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IMailService mailService;

        public EmailSender(IMailService mailService)
        {
            this.mailService = mailService;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            MailRequest ml = new MailRequest()
            {
                Body = message,
                Subject = subject,
                ToEmail = email
            };

            return mailService.SendEmailAsync(ml);
        }
    
    }
}
