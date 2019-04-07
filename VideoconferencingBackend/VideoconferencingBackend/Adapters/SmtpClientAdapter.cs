using System;
using System.Net.Mail;
using System.Threading.Tasks;
using VideoconferencingBackend.Interfaces.Adapters;

namespace VideoconferencingBackend.Wrappers
{
    public class SmtpClientAdapter : ISmtpClientAdapter
    {
        ///<inheritdoc/>
        public Task SendMailAsync(MailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
