using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SimpleMvcWithViews.Models;
using SimpleMvcWithViews.Models.AccountViewModels;
using SimpleMvcWithViews.Services;
using Microsoft.AspNetCore.Http;

namespace SimpleMvcWithViews.Controllers
{
    [Authorize]
    public class FormController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ViewResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public string UploadFile(IFormFile file)
        {
            return $"Got file with name {file.FileName}!";
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public string UploadFiles(IEnumerable<IFormFile> files)
        {
            return $"Got {files.Count()} n.o. files!";
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult SubmitWithAntiForgeryToken()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public string SubmitWithAntiForgeryToken(string hiddenValue)
        {
            return "Got submission!";
        }
    }
}
