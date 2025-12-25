using SchoolPolls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolPolls.Controllers
{
    public class UsersController : Controller
    {

        private SchoolPollsDBEntities db = new SchoolPollsDBEntities();

        // LIST
        public ActionResult Index()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        // CREATE (GET)
        public ActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public ActionResult Create(User user)
        {
            user.QRCodeToken = Guid.NewGuid();
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;
            user.Role = "Student";

            db.Users.Add(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ACTIVATE / DEACTIVATE
        public ActionResult Toggle(int id)
        {
            var user = db.Users.Find(id);
            user.IsActive = !user.IsActive;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
