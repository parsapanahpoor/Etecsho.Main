using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Etecsho.DataAccess.Context;
using Etecsho.Models.Entites.Blog;
using Etecsho.DataAccess.Services.Blog;
using Microsoft.AspNetCore.Authorization;
using Etecsho.DataAccess.Security;

namespace Etecsho.Main.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BlogCategoriesController : Controller
    {
        private IBlogCategoryService _blogCategryService;

        public BlogCategoriesController( IBlogCategoryService blogCategryService)
        {
            _blogCategryService = blogCategryService;
        }
        [PermissionChecker(12)]

        public IActionResult Index(bool Create = false, bool Edit = false, bool Delete = false)
        {
            ViewBag.Create = Create;
            ViewBag.Edit = Edit;
            ViewBag.Delete = Delete;
            return View(_blogCategryService.GetAllBlogCategories());
        }

        [PermissionChecker(13)]

        public IActionResult Create(int? id)
        {
            return View(new BlogCategory()
                {

                   ParentId =    id
           
                 });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("BlogCategoryId,CategoryTitle,IsDelete,ParentId")] BlogCategory blogCategory)
        {
            if (ModelState.IsValid)
            {
                _blogCategryService.AddBlogCategory(blogCategory);
                return Redirect("/Admin/BlogCategories/Index?Create=true");


            }
            return View(blogCategory);
        }
        [PermissionChecker(14)]

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogCategory = _blogCategryService.GetBlogCategoryById((int)id);
            if (blogCategory == null)
            {
                return NotFound();
            }
            return View(blogCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, [Bind("BlogCategoryId,CategoryTitle,IsDelete,ParentId")] BlogCategory blogCategory)
        {
            if (id != blogCategory.BlogCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                    _blogCategryService.UpdateBlogCategroy(blogCategory, id);


                return Redirect("/Admin/BlogCategories/Index?Edit=true");
            }
            return View(blogCategory);
        }
        [PermissionChecker(15)]

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogCategory = _blogCategryService.GetBlogCategoryById((int)id);
            if (blogCategory == null)
            {
                return NotFound();
            }

            return View(blogCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {


            _blogCategryService.DeleteBlogCategory((int)id);


            return Redirect("/Admin/BlogCategories/Index?Delete=true");
        }

       
    }
}
