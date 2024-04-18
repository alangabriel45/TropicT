using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TropicTrail.Utils;

namespace TropicTrail.Repository
{
    public class OffersManager
    {
        UserManager _userMgr;
        BaseRepository<Offers> _offers;
        public OffersManager()
        {
            _userMgr = new UserManager();
            _offers = new BaseRepository<Offers>();
        }
        public List<Offers> ListActiveOffers()
        {
            return _offers._table.Where(m => m.status == 1).ToList();
        }
        public List<Offers> ListOffers(String username)
        {
            var user = _userMgr.GetUserByUsername(username);
            return _offers._table.Where(m => m.userId == user.userId).ToList();
        }

        public Offers GetOffersById(int? id)
        {
            return _offers.Get(id);
        }

        public Offers GetOffersBygUId(String gUid)
        {
            return _offers._table.Where(m => m.offersgUId == gUid).FirstOrDefault();
        }

        public ErrorCode CreateOffers(Offers offer, ref String err)
        {
            return _offers.Create(offer, out err);
        }

        public ErrorCode DeleteOffers(int? id, ref String error)
        {
            return _offers.Delete(id, out error);
        }
    }
}