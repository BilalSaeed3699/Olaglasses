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
    
    public partial class tblVariation
    {
        public int VariationID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ColorCode { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string size { get; set; }
        public Nullable<double> FrameAWidth { get; set; }
        public Nullable<double> FrameBHeight { get; set; }
        public string FrameED { get; set; }
        public string FrameDBBridger { get; set; }
        public string FrameTempleLegs { get; set; }
        public string FrameTotalWidth { get; set; }
        public string MinPDPositive { get; set; }
        public string MinPDNeg { get; set; }
        public string ImagePath { get; set; }
        public string CreatedDate { get; set; }
    }
}
