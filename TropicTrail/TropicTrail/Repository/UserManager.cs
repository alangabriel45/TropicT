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
        public UserManager()
        {
            _userAcc = new BaseRepository<UserAccount>();
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

        public ErrorCode SignUp(UserAccount u, ref String errMsg)
        {
            u.userId = Utilities.gUid;
            u.date_created = DateTime.Now;
            u.roleId = 1;
            u.status = (Int32)Status.InActive;

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

            // use the generated code for OTP "ua.code"
            // send email or sms here...........

            return ErrorCode.Success;
        }

        public ErrorCode UpdateUser(UserAccount ua, ref String errMsg)
        {
            return _userAcc.Update(ua.id, ua, out errMsg);
        }
    }
}