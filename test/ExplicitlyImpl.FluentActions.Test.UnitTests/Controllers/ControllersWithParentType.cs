using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithParentTypeReturnsString : BaseController
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }
}
