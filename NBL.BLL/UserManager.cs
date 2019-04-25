using System.Collections.Generic;
using System.Web;
using AutoMapper;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Securities;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
   
    public class UserManager:IUserManager
    {
        readonly UserGateway _userGateway = new UserGateway();
        public IEnumerable<User> GetAll => _userGateway.GetAll;
       public User GetUserInformationByUserId(int userId)
        {
            return _userGateway.GetUserInformationByUserId(userId);
        }
        public IEnumerable<User> GetAllUserForAutoComplete()
        {
            return _userGateway.GetAllUserForAutoComplete();
        }
        public ViewUser GetUserByUserNameAndPassword(string userName, string password)
        {
            return _userGateway.GetUserByUserNameAndPassword(userName, password);
        }
        

        public bool ChangeLoginStatus(ViewUser user, int status)
        {
            return _userGateway.ChangeLoginStatus(user, status);
        }

        public string AddNewUser(User user)
        {
            int rowAffected = _userGateway.AddNewUser(user);
            if (rowAffected > 0)
            {
                return "User Added Successfully!";
            }
            return "Failed to add user";
        }
        public User GetUserByUserName(string userName)
        {
            return _userGateway.GetUserByUserName(userName);
        }
        public User GetUserByUserName1(string userName)
        {
         
            var anUser = GetUserByUserName(userName);
            anUser.Roles = "SalesManager";
            return anUser;
        }
        public bool ValidateUser(User user)
        {
            bool result = false;
            var userByUserName = _userGateway.GetUserByUserName(user.UserName);
           
            if (userByUserName !=null)
            {
                var originalPasss = StringCipher.Decrypt(userByUserName.Password, "salam_cse_10_R");
                if (user.Password == originalPasss)
                {
                    result = true;
                }
              
            }
            return result;
        }


        public bool UpdatePassword(User model)
        {
            int rowAffected = _userGateway.UpdatePassword(model);
            return rowAffected > 0;
        }

        public ICollection<ViewAssignedUserRole> GetAssignedUserRolesByUserId(int userId)
        {
            return _userGateway.GetAssignedUserRolesByUserId(userId);
        }
    }
}