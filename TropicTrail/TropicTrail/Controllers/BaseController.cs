using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public CardManager _card;
        public ReservationManager _reservationManager;
        public TransactionManager _transactionManager;

        public String Username { get { return User.Identity.Name; } }
        public String UserId { get { return _userManager.GetUserByUsername(Username).userId; } }
        public String UserEmail { get { return _userManager.GetUserByUsername(Username).email; } }
        public DbSet <Province> province { get; set; }
        public DbSet <City> city { get; set; }
        public DbSet <Street> street { get; set; }

        public BaseController()
        {
            ErrorMessage = String.Empty;
            _userManager = new UserManager();
            _tourTypeManager = new TourTypeManager();
            _offersManager = new OffersManager();
            _db = new TropicTEntities();
            _card = new CardManager();
            _reservationManager = new ReservationManager();
            _transactionManager = new TransactionManager();
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