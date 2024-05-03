using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TropicTrail.Utils;

namespace TropicTrail.Repository
{
    public class TransactionManager
    {
        UserManager _userMgr;
        BaseRepository<Transaction> _transact;
        BaseRepository<vw_Transactions> _viewTransact;
        public TransactionManager()
        {
            _userMgr = new UserManager();
            _transact = new BaseRepository<Transaction>();
            _viewTransact = new BaseRepository<vw_Transactions>();
        }
        public ErrorCode CreateTransaction(Transaction transac, ref String err)
        {
            return _transact.Create(transac, out err);
        }

        public List<vw_Transactions> ListOfTransaction(string search = "")
        {
            IQueryable<vw_Transactions> query = _viewTransact._table;

            // Apply search filter if search term is provided
            if (!string.IsNullOrEmpty(search))
            {
                // Filter by Reservation_Id, Booked_By, or Transaction_Date
                query = query.Where(t =>
                    t.Reservation_Id.ToString().Contains(search) ||
                    t.Booked_By.Contains(search) ||
                    t.Transaction_Date.ToString().Contains(search)
                );
            }

            return query.ToList();
        }
    }
}