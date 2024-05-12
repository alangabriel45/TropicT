using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TropicTrail.Utils;
using TropicTrail.Models;
using System.Web.Security;
using TropicTrail.Repository;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace TropicTrail.Controllers
{
    [HandleError]
    [Authorize(Roles = "Customer, Admin")]
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var getUserInfo = _userManager.getAllUserInformation(UserId);

                var indexModel = new Lists()
                {
                    userInfo = getUserInfo
                };
                return View(indexModel);
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login(String ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
                //return RedirectToAction("Index");
                return View();

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

                if (user.status != (Int32)Status.Active)
                {
                    TempData["username"] = username;
                    return RedirectToAction("Verify");
                }
                //
                FormsAuthentication.SetAuthCookie(username, false);
                //
                if (!String.IsNullOrEmpty(ReturnUrl))
                    //return Redirect(ReturnUrl);
                    return View();

                switch (user.Role.roleName)
                {
                    case Constant.Role_Customer:
                        return RedirectToAction("Index");
                    //return View();
                    case Constant.Role_Admin:
                        return RedirectToAction("Index", "Admin");
                    //return View();
                    default:
                        return RedirectToAction("Index");
                        //return View();
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
        public ActionResult SignUp(UserAccount u, UserInformation ui, String ConfirmPass)
        {
            u.roleId = 1;
            ui.email = u.email;
            ui.userId = u.userId;
            try
            {
                if (!u.password.Equals(ConfirmPass))
                {
                    ModelState.AddModelError(String.Empty, "Password not match");
                    ViewBag.Role = Utilities.ListRole;
                    return View(u);
                }

                if (_userManager.SignUp(u, ui, ref ErrorMessage) != ErrorCode.Success)
                {
                    ModelState.AddModelError(String.Empty, ErrorMessage);

                    ViewBag.Role = Utilities.ListRole;
                    return View(u);
                }
                TempData["username"] = u.username;
                Session["users"] = u.username;
                Session["NewAccountId"] = u.userId;

                Random random = new Random();
                int randomOTP = random.Next(1000, 10000);
                Session["randomOTP"] = randomOTP;

                MailManager sendOTP = new MailManager();
                string subject = "Welcome to our website!";
                string userEmail = u.email;
                string body = $@"<html>
<head>
    <style>
        /* Add your custom styles here */
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            text-align: center;
            margin-bottom: 20px;
        }}
        p {{
            font-size: 16px;
            margin-bottom: 20px;
        }}
        .otp-container {{
            background-color: #f9f9f9;
            padding: 15px;
            text-align: center;
            font-size: 20px;
            border-radius: 5px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>Welcome to our website!</h2>
        <p>Thank you for registering with us. You can now book freely.</p>
        <div class='otp-container'>
            <p>Your OTP:</p>
            <p style='font-size: 30px;'>{randomOTP}</p>
        </div>
    </div>
</body>
</html>";
                string errorResponse = "";

                bool isOTPSent = sendOTP.SendEmail(userEmail, subject, body, ref errorResponse);
                if (isOTPSent)
                {
                    return RedirectToAction("Verify");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["msg"] = $"Error! " + ex.Message;
                return RedirectToAction("Register");
            }

        }
        [AllowAnonymous]
        public ActionResult ProductCard()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Verify()
        {
            if (String.IsNullOrEmpty(TempData["username"] as String))
                return RedirectToAction("Login");

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Verify(int otp1, int otp2, int otp3, int otp4)
        {
            var user = _userManager.GetUserByUsername((String)Session["users"]);

            string enteredOTP = $"{otp1}{otp2}{otp3}{otp4}";

            string expectedOTP = Session["randomOTP"].ToString();

            if (enteredOTP == expectedOTP)
            {
                String newAccId = (String)Session["NewAccountID"];
                TempData["SuccessMessage"] = "Email has been verified!";
                _userManager.UpdateUserStatus(user.id, (Int32)Status.Active, ref ErrorMessage);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Incorrect OTP. Please try again!";
                return RedirectToAction("Verify");
            }

        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public ActionResult Offers()
        {
            var activeOffer = _offersManager.ListActiveOffers();
            var getUserInfo = _userManager.getAllUserInformation(UserId);

            var listOffer = new Lists()
            {
                offers = activeOffer,
                userInfo = getUserInfo
            };

            return View(listOffer);
        }
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
                return RedirectToAction("PageNotFound");

            var getUserInfo = _userManager.getAllUserInformation(UserId);
            var offersInfo = _offersManager.GetOffersById(id);

            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                getOffers = offersInfo
            };


            return View(indexModel);
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
            var getUserInfo = _userManager.getAllUserInformation(UserId);

            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                getOffers = offersInfo
            };

            return View(indexModel);
        }
        [HttpPost]
        public ActionResult BookNow(String checkInDate, int numGuests, decimal price, int? id)
        {
            Session["checkInDate"] = checkInDate;
            Session["numGuests"] = numGuests;
            Session["price"] = price;

            var offersInfo = _offersManager.GetOffersById(id);
            var getUserInfo = _userManager.getAllUserInformation(UserId);
            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                getOffers = offersInfo,
            };
            if (Session["checkInDate"] == null || Session["numGuests"] == null || Session["price"] == null)
            {
                ModelState.AddModelError("price", "Invalid");
            }
            else
            {
                if (offersInfo.maxGuest != numGuests)
                {
                    TempData["NotEqual"] = "Dont Change The Number Of Guests";
                    return View(indexModel);
                }
                return RedirectToAction("ContinueBook");
            }
            return View(indexModel);
        }
        public ActionResult ContinueBook()
        {
            var getUserInfo = _userManager.getAllUserInformation(UserId);

            var indexModel = new Lists()
            {
                userInfo = getUserInfo
            };
            return View(indexModel);
        }
        [HttpPost]
        public ActionResult ContinueBook(TropicTrail.Lists r, String ExpiryDate, String CardNumber)
        {
            r.getReserve.checkIn = DateTime.Parse(Session["checkInDate"].ToString());
            r.getReserve.maxGuest = Convert.ToInt32(Session["numGuests"]);
            r.getReserve.price = Convert.ToDecimal(Session["price"]);
            r.getReserve.status = 1;
            r.getReserve.userId = UserId;

            var card = _card.FindCardByCardNumber(CardNumber, ExpiryDate);
            var enough = _card.EnoughBalance(CardNumber, r.getReserve.payment);
            var offersInfo = _offersManager.GetOffersById(Convert.ToInt32(Session["OfferId"]));

            r.getReserve.offersName = offersInfo.offersName;
            r.getReserve.tourId = offersInfo.TourType.tourId;

            var getUserInfo = _userManager.getAllUserInformation(UserId);

            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                getReserve = r.getReserve
            };

            if (card == null)
            {
                TempData["ErrorMessage"] = "Invalid Card Number or Expiry Date!";
                return View(indexModel);
            }
            if (enough == null)
            {
                TempData["ErrorMessage"] = "Insufficient balance.";
                return View(indexModel);
            }
            decimal? halfPrice = r.getReserve.price / 2;
            if (r.getReserve.payment < halfPrice)
            {
                TempData["ErrorPayment"] = "Please pay at least half of the price.";
                return View(indexModel);
            }

            decimal? remainingBalance = r.getReserve.price - r.getReserve.payment;
            if (remainingBalance <= 0)
            {
                remainingBalance = 0;
            }

            r.getReserve.balance = remainingBalance;

            if (_reservationManager.CreateReservation(r, ref ErrorMessage) == ErrorCode.Error)
            {
                ModelState.AddModelError(String.Empty, ErrorMessage);
                return View(indexModel);
            }
            var remaining = card.balance - r.getReserve.payment;
            _db.sp_Balance(remaining, CardNumber, ExpiryDate);
            TempData["Message"] = $"Product {r.getReserve.lastName} added!";
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult PageNotFound()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult AboutUs()
        {
            if (User.Identity.IsAuthenticated)
            {
                var getUserInfo = _userManager.getAllUserInformation(UserId);

                var indexModel = new Lists()
                {
                    userInfo = getUserInfo
                };
                return View(indexModel);
            }
            return View();
        }
        public ActionResult YourReservation()
        {
            var getUserInfo = _userManager.getAllUserInformation(UserId);
            var yourReserve = _reservationManager.GetReservationByUserId(UserId);
            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                yourReservation = yourReserve
            };

            return View(indexModel);
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

        public ActionResult EditProfile()
        {
            IsUserLoggedSession();
            var user = _userManager.CreateOrRetrieve(User.Identity.Name, ref ErrorMessage);
            var getUserInfo = _userManager.getAllUserInformation(UserId);

            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                createRetrieve = user
            };

            return View(indexModel);
        }
        [HttpPost]
        public ActionResult EditProfile(TropicTrail.Lists userInf, HttpPostedFileBase profilePic)
        {
            var getUserInfo = _userManager.getAllUserInformation(UserId);
            var getInfo = _userManager.GetUserInfoByUserId(UserId);
            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
            };
            if (getInfo.profilePic == null && profilePic == null)
            {
                TempData["ErrorProfile"] = "Please Select a profile Pic";
                return View(indexModel);
            }
            if (!Regex.IsMatch(userInf.createRetrieve.phone, @"^09\d{9}$"))
            {
                TempData["ErrorNumber"] = "Phone number must start with 09 and be 11 digits long.";
                return View(indexModel);
            }

            // Check if the provided email is valid
            if (!string.IsNullOrEmpty(userInf.createRetrieve.email) && !new EmailAddressAttribute().IsValid(userInf.createRetrieve.email))
            {
                TempData["ErrorEmail"] = "Invalid email address.";
                return View(indexModel);
            }

            // Check if the provided zip code is valid
            if (!Regex.IsMatch(userInf.createRetrieve.zipCode, @"^\d+$"))
            {
                TempData["ErrorZip"] = "Zip code must be 5 digits long.";
                return View(indexModel);
            }
            // Save profile picture if provided
            if (profilePic != null && profilePic.ContentLength > 0)
            {
                var inputFileName = Path.GetFileName(profilePic.FileName);
                var serverSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/"), inputFileName);

                if (!Directory.Exists(Server.MapPath("~/UploadedFiles/")))
                    Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/"));

                profilePic.SaveAs(serverSavePath);

                userInf.createRetrieve.profilePic = inputFileName;

                _db.sp_UpdateUserInformation(UserId, userInf.createRetrieve.lastName, userInf.createRetrieve.fistName, userInf.createRetrieve.phone, userInf.createRetrieve.street, userInf.createRetrieve.city, userInf.createRetrieve.state, userInf.createRetrieve.zipCode, userInf.createRetrieve.profilePic);

                TempData["Message"] = "User Information updated!";
                return RedirectToAction("MyProfile");
            }
            if (profilePic == null && getInfo.profilePic != null)
            {
                _db.sp_UpdateUserInformation(UserId, userInf.createRetrieve.lastName, userInf.createRetrieve.fistName, userInf.createRetrieve.phone, userInf.createRetrieve.street, userInf.createRetrieve.city, userInf.createRetrieve.state, userInf.createRetrieve.zipCode, getInfo.profilePic);

                TempData["Message"] = "User Information updated!";
                return RedirectToAction("MyProfile");
            }
            return View();
        }

        public ActionResult MyProfile()
        {
            var user = _userManager.GetUserInfoByUserId(UserId);
            var getUserInfo = _userManager.getAllUserInformation(UserId);

            var indexModel = new Lists()
            {
                createRetrieve = user,
                userInfo = getUserInfo
            };
            return View(indexModel);
        }
    }
}