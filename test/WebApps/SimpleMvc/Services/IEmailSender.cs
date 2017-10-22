using System.Threading.Tasks;

namespace SimpleMvc.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
