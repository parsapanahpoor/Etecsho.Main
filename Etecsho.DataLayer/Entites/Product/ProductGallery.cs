using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Etecsho.Models.Entites.Product
{
    public class ProductGallery
    {

        public ProductGallery()
        {

        }
        [Key]
        public int ProductGalleryId { get; set; }
        public int ProductID { get; set; }

        [Display(Name = "عنوان تصویر    ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Title { get; set; }

        [MaxLength(50)]
        public string ImageName { get; set; }

        #region Relations

        public virtual Product Product { get; set; }

        #endregion

    }
}
