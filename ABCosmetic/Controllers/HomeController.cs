using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCosmetic.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Manager()
        {
            return View();
        }
        public ActionResult Staff()
        {
            return View();
        }
    }
}