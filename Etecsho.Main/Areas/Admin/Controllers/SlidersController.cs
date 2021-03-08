using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etecsho.DataAccess.Context;
using Etecsho.Models.Entites.Slider;
using Etecsho.DataAccess.Services.Blog;
using Etecsho.DataAccess.Services.Users;
using Etecsho.DataAccess.Services.Comment;
using Microsoft.AspNetCore.Http;
using Etecsho.DataAccess.Services.Slider;

namespace Etecsho.Main.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {

        private IBlogCategoryService _blog;
        private IUserService _userservice;
        private ICommentService _comment;
        private ISliderService _slider;

        public SlidersController(IBlogCategoryService blog, IUserService userService, ICommentService comment, ISliderService slider)
        {
            _blog = blog;
            _userservice = userService;
            _comment = comment;
            _slider = slider;
        }


        public IActionResult Index(bool Create = false, bool Edit = false, bool Delete = false)
        {
            ViewBag.Create = Create;
            ViewBag.Edit = Edit;
            ViewBag.Delete = Delete;


            return View(_slider.GetAllSliders());
        }

        public IActionResult DeletedSldiers()
        {

            return View(_slider.GetAllDeletedSliders());

        }

        public IActionResult Details(int? id, bool Delete = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Delete != false)
            {
                ViewData["Delete"] = Delete;
            }

            var slider = _slider.GetSliderById((int)id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // GET: Admin/Sliders/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SliderId,UserId,FirstText,SecondeText,ThirdText,Link,BlogImageName,StartDate,EndDatetDate,IsActive,IsDelete")] Slider slider, IFormFile imgBlogUp)
        {
            if (ModelState.IsValid)
            {
                var user = _userservice.GetUserByUserName(User.Identity.Name);
                _slider.AddSlider(slider, imgBlogUp, user);


                return Redirect("/Admin/Sliders/Index?Create=true");
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = _slider.GetSliderById((int)id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("SliderId,UserId,FirstText,SecondeText,ThirdText,Link,BlogImageName,StartDate,EndDatetDate,IsActive,IsDelete")] Slider slider, IFormFile imgBlogUp)
        {
            if (id != slider.SliderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                _slider.UpdateSlider(slider, imgBlogUp);

                return Redirect("/Admin/Videos/Index?Edit=true");
            }
            return View(slider);
        }

        public IActionResult Delete(int id)
        {
            var slider = _slider.GetSliderById(id);
            _slider.DeleteSlider(slider);
            return Redirect("/Admin/Sliders/Index?Delete=true");
        }


        public IActionResult LockSlider(int SliderId, int id)
        {
            var slider = _slider.GetSliderById(SliderId);

            if (id == 1)
            {
                slider.IsActive = false;

            }
            if (id == 2)
            {
                slider.IsActive = true;

            }

            _slider.UpdateSlider(slider);
            return RedirectToAction(nameof(Index));
        }
    }
}
