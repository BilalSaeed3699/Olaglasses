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
    
    public partial class tblReview
    {
        public int ReviewID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> GlassID { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Review { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ReviewImage { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
    }
}
