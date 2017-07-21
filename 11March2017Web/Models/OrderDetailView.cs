using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _11March2017Web.Models
{
    public class OrderDetailView
    {
        public List<Cart> cart_List;
        public float totalPrice;
        public int statusID;
        public string statusName;
        public DateTime? datetime;
    }
}