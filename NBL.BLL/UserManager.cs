using System.Collections.Generic;
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

        public bool ValidateUser(User user)
        {
            bool result = false;
            var userByUserName = _userGateway.GetUserByUserName(user.UserName);
           
            if (!userByUserName.Equals(null))
            {
                var originalPasss = StringCipher.Decrypt(userByUserName.Password, "salam_cse_10_R");
                if (user.Password == originalPasss)
                {
                    result = true;
                }
              
            }
            return result;
        }
    }
}