using SchoolPolls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolPolls.Controllers
{
    public class AccountController : Controller
    {
        private SchoolPollsDBEntities db = new SchoolPollsDBEntities();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(Guid qrToken)
        {
            var user = db.Users.FirstOrDefault(u => u.QRCodeToken == qrToken);

            if (user == null)
            {
                ViewBag.Error = "Невалиден QR код";
                return View();
            }

            Session["UserId"] = user.UserId;
            Session["Role"] = user.Role;
            Session["UserName"] = user.FullName;

            return RedirectToAction("Index", "Polls");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}