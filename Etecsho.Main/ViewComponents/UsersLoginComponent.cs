using Etecsho.DataAccess.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etecsho.Main.ViewComponents
{
    public class UsersLoginComponent : ViewComponent
    {
        private IUserService _user;

        public UsersLoginComponent( IUserService user)
        {
            _user = user;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return await Task.FromResult((IViewComponentResult)View("LoginComponent"));
        
        }


    }
}
