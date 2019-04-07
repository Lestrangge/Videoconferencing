using System.Net.Mail;
using System.Threading.Tasks;

namespace VideoconferencingBackend.Interfaces.Adapters
{
    public interface ISmtpClientAdapter
    {
        /// <summary>
        /// Sends an email 
        /// </summary>
        /// <returns></returns>
        Task SendMailAsync(MailMessage message);
    }
}
