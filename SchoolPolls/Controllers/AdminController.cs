using SchoolPolls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace SchoolPolls.Controllers
{
    public class AdminController : Controller
    {
        protected SchoolPollsDBEntities db = new SchoolPollsDBEntities();

        // Централна защита
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] == null || Session["Role"]?.ToString() != "Admin")
            {
                filterContext.Result = new HttpStatusCodeResult(403);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            ViewBag.Polls = db.Polls.Count();
            ViewBag.ActivePolls = db.Polls.Count(p => p.IsActive);
            ViewBag.Votes = db.Votes.Count();

            return View();
        }
    }
}
