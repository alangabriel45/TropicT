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
        public TransactionManager()
        {
            _userMgr = new UserManager();
            _transact = new BaseRepository<Transaction>();
        }
        public ErrorCode CreateTransaction(Transaction transac, ref String err)
        {
            return _transact.Create(transac, out err);
        }
    }
}