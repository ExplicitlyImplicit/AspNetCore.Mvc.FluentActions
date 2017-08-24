using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class BaseController : Controller
    {
        public string Hello(string name = "Unnamed player")
        {
            return $"Hello {name}!";
        }
    }

    public class BaseController2 : Controller
    {
    }
}
