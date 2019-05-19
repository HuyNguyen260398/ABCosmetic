using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABCosmetic.Models;

namespace ABCosmetic.Controllers
{
    public class OrdersController : Controller
    {
        private ABCosmeticEntities db = new ABCosmeticEntities();


        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Staff_ID = new SelectList(db.Users, "ID", "Fullname", order.Staff_ID);
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Address", order.Store_ID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Staff_ID,Customer_Name,Date,Total_Payment,Store_ID,Status,CheckoutDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Staff_ID = new SelectList(db.Users, "ID", "Fullname", order.Staff_ID);
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Address", order.Store_ID);
            return View(order);
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
