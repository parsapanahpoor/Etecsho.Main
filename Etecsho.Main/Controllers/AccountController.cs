using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etecsho.DataAccess.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Etecsho.Models.ViewModels;
using Etecsho.Utilities.Convertors;
using Etecsho.Utilities.Genarator;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Etecsho.Models.Entites.Users;
using Microsoft.AspNetCore.Authorization;

namespace Etecsho.Main.Controllers
{
    //این کنترل برای مدیریت بخش های در ارتباط با کاربر تا قبا از ورود می باشد  


    public class AccountController : Controller
    {
        private IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        #region Register


        [Route("Register")]
        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [Route("Register")]
        public ActionResult Register(RegisterViewModel register)
        {

            if (!ModelState.IsValid)
            {
                return View(register);
            }


            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری معتبر نمی باشد");
                return View(register);
            }

            if (_userService.IsExistEmail(FixedText.FixEmail(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل معتبر نمی باشد");
                return View(register);
            }
            if (_userService.IsExistPhoneNumber(FixedText.FixEmail(register.PhoneNumber)))
            {
                ModelState.AddModelError("PhoneNumber", "شماره ی وارد شده معتبر نمی باشد ");
                return View(register);
            }

            Etecsho.Models.Entites.Users.User user = new Etecsho.Models.Entites.Users.User()
            {
                ActiveCode = RandomNumberGenerator.GetNumber(),
                Email = FixedText.FixEmail(register.Email),
                IsActive = true,
                PhoneNumber = register.PhoneNumber,
                Password = register.Password,
                RegisterDate = DateTime.Now,
                UserAvatar = "Defult.jpg",
                UserName = register.UserName
            };
            _userService.AddUser(user);

            //#region Send Activation Email

            //string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            //SendEmail.Send(user.Email, "فعالسازی", body);

            //#endregion

            return Redirect("/Login?Register=true");
        }

        #endregion

        #region login


        [Route("Login")]
        public ActionResult Login(bool EditProfile = false, bool Register = false, bool recovery = false, bool permission = false)
        {
            ViewBag.EditProfile = EditProfile;
            ViewBag.permission = permission;
            ViewBag.Register = Register;
            ViewBag.recovery = recovery;
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userService.LoginUser(login);
            if (user != null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };
                    HttpContext.SignInAsync(principal, properties);

                    ViewBag.IsSuccess = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("phoneNumber", "حساب کاربری شما فعال نمی باشد");
                }
            }
            ModelState.AddModelError("phoneNumber", "کاربری با مشخصات وارد شده یافت نشد");
            return View(login);
        }


        #endregion

        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion

        #region ForgotPassword
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (ModelState.IsValid)
            {
                User user = _userService.GetUserByPhoneNumber(forgot.Mobile);
                if (user != null)
                {
                    if (user.IsActive)
                    {


                        //دراین بخش باید کد مربوط به ارسال اس ام اس قربر بگیرد 
                        //ارسال کد 5رقمی 

                        return Redirect("/Account/CheckTheSentCodeToMobile");
                    }

                }
                else
                {
                    ModelState.AddModelError("Mobile", "کاربری با این تلفن  یافت نشد.");
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult CheckTheSentCodeToMobile()
        {

            return View();

        }
        [HttpPost]
        public ActionResult CheckTheSentCodeToMobile(User users)
        {

            User user = _userService.GetUserByActiveCode(users.ActiveCode);
            if (user == null)
            {

                ModelState.AddModelError("ActiveCode", "کد وارد شده معتبر نمی باشد ");

                return View();

            }
            else
            {
                return Redirect("/Account/RecoveryPassword?id=" + user.UserId);

            }




        }



        public ActionResult RecoveryPassword(int id)
        {
            return View(new RecoverPasswordViewModel()
            {

                Userid = id
            });
        }
        [HttpPost]
        public ActionResult RecoveryPassword(RecoverPasswordViewModel recovery)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserById(recovery.Userid);
                if (user == null)
                {
                    return View("/View/Shared/Error.cshtml");
                }
                user.Password = recovery.Password;
                user.ActiveCode = Etecsho.Utilities.Genarator.RandomNumberGenerator.GetNumber();
                _userService.UpdateUser(user);
                return Redirect("/Login?recovery=true");
            }
            return View();
        }

        #endregion

        #region CheckUserRoleForLogin
        public IActionResult ManageUserForLogin()
        {

            List<int> UserRoles = _userService.GetUsersRoles(User.Identity.Name);

            if (UserRoles.Any() == false)
            {

                return Redirect("/User/Home/Index");

            }
            else
            {
                if (UserRoles.Contains(1))
                {

                    return Redirect("/Admin/Home/Index");

                }
                else
                {
                    if (UserRoles.Contains(4))
                    {
                        return Redirect("/Admin/Users/Index");
                    }
                    if (UserRoles.Contains(2))
                    {
                        return Redirect("/Admin/Users/Index");
                    }

                }



            }

            return View();

        }


        #endregion
    }
}
