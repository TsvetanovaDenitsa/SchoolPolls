using SchoolPolls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolPolls.Controllers
{
    public class VotesController : Controller
    {
        private SchoolPollsDBEntities db = new SchoolPollsDBEntities();

        // POST: Votes/Vote
        [HttpPost]
        public ActionResult Vote(int optionId)
        {
            // 1. Проверка за login
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            // 2. Само ученик може да гласува
            if (Session["Role"].ToString() != "Student")
                return new HttpStatusCodeResult(403);

            int userId = (int)Session["UserId"];

            // 3. Намираме анкетата чрез опцията
            var option = db.Options.FirstOrDefault(o => o.OptionId == optionId);
            if (option == null)
                return HttpNotFound();

            int pollId = option.PollId;

            // 4. Проверка дали вече е гласувал в тази анкета
            bool alreadyVoted = db.Votes.Any(v =>
                v.UserId == userId &&
                v.Option.PollId == pollId);

            if (alreadyVoted)
            {
                TempData["Error"] = "Вече сте гласували в тази анкета.";
                return RedirectToAction("Details", "Polls", new { id = pollId });
            }

            // 5. Запис на гласа
            var vote = new Vote
            {
                UserId = userId,
                OptionId = optionId
            };

            db.Votes.Add(vote);
            db.SaveChanges();

            // 6. Пренасочване към резултати (по-късно)
            return RedirectToAction("Results", "Polls", new { id = pollId });
        }
    }
}