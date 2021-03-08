using Etecsho.Models.Entites.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etecsho.DataAccess.Services.Slider
{
    public  interface ISliderService
    {
        #region SliderPanelAdmin
        void AddSlider(Models.Entites.Slider.Slider slider, IFormFile imgBlogUp, User user);

        List<Models.Entites.Slider.Slider> GetAllSliders();

        Models.Entites.Slider.Slider GetSliderById(int sliderid);

        void UpdateSlider(Models.Entites.Slider.Slider slider, IFormFile imgBlogUp);

        void DeleteSlider(Models.Entites.Slider.Slider slider);

        void UpdateSlider(Models.Entites.Slider.Slider slider);

        List<Models.Entites.Slider.Slider> GetAllDeletedSliders();

        #endregion

        #region SliderForForntEnd

        List<Models.Entites.Slider.Slider> GetSliderForShowInHomePage();

        #endregion


    }
}
