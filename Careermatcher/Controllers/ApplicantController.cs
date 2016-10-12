using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Careermatcher.Models;
using System.IO;

namespace Careermatcher.Controllers
{
    public class ApplicantController : Controller
    {
        private ApplicantDBContext db = new ApplicantDBContext();

        // GET: Applicant
        [Authorize(Roles = "Applicant")]
        public ActionResult Index()
        {
            Applicant applicant = db.Applicants.Find(User.Identity.Name);
            if (applicant == null)
                return RedirectToAction("Create", "Applicant");
            return View(db.Applicants.ToList());
        }

        // GET: Applicant/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // GET: Applicant/Create
        public ActionResult Create()
        {
            ViewBag.Identification = User.Identity.Name;
            ViewBag.Education = "A";
            ViewBag.IntrestedJobs = "A";
           ViewBag.phoneNumber = 00;
            Applicant applicant = new Applicant { email = User.Identity.Name ,Education="A",IntrestedJobs="B" };
            return View(applicant);
        }

        // POST: Applicant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "email,firstName,lastName,phoneNumber,Education,IntrestedJobs")] Applicant applicant, HttpPostedFileBase file,HttpPostedFileBase photo)
        {
            string pathToCreateResume = "~/FolderOfApplicants/" + applicant.email+ "/Resume/";
            string pathToCreateProfile = "~/FolderOfApplicants/" + applicant.email + "/Profile/";
            //string pathToCreate = "~/Applicant/Resume/" + applicant.email;
            Directory.CreateDirectory(Server.MapPath(pathToCreateResume));
            Directory.CreateDirectory(Server.MapPath(pathToCreateProfile));

            string path = Server.MapPath("~/FolderOfApplicants/" + applicant.email+ "/Resume/" + file.FileName);
            file.SaveAs(path);
            string path2 = Server.MapPath("~/FolderOfApplicants/" + applicant.email + "/Profile/" + photo.FileName);
            file.SaveAs(path2);
            applicant.Path2Photo = path2;
            

            ViewBag.Path = path;
            if (ModelState.IsValid)
            {
                db.Applicants.Add(applicant);
                db.SaveChanges();
                ///return RedirectToAction("Index");
               return RedirectToAction("getJobAndEducationInformation", "Job");
            }

            return View(applicant);
        }

        // GET: Applicant/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // POST: Applicant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "email,firstName,lastName,phoneNumber,Education,IntrestedJobs")] Applicant applicant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicant);
        }

        // GET: Applicant/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // POST: Applicant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Applicant applicant = db.Applicants.Find(id);
            db.Applicants.Remove(applicant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ApplicantMatches()
        {
            MatchDBContext dbMatch = new MatchDBContext();
            JobDBContext dbJob = new JobDBContext();
            Applicant applicant = db.Applicants.Find(User.Identity.Name);
            var requriedEducationarray = applicant.Education.Split('|');
            var requriedIntrestedJobsarray = applicant.IntrestedJobs.Split('|');
            //var test = dbApplicant.Applicants.Any(str => str.Education.Split('|').Intersect(education.Split('|')).Any());
            var result = from p in dbJob.Jobs
                         where requriedEducationarray.Any(val => p.Education.Contains(val))
                         select p;
            //type=job
            var allPossibleJobs = from p in result
                          where requriedIntrestedJobsarray.Any(val => p.Tags.Contains(val))
                          select p;
            //Job matches that the user already has
            var thisApplicantMatches = dbMatch.Matches.Where(x => (x.ApplicantEmailAddress.Equals(User.Identity.Name)));
            //Convert the jobs into matches
            List<Match> matches=new List<Match>();
            foreach (var job in allPossibleJobs)
            {
                var educationListOfCurrentApplicant = job.Education.Split('|');
                var countSimilarityEducation = requriedEducationarray.Intersect(educationListOfCurrentApplicant).Count();

                var intrestedJobsListOfCurrentApplicant = job.Tags.Split('|');
                var countSimilarityIntrestedJobs = requriedIntrestedJobsarray.Intersect(intrestedJobsListOfCurrentApplicant).Count();
                var startScore = 5;
                var a1 = requriedEducationarray.Length - countSimilarityEducation;
                var a2 = requriedIntrestedJobsarray.Length - countSimilarityIntrestedJobs;
                var overAll = startScore - (a1 + a2);
                Match match = new Match
                {
                    EmployerEmailAddress = job.EmployerEmailAddress,
                    JobTitle = job.JobTitle,
                    PublishDate = job.PublishDate,
                    ApplicantEmailAddress = User.Identity.Name,
                    ApplicantName = applicant.firstName + " " + applicant.lastName,
                    overAllScore = overAll,
                    acceptedByApplicant=false,
                    acceptedByEmployer=false,
                    rejectedByApplicant=false,
                    rejectedByEmployer=false
                };
                matches.Add(match);
            }
            //Now start removing existing matches
            //var newMatches = matches.Where(y => (thisApplicantMatches.Any(z => z.JobTitle != y.JobTitle))&&
            //thisApplicantMatches.Any(z => z.PublishDate != y.PublishDate));
            //var matchesAll = thisApplicantMatches.Select(a => new { a.JobTitle, a.EmployerEmailAddress, a.PublishDate });
            //var toAddMatches = matches.Where(a => !matchesAll.Contains(new { a.JobTitle, a.EmployerEmailAddress, a.PublishDate })).ToList();
            var toAddMatches=matches.Where(sc => !thisApplicantMatches.Any(dc => dc.JobTitle == sc.JobTitle 
            && dc.PublishDate ==sc.PublishDate && dc.EmployerEmailAddress==sc.EmployerEmailAddress));
            foreach (var item in toAddMatches)
            {
                dbMatch.Matches.Add(item);
            }

            dbMatch.SaveChanges();
            var thisApplicantMatches2 = dbMatch.Matches.Where(x => (x.ApplicantEmailAddress.Equals(User.Identity.Name)));
            // Only showing matches which were not rejected by this employer
            var filteredBasedOnRejectedByEmployer = thisApplicantMatches2.Where(x => x.rejectedByEmployer == false);
            // Only showing matches which were not rejected by an applicant
            var filteredBasedOnRejectedByApplicantList = filteredBasedOnRejectedByEmployer.Where(x => x.rejectedByEmployer == false);
            //return View(thisApplicantMatches);
            return View(filteredBasedOnRejectedByApplicantList.ToList());
        }
    }
}
