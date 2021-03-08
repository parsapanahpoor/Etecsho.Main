using Etecsho.DataAccess.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etecsho.Main.ViewComponents
{
    public class UserPanalComponent : ViewComponent
    {
        private IUserService _user;
        public UserPanalComponent(IUserService user)
        {

            _user = user;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _user.GetUserByUserName(User.Identity.Name);

            return await Task.FromResult((IViewComponentResult)View("UserPanalComponent" , user));

        }

    }
}
