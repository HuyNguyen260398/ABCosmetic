using ABCosmetic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ABCosmetic.Controllers
{
    public class LoginController : Controller
    {
        private ABCosmeticEntities db = new ABCosmeticEntities();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            var res = db.Users.SingleOrDefault(s => s.Username == user.Username && s.Password == user.Password);
            if (res == null)
            {
                ModelState.AddModelError("", "Username or Password is invalid!");
                return View();
            }
            else
            {
                Session["Staff ID"] = res.ID.ToString();
                Session["Fullname"] = res.Fullname.ToString();
                Session["Username"] = res.Username.ToString();
                Session["Email"] = res.Email.ToString();
                Session["Phone"] = res.Phone.ToString();
                Session["Address"] = res.Address.ToString();
                Session["Store ID"] = res.Store_ID.ToString();
                Session["Store Address"] = res.Store.Address.ToString();
                Session["Role"] = res.Role.ToString();
                Session["Store City"] = res.Store.City.ToString();
                Session["DateTime"] = DateTime.Now.ToString();
                Session["Staff"] = res;
                if( res.Role == 1)
                {
                    return RedirectToAction("Manager", "Home");
                }
                else
                {
                    return RedirectToAction("Staff", "Home");
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}