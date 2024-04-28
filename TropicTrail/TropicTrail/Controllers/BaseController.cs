using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TropicTrail.Models;
using TropicTrail.Repository;

namespace TropicTrail.Controllers
{
    public class BaseController : Controller
    {
        public String ErrorMessage;
        public UserManager _userManager;
        public TourTypeManager _tourTypeManager;
        public OffersManager _offersManager;
        public TropicTEntities _db;

        public String Username { get { return User.Identity.Name; } }
        public String UserId { get { return _userManager.GetUserByUsername(Username).userId; } }

        public BaseController()
        {
            ErrorMessage = String.Empty;
            _userManager = new UserManager();
            _tourTypeManager = new TourTypeManager();
            _offersManager = new OffersManager();
            _db = new TropicTEntities();
        }


        public void IsUserLoggedSession()
        {
            UserLogged userLogged = new UserLogged();
            if (User != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    userLogged.UserAccount = _userManager.GetUserByUsername(User.Identity.Name);

                }
            }
            Session["User"] = userLogged;
        }
    }
}