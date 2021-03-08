using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Etecsho.Models.Entites.Blog
{
    public  class BlogSelectedCategory
    {

        public BlogSelectedCategory()
        {
                
        }

        [Key]
        public int BlogSelectedCategoryId { get; set; }

        public int BlogCategoryId { get; set; }
        public int BlogId { get; set; }

        #region Relations

        public virtual  BlogCategory BlogCategory { get; set; }

        public virtual  Blog Blog { get; set; }
        #endregion


    }
}
