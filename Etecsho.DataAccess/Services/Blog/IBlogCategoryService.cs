using Etecsho.Models.Entites.Blog;
using Etecsho.Models.Entites.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Etecsho.DataAccess.Services.Blog
{
    public interface IBlogCategoryService
    {
        #region BlogCategory
        BlogCategory GetBlogCategoryById(int id);
        List<BlogCategory> GetAllBlogCategories();
        void AddBlogCategory(BlogCategory blogCategory);
        void UpdateBlogCategroy(BlogCategory blogCategory, int id);
        void DeleteBlogCategory(int id);

        #endregion


        #region Videos
        List<Models.Entites.Blog.Video> GetAllVideos();
        List<Models.Entites.Blog.Video> GetAllDeletedVideos();

        int AddVideo(Models.Entites.Blog.Video video, IFormFile imgBlogUp, IFormFile demoUp, User user);
        int UpdateVideo(Models.Entites.Blog.Video video, IFormFile imgBlogUp, IFormFile demoUp);

        void AddCategoryToVideo(List<int> Categories, int videoid);

        List<VideoSelectedCategory> GetAllVideoSelectedCategories();
        Models.Entites.Blog.Video GetVideoById(int VideoId);
        void EditeVideoSelectedCategory(List<int> Categories, int videoid);
        void DeleteVideos(Models.Entites.Blog.Video video);
        void UpdateBlogForLock(Models.Entites.Blog.Video video);


        #endregion
        #region Blogs

        List<Models.Entites.Blog.Blog> GetAllBlogs();

        int AddBlog(Models.Entites.Blog.Blog blog, IFormFile imgBlogUp, User user);
        void AddCategoryToBlog(List<int> Categories, int BlogId);

        int UpdateBlog(Models.Entites.Blog.Blog blog, IFormFile imgBlogUp);

        Models.Entites.Blog.Blog GetBlogById(int blogid);

        List<BlogSelectedCategory> GetAllBlogSelectedCategory();

        void EditBlogSelectedCategory(List<int> Categories, int BlogId);

        string GetUserNameByBlog(int blogid);

        void UpdateBlog(Models.Entites.Blog.Blog blog);

        void DeleteBlog(Models.Entites.Blog.Blog blog);

        List<Models.Entites.Blog.Blog> GetAllDeletedBlogs();


        #endregion

        #region HomPage

        Tuple<List<Models.Entites.Blog.Blog>, int> GetBlogsForShowInHomePage(int? Categroyid, int pageId = 1, string filter = "",
                        int take = 0);

        Tuple<List<Models.Entites.Blog.Video>, int> GetVideosForShowInHomePage(int? Categroyid, int pageId = 1, string filter = "",
               int take = 0);
        List<Models.Entites.Blog.Blog> GetLastestBlogs();
        List<Models.Entites.Blog.Video> GetLastestVideos();


        #endregion




    }
}
