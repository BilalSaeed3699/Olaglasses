//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Olaglasses.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblOrder
    {
        public int OrderID { get; set; }
        public Nullable<int> userID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Status { get; set; }
        public Nullable<int> IsPaid { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string Vision { get; set; }
        public string LensType { get; set; }
        public string OrderComments { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string UserAddress { get; set; }
        public string BFname { get; set; }
        public string BLname { get; set; }
        public string BAddress { get; set; }
        public string Bcity { get; set; }
        public string BState { get; set; }
        public string BPostalCode { get; set; }
        public string BCountry { get; set; }
        public string BPhone { get; set; }
        public string BEmail { get; set; }
        public string SFname { get; set; }
        public string SLname { get; set; }
        public string SAddress { get; set; }
        public string Scity { get; set; }
        public string SState { get; set; }
        public string SPostalCode { get; set; }
        public string SCountry { get; set; }
        public string SPhone { get; set; }
        public string SEmail { get; set; }
    }
}