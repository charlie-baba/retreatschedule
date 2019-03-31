using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Areas.Identity.Services
{
    public interface IRetreatEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);

        Task SendEmailAsync(string email, string subject, string htmlMessage, bool sendAdmin);
    }
}
