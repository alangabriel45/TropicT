using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TropicTrail.Utils;

namespace TropicTrail.Repository
{
    public class UserManager
    {
        private BaseRepository<UserAccount> _userAcc;
        private BaseRepository<UserInformation> _userInf;

        public UserManager()
        {
            _userAcc = new BaseRepository<UserAccount>();
            _userInf = new BaseRepository<UserInformation>();
        }

        #region Get User By ---
        public UserAccount GetUserById(String Id)
        {
            return _userAcc.Get(Id);
        }
        public List<UserAccount> ListOfUsers()
        {
            return _userAcc._table.ToList();
        }
        public UserAccount GetUserByUsername(String username)
        {
            return _userAcc._table.Where(m => m.username == username).FirstOrDefault();
        }
        public UserInformation GetInformationByUserId(String userId)
        {
            return _userInf._table.Where(m => m.userId == userId).FirstOrDefault();
        }
        public UserAccount GetUserByEmail(String email)
        {
            return _userAcc._table.Where(m => m.email == email).FirstOrDefault();
        }
        #endregion
        public ErrorCode SignIn(String username, String password, ref String errMsg)
        {
            var userSignIn = GetUserByUsername(username);
            if (userSignIn == null)
            {
                errMsg = "User not exist!";
                return ErrorCode.Error;
            }

            if (!userSignIn.password.Equals(password))
            {
                errMsg = "Password is Incorrect";
                return ErrorCode.Error;
            }

            // user exist
            errMsg = "Login Successful";
            return ErrorCode.Success;
        }

        public ErrorCode SignUp(UserAccount u, UserInformation ui, ref String errMsg)
        {
            u.userId = Utilities.gUid;
            u.date_created = DateTime.Now;
            u.roleId = 1;
            u.status = (Int32)Status.InActive;
            ui.email = u.email;
            ui.userId = u.userId;

            if (GetUserByUsername(u.username) != null)
            {
                errMsg = "Username Already Exist";
                return ErrorCode.Error;
            }

            if (GetUserByEmail(u.email) != null)
            {
                errMsg = "Email Already Exist";
                return ErrorCode.Error;
            }

            if (_userAcc.Create(u, out errMsg) != ErrorCode.Success)
            {
                return ErrorCode.Error;
            }
            if (_userInf.Create(ui, out errMsg) != ErrorCode.Success)
            {
                return ErrorCode.Error;
            }
            // use the generated code for OTP "ua.code"
            // send email or sms here...........

            return ErrorCode.Success;
        }
        public ErrorCode UpdateUser(UserAccount ua, ref String errMsg)
        {
            return _userAcc.Update(ua.id, ua, out errMsg);
        }
        public ErrorCode UpdateUserStatus(int userId, int newStatus, ref string errMsg)
        {
            // First, retrieve the user account by its ID
            var user = _userAcc.Get(userId);

            if (user != null)
            {
                // Update the status field
                user.status = newStatus;

                // Now, call the Update method to save the changes
                return _userAcc.Update(userId, user, out errMsg);
            }
            else
            {
                errMsg = "User not found";
                return ErrorCode.Error;
            }
        }

        public UserInformation GetUserInfoByUserId(String userId)
        {
            return _userInf._table.Where(m => m.userId == userId).FirstOrDefault();
        }
        public UserAccount GetUserByUserId(String userId)
        {
            return _userAcc._table.Where(m => m.userId == userId).FirstOrDefault();
        }
        public UserInformation CreateOrRetrieve(String username, ref String err)
        {
            var User = GetUserByUsername(username);
            var UserInfo = GetUserInfoByUserId(User.userId);
            if (UserInfo != null)
                return UserInfo;

            UserInfo = new UserInformation();
            UserInfo.userId = User.userId;

            _userInf.Create(UserInfo, out err);

            return GetUserInfoByUserId(User.userId);
        }

        public List<UserInformation> getAllUserInformation(String userId)
        {
            return _userInf._table.Where(m => m.userId == userId).ToList();
        }
        public ErrorCode DeleteUsers(int? userId, ref String error)
        {
            return _userAcc.Delete(userId, out error);
        }
        public ErrorCode DeleteInformation(String userId, ref String error)
        {
            return _userInf.Delete(userId, out error);
        }
    }
}