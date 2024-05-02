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
        public ActionResult BookNow(int? id)
        {
            if (id == null || id == 0)
                return RedirectToAction("PageNotFound");

            var offersInfo = _offersManager.GetOffersById(id);

            return View(offersInfo);
        }
        [HttpPost]
        public ActionResult BookNow(String checkInDate, int numGuests, decimal price)
        {
            Session["checkInDate"] = checkInDate;
            Session["numGuests"] = numGuests;
            Session["price"] = price;
            
            if (Session["checkInDate"] == null || Session["numGuests"] == null || Session["price"] == null)
            {
                ModelState.AddModelError("price", "Invalid");
            }
            else
            {
                return RedirectToAction("ContinueBook");
            }
            return View();
        }
        public ActionResult ContinueBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContinueBook(Reservation r, String ExpiryDate, String CardNumber)
        {
            r.checkIn = DateTime.Parse(Session["checkInDate"].ToString());
            r.maxGuest = Convert.ToInt32(Session["numGuests"]);
            r.price = Convert.ToDecimal(Session["price"]);
            r.status = 0;
            r.userId = UserId;

            var card = _card.FindCardByCardNumber(CardNumber, ExpiryDate);
            var enough = _card.EnoughBalance(CardNumber, r.payment);
            var offersInfo = _offersManager.GetOffersById(Convert.ToInt32(Session["OfferId"]));

            r.offersName = offersInfo.offersName;
            r.tourId = offersInfo.TourType.tourId;
            if (card == null)
            {
                ModelState.AddModelError("CardNumber", "Invalid Card Number.");
                return View(r);
            }
            if (enough == null)
            {
                ModelState.AddModelError("CardNumber", "Insufficient balance.");
                return View(r);
            }
            decimal? halfPrice = r.price / 2;
            if (r.payment < halfPrice)
            {
                ModelState.AddModelError("Payment", "Please pay at least half of the price.");
                return View(r);
            }

            decimal? remainingBalance = r.price - r.payment;
            if (remainingBalance <= 0)
            {
                remainingBalance = 0;
            }

            r.balance = remainingBalance;

            if (_reservationManager.CreateReservation(r, ref ErrorMessage) == ErrorCode.Error)
            {
                ModelState.AddModelError(String.Empty, ErrorMessage);
                return View(r);
            }
            TempData["Message"] = $"Product {r.lastName} added!";
            return RedirectToAction("Index");
        }
        [AllowAnonymous]   
        public ActionResult PageNotFound()
        {
            return Content("Not Found Error 404");
        }
        [AllowAnonymous]
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult YourReservation()
        {
            return View(_reservationManager.GetReservationByUserId(UserId));
        }
        public ActionResult Finish(int id, int rating)
        {
            var reservation = _reservationManager.GetReservationById(id);

            reservation.status = 3;
            _reservationManager.UpdateReservation(reservation, ref ErrorMessage);

            Transaction transaction = new Transaction
            {
                reservationId = id,
                transactionDate = DateTime.Now,
                amout = reservation.payment,
                ratings = rating, 
                status = 3
            };

            _transactionManager.CreateTransaction(transaction, ref ErrorMessage);

            return RedirectToAction("Index");
        }
    }
}