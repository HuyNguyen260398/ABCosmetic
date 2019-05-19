using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABCosmetic.Models;

namespace CosmeticShopMS_2.Controllers
{
    public class CartController : Controller
    {
        ABCosmeticEntities db = new ABCosmeticEntities();

        #region Create Cart

        // Get Cart
        public List<Cart> getCart()
        {
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart == null)
            {
                listCart = new List<Cart>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }

        // Add Cart
        public ActionResult AddCart(int proId, string strURL)
        {
            Product product = db.Products.SingleOrDefault(p => p.ID == proId);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> listCart = getCart();
            Cart item = listCart.Find(c => c.ProductId == proId);
            if (item == null)
            {
                item = new Cart(proId);
                listCart.Add(item);
                return Redirect(strURL);
            }
            else
            {
                item.Quantity++;
                return Redirect(strURL);
            }
        }

        // Update Cart
        public ActionResult UpdateCart(int proId, FormCollection f)
        {
            Product product = db.Products.SingleOrDefault(p => p.ID == proId);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> listCart = getCart();
            Cart item = listCart.SingleOrDefault(i => i.ProductId == proId);
            if (item != null)
            {
                item.Quantity = int.Parse(f["txtQuantity"].ToString());
            }
            return RedirectToAction("Cart");
        }

        // Delete Cart
        public ActionResult DeleteCart(int proId)
        {
            Product product = db.Products.SingleOrDefault(p => p.ID == proId);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> listCart = getCart();
            Cart item = listCart.SingleOrDefault(i => i.ProductId == proId);
            if (item != null)
            {
                listCart.RemoveAll(c => c.ProductId == proId);
            }
            return RedirectToAction("Cart");
        }

        // Cart Action
        public ActionResult Cart()
        {
            List<Cart> listCart = getCart();
            Session["Payment"] = TotalMoney();
            return View(listCart);
        }

        // Get Total Amount
        private int TotalAmount()
        {
            int totalAmount = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                totalAmount = listCart.Sum(q => q.Quantity);   
            }
            return totalAmount;
        }

        // Get Total Money
        private double TotalMoney()
        {
            double totalMoney = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                totalMoney = listCart.Sum(q => q.Subtotal);
            }
            return totalMoney;
        }

        // Create Cart Partial
        public ActionResult CartPartial()
        {
            if (TotalAmount() == 0)
            {
                return PartialView();
            }
            ViewBag.TotalAmount = TotalAmount();
            ViewBag.TotalMoney = TotalMoney();
            return PartialView();
        }

        // Update Quantity
        public ActionResult UpdateQuantity()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Cart> listCart = getCart();
            return View(listCart);
        }
        #endregion

        #region Make Order
        public ActionResult OrderPartial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderPartial(Order order)
        {
            List<Cart> listCart = getCart();

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();

                // Add Order Items
                foreach (var item in listCart)
                {
                    Order_Detail orderDetail = new Order_Detail();
                    orderDetail.Order_ID = order.ID;
                    orderDetail.Product_ID = item.ProductId;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Price = item.ProductPrice;
                    orderDetail.Subtotal = item.Subtotal;
                    db.Order_Details.Add(orderDetail);
                }
                db.SaveChanges();

                listCart.Clear();

                return RedirectToAction("Staff", "Home");
            }
            return View(order);
        }
        public ActionResult SaveOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveOrder(Order order)
        {
            List<Cart> listCart = getCart();

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();

                // Add Order Items
                foreach (var item in listCart)
                {
                    Order_Detail orderDetail = new Order_Detail();
                    orderDetail.Order_ID = order.ID;
                    orderDetail.Product_ID = item.ProductId;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Price = item.ProductPrice;
                    orderDetail.Subtotal = item.Subtotal;
                    db.Order_Details.Add(orderDetail);
                }
                db.SaveChanges();

                listCart.Clear();

                return RedirectToAction("Staff", "Home");
            }
            return View(order);
        }
        public ActionResult WaitingOrderList()
        {
            var orderList = db.ViewOrders.Where(o => o.Status.Equals("Waiting"));
            return View(orderList);
        }

        #endregion
    }
}