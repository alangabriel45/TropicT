using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TropicTrail.Repository
{
    public class ProvinceManager
    {
        BaseRepository<Province> _provinceManager;
        private UserManager _userMgr;
        public ProvinceManager()
        {
            _provinceManager = new BaseRepository<Province>();
            _userMgr = new UserManager();
        }
        public List<Province> ListProvince()
        {
            return _provinceManager._table.ToList();
        }
    }
}