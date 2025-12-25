using SchoolPolls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SchoolPolls.Controllers
{

    public class AdminOptionsController : AdminController
    {
        protected SchoolPollsDBEntities db = new SchoolPollsDBEntities();

        public ActionResult Index(int pollId)
        {
            ViewBag.PollId = pollId;
            var options = db.Options
                            .Where(o => o.PollId == pollId)
                            .ToList();
            return View(options);
        }

        [HttpPost]
        public ActionResult Create(int pollId, string text)
        {
            db.Options.Add(new Option
            {
                PollId = pollId,
                Text = text
            });

            db.SaveChanges();
            return RedirectToAction("Index", new { pollId });
        }
    }
}
