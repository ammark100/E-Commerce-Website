using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace _11March2017Web.Models
{
    [MetadataType(typeof(MetaDatatblVendor))]
    public partial class tblVendor
    {
    }

    public class MetaDatatblVendor
    {

        [Required]
        [Display(Name = "Name")]
        public string vendorName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string vendorEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string vendorPassword { get; set; }

        [Required]
        [Display(Name = "Age")]
        public int vendorAge { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string vendorAddress { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string vendorPhone { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int vendorGender { get; set; }

        [Required]
        [Display(Name = "Type")]
        public int vendorType { get; set; }

        [Required]
        [Display(Name = "CNIC")]
        public string vendorCNIC { get; set; }
    }
}