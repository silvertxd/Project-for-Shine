//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project
{
    using System;
    using System.Collections.Generic;
    
    public partial class Supply
    {
        public int Id { get; set; }
        public System.DateTime SupplyDate { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int SellerId { get; set; }
        public double TotalPrice { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Seller Seller { get; set; }
    }
}