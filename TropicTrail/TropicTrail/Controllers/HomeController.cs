using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TropicTrail.Utils;
using TropicTrail.Models;
using System.Web.Security;

namespace TropicTrail.Controllers
{
    [Authorize(Roles = "Customer")]
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login(String ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            ViewBag.Error = String.Empty;
            ViewBag.ReturnUrl = ReturnUrl;

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(String username, String password, String ReturnUrl)
        {
            if (_userManager.SignIn(username, password, ref ErrorMessage) == ErrorCode.Success)
            {
                var user = _userManager.GetUserByUsername(username);

                //if (user.status != (Int32)Status.Active)
                //{
                //    TempData["username"] = username;
                //    return RedirectToAction("Verify");
                //}
                //
                FormsAuthentication.SetAuthCookie(username, false);
                //
                if (!String.IsNullOrEmpty(ReturnUrl))
                    return Redirect(ReturnUrl);

                switch (user.Role.roleName)
                {
                    case Constant.Role_Customer:
                        return RedirectToAction("Index");
                    case Constant.Role_Admin:
                        return RedirectToAction("Index", "Admin");
                    default:
                        return RedirectToAction("Index");
                }
            }
            ViewBag.Error = ErrorMessage;

            return View();
        }
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            ViewBag.Role = Utilities.ListRole;

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SignUp(UserAccount u, String ConfirmPass)
        {
            u.roleId = 1;
            if (!u.password.Equals(ConfirmPass))
            {
                ModelState.AddModelError(String.Empty, "Password not match");
                ViewBag.Role = Utilities.ListRole;
                return View(u);
            }

            if (_userManager.SignUp(u, ref ErrorMessage) != ErrorCode.Success)
            {
                ModelState.AddModelError(String.Empty, ErrorMessage);

                ViewBag.Role = Utilities.ListRole;
                return View(u);
            }
            TempData["username"] = u.username;
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Offers()
        {
            var off = _offersManager.ListActiveOffers();
            return View(off);
        }
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
                return RedirectToAction("PageNotFound");

            var offersInfo = _offersManager.GetOffersById(id);

            return View(offersInfo);
        }
        public ActionResult ViewProfile()
        {
            return View();
        }
        [AllowAnonymous]   
        public ActionResult PageNotFound()
        {
            return Content("Not Found Error 404");
        }
    }
}