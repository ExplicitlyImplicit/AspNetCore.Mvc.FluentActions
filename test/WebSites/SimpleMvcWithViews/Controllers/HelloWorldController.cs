using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SimpleMvcWithViews.Controllers
{
    public class HelloWorldController : Controller
    {
        public string HelloWorld()
        {
            return "Hello World!";
        }

        public async Task<string> HelloWorldAsync()
        {
            await Task.Delay(2000);
            return "Hello World Async!";
        }
    }
}
