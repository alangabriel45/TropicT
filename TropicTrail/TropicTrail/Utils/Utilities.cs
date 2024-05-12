using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TropicTrail.Repository;

namespace TropicTrail.Utils
{
    public enum ErrorCode
    {
        Success,
        Error
    }
    public enum Status
    {
        InActive,
        Active
    }

    public enum RoleType
    {
        Customer,
        Staff
    }

    public enum ProductStatus
    {
        NoStock,
        HasStock
    }

    public enum OrderStatus
    {
        Open,
        Pending,
        Paid,
        Delivered,
        Close
    }

    public class Constant
    {
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";

        public const int ERROR = 1;
        public const int SUCCESS = 0;

        public const string X = "X";
        public const string MINUS = "−";
        public const string PLUS = "+";
    }

    public class Utilities
    {
        public static String gUid
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        // Return random number for OTP
        public static int code
        {
            get
            {
                Random r = new Random();
                return r.Next(100000, 999999);
            }
        }

        public static List<SelectListItem> ListRole
        {
            get
            {
                BaseRepository<Role> role = new BaseRepository<Role>();
                var list = new List<SelectListItem>();
                foreach (var item in role.GetAll())
                {
                    var r = new SelectListItem
                    {
                        Text = item.roleName,
                        Value = item.roleId.ToString()
                    };

                    list.Add(r);
                }

                return list;
            }
        }
        public static List<SelectListItem> SelectListItemTourTypeByUser(String username)
        {
            TourTypeManager _tourTypeMgr = new TourTypeManager();
            var list = new List<SelectListItem>();
            foreach (var item in _tourTypeMgr.ListTourType(username))
            {
                var r = new SelectListItem
                {
                    Text = item.tourName,
                    Value = item.tourId.ToString()
                };
                list.Add(r);
            }

            return list;
        }
        public static List<SelectListItem> SelectListProvinceByUser()
        {
            ProvinceManager _provinceMgr = new ProvinceManager();
            var list = new List<SelectListItem>();
            foreach (var item in _provinceMgr.ListProvince())
            {
                var r = new SelectListItem
                {
                    Text = item.provinceName,
                    Value = item.id.ToString()
                };
                list.Add(r);
            }
            return list;
        }
    }
}