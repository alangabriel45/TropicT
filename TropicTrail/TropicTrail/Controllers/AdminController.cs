using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TropicTrail.Models;
using TropicTrail.Utils;

namespace TropicTrail.Controllers
{
    [HandleError]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
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

        #region Offers management
        public ActionResult ManageOffers()
        {
            IsUserLoggedSession();
            var ListActiveOffers = _offersManager.ListOffers(Username);
            var getUserInfo = _userManager.getAllUserInformation(UserId);

            var listOffers = new Lists()
            {
                offers = ListActiveOffers,
                userInfo = getUserInfo
            };
            return View(listOffers);
        }
        public ActionResult AddOffers()
        {
            var getUserInfo = _userManager.getAllUserInformation(UserId);

            var indexModel = new Lists()
            {
                userInfo = getUserInfo
            };
            ViewBag.TourType = Utilities.SelectListItemTourTypeByUser(Username);
            return View(indexModel);
        }
        [HttpPost]
        public ActionResult AddOffers(TropicTrail.Lists offers)
        {
            ViewBag.TourType = Utilities.SelectListItemTourTypeByUser(Username);

            var offersgUid = $"Offers-{Utilities.gUid}";

            offers.getOffers.status = 1;
            offers.getOffers.dateCreated = DateTime.Now;
            offers.getOffers.offersgUId = offersgUid;
            offers.getOffers.userId = UserId;

            var addOffer = _offersManager.CreateOffers(offers.getOffers, ref ErrorMessage);
            if (addOffer == ErrorCode.Error)
            {
                var getUserInfo = _userManager.getAllUserInformation(UserId);
                var index = new Lists()
                {
                    userInfo = getUserInfo,
                    addOffers = addOffer
                };
                ModelState.AddModelError(String.Empty, ErrorMessage);
                return View(index);
            }
            TempData["Message"] = $"Product {offers.getOffers.offersName} added!";
            return RedirectToAction("Index");
        }
        public ActionResult OffersDetails(int? id)
        {
            ViewBag.TourType = Utilities.SelectListItemTourTypeByUser(Username);

            var off = _offersManager.GetOffersById(id);
            var getUserInfo = _userManager.getAllUserInformation(UserId);

            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                getOffers = off
            };

            if (id == null)
                return RedirectToAction("PageNotFound", "Home");           

            if (off == null)
                return RedirectToAction("PageNotFound", "Home");

            return View(indexModel);
        }
        public JsonResult OffersDelete(int? id)
        {
            var res = new Response();
            res.code = (Int32)_offersManager.DeleteOffers(id, ref ErrorMessage);
            res.message = ErrorMessage;

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult ManageUsers()
        {
            var getUserInfo = _userManager.getAllUserInformation(UserId);
            var listOfUser = _userManager.ListOfUsers();

            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                listOfUsers = listOfUser
            };
            return View(indexModel);
        }
        public ActionResult EditUser(TropicTrail.Lists users, String userid)
        {
            var usersAcc = _userManager.GetUserByUserId(userid);

            var index = new Lists()
            {
                getUsers = usersAcc
            };
            return View(index);
        }
        public JsonResult DeleteUsers(String userId, int? id)
        {
            var res = new Response();
            res.code = (Int32)_userManager.DeleteUsers(id, ref ErrorMessage);            
            res.message = ErrorMessage;

            _userManager.DeleteInformation(userId, ref ErrorMessage);

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ManageReservations()
        {

            var getUserInfo = _userManager.getAllUserInformation(UserId);
            var listOfReserve = _reservationManager.ListReservation();
            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                reserve = listOfReserve
            };
            IsUserLoggedSession();
            return View(indexModel);
        }
        [HttpPost]
        public ActionResult ManageReservation(int id)
        {
            var reservation = _reservationManager.GetReservationById(id);
            reservation.status = 1; // Update status to 1 (confirmed)
            _reservationManager.UpdateReservation(reservation, ref ErrorMessage);
            return RedirectToAction("ManageReservations");
        }
        public ActionResult Transaction()
        {
            var getUserInfo = _userManager.getAllUserInformation(UserId);
            var listOfTransac = _transactionManager.ListOfTransaction();

            var indexModel = new Lists()
            {
                userInfo = getUserInfo,
                listOfTransact = listOfTransac
            };
            return View(indexModel);
        }
    }
}