using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Etecsho.Models.Entites.Product
{
    public  class ProductCategories
    {

        public ProductCategories()
        {

        }

        [Key]
        public int ProductCategoryId { get; set; }

        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CategoryTitle { get; set; }

        [Display(Name = "حذف شده ؟")]
        public bool IsDelete { get; set; }

        [Display(Name = "گروه اصلی")]
        public int? ParentId { get; set; }


        #region Relations

        public virtual List<ProductSelectedCategory> ProductSelectedCategory { get; set; }

        #endregion



    }
}
