using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _11March2017Web.Models;

namespace _11March2017Web.Controllers
{
    public class HomeController : Controller
    {
        private MVCFirstWebDataEntities db = new MVCFirstWebDataEntities();

        // GET: Home
        public ActionResult Index()
        {
            List<tblProduct> products = db.tblProducts.Where(pic => pic.productThumbnailPath != null).OrderByDescending(x => x.productID).Take(3).ToList();
            return View(products);
        }

        public ActionResult Index1()
        {
            return View();
        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblVendor tblVendor = db.tblVendors.Find(id);
            if (tblVendor == null)
            {
                return HttpNotFound();
            }
            return View(tblVendor);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblVendor tblVendor = db.tblVendors.Find(id);
            if (tblVendor == null)
            {
                return HttpNotFound();
            }
            ViewBag.vendorType = new SelectList(db.tblBusinessTypes, "businessTypeId", "businessTypeName", tblVendor.vendorType);
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName", tblVendor.vendorGender);
            return View(tblVendor);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "vendorID,vendorName,vendorEmail,vendorPassword,vendorAge,vendorAddress,vendorPhone,vendorGender,vendorType,vendorCNIC")] tblVendor tblVendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblVendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.vendorType = new SelectList(db.tblBusinessTypes, "businessTypeId", "businessTypeName", tblVendor.vendorType);
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName", tblVendor.vendorGender);
            return View(tblVendor);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblVendor tblVendor = db.tblVendors.Find(id);
            if (tblVendor == null)
            {
                return HttpNotFound();
            }
            return View(tblVendor);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblVendor tblVendor = db.tblVendors.Find(id);
            db.tblVendors.Remove(tblVendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
