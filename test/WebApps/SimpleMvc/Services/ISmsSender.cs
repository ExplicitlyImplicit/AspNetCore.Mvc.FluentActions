using System.Threading.Tasks;

namespace SimpleMvc.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
