using _11March2017Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _11March2017Web.Controllers
{
    public class ProductController : Controller
    {
        MVCFirstWebDataEntities db = new MVCFirstWebDataEntities();
        // GET: Product
        public ActionResult Products()
        {
            List<tblProduct> products = db.tblProducts.ToList();
            return View(products);
        }

        public ActionResult ProductDetails(tblProduct tblProduct)
        {
            List<tblImage> img = db.tblImages.Where(prodID => prodID.prod_ID == tblProduct.productID).ToList();
            tblProduct.tblImages = img;
            

            ViewBag.productCategory = new SelectList(db.tblCategories, "categoryID", "categoryName", tblProduct.productCategory);
            return View(tblProduct);
        }

        [HttpPost,ActionName("ProductDetails")]
        public ActionResult ProductDetails_post([Bind(Include = "productID, productQuantity, productThumbnailPath")]tblProduct prod)
        {
            tblProduct product = db.tblProducts.Where(temp => temp.productID == prod.productID).FirstOrDefault();

            List<Cart> cart_list = Session["my_cart"] != null ? (List<Cart>)Session["my_cart"] : new List<Cart>();

            Cart temp_cart = new Cart();

            if (prod.productQuantity > product.productQuantity)
            {
                ModelState.AddModelError("productQuantity", "Sorry, We do not have your required quantity");
                List<tblImage> img = db.tblImages.Where(prodID => prodID.prod_ID == product.productID).ToList();
                product.tblImages = img;


                ViewBag.productCategory = new SelectList(db.tblCategories, "categoryID", "categoryName", product.productCategory);
                return View(product);
            }

            /*  if (ModelState.IsValid)
              {
                  temp_cart.productQty = prod.productQuantity == null ? 1 : prod.productQuantity;
              }*/
            else
            {



                temp_cart.productID = product.productID;
                temp_cart.productName = product.productName;
                temp_cart.productUnitPrice = product.productPrice;
                temp_cart.productQty = prod.productQuantity == null ? 1 : prod.productQuantity;
                temp_cart.productTotalPrice = temp_cart.productUnitPrice * (float)temp_cart.productQty;
                temp_cart.vendorID = 3;

                if (product.productThumbnailPath == null)
                {
                    temp_cart.productThumbnailPath = "~/Content/img/noImage.jpg";
                }
                else
                {
                    temp_cart.productThumbnailPath = product.productThumbnailPath;
                }

                cart_list.Add(temp_cart);
                Session["my_cart"] = cart_list;

                return RedirectToAction("Shopping_cart", "Order");
            }
        }
    }
}