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
    public class ProductsViewController : Controller
    {
        private ABCosmeticEntities db = new ABCosmeticEntities();

        // GET: ProductsView
        public ActionResult Index(int? pageNumber, string search, string sort)
        {
            ViewBag.SortByProduct = string.IsNullOrEmpty(sort) ? "sort product" : "";
            ViewBag.SortByPrice = string.IsNullOrEmpty(sort) ? "sort price" : "";
            ViewBag.SortByCategory = string.IsNullOrEmpty(sort) ? "sort category" : "";

            var records = db.Products.AsQueryable();

            records = records.Where(p => p.Name.Contains(search) || search == null);

            switch (sort)
            {
                case "sort product":
                    records = records.OrderBy(x => x.Name);
                    break;

                case "sort price":
                    records = records.OrderBy(x => x.Price);
                    break;

                case "sort category":
                    records = records.OrderBy(x => x.Category_ID);
                    break;

                default:
                    records = records.OrderByDescending(x => x.Price);
                    break;
            }

            return View(records.ToPagedList(pageNumber ?? 1, 5));
        }
    }
}
