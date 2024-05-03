using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TropicTrail.Models;
using TropicTrail.Utils;

namespace TropicTrail.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Offers management
        public ActionResult ManageOffers()
        {
            IsUserLoggedSession();
            return View(_offersManager.ListOffers(Username));
        }
        public ActionResult AddOffers()
        {
            ViewBag.TourType = Utilities.SelectListItemTourTypeByUser(Username);
            return View();
        }
        [HttpPost]
        public ActionResult AddOffers(Offers offers)
        {
            ViewBag.TourType = Utilities.SelectListItemTourTypeByUser(Username);

            var offersgUid = $"Offers-{Utilities.gUid}";

            offers.status = 1;
            offers.dateCreated = DateTime.Now;  
            offers.offersgUId = offersgUid;
            offers.userId = UserId;

            if (_offersManager.CreateOffers(offers, ref ErrorMessage) == ErrorCode.Error)
            {
                ModelState.AddModelError(String.Empty, ErrorMessage);
                return View(offers);
            }
            TempData["Message"] = $"Product {offers.offersName} added!";
            return RedirectToAction("Index");
        }
        public ActionResult OffersDetails(int? id)
        {
            ViewBag.TourType = Utilities.SelectListItemTourTypeByUser(Username);

            if (id == null)
                return RedirectToAction("PageNotFound", "Home");

            var off = _offersManager.GetOffersById(id);

            if (off == null)
                return RedirectToAction("PageNotFound", "Home");

            return View(off);
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
            return View(_userManager.ListOfUsers());
        }
        public ActionResult ManageReservations()
        {
            IsUserLoggedSession();
            return View(_reservationManager.ListReservation());
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
            return View(_transactionManager.ListOfTransaction());
        }
    }
}