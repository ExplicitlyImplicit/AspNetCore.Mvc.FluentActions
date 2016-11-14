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
    public class FormController : Controller
    {
        [HttpGet]
        public ViewResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public string UploadFile(IFormFile file)
        {
            return $"Got file with name {file.FileName}!";
        }

        [HttpGet]
        public ViewResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public string UploadFiles(IEnumerable<IFormFile> files)
        {
            return $"Got {files.Count()} n.o. files!";
        }

        [HttpGet]
        public ViewResult SubmitWithAntiForgeryToken()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string SubmitWithAntiForgeryToken(string hiddenValue)
        {
            return "Got submission!";
        }

        [HttpGet]
        public ViewResult SubmitWithModelState()
        {
            return View();
        }

        [HttpPost]
        public string SubmitWithModelState(ModelStateFormModel model)
        {
            return ModelState.IsValid ? "Model valid! :)" : "Model invalid :(";
        }
    }
}
