using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Etecsho.DataAccess.Context;
using Etecsho.DataAccess.Services.Users;
using Etecsho.Models.Entites.Blog;
using Etecsho.Models.Entites.Users;
using Etecsho.Utilities.Convertors;
using Etecsho.Utilities.Genarator;
using Etecsho.Utilities.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Etecsho.DataAccess.Services.Blog
{
    public class BlogCategryService : IBlogCategoryService
    {

        private EtecshoContext _context;

        public BlogCategryService(EtecshoContext context)
        {
            _context = context;
        }

        public int AddBlog(Models.Entites.Blog.Blog blog, IFormFile imgBlogUp, User users)
        {
            blog.UserId = users.UserId;
            blog.IsActive = true;
            blog.CreateDate = DateTime.Now;
            blog.BlogImageName = "no-photo.png";  //تصویر پیشفرض
            //TODO Check Image
            if (imgBlogUp != null && imgBlogUp.IsImage())
            {
                blog.BlogImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBlogUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/image", blog.BlogImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBlogUp.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/thumb", blog.BlogImageName);

                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }


            _context.Add(blog);
            _context.SaveChanges();

            return blog.BlogId;
        }

        public void AddBlogCategory(BlogCategory blogCategory)
        {
            BlogCategory cat = new BlogCategory();
            cat.CategoryTitle = blogCategory.CategoryTitle;
            cat.IsDelete = false;
            cat.ParentId = blogCategory.ParentId;

            _context.BlogCategories.Add(cat);
            _context.SaveChanges();
        }

        public void AddCategoryToBlog(List<int> Categories, int BlogId)
        {
            foreach (var item in Categories)
            {
                _context.BlogSelectedCategories.Add(new BlogSelectedCategory()
                {

                    BlogCategoryId = item,
                    BlogId = BlogId

                });

                _context.SaveChanges();

            }
        }

        public void AddCategoryToVideo(List<int> Categories, int videoid)
        {
            foreach (var item in Categories)
            {
                _context.VideoSelectedCategory.Add(new VideoSelectedCategory()
                {

                    BlogCategoryId = item,
                    VideoId = videoid

                });

                _context.SaveChanges();

            }
        }

        public int AddVideo(Video video, IFormFile imgBlogUp, IFormFile demoUp, User user)
        {
            video.UserId = user.UserId;
            video.IsActive = true;
            video.CreateDate = DateTime.Now;
            video.VideoImageName = "no-photo.png";  //تصویر پیشفرض
            //TODO Check Image
            if (imgBlogUp != null && imgBlogUp.IsImage())
            {
                video.VideoImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBlogUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/image", video.VideoImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBlogUp.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/thumb", video.VideoImageName);

                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }

            if (demoUp != null)
            {
                video.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(demoUp.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/Videos", video.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    demoUp.CopyTo(stream);
                }
            }


            _context.Add(video);
            _context.SaveChanges();

            return video.VideoId;
        }

        public void DeleteBlog(Models.Entites.Blog.Blog blog)
        {
            blog.IsDelete = true;
            _context.Update(blog);
            _context.SaveChanges();
        }

        public void DeleteBlogCategory(int id)
        {
            BlogCategory blogCategory = GetBlogCategoryById(id);
            blogCategory.IsDelete = true;
            _context.Update(blogCategory);
            _context.SaveChanges();
        }

        public void DeleteVideos(Video video)
        {
            video.IsDelete = true;
            _context.Update(video);
            _context.SaveChanges();
        }

        public void EditBlogSelectedCategory(List<int> Categories, int BlogId)
        {
            _context.BlogSelectedCategories.Where(p => p.BlogId == BlogId).ToList()
                                        .ForEach(p => _context.BlogSelectedCategories.Remove(p));

            AddCategoryToBlog(Categories, BlogId);

        }

        public void EditeVideoSelectedCategory(List<int> Categories, int videoid)
        {


            var groups =  _context.VideoSelectedCategory.Where(p => p.VideoId == videoid).ToList();




            foreach (var item in groups)
            {
                _context.VideoSelectedCategory.Remove(item);

            }

            AddCategoryToVideo(Categories, videoid);
        }

        public List<BlogCategory> GetAllBlogCategories()
        {
            return _context.BlogCategories.ToList();
        }

        public List<Models.Entites.Blog.Blog> GetAllBlogs()
        {
            return _context.Blog.Include(p => p.Users).ToList();
        }

        public List<BlogSelectedCategory> GetAllBlogSelectedCategory()
        {
            return _context.BlogSelectedCategories.ToList();
        }

        public List<Models.Entites.Blog.Blog> GetAllDeletedBlogs()
        {
            IQueryable<Models.Entites.Blog.Blog> result = _context.Blog.Include(p=>p.Users)
                .IgnoreQueryFilters().Where(u => u.IsDelete);

            return result.ToList();
        }

        public List<Video> GetAllDeletedVideos()
        {
            IQueryable<Models.Entites.Blog.Video> result = _context.Video.Include(p=>p.Users)
                .IgnoreQueryFilters().Where(u => u.IsDelete);

            return result.ToList();
        }

        public List<Video> GetAllVideos()
        {
            return _context.Video.Include(p => p.Users).ToList();
        }

        public List<VideoSelectedCategory> GetAllVideoSelectedCategories()
        {
            return _context.VideoSelectedCategory.ToList();
        }

        public Models.Entites.Blog.Blog GetBlogById(int blogid)
        {
            return _context.Blog.Include(p => p.Users).FirstOrDefault(p => p.BlogId == blogid);
        }

        public BlogCategory GetBlogCategoryById(int id)
        {
            return _context.BlogCategories.Find(id);
        }

        public Tuple<List<Models.Entites.Blog.Blog>, int> GetBlogsForShowInHomePage(int? Categroyid, int pageId = 1, string filter = "", int take = 0)
        {
            if (take == 0) take = 8;

            IQueryable<Models.Entites.Blog.Blog> blogs = _context.Blog.Include(p => p.Users)
                                                            .Include(p => p.BlogSelectedCategory)
                                                                .Where(p=>p.IsActive)
                                                                .OrderByDescending(p=>p.CreateDate);


            if (!string.IsNullOrEmpty(filter))
            {
                blogs = _context.Blog.Where(c => c.BlogTitle.Contains(filter) || c.Tags.Contains(filter)).Include(p => p.Users);
            }

            if (Categroyid != null)
            {

                IQueryable<Models.Entites.Blog.Blog> blogid = _context.BlogSelectedCategories.Where(p => p.BlogCategoryId == Categroyid)
                                                                        .Include(p => p.Blog)
                                                                        .ThenInclude(p => p.Users)
                                                                               .Select(p => p.Blog);


                if (Categroyid != null)
                {
                    if (blogid.Any() && blogid != null)
                    {

                        blogs = blogid;

                    }
                }

            }

            int skip = (pageId - 1) * take;
            int pageCount = (blogs.Count() / take) ;

            if ((pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            var query = blogs.Skip(skip).Take(take).ToList();

            return Tuple.Create(query, pageCount);


        }

        public List<Models.Entites.Blog.Blog> GetLastestBlogs()
        {
            var blogs = _context.Blog.OrderByDescending(p=>p.CreateDate).ToList();
            if (blogs.Any() && blogs.Count >=3)
            {
                return blogs.Take(3).ToList();
            }
            return blogs;
        }

        public List<Video> GetLastestVideos()
        {
            var videos = _context.Video.OrderByDescending(p => p.CreateDate).ToList();
            if (videos.Any() && videos.Count >= 3)
            {
                return videos.Take(3).ToList();
            }
            return videos;
        }

        public string GetUserNameByBlog(int blogid)
        {

            return _context.Blog.Where(p => p.BlogId == blogid).Include(p => p.Users).Select(p => p.Users.UserName).Single();
        }

        public Video GetVideoById(int VideoId)
        {
            return _context.Video.Include(p => p.Users).Include(p=>p.VideoSelectedCategory)
                                .FirstOrDefault(p => p.VideoId == VideoId);
        }

        public Tuple<List<Video>, int> GetVideosForShowInHomePage(int? Categroyid, int pageId = 1, string filter = "", int take = 0)
        {
            if (take == 0) take = 4;

            IQueryable<Models.Entites.Blog.Video> blogs = _context.Video.Include(p => p.Users)
                                                            .Include(p => p.VideoSelectedCategory)
                                                                .Where(p=>p.IsActive)
                                                                .OrderByDescending(p=>p.CreateDate);


            if (!string.IsNullOrEmpty(filter))
            {
                blogs = _context.Video.Where(c => c.VideoTitle.Contains(filter) || c.Tags.Contains(filter)).Include(p => p.Users);
            }

            if (Categroyid != null)
            {

                IQueryable<Models.Entites.Blog.Video> blogid = _context.VideoSelectedCategory.Where(p => p.BlogCategoryId == Categroyid)
                                                                        .Include(p => p.Video)
                                                                        .ThenInclude(p => p.Users)
                                                                               .Select(p => p.Video);


                if (Categroyid != null)
                {
                    if (blogid.Any() && blogid != null)
                    {

                        blogs = blogid;

                    }
                }

            }

            int skip = (pageId - 1) * take;
            int pageCount = (blogs.Count() / take) ;

            if ((pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            var query = blogs.Skip(skip).Take(take).ToList();

            return Tuple.Create(query, pageCount);
        }

        public int UpdateBlog(Models.Entites.Blog.Blog blog, IFormFile imgBlogUp)
        {


            //TODO Check Image
            if (imgBlogUp != null && imgBlogUp.IsImage())
            {

                if (blog.BlogImageName != "no-photo.png")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/image", blog.BlogImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/thumb", blog.BlogImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }



                blog.BlogImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBlogUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/image", blog.BlogImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBlogUp.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/thumb", blog.BlogImageName);

                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }


            _context.Blog.Update(blog);
            _context.SaveChanges();

            return blog.BlogId;
        }

        public void UpdateBlog(Models.Entites.Blog.Blog blog)
        {
            _context.Update(blog);
            _context.SaveChanges();
        }

        public void UpdateBlogCategroy(BlogCategory blogCategory, int id)
        {
            BlogCategory category = GetBlogCategoryById(id);
            category.CategoryTitle = blogCategory.CategoryTitle;
            _context.BlogCategories.Update(category);
            _context.SaveChanges();
        }

        public void UpdateBlogForLock(Video video)
        {
            _context.Update(video);
            _context.SaveChanges();
        }

        public int UpdateVideo(Video video, IFormFile imgBlogUp, IFormFile demoUp)
        {
            
            //TODO Check Image
            if (imgBlogUp != null && imgBlogUp.IsImage())
            {

                if (video.VideoImageName != "no-photo.jpg")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/image", video.VideoImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/thumb", video.VideoImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }


                video.VideoImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBlogUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/image", video.VideoImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBlogUp.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/thumb", video.VideoImageName);

                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }

            if (demoUp != null)
            {

                if (video.DemoFileName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/Videos", video.DemoFileName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }
                }

                video.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(demoUp.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Blog/Videos", video.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    demoUp.CopyTo(stream);
                }
            }


            _context.Video.Update(video);
            _context.SaveChanges();

            return video.VideoId;
        }
    }
}
