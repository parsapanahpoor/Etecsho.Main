using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Etecsho.Main.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Etecsho.Models.Entites.ContactUs;
using Etecsho.DataAccess.Services.Users;

namespace Etecsho.Main.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _user;


        public HomeController(IUserService user)
        {
            _user = user;
        }


        public IActionResult Index()
        {
            return View();
        }


        //AboutUS Page
        public IActionResult AboutUs()
        {

            return View();
        }



        #region ContactUS

        //ContactUS
        public IActionResult ContactUs()
        {

            return View();
        } 
        [HttpPost]
        public IActionResult ContactUs(ContactUs contactUs)
        {
            if (!ModelState.IsValid) { return View(contactUs); }


            _user.addMessage(contactUs);

            return View();
        }
        #endregion



        #region CKEditorFileUploader
        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/MyImages/"}{fileName}";


            return Json(new { uploaded = true, url });
        }


        #endregion

    }
}
