using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _11March2017Web.Models
{

    [MetadataType(typeof(MetaDatatblCustomer))]
    public partial class tblCustomer
    {

    }

    public class MetaDatatblCustomer
    {
        [Required]
        [Display(Name = "Name")]
        public string customerName { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string customerAddress { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string customerPhone { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Remote("CheckCustomerEmail", "Customer", ErrorMessage = "Already in use")]
        public string customerEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string customerPassword { get; set; }
    }
}