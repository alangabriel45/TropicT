//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TropicTrail
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_Transactions
    {
        public int Reservation_Id { get; set; }
        public string Booked_By { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<int> Ratings { get; set; }
        public string Status { get; set; }
    }
}
