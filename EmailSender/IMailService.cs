using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.EmailSender
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
