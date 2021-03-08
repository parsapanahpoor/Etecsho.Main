using Etecsho.DataAccess.Context;
using Etecsho.Models.Entites.Users;
using Etecsho.Models.ViewModels;
using Etecsho.Utilities.Convertors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Etecsho.Utilities.Genarator;
using System.IO;
using Etecsho.Models.Entites.ContactUs;

namespace Etecsho.DataAccess.Services.Users
{
    public class UserService : IUserService
    {
        private EtecshoContext _context;

        public UserService(EtecshoContext context)
        {
            _context = context;
        }
        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsExistPhoneNumber(string PhoneNumber)
        {
            return _context.Users.Any(p=>p.PhoneNumber == PhoneNumber);
        }
        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(LoginViewModel login)
        {
            string PhoneNumber = FixedText.FixEmail(login.phoneNumber);
            return _context.Users.SingleOrDefault(u => u.PhoneNumber == PhoneNumber && u.Password == login.Password);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByPhoneNumber(string PhoneNumber)
        {
            return _context.Users.FirstOrDefault(p=>p.PhoneNumber == PhoneNumber.Trim().ToLower());
        }

        public User GetUserByActiveCode(string ActiveCode)
        {
            return _context.Users.FirstOrDefault(p=>p.ActiveCode == ActiveCode);
        }

        public User GetUserById(int Userid)
        {
            return _context.Users.Find(Userid);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public List<User> GetUsers()
        {


            return _context.Users.ToList();
        }

        public List<User> GetDeleteUsers()
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u => u.IsDelete);

            return result.ToList();
        }

        public int AddUserFromAdmin(CreateUserViewModel user)
        {
            User addUser = new User();
            addUser.Password = user.Password;
            addUser.PhoneNumber = user.PhoneNumber;
            addUser.ActiveCode = Etecsho.Utilities.Genarator.RandomNumberGenerator.GetNumber();
            addUser.Email = user.Email;
            addUser.IsActive = true;
            addUser.RegisterDate = DateTime.Now;
            addUser.UserName = user.UserName;
            addUser.IsDelete = false;

            #region Save Avatar

            if (user.UserAvatar != null)
            {
                string imagePath = "";
                addUser.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(user.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", addUser.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }
            }

            #endregion

            return AddUser(addUser);

        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel()
                {
                    UserId = u.UserId,
                    PhoneNumber = u.PhoneNumber,
                    AvatarName = u.UserAvatar,
                    Password = u.Password,
                    Email = u.Email,
                    UserName = u.UserName,
                    UserRoles = u.UserRoles.Select(r => r.RoleId).ToList()
                }).Single();
        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = GetUserById(editUser.UserId);
            user.Email = editUser.Email;
            user.PhoneNumber = editUser.PhoneNumber;
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = editUser.Password;
            }

            if (editUser.UserAvatar != null)
            {
                //Delete old Image
                if (editUser.AvatarName != "Defult.jpg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }

                //Save New Image
                user.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.Single(u => u.UserName == userName).UserId;
        }

        public void DeleteUser(int userId)
        {
            User user = GetUserById(userId);
            user.IsDelete = true;
            UpdateUser(user);
        }

        public SideBarUserPanelViewModel GetSideBarUserPanelData(string username)
        {
            return _context.Users.Where(p => p.UserName == username).Select(p => new SideBarUserPanelViewModel()
            {

                UserName = p.UserName,
                ImageName = p.UserAvatar

            }).Single();
        }
        public User GetUserByUserName(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username);
        }
        public InformationUserViewModel GetUserInformation(string username)
        {
            var user = GetUserByUserName(username);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisterDate = user.RegisterDate;
            information.PhoneNumber = user.PhoneNumber;

            return information;

        }

        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserById(userId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisterDate = user.RegisterDate;
            information.PhoneNumber = user.PhoneNumber;

            return information;
        }

  

        public EditProfileViewModel GetDataForEditProfileUser(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new EditProfileViewModel()
            {
                AvatarName = u.UserAvatar,
                Email = u.Email,
                UserName = u.UserName,
                PhoneNumber = u.PhoneNumber

            }).Single();
        }

        public void EditProfile(string username, EditProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.AvatarName != "Defult.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                profile.AvatarName = NameGenerator.GenerateUniqCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }

            }

            var user = GetUserByUserName(username);
            user.Email = profile.Email;
            user.PhoneNumber = profile.PhoneNumber;
            user.UserAvatar = profile.AvatarName;

            UpdateUser(user);

        }

        public bool CompareOldPassword(string oldPassword, string username)
        {
            string hashOldPassword = oldPassword;
            return _context.Users.Any(u => u.UserName == username && u.Password == hashOldPassword);
        }

        public void ChangeUserPassword(string userName, string newPassword)
        {
            var user = GetUserByUserName(userName);
            user.Password = newPassword;
            UpdateUser(user);
        }

        public List<User> GetUsersInRoles(int Role)
        {
            var userid = _context.UsersRoles.Where(p => p.RoleId == Role).Select(p => p.UserId).ToList();
            var users = _context.Users.Where(p => userid.Contains(p.UserId)).ToList();
            return users;
        }

        public List<int> GetUsersRoles(string username)
        {
            User user = GetUserByUserName(username);
            return _context.UsersRoles.Where(p => p.UserId == user.UserId).Select(p => p.RoleId).ToList();
        }

        public void addMessage(ContactUs contactus)
        {
            ContactUs contact = new ContactUs()
            {

                Email = contactus.Email,
                PhoneNumber = contactus.PhoneNumber,
                UserName = contactus.UserName,
                ShortDescription = contactus.ShortDescription,
                LongDescription = contactus.LongDescription



            };



            _context.ContactUs.Add(contact);
            _context.SaveChanges();
        }

        public List<ContactUs> GetAllMessages()
        {
           return  _context.ContactUs.ToList();
        }

        public ContactUs GetMessageById(int id)
        {
            return _context.ContactUs.Find(id);
        }
    }
}
