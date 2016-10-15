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
        public ActionResult AcceptedByApplicant(String name, String email, String jobTitle, String dateTime)
        {
            MatchDBContext dbMatch = new MatchDBContext();
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime jobPublishdate = DateTime.ParseExact(dateTime, "MM/dd/yyyy HHH:mm:ss", provider);
            Match applicant = dbMatch.Matches.Find(email, jobTitle, jobPublishdate.ToString(), User.Identity.Name);
            applicant.acceptedByApplicant = true;
            dbMatch.Entry(applicant).State = EntityState.Modified;
            dbMatch.SaveChanges();
            return RedirectToAction("Index", "Applicant");
        }
        public ActionResult Reject(String name, String email, String jobTitle, String dateTime)
        {
            MatchDBContext dbMatch = new MatchDBContext();
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime jobPublishdate = DateTime.ParseExact(dateTime, "MM/dd/yyyy HHH:mm:ss", provider);
            Match applicant = dbMatch.Matches.Find(User.Identity.Name, jobTitle, jobPublishdate.ToString(), email);
            applicant.rejectedByEmployer = true;
            dbMatch.Entry(applicant).State = EntityState.Modified;
            dbMatch.SaveChanges();
            return RedirectToAction("Index", "Job");
        }
        public ActionResult RejectedByApplicant(String name, String email, String jobTitle, String dateTime)
        {
            MatchDBContext dbMatch = new MatchDBContext();
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime jobPublishdate = DateTime.ParseExact(dateTime, "MM/dd/yyyy HHH:mm:ss", provider);
            String date = jobPublishdate.ToString();
            Match applicant = dbMatch.Matches.Find(email, jobTitle, date, User.Identity.Name);
            applicant.rejectedByApplicant = true;
            dbMatch.Entry(applicant).State = EntityState.Modified;
            dbMatch.SaveChanges();
            return RedirectToAction("Index", "Applicant");
        }
        public ActionResult ViewPotentialApplicant(String name, String email, String jobTitle, String dateTime)
        {
            ApplicantDBContext dbApplicant = new ApplicantDBContext();
            Applicant applicant = dbApplicant.Applicants.Find(email);
            var dir = new System.IO.DirectoryInfo(Server.MapPath(applicant.Path2Photo));
            return View(applicant);
        }
    }
}