using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _11March2017Web.Models
{
    public class Cart
    {
        public int productID;
        public string productName;
        public float productUnitPrice;
        public float productTotalPrice;
        public int? productQty;
        public int vendorID;
        public string productThumbnailPath;
    }
}