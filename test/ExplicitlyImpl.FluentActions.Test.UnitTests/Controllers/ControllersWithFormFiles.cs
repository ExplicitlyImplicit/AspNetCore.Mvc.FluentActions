using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithFormFileReturnsString : Controller
    {
        [HttpPost]
        [Route("/route/url")]
        public string HandlerAction(IFormFile file)
        {
            return $"Got file with name {file.FileName}!";
        }
    }

    public class ControllerWithFormFileReturnsStringAsync : Controller
    {
        [HttpPost]
        [Route("/route/url")]
        public async Task<string> HandlerAction(IFormFile file)
        {
            await Task.Delay(1);
            return $"Got file with name {file.FileName}!";
        }
    }

    public class ControllerWith2FormFilesReturnsString : Controller
    {
        [HttpPost]
        [Route("/route/url")]
        public string HandlerAction(IEnumerable<IFormFile> files)
        {
            return $"Got {files.Count()} n.o. files!";
        }
    }

    public class ControllerWith2FormFilesReturnsStringAsync : Controller
    {
        [HttpPost]
        [Route("/route/url")]
        public async Task<string> HandlerAction(IEnumerable<IFormFile> files)
        {
            await Task.Delay(1);
            return $"Got {files.Count()} n.o. files!";
        }
    }
}
