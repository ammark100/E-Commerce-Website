using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using _11March2017Web.Models;

namespace _11March2017Web.Controllers
{
    public class AccountController : Controller
    {
        MVCFirstWebDataEntities db = new MVCFirstWebDataEntities();

        public ActionResult Register()
        {
            if (Session["username"] != null && Session["userID"] != null)
            {
                return RedirectToAction("DashBoard", "VendorAdminPanel");
            }

            ViewBag.vendorType = new SelectList(db.tblBusinessTypes, "businessTypeId", "businessTypeName");
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName");
            return View();
        }

        [HttpPost, ActionName("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult Register_Post([Bind(Include = "vendorID,vendorName,vendorEmail,vendorPassword,vendorAge,vendorAddress,vendorPhone,vendorGender,vendorType,vendorCNIC")]tblVendor tblVendor)
        {
            if (ModelState.IsValid)
            {
                db.tblVendors.Add(tblVendor);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            ViewBag.vendorType = new SelectList(db.tblBusinessTypes, "businessTypeId", "businessTypeName", tblVendor.vendorType);
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName", tblVendor.vendorGender);
            return View(tblVendor);
        }

        // GET: Account
        public ActionResult Login()
        {
            if (Session["username"] != null && Session["userID"] != null) {
                return RedirectToAction("DashBoard", "VendorAdminPanel");
            }
            //ViewBag.vendorType = new SelectList(db.tblBusinessTypes, "businessTypeId", "businessTypeName");
            //ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName");
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult Login_Post(tblVendor vendor)
        {
            using (MVCFirstWebDataEntities db = new MVCFirstWebDataEntities())
            {
                tblVendor user = db.tblVendors.Where(x => x.vendorEmail == vendor.vendorEmail && x.vendorPassword == vendor.vendorPassword).FirstOrDefault();
                
                // TryUpdateModel(user, new string[] { "vendorEmail,vendorPassword"});
                if (user != null)
                {
                    Session["username"] = user.vendorEmail;
                    Session["userID"] = user.vendorID;
                    return RedirectToAction("DashBoard", "VendorAdminPanel");
                }
            }

            ViewBag.vendorType = new SelectList(db.tblBusinessTypes, "businessTypeId", "businessTypeName");
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName");
            return View();
        }

        public ActionResult LoggedIn()
        {
            // ViewBag.Username = Session["username"].ToString();
            if (Session["username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}