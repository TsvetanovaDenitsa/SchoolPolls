using SchoolPolls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace SchoolPolls.Controllers
{

    public class AdminOptionsController : AdminController
    {
        protected SchoolPollsDBEntities db = new SchoolPollsDBEntities();

        // LIST
        public ActionResult Index(int pollId)
        {
            ViewBag.PollId = pollId;
            ViewBag.PollTitle = db.Polls
                                  .Where(p => p.PollId == pollId)
                                  .Select(p => p.Title)
                                  .FirstOrDefault();

            var options = db.Options
                            .Where(o => o.PollId == pollId)
                            .ToList();

            return View(options);
        }

        // CREATE (POST)
        [HttpPost]
        public ActionResult Create(int pollId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                TempData["Error"] = "Текстът на опцията е задължителен.";
                return RedirectToAction("Index", new { pollId });
            }

            db.Options.Add(new Option
            {
                PollId = pollId,
                Text = text
            });

            db.SaveChanges();

            return RedirectToAction("Index", new { pollId });
        }

        // DELETE (по избор)
        public ActionResult Delete(int id)
        {
            var option = db.Options.Find(id);
            int pollId = option.PollId;

            int optionsCount = db.Options.Count(o => o.PollId == pollId);

            if (optionsCount <= 2)
            {
                TempData["Error"] = "Анкетата трябва да има поне 2 опции.";
                return RedirectToAction("Index", new { pollId });
            }

            db.Options.Remove(option);
            db.SaveChanges();

            return RedirectToAction("Index", new { pollId });
        }
    }
}
