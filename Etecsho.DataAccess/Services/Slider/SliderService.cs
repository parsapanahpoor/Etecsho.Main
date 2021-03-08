using Etecsho.DataAccess.Context;
using Etecsho.Models.Entites.Users;
using Etecsho.Utilities.Convertors;
using Etecsho.Utilities.Genarator;
using Etecsho.Utilities.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Etecsho.DataAccess.Services.Slider
{
    public class SliderService : ISliderService
    {
        private EtecshoContext _context;
        public SliderService(EtecshoContext context)
        {
            _context = context;
        }


        public void AddSlider(Models.Entites.Slider.Slider slider, IFormFile imgBlogUp, User user)
        {
            slider.UserId = user.UserId;
            slider.IsActive = true;
            slider.StartDate = DateTime.Now;
            slider.BlogImageName = "no-photo.png";  //تصویر پیشفرض

            if (imgBlogUp != null && imgBlogUp.IsImage())
            {
                slider.BlogImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBlogUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Slider/image", slider.BlogImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBlogUp.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Slider/thumb", slider.BlogImageName);

                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }

            _context.Slider.Add(slider);
            _context.SaveChanges();

        }

        public void DeleteSlider(Models.Entites.Slider.Slider slider)
        {
            slider.IsDelete = true;
            _context.Update(slider);
            _context.SaveChanges();
        }

        public List<Models.Entites.Slider.Slider> GetAllDeletedSliders()
        {
            return _context.Slider.Include(p => p.Users).IgnoreQueryFilters()
                                            .Where(p => p.IsDelete).ToList();
        }

        public List<Models.Entites.Slider.Slider> GetAllSliders()
        {
            return _context.Slider.Include(p => p.Users).ToList();
        }

        public Models.Entites.Slider.Slider GetSliderById(int sliderid)
        {
           return  _context.Slider.Include(p => p.Users).FirstOrDefault(p => p.SliderId == sliderid);
        }

        public List<Models.Entites.Slider.Slider> GetSliderForShowInHomePage()
        {
            return _context.Slider.Where(p => p.IsActive).ToList();
        }

        public void UpdateSlider(Models.Entites.Slider.Slider slider, IFormFile imgBlogUp)
        {
            if (imgBlogUp != null && imgBlogUp.IsImage())
            {

                if (slider.BlogImageName != "no-photo.png")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Slider/image", slider.BlogImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Slider/thumb", slider.BlogImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }



                slider.BlogImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBlogUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Slider/image", slider.BlogImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBlogUp.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Slider/thumb", slider.BlogImageName);

                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }


            _context.Slider.Update(slider);
            _context.SaveChanges();


        }

        public void UpdateSlider(Models.Entites.Slider.Slider slider)
        {
            _context.Update(slider);
            _context.SaveChanges();    
        }
    }
}
