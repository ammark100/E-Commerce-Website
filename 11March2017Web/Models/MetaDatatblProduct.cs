using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace _11March2017Web.Models
{
    [MetadataType(typeof(MetaDatatblProduct))]
    public partial class tblProduct
    {

    }

    public class MetaDatatblProduct
    {
        [Required]
        [Display(Name = "Title")]
        public string productName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string productDescription { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int productPrice { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int productCategory { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int productQuantity { get; set; }

        //   public List<tblImage> images { get; set; }
    }
}