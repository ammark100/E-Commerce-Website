using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _11March2017Web.Models;

namespace _11March2017Web.Controllers
{
    public class CustomerController : Controller
    {
        MVCFirstWebDataEntities db = new MVCFirstWebDataEntities();
        // GET: Customer
        public ActionResult Register()
        {
            if (Session["customerName"] != null && Session["customerID"] != null)
            {
                return RedirectToAction("DashBoard");
            }

            ViewBag.customerGender = new SelectList(db.tblGenders, "genderId", "genderName");
            return View();
        }

        [HttpPost, ActionName("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult Register_Post([Bind(Include = "customerID, customerName, customerAddress, customerGender, customerPhone, customerEmail, customerPassword")]tblCustomer tblcustomer)
        {
            if (ModelState.IsValid)
            {
                db.tblCustomers.Add(tblcustomer);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName", tblcustomer.customerGender);
            return View(tblcustomer);
        }

        [HttpGet]
        public JsonResult CheckCustomerEmail(string Email)
        {
            var result = true;
            var user = db.tblCustomers.Where(x => x.customerEmail == Email).FirstOrDefault();

            if (user != null)
                result = false;

            return Json(result);
        }

        public ActionResult Login()
        {
            if (Session["customerName"] != null && Session["customerID"] != null)
            {
                return RedirectToAction("DashBoard");
            }
            
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult Login_Post(tblCustomer customer)
        {
            using (MVCFirstWebDataEntities db = new MVCFirstWebDataEntities())
            {
                tblCustomer user = db.tblCustomers.Where(x => x.customerEmail == customer.customerEmail && x.customerPassword == customer.customerPassword).FirstOrDefault();

                if (user != null)
                {
                    Session["customerName"] = user.customerEmail;
                    Session["customerID"] = user.customerID;

                    if(Session["my_cart"] != null)
                    {
                        return RedirectToAction("Checkout", "Order");
                    }
                    else
                    {
                        return RedirectToAction("DashBoard");
                    }
                }
            }
            ViewBag.vendorGender = new SelectList(db.tblGenders, "genderId", "genderName");
            return View();
        }

        public ActionResult DashBoard()
        {
            if (Session["customerID"] != null)
            {
                int id = Convert.ToInt32(Session["customerID"]);
                var customer = db.tblCustomers.Where(x => x.customerID == id).FirstOrDefault();
                return View(customer);
            }
            return RedirectToAction("Login");
        }

        public ActionResult AccountSettings()
        {
            int id = Convert.ToInt32(Session["customerID"]);
            tblCustomer customer = db.tblCustomers.Find(id);
            if (customer == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.customerGender = new SelectList(db.tblGenders, "genderId", "genderName", customer.customerGender);
            return View(customer);
        }

        public ActionResult MyOrders()
        {
            int id = Convert.ToInt32(Session["customerID"]);
            List<tblOrder> orderlist = db.tblOrders.Where(x => x.customerID == id).ToList();
            return View(orderlist);
        }

        public ActionResult OrderDetail(tblOrder order)
        {
            List<tblOrderDetail> orderDetailList = db.tblOrderDetails.Where(x => x.orderID == order.orderID).ToList();
            OrderDetailView orderdetailview = new OrderDetailView();
            List<Cart> cart_list = new List<Cart>();

            foreach(var item in orderDetailList)
            {
                tblProduct product = db.tblProducts.Where(x => x.productID == item.productID).FirstOrDefault();
                Cart cart = new Cart();
                cart.productID = item.productID;
                cart.productName = product.productName;
                cart.productQty = item.productQty;
                cart.productTotalPrice = (float)item.productTotalPrice;
                cart.productUnitPrice = (float)item.productUnitPrice;

                if(product.productThumbnailPath != null)
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
            if(order.orderStatus == 1)
            {
                orderdetailview.statusName = "Pending";
            }
            else if(order.orderStatus == 2)
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

        public ActionResult Logout()
        {
            Session.Remove("customerID");
            Session.Remove("customerName");
            return RedirectToAction("Login");
        }
    }
}