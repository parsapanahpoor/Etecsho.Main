using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etecsho.DataAccess.Security;
using Etecsho.DataAccess.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etecsho.Main.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [PermissionChecker(1)]


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


        #region ContactUs

        public IActionResult ContactUs()
        {

            return View(_user.GetAllMessages());

        }

        public IActionResult periwe(int id)
        {

            var message = _user.GetMessageById(id);


            return View(message);
        }

        #endregion
    }

}