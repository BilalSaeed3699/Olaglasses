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
    
    public partial class tblUser
    {
        public int UserID { get; set; }
        public string UserEmail { get; set; }
        public string UserPass { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserImage { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string Gender { get; set; }
        public Nullable<int> UserStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
