using Etecsho.Models.Entites.ContactUs;
using Etecsho.Models.Entites.Users;
using Etecsho.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etecsho.DataAccess.Services.Users
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        bool IsExistPhoneNumber(string PhoneNumber);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByPhoneNumber(string PhoneNumber);
        User GetUserByActiveCode(string ActiveCode);
        User GetUserById(int Userid);
        void UpdateUser(User user);
        void DeleteUser(int userId);
        int GetUserIdByUserName(string userName);
        User GetUserByUserName(string username);
        List<int> GetUsersRoles(string username);


        #region User Panel

        InformationUserViewModel GetUserInformation(string username);
        InformationUserViewModel GetUserInformation(int userId);
        EditProfileViewModel GetDataForEditProfileUser(string username);
        void EditProfile(string username, EditProfileViewModel profile);
        bool CompareOldPassword(string oldPassword, string username);

        void ChangeUserPassword(string userName, string newPassword);

        List<User> GetUsersInRoles(int Role);

        #endregion

        #region Panel Admin
        List<User> GetUsers();
        List<User> GetDeleteUsers();
        int AddUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);
        SideBarUserPanelViewModel GetSideBarUserPanelData(string username);
        #endregion

        #region ContactUs


        void addMessage(ContactUs contactus);

        List<ContactUs> GetAllMessages();

        ContactUs GetMessageById(int id);

        #endregion

    }
}
