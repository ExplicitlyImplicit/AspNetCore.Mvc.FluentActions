using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SimpleMvcWithViews.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Hello()
        {
            var hej = 1337;
            return View("~/views/users/list.cshtml", hej);
        }

        public IActionResult Index()
        {
            var hej = 1;
            return View("~/views/users/list.cshtml", hej);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            TempData["Text"] = "TempData message from HomeController.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
