using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using _11March2017Web.Models;

namespace _11March2017Web.Controllers
{
    public class OrderController : Controller
    {
        MVCFirstWebDataEntities db = new MVCFirstWebDataEntities();
        // GET: Order
        public ActionResult Shopping_cart()
        {
            float totalPrice = 0;
            List<Cart> cart_list = Session["my_cart"] != null ? (List<Cart>)Session["my_cart"] : new List<Cart>();
            foreach(var item in cart_list)
            {
                totalPrice += item.productTotalPrice;
            }
            ViewBag.Total_amount = totalPrice;
            return View(cart_list);
        }

        public ActionResult Checkout()
        {
            if(Session["customerName"] == null && Session["customerID"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            float totalPrice = 0;
            int VendorID=0;
            List<Cart> cart_list = Session["my_cart"] != null ? (List<Cart>)Session["my_cart"] : new List<Cart>();

            tblOrder order = new tblOrder();
            List<tblOrderDetail> orderDetailList = new List<tblOrderDetail>();

            foreach (var item in cart_list)
            {
                tblOrderDetail orderDetails = new tblOrderDetail();

                orderDetails.productID = item.productID;
                orderDetails.productTotalPrice = item.productTotalPrice;
                orderDetails.productUnitPrice = item.productUnitPrice;
                orderDetails.productQty = item.productQty;

                VendorID = item.vendorID;

                orderDetailList.Add(orderDetails);

                totalPrice += item.productTotalPrice;
            }

            int custID = Convert.ToInt32(Session["customerID"]);
            tblCustomer customer = db.tblCustomers.Where(x => x.customerID == custID).FirstOrDefault();

            order.vendorID = VendorID;
            order.orderStatus = 1;
            order.customerID = Convert.ToInt32(Session["customerID"]);
            order.totalOrderPrice = totalPrice;
            order.orderDelieveryAddress = customer.customerAddress;
            order.orderPlacedTime = DateTime.Now;
            order.tblOrderDetails = orderDetailList;

            db.tblOrders.Add(order);
            db.SaveChanges();

            int orderID = order.orderID;

            foreach(tblOrderDetail order_det in order.tblOrderDetails)
            {
                order_det.orderID = orderID;
                db.Entry(order_det).State = EntityState.Modified;
            }
            db.SaveChanges();

            Session.Remove("my_cart");

            return RedirectToAction("DashBoard", "Customer");

        }

        
    }
}