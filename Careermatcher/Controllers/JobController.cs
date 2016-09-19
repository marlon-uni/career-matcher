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
    public class JobController : Controller
    {
        private JobDBContext db = new JobDBContext();
        public enum items { Action=0, Drama, Comedy, Science_Fiction };
        // GET: Job
        public ActionResult Index()
        {
            DropDown objData = new DropDown();
            //create object data for DropdownModel
            objData.ParentDataModel = new List<Parent>();
            objData.ClildDataModel = new List<Child>();

            objData.ParentDataModel = GetParentData();
            objData.ClildDataModel = GetChildData();



            return View(objData);
        }
        [HttpPost]
        public String Index(items[] groupselect)
        {
            return "yay!!";
        }

        // GET: Job/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Applicants.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Job/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Job/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Education,Intrest")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Applicants.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        // GET: Job/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Applicants.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Job/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Education,Intrest")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: Job/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Applicants.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Job job = db.Applicants.Find(id);
            db.Applicants.Remove(job);
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
        private List<Parent> GetParentData()
        {
            List<Parent> lstParent = new List<Parent>();
            lstParent.Add(new Parent { ParentId = 1, ParentName = "School" });
            lstParent.Add(new Parent { ParentId = 2, ParentName = "Vocational Education and Training (VET)" });
            lstParent.Add(new Parent { ParentId = 3, ParentName = "Technical and Further Education (TAFE)" });
            lstParent.Add(new Parent { ParentId = 4, ParentName = "Higher education — undergraduate" });
            lstParent.Add(new Parent { ParentId = 5, ParentName = "MBA and management education" });

            
            return lstParent;
        }
        private List<Child> GetChildData()
        {
            //http://www.studiesinaustralia.com/types-of-education
            List<Child> lstChild = new List<Child>();
            lstChild.Add(new Child { ParentId = 1, ChildId = 1, ChildName = "Primary School Completion" });
            lstChild.Add(new Child { ParentId = 1, ChildId = 2, ChildName = "Yr 10 Equivalent Completion" });
            lstChild.Add(new Child { ParentId = 1, ChildId = 3, ChildName = "Yr 12 Equivalent Completion" });
            lstChild.Add(new Child { ParentId = 2, ChildId = 4, ChildName = "VET:Certificate I to IV" });
            lstChild.Add(new Child { ParentId = 2, ChildId = 5, ChildName = "VET:Diploma" });
            lstChild.Add(new Child { ParentId = 2, ChildId = 6, ChildName = "VET:Advanced diploma" });
            lstChild.Add(new Child { ParentId = 2, ChildId = 7, ChildName = "VET:Vocational graduate certificate/diploma" });
            lstChild.Add(new Child { ParentId = 3, ChildId = 8, ChildName = "TAFE:Diploma" });
            lstChild.Add(new Child { ParentId = 3, ChildId = 9, ChildName = "TAFE:Advanced diploma" });
            lstChild.Add(new Child { ParentId = 3, ChildId = 10, ChildName = "TAFE:Vocational graduate certificate/diploma" });
            lstChild.Add(new Child { ParentId = 4, ChildId = 11, ChildName = "Bachelor degree" });
            lstChild.Add(new Child { ParentId = 4, ChildId = 12, ChildName = "Bachelor degree (honours):" });
            lstChild.Add(new Child { ParentId = 4, ChildId = 13, ChildName = "Masters degree" });
            lstChild.Add(new Child { ParentId = 4, ChildId = 14, ChildName = "Doctoral degree" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 15, ChildName = "Accounting" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 16, ChildName = "Business policy and strategy" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 17, ChildName = "Data Analysis And Statistics" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 18, ChildName = "Economics" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 19, ChildName = "Finance" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 20, ChildName = "Global Business" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 21, ChildName = "Human Resources Management" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 22, ChildName = "Leadership" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 23, ChildName = "Marketing" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 24, ChildName = "Operations" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 25, ChildName = "Organisational Behaviour" });

            return lstChild;
        }

    }
}
