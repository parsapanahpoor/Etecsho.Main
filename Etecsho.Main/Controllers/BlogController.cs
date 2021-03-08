using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etecsho.DataAccess.Services.Blog;
using Etecsho.DataAccess.Services.Comment;
using Etecsho.DataAccess.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Etecsho.Main.Controllers
{
    public class BlogController : Controller
    {
        private IBlogCategoryService _blog;
        private IUserService _user;
        private ICommentService _comment;
        public BlogController(IBlogCategoryService blog , IUserService user , ICommentService comment)
        {
            _blog = blog;
            _user = user;
            _comment = comment;
        }


        #region Blogs
        public IActionResult Index(int? Categroyid, int pageId = 1, string filter = "")
        {

            ViewBag.Groups = _blog.GetAllBlogCategories();
            ViewBag.pageId = pageId;
            ViewBag.Categroyid = Categroyid;
            ViewBag.Filter = filter;



            return View(_blog.GetBlogsForShowInHomePage(Categroyid, pageId, filter, 4));
        }

        public IActionResult SingleBlogsPage(int id)
        {
            Etecsho.Models.Entites.Blog.Blog blog = _blog.GetBlogById(id);


            return View(blog);
        }
        #endregion


        #region Videos

        public IActionResult Video(int? Categroyid, int pageId = 1, string filter = "")
        {

            ViewBag.Groups = _blog.GetAllBlogCategories();
            ViewBag.pageId = pageId;
            ViewBag.Categroyid = Categroyid;
            ViewBag.Filter = filter;



            return View(_blog.GetVideosForShowInHomePage(Categroyid, pageId, filter, 4));
        }
        public IActionResult SingleVideoPage(int id)
        {
            Etecsho.Models.Entites.Blog.Video blog = _blog.GetVideoById(id);


            return View(blog);
        }

        #endregion

        #region BlogsComments

        [HttpPost]
        public IActionResult CreateCommente(Etecsho.Models.Entites.Comment.Comment comment)
        {
            comment.IsDelete = false;
            comment.CreateDate = DateTime.Now;
            comment.ProductTypeId = 2;
            comment.UserId = _user.GetUserIdByUserName(User.Identity.Name);
            _comment.AddComment(comment);

            return View("ShowComment", _comment.GetBlogComment((int)comment.BlogId));
        }

        public IActionResult ShowComment(int id, int pageId = 1)
        {
            return View(_comment.GetBlogComment(id, pageId));
        }
        #endregion

        #region VideoComments

        public IActionResult CreateVideoComments(Etecsho.Models.Entites.Comment.Comment comment)
        {
            comment.IsDelete = false;
            comment.CreateDate = DateTime.Now;
            comment.ProductTypeId = 3;
            comment.UserId = _user.GetUserIdByUserName(User.Identity.Name);
            _comment.AddComment(comment);

            return View("ShowCommentVideo", _comment.GetVideoComment((int)comment.VideoId));
        }

        public IActionResult ShowCommentVideo(int id, int pageId = 1)
        {
            return View(_comment.GetVideoComment(id, pageId));
        }

        #endregion

    }
}
