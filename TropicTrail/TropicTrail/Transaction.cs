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
    
    public partial class Transaction
    {
        public int id { get; set; }
        public Nullable<int> reservationId { get; set; }
        public Nullable<System.DateTime> transactionDate { get; set; }
        public Nullable<decimal> amout { get; set; }
        public Nullable<int> ratings { get; set; }
        public Nullable<int> status { get; set; }
    }
}
