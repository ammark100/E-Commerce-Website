//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _11March2017Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblOrderDetail
    {
        public int orderDetail_ID { get; set; }
        public int orderID { get; set; }
        public int productID { get; set; }
        public double productTotalPrice { get; set; }
        public double productUnitPrice { get; set; }
        public string orderDescription { get; set; }
        public Nullable<int> productQty { get; set; }
    
        public virtual tblOrder tblOrder { get; set; }
        public virtual tblProduct tblProduct { get; set; }
    }
}
