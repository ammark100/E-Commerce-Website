using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _11March2017Web.Models
{
    public class ProductDisplay
    {

        public tblProduct product { get; set; }
        public List<tblImage> images { get; set; }
    }
}