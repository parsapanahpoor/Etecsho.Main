using Etecsho.DataAccess.Services.Slider;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etecsho.Main.ViewComponents
{
     public class SldierComponent : ViewComponent
    {

        private ISliderService _slider ;

        public SldierComponent(ISliderService slider)
        {
            _slider = slider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {


            return await Task.FromResult((IViewComponentResult)View("SldierComponent", _slider.GetSliderForShowInHomePage()));

        }

    }
}
