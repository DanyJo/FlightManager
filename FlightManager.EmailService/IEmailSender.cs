using System.Threading.Tasks;

namespace FlightManager.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}
