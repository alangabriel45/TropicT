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
    
    public partial class Reservation
    {
        public int id { get; set; }
        public string userId { get; set; }
        public string offersName { get; set; }
        public int maxGuest { get; set; }
        public int tourId { get; set; }
        public System.DateTime checkIn { get; set; }
        public Nullable<decimal> price { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string contactNum { get; set; }
        public string email { get; set; }
        public decimal payment { get; set; }
        public Nullable<decimal> balance { get; set; }
        public int status { get; set; }
    }
}
