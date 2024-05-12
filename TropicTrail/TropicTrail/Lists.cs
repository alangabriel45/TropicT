using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TropicTrail.Utils;

namespace TropicTrail
{
    public class Lists
    {
        public List<UserInformation> userInfo { get; set; }
        public UserInformation createRetrieve { get; set; }
        public List<Offers> offers { get; set; }
        public Offers getOffers { get; set; }
        public ErrorCode addOffers { get; set; }
        public Reservation getReserve { get; set; }
        public List<Reservation> reserve { get; set; }
        public List<UserAccount> listOfUsers { get; set; }
        public UserAccount getUsers { get; set; }
        public List<vw_Transactions> listOfTransact { get; set; }
        public vw_Transactions getTransact { get; set; }
        public ErrorCode errors { get; set; }
        public List<vw_UserAcc> listUsers { get; set; }
        public List<vw_manageOffers> listOffers { get; set; }
        public List<vw_manageReservations> manageReserve { get; set; }
        public List<vw_ViewReservation> yourReservation { get; set; }
        public vw_ViewReservation getReservation { get; set; }
    }
}