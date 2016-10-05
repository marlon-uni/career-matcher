using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Careermatcher.Models;

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
        public ActionResult Create([Bind(Include = "email,firstName,lastName,phoneNumber,Education,IntrestedJobs")] Applicant applicant)
        {
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
            var thisApplicantMatches = dbMatch.Matches.Where(x => (x.ApplicantEmailAddress.Equals(User.Identity.Name)));
            return View(thisApplicantMatches);
        }
    }
}
