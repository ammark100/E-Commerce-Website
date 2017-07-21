using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _11March2017Web.Models;
using System.IO;

namespace _11March2017Web.Controllers
{
    public class VendorAdminPanelController : Controller
    {
        MVCFirstWebDataEntities db = new MVCFirstWebDataEntities();
        // GET: VendorAdminPanel
        public ActionResult DashBoard()
        {
            if (Session["userID"] != null) {
                int id = Convert.ToInt32(Session["userID"]);
                var vendor = db.tblVendors.Where(x => x.vendorID == id).FirstOrDefault();
                return View(vendor);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            if (Session["username"] != null && Session["userID"] != null)
            {
                ViewBag.productCategory = new SelectList(db.tblCategories, "categoryID", "categoryName");
                ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName");
                return View();
            }
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        [ActionName("AddProduct")]
        public ActionResult AddProduct_Post([Bind(Include = "productName, productDescription, productPrice, productCategory, productQuantity")]tblProduct tblProduct,List<HttpPostedFileBase> file)
        {

            List<tblImage> prodImages = new List<tblImage>();
           // prodImages = null;
            var path = "";
            foreach (var item in file)
            {
                if (item != null)
                {
                    tblImage img = new tblImage();
                    img.ImageFile = new byte[item.ContentLength];
                    img.ImageName = string.Format(@"{0}.JPG", Guid.NewGuid());
            //        img.prod_ID = latestProdID;
                    img.vend_ID = Convert.ToInt32(Session["userID"]);

                    item.InputStream.Read(img.ImageFile, 0, item.ContentLength);

                    path = Path.Combine(Server.MapPath("~/Content/img"), img.ImageName);
                    var pathForDB = "~/Content/img/" + img.ImageName;
                    item.SaveAs(path);
                    img.ImagePath = pathForDB;
                    prodImages.Add(img);
                }
            }

            tblProduct.venodrID = Convert.ToInt32(Session["userID"]);
            if(prodImages.Count != 0)
            {
                tblProduct.productThumbnailPath = prodImages.First().ImagePath;
                tblProduct.tblImages = prodImages;
            }
            
            
            if (ModelState.IsValid)
            {

                db.tblProducts.Add(tblProduct);
                db.SaveChanges();
                int latestProdID = tblProduct.productID;

                foreach (tblImage tblImg in tblProduct.tblImages)
                {
                    tblImg.prod_ID = latestProdID;
                    db.Entry(tblImg).State = EntityState.Modified;
                }
                db.SaveChanges();

                return RedirectToAction("DashBoard");
            }
            ViewBag.productCategory = new SelectList(db.tblCategories, "categoryID", "categoryName");
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName");
            return View();
        }

        public ActionResult AccountSettings()
        {
            int id = Convert.ToInt32(Session["userID"]);
            tblVendor tblVend = db.tblVendors.Find(id);
            if(tblVend == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.vendorType = new SelectList(db.tblBusinessTypes, "businessTypeId", "businessTypeName", tblVend.vendorType);
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName", tblVend.vendorGender);
            return View(tblVend);
        }

        [HttpPost, ActionName("AccountSettings")]
        public ActionResult AccountSettings_Post([Bind(Include = "vendorID,vendorName,vendorEmail,vendorPassword,vendorAge,vendorAddress,vendorPhone,vendorGender,vendorType,vendorCNIC")]tblVendor tblVend)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblVend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DashBoard");
            }
            ViewBag.vendorType = new SelectList(db.tblBusinessTypes, "businessTypeId", "businessTypeName", tblVend.vendorType);
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName", tblVend.vendorGender);
            return View(tblVend);
        }

        public ActionResult MyProducts()
        {
            int id = Convert.ToInt32(Session["userID"]);
            tblVendor tblVend = db.tblVendors.Find(id);
            if (tblVend == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<tblProduct> products = db.tblProducts.Where(vendId => vendId.venodrID == tblVend.vendorID).ToList();
            return View(products);
        }

        public ActionResult ProductDetails(tblProduct tblProduct)
        {
            if (Session["username"] == null && Session["userID"] == null && tblProduct == null)
            {
                return RedirectToAction("Login", "Account");
            }

            List<tblImage> img = db.tblImages.Where(prodID => prodID.prod_ID == tblProduct.productID).ToList();
            tblProduct.tblImages = img;

            ViewBag.productCategory = new SelectList(db.tblCategories, "categoryID", "categoryName", tblProduct.productCategory);
            return View(tblProduct);
        }

        [HttpPost, ActionName("ProductDetails")]
        public ActionResult ProductDetails_Post([Bind(Include = "productID,productName, productDescription, productPrice, productCategory, productThumbnailPath, productQuantity")]tblProduct tblprod)
        {
            tblprod.venodrID = Convert.ToInt32(Session["userID"]);
            List<tblImage> img = db.tblImages.Where(prodID => prodID.prod_ID == tblprod.productID).ToList();

            tblprod.tblImages = img;

           // tblProduct temp_prod = db.tblProducts.Where(prod_id => prod_id.productID == tblprod.productID).Single();
           // tblprod.productThumbnailPath = temp_prod.productThumbnailPath;

            if (ModelState.IsValid)
            {
                db.Entry(tblprod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyProducts");
            }
            ViewBag.productCategory = new SelectList(db.tblCategories, "categoryID", "categoryName",tblprod.productCategory);
            return View(tblprod);
        }

        public ActionResult Logout()
        {
            Session.Remove("userID");
            Session.Remove("username");
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Test()
        {
            ProductDisplay prod = new ProductDisplay();
            return View(prod);
        }

        public ActionResult Orders()
        {
            int id = Convert.ToInt32(Session["userID"]);
            List<tblOrder> orderlist = db.tblOrders.Where(x => x.vendorID == id).ToList();
            return View(orderlist);
        }

        public ActionResult OrderDetail(tblOrder order)
        {
            List<tblOrderDetail> orderDetailList = db.tblOrderDetails.Where(x => x.orderID == order.orderID).ToList();
            OrderDetailView orderdetailview = new OrderDetailView();
            List<Cart> cart_list = new List<Cart>();

            foreach (var item in orderDetailList)
            {
                tblProduct product = db.tblProducts.Where(x => x.productID == item.productID).FirstOrDefault();
                Cart cart = new Cart();
                cart.productID = item.productID;
                cart.productName = product.productName;
                cart.productQty = item.productQty;
                cart.productTotalPrice = (float)item.productTotalPrice;
                cart.productUnitPrice = (float)item.productUnitPrice;

                if (product.productThumbnailPath != null)
                {
                    cart.productThumbnailPath = product.productThumbnailPath;
                }
                else
                {
                    cart.productThumbnailPath = "~/Content/img/noImage.jpg";
                }
                cart_list.Add(cart);
            }
            orderdetailview.cart_List = cart_list;
            orderdetailview.datetime = order.orderPlacedTime;
            orderdetailview.statusID = order.orderStatus;
            if (order.orderStatus == 1)
            {
                orderdetailview.statusName = "Pending";
            }
            else if (order.orderStatus == 2)
            {
                orderdetailview.statusName = "Confirmed";
            }
            else
            {
                orderdetailview.statusName = "Delivered";
            }
            //orderdetailview.statusName = order.tblOrderStatu.statusName;
            orderdetailview.totalPrice = (float)order.totalOrderPrice;

            return View(orderdetailview);
        }
    }
}