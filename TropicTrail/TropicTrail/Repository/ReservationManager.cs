using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TropicTrail.Utils;

namespace TropicTrail.Repository
{
    public class ReservationManager
    {
        UserManager _userMgr;
        BaseRepository<Reservation> _reservation;

        public ReservationManager()
        {
            _userMgr = new UserManager();
            _reservation = new BaseRepository<Reservation>();
        }
        public List<Reservation> ListActiveReservation()
        {
            return _reservation._table.Where(m => m.status == 1).ToList();
        }
        public List<Reservation> GetReservationByUserId(String userId)
        {
            return _reservation._table.Where(m => m.userId == userId).ToList();
        }
        public List<Reservation> ListReservation()
        {
            return _reservation._table.ToList();
        }
        public Reservation GetReservationById(int? id)
        {
            return _reservation.Get(id);
        }

        public ErrorCode CreateReservation(Reservation reserve, ref String err)
        {
            return _reservation.Create(reserve, out err);
        }
        public ErrorCode UpdateReservation(Reservation reserve, ref String err)
        {
            return _reservation.Update(reserve.id, reserve, out err);
        }

        public ErrorCode DeleteReservation(int? id, ref String error)
        {
            return _reservation.Delete(id, out error);
        }
    }
}