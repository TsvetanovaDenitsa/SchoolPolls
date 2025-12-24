using SchoolPolls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace SchoolPolls.Controllers
{
    public class PollsController : Controller
    {
        private SchoolPollsDBEntities db = new SchoolPollsDBEntities();

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            var polls = db.Polls
                          .Where(p => p.IsActive)
                          .OrderByDescending(p => p.CreatedAt)
                          .ToList();

            return View(polls);
        }

        // 🔹 DETAILS
        public ActionResult Details(int id)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            var poll = db.Polls
                         .Include(p => p.Options)
                         .FirstOrDefault(p => p.PollId == id);

            if (poll == null)
                return HttpNotFound();

            return View(poll);
        }
    }
}

