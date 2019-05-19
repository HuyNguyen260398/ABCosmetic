using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ABCosmetic.Models;

namespace ABCosmetic.Controllers
{
    public class ViewOrdersController : Controller
    {
        private ABCosmeticEntities db = new ABCosmeticEntities();

        public ActionResult Index(string option, string dateoption, string search, int? pageNumber, string sort, DateTime? startdate, DateTime? enddate)
        {
            ViewBag.SortByCustomer = string.IsNullOrEmpty(sort) ? "sort customer" : "";
            ViewBag.SortByStaff = string.IsNullOrEmpty(sort) ? "sort staff" : "";
            ViewBag.SortByStore = string.IsNullOrEmpty(sort) ? "sort store" : "";
            ViewBag.SortByDate = string.IsNullOrEmpty(sort) ? "sort date" : "";
            ViewBag.SortByPDate = string.IsNullOrEmpty(sort) ? "sort pdate" : "";
            ViewBag.SortByPayment = string.IsNullOrEmpty(sort) ? "sort payment" : "";
            ViewBag.SortByStatus = string.IsNullOrEmpty(sort) ? "sort status" : "";
            var records = db.ViewOrders.AsQueryable();

            if (option == "Store" && dateoption == null)
            {
                records = records.Where(s => s.Address.Contains(search) || search == null);
            }
            else if (option == "Store" && dateoption == "Date")
            {
                records = records.Where(s => s.Address.Contains(search) && s.Date >= startdate && s.Date <= enddate || search == null);
            }
            else if (option == "Staff" && dateoption == null)
            {
                records = records.Where(s => s.Fullname.Contains(search) || search == null);
            }
            else if (option == "Staff" && dateoption == "Date")
            {
                records = records.Where(s => s.Fullname.Contains(search) && s.Date >= startdate && s.Date <= enddate || search == null);
            }
            else if (option == null && dateoption == "Date")
            {
                records = records.Where(y => y.Date >= startdate && y.Date <= enddate || startdate == null || enddate == null);
            }
            else
            {
                records = records.Where(s => s.Customer_Name.Contains(search) || search == null);
            }

            switch (sort)
            {
                case "sort customer":
                    records = records.OrderBy(x => x.Customer_Name);
                    break;

                case "sort staff":
                    records = records.OrderBy(x => x.Fullname);
                    break;

                case "sort store":
                    records = records.OrderBy(x => x.Address);
                    break;

                case "sort date":
                    records = records.OrderBy(x => x.Date.ToString());
                    break;

                case "sort pdate":
                    records = records.OrderByDescending(x => x.CheckoutDate.ToString());
                    break;

                case "sort payment":
                    records = records.OrderByDescending(x => x.Total_Payment);
                    break;

                case "sort status":
                    records = records.OrderBy(x => x.Status.ToString());
                    break;

                default:
                    records = records.OrderByDescending(x => x.Date.ToString());
                    break;
            }

            return View(records.ToPagedList(pageNumber ?? 1, 5));
        }

        public ActionResult OrderDetails(int? id)
        {
            var item = db.Order_Details.Where(o => o.Order_ID == id);
            var order = db.ViewOrders.Single(o => o.ID == id);
            Session["Order-Customer"] = order.Customer_Name;
            Session["Order-Staff"] = order.Fullname;
            Session["Order-Store"] = order.Address;
            Session["Order-Date"] = String.Format("{0:dd-MM-yyyy}", order.Date);
            Session["Purchase-Date"] = String.Format("{0:dd-MM-yyyy}", order.CheckoutDate);
            Session["Order-Payment"] = order.Total_Payment;
            Session["Order-Status"] = order.Status;
            return View(item);
        }
        public ActionResult StaffOrderDetails(int? id)
        {
            var item = db.Order_Details.Where(o => o.Order_ID == id);
            var order = db.ViewOrders.Single(o => o.ID == id);
            Session["Order-Customer"] = order.Customer_Name;
            Session["Order-Staff"] = order.Fullname;
            Session["Order-Store"] = order.Address;
            Session["Order-Date"] = String.Format("{0:dd-MM-yyyy}" ,order.Date);
            Session["Purchase-Date"] = String.Format("{0:dd-MM-yyyy}", order.CheckoutDate);
            Session["Order-Payment"] = order.Total_Payment;
            Session["Order-Status"] = order.Status;
            return View(item);
        }

        public ActionResult StaffList()
        {
            var staff = db.Users.Where(s => s.Role == 2).ToList();
            return View(staff);
        }

        public ActionResult StaffReport(string staffName,int? pageNumber, DateTime? startdate, DateTime? enddate)
        {
            var records = db.ViewOrders.AsQueryable();

            if (startdate == null && enddate == null)
            {
                records = records.Where(x => x.Fullname.Equals(staffName));
                Session["staff-report"] = staffName;

                var totalOrder = records.Count();
                var totalRevenue = records.Sum(o => o.Total_Payment);

                ViewBag.TotalOrder = totalOrder;
                ViewBag.TotalRevenue = totalRevenue;
            }
            else if (startdate != null && enddate != null)
            {
                records = records.Where(y => y.Date >= startdate && y.Date <= enddate || startdate == null || enddate == null);

                var totalOrder = records.Count();
                var totalRevenue = records.Sum(o => o.Total_Payment);

                ViewBag.TotalOrder = totalOrder;
                ViewBag.TotalRevenue = totalRevenue;
            }
            return View(records.ToList().ToPagedList(pageNumber ?? 1, 5));
        }

        public ActionResult StaffOrder(string staffName, int? pageNumber, DateTime? startdate, DateTime? enddate)
        {
            var records = db.ViewOrders.AsQueryable();

            if (startdate == null && enddate == null)
            {
                records = records.Where(x => x.Fullname.Equals(staffName));
                Session["staff-order"] = staffName;

                var totalOrder = records.Count();
                var totalRevenue = records.Sum(o => o.Total_Payment);

                ViewBag.TotalOrder = totalOrder;
                ViewBag.TotalRevenue = totalRevenue;
            }
            else if (startdate != null && enddate != null)
            {
                records = records.Where(y => y.Date >= startdate && y.Date <= enddate || startdate == null || enddate == null);

                var totalOrder = records.Count();
                var totalRevenue = records.Sum(o => o.Total_Payment);

                ViewBag.TotalOrder = totalOrder;
                ViewBag.TotalRevenue = totalRevenue;
            }
            return View(records.ToList().ToPagedList(pageNumber ?? 1, 5));
        }

        public ActionResult StoreList()
        {
            var store = db.Stores.ToList();
            return View(store);
        }

        public ActionResult StoreReport(string address, int? pageNumber, DateTime? startdate, DateTime? enddate)
        {
            var records = db.ViewOrders.AsQueryable();

            if (startdate == null && enddate == null)
            {
                records = records.Where(x => x.Address.Equals(address));
                Session["store-report"] = address;

                var totalOrder = records.Count();
                var totalRevenue = records.Sum(o => o.Total_Payment);

                ViewBag.TotalOrder = totalOrder;
                ViewBag.TotalRevenue = totalRevenue;
            }
            else if (startdate != null && enddate != null)
            {
                records = records.Where(y => y.Date >= startdate && y.Date <= enddate || startdate == null || enddate == null);

                var totalOrder = records.Count();
                var totalRevenue = records.Sum(o => o.Total_Payment);

                ViewBag.TotalOrder = totalOrder;
                ViewBag.TotalRevenue = totalRevenue;
            }
            return View(records.ToList().ToPagedList(pageNumber ?? 1, 5));
        }
        public ActionResult GeneralReport(int? pageNumber, DateTime? startdate, DateTime? enddate)
        {
            var records = db.ViewOrders.AsQueryable();

            records = records.Where(y => y.Date >= startdate && y.Date <= enddate || startdate == null || enddate == null);

            var totalOrder = records.Count();
            var totalRevenue = records.Sum(o => o.Total_Payment);

            ViewBag.TotalOrder = totalOrder;
            ViewBag.TotalRevenue = totalRevenue;

            return View(records.ToList().ToPagedList(pageNumber ?? 1, 5));

        }
    }
}
