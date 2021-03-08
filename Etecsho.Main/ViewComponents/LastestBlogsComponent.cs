using Etecsho.DataAccess.Services.Blog;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etecsho.Main.ViewComponents
{
    public class LastestBlogsComponent : ViewComponent
    {
        private IBlogCategoryService _blog;

        public LastestBlogsComponent(IBlogCategoryService blog)
        {
            _blog = blog;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {


            return await Task.FromResult((IViewComponentResult)View("LastestBlogsComponent" , _blog.GetLastestBlogs()));

        }


    }
}
