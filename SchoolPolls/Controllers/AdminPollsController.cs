using SchoolPolls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolPolls.Controllers
{
    public class AdminPollsController : Controller
    {
        protected SchoolPollsDBEntities db = new SchoolPollsDBEntities();

        // GET: AdminPolls
        // READ – списък
        public ActionResult Index()
        {
            var polls = db.Polls
                          .OrderByDescending(p => p.CreatedAt)
                          .ToList();
            return View(polls);
        }

        // CREATE – форма
        public ActionResult Create()
        {
            return View();
        }

        // CREATE – запис
        [HttpPost]
        public ActionResult Create(Poll poll)
        {
            if (!ModelState.IsValid)
                return View(poll);

            poll.CreatedByUserId = (int)Session["UserId"];
            poll.IsActive = false;

            db.Polls.Add(poll);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // UPDATE – форма
        public ActionResult Edit(int id)
        {
            var poll = db.Polls.Find(id);
            if (poll == null)
                return HttpNotFound();

            return View(poll);
        }

        // UPDATE – запис
        [HttpPost]
        public ActionResult Edit(Poll poll)
        {
            if (!ModelState.IsValid)
                return View(poll);

            var dbPoll = db.Polls.Find(poll.PollId);
            if (dbPoll == null)
                return HttpNotFound();

            dbPoll.Title = poll.Title;
            dbPoll.Description = poll.Description;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // DELETE – потвърждение
        public ActionResult Delete(int id)
        {
            var poll = db.Polls.Find(id);
            if (poll == null)
                return HttpNotFound();

            return View(poll);
        }

        // DELETE – реално изтриване
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var poll = db.Polls.Find(id);
            if (poll == null)
                return HttpNotFound();

            db.Polls.Remove(poll);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Активиране / деактивиране
        public ActionResult ToggleStatus(int id)
        {
            var poll = db.Polls.Find(id);
            if (poll == null)
                return HttpNotFound();

            poll.IsActive = !poll.IsActive;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
