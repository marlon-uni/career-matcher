using Careermatcher.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Careermatcher.Controllers
{
    public class InvitationDeciderController : Controller
    {
        // GET: InvitationDecider
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Accept(String name, String email,String jobTitle,String dateTime)
        {
            MatchDBContext dbMatch = new MatchDBContext();
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime jobPublishdate = DateTime.ParseExact(dateTime, "MM/dd/yyyy HHH:mm:ss", provider);
            Match applicant = dbMatch.Matches.Find(User.Identity.Name, jobTitle, jobPublishdate.ToString(), email);
            applicant.acceptedByEmployer = true;
            dbMatch.Entry(applicant).State = EntityState.Modified;
            dbMatch.SaveChanges();
            return RedirectToAction("Index", "Job");
        }
    }
}