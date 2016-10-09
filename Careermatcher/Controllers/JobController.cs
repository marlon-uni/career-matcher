using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Careermatcher.Models;
using System.Globalization;

namespace Careermatcher.Controllers
{

    public class JobController : Controller
    {
        int ctr = 0;
        private JobDBContext db = new JobDBContext();
        public enum items
        {
            Primary_School_Completion = 1, Yr_10_Equivalent_Completion, Yr_12_Equivalent_Completion,
            VET_Certificate_I_to_IV, VET_Diploma, VET_Advanced_diploma, VET_Vocational_graduate_certificate, TAFE_Diploma,
            TAFE_Advanced_diploma, TAFE_Vocational_graduate_certificate, Bachelor_degree, Bachelor_degree_honours_, Masters_degree, Doctoral_degree, Accounting, Business_policy_and_strategy, Data_Analysis_And_Statistics, Economics, Finance,
            Global_Business, Human_Resources_Management, Leadership, Marketing, Operations, Organisational_Behaviour
        };
        public ActionResult eligibleApplicants(String id, String Hour, String Minute, String Seconds, String Millisecond, String Ticks, String jobTitle, String tags, String education,String time)
        {
            //http://stackoverflow.com/questions/1757214/linq-entity-string-field-contains-any-of-an-array-of-strings
            ApplicantDBContext dbApplicant = new ApplicantDBContext();
            //String test = { "mustard", "pickles", "relish" };
            //var possible = dbApplicant.Applicants.Any(x => (x.Education.Contains(education)));
            //var possible = from p in dbApplicant.Applicants
            //               where search.Any(x=>p.Education.Contains);
            var requriedEducationarray = education.Split('|');
            var requriedIntrestedJobsarray = tags.Split('|');
            //var test = dbApplicant.Applicants.Any(str => str.Education.Split('|').Intersect(education.Split('|')).Any());
            var result = from p in dbApplicant.Applicants
                         where requriedEducationarray.Any(val => p.Education.Contains(val))
                     select p;
            var result2 = from p in result
                          where requriedIntrestedJobsarray.Any(val => p.IntrestedJobs.Contains(val))
                         select p;
            

            MatchDBContext dbMatch = new MatchDBContext();
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime jobPublishdate = DateTime.ParseExact(time, "MM/dd/yyyy HHH:mm:ss", provider);
            String theDate = jobPublishdate.ToString();

            //old matches
            //var thisEmployersMatches = dbMatch.Matches.Where(x => (x.EmployerEmailAddress.Equals(User.Identity.Name))).Select(x => x.ApplicantEmailAddress).ToArray();
            //Filtering based on employer id and the job title
            var thisEmployersMatches = dbMatch.Matches.Where(x => ((x.EmployerEmailAddress.Equals(User.Identity.Name))
            &&(x.JobTitle.Equals(jobTitle))));

            //Filtering based on job pubish date as it a unique identifier
            var filteredBasedOnJobPublishedDate = thisEmployersMatches.Where(x => x.PublishDate.Equals(theDate));

            ///////////For filtering  rejected Applicants to display ONLY FOR returning to view//////////////
            
            // Only showing matches which were not rejected by this employer
            var filteredBasedOnRejectedByEmployer = filteredBasedOnJobPublishedDate.Where(x => x.rejectedByEmployer == false);
            // Only showing matches which were not rejected by an applicant
            var filteredBasedOnRejectedByApplicantList = filteredBasedOnRejectedByEmployer.Where(x => x.rejectedByEmployer == false);
            //list of Applicant names that were not rejected
            var listOfNonRejectedApplicantNames = filteredBasedOnRejectedByApplicantList.Select(x => x.ApplicantEmailAddress).ToList();
            
            
            //////////////////////////////////////////////
            //Gives a list of applicants to ignore for adding to the db
            var listOfExisingApplicantNames= filteredBasedOnJobPublishedDate.Select(x => x.ApplicantEmailAddress).ToArray();
            //var all = amp.Select(x =>x.ApplicantEmailAddress);
            //var result2filtered = ;

            var result4 = from p in result2
                          where !listOfExisingApplicantNames.Any(val => p.email.Contains(val))
                          select p;

            if(result4.Count()==0)
            {
                var test1 = thisEmployersMatches.ToList();
                var test2 = listOfNonRejectedApplicantNames;
                return View(filteredBasedOnRejectedByApplicantList.ToList());
            }
           


            foreach (var item in result4)
            {
                //int count = requriedEducationarray.Where(val => item.Education.Contains(val)).Count();
                var educationListOfCurrentApplicant = item.Education.Split('|');
                var countSimilarityEducation = requriedEducationarray.Intersect(educationListOfCurrentApplicant).Count();

                var intrestedJobsListOfCurrentApplicant = item.IntrestedJobs.Split('|');
                var countSimilarityIntrestedJobs = requriedIntrestedJobsarray.Intersect(intrestedJobsListOfCurrentApplicant).Count();

                Match match = new Match
                {
                    EmployerEmailAddress = User.Identity.Name,
                    JobTitle = jobTitle,
                    ApplicantEmailAddress = item.email,
                    PublishDate= jobPublishdate.ToString(),
                    ApplicantName=item.firstName+" "+item.lastName,
                    indifferenceInEducationRequirment = requriedEducationarray.Length- countSimilarityEducation,
                    indiffernceInIntrestedJobsRequirment = requriedIntrestedJobsarray.Length - countSimilarityIntrestedJobs,
                    acceptedByApplicant=false,
                    acceptedByEmployer=false,
                    rejectedByApplicant=false,
                    rejectedByEmployer=false

                };

                dbMatch.Matches.Add(match);
                
            }
            dbMatch.SaveChanges();
            
            //return View(dbMatch.Matches.ToList());
            return View(listOfNonRejectedApplicantNames);

        }

        // GET: Job
        public ActionResult Index()
        {
            //DropDown objData = new DropDown();
            ////create object data for DropdownModel
            //objData.ParentDataModel = new List<Parent>();
            //objData.ClildDataModel = new List<Child>();

            //objData.ParentDataModel = GetParentData();
            //objData.ClildDataModel = GetChildData();
            ctr++;
            return View(db.Jobs.ToList());

            // return View(objData);
        }
        //[HttpPost]
        //public String Index(items[] groupselect)
        //{
        //    String text = "";
        //    for(int i=0;i<groupselect.Length;i++)
        //    {
        //        text+=groupselect[i].ToString()+",";
        //    }
        //    return text;
        //}
        //For employer
        public ActionResult Index2()
        {
            DropDown objData = new DropDown();
            //create object data for DropdownModel
            objData.ParentDataModel = new List<Parent>();
            objData.ClildDataModel = new List<Child>();

            objData.ParentDataModel = GetParentData();
            objData.ClildDataModel = GetChildData();
            DropDown objData2 = new DropDown();
            objData2.ParentDataModel = GetParentData_Jobs();
            objData2.ClildDataModel = GetChildData_Jobs();
            DropDown_Collection requirements = new DropDown_Collection();
            requirements.dropDownOne = objData;
            requirements.dropDownTwo = objData2;

            return View(requirements);
        }
        //For employer
        [HttpPost]
        public ActionResult Index2(String Title, String[] groupselect, String[] groupselect2)
        {
            //String text = "";
            //for (int i = 0; i < groupselect.Length; i++)
            //{
            //    text += groupselect[i].ToString() + ",";
            //}
            //return text;
            JobRequirements requirements = new JobRequirements();
            requirements.EducationString = groupselect;
            requirements.JobString = groupselect2;
            DateTime currentTime = DateTime.Now;
            Job job = new Job
            {
                EmployerEmailAddress = User.Identity.Name,
                JobTitle = Title,
                Education = string.Join("|", groupselect),
                Tags = string.Join("|", groupselect2),
                PublishDate = currentTime.ToString()

            };


            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Job", new { area = "" });
        }

        //For Applicant
        public ActionResult getJobAndEducationInformation()
        {
            DropDown objData = new DropDown();
            //create object data for DropdownModel
            objData.ParentDataModel = new List<Parent>();
            objData.ClildDataModel = new List<Child>();

            objData.ParentDataModel = GetParentData();
            objData.ClildDataModel = GetChildData();
            DropDown objData2 = new DropDown();
            objData2.ParentDataModel = GetParentData_Jobs();
            objData2.ClildDataModel = GetChildData_Jobs();
            DropDown_Collection requirements = new DropDown_Collection();
            requirements.dropDownOne = objData;
            requirements.dropDownTwo = objData2;
            ViewBag.Title = "Applicant";
            return View(requirements);
        }

        //For employer
        [HttpPost]
        public ActionResult getJobAndEducationInformation(String Title, String[] groupselect, String[] groupselect2)
        {


            //This needs to be changed
            ApplicantDBContext Applicantdb = new ApplicantDBContext();
            Applicant applicant = Applicantdb.Applicants.Find(User.Identity.Name);
            applicant.Education = string.Join("|", groupselect);
            applicant.IntrestedJobs = string.Join("|", groupselect2);
            Applicantdb.Entry(applicant).State = EntityState.Modified;
            Applicantdb.SaveChanges();
            return RedirectToAction("Index", "Applicant", new { area = "" });
        }


        public ActionResult getJobtags()
        {
            DropDown objData = new DropDown();
            //create object data for DropdownModel
            objData.ParentDataModel = new List<Parent>();
            objData.ClildDataModel = new List<Child>();

            objData.ParentDataModel = GetParentData_Jobs();
            objData.ClildDataModel = GetChildData_Jobs();

            return View(objData);
        }

        // GET: Job/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
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
                db.Jobs.Add(job);
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
            Job job = db.Jobs.Find(id);
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
        public ActionResult Delete(String id, String Hour, String Minute, String Seconds, String Millisecond, String Ticks, String jobTitle, String time)
        {
            //String timeandDate = id + Hour + Minute + Seconds;
            //CultureInfo provider = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //provider.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime jobPublishdate = DateTime.ParseExact(time, "MM/dd/yyyy HHH:mm:ss", provider);

            String dateAndTime = jobPublishdate.ToString();
            //var thisEmployersJobs = db.Jobs.Where(x => (x.PublishDate.Equals(dateAndTime)));
            var Job = db.Jobs.Find(User.Identity.Name, jobTitle, dateAndTime);
            return View(Job);
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(FormCollection collection)
        {
            var jobTitle = collection["JobTitle"];
            var publishDate = collection["publishDate"];
            var Job = db.Jobs.Find(User.Identity.Name, jobTitle, publishDate);
            //var Job = db.Jobs.Find(User.Identity.Name, collection., dateAndTime);
            //Job job = db.Jobs.Find(id);
            db.Jobs.Remove(Job);
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
        private List<Parent> GetParentData_Jobs()
        {
            List<Parent> lstParent = new List<Parent>();
            lstParent.Add(new Parent { ParentId = 1, ParentName = "Agriculture, Food and Natural Resources" });
            lstParent.Add(new Parent { ParentId = 2, ParentName = "Architecture and Contruction" });
            lstParent.Add(new Parent { ParentId = 3, ParentName = "Arts, Audio, Video Technology Communications:" });
            lstParent.Add(new Parent { ParentId = 4, ParentName = "Business Management and Administration" });
            lstParent.Add(new Parent { ParentId = 5, ParentName = "Finance" });
            lstParent.Add(new Parent { ParentId = 5, ParentName = "Government and Public Administration" });
            lstParent.Add(new Parent { ParentId = 5, ParentName = "Health Science" });
            lstParent.Add(new Parent { ParentId = 5, ParentName = "Information Technology:" });


            return lstParent;
        }
        private List<Child> GetChildData_Jobs()
        {

            List<Child> lstChild = new List<Child>();
            lstChild.Add(new Child { ParentId = 1, ChildId = 1, ChildName = "Agricultural Equipment Operator" });
            lstChild.Add(new Child { ParentId = 1, ChildId = 2, ChildName = "Agricultural Inspector" });
            lstChild.Add(new Child { ParentId = 1, ChildId = 3, ChildName = "Butcher" });
            lstChild.Add(new Child { ParentId = 2, ChildId = 4, ChildName = "Architect" });
            lstChild.Add(new Child { ParentId = 2, ChildId = 5, ChildName = "Carpenter" });
            lstChild.Add(new Child { ParentId = 2, ChildId = 6, ChildName = "Concrete Finisher" });
            lstChild.Add(new Child { ParentId = 2, ChildId = 7, ChildName = "Civil Engineer" });
            lstChild.Add(new Child { ParentId = 3, ChildId = 8, ChildName = "Actor" });
            lstChild.Add(new Child { ParentId = 3, ChildId = 9, ChildName = "Audio and Video Equipment Technician" });
            lstChild.Add(new Child { ParentId = 3, ChildId = 10, ChildName = "Broadcast Technician" });
            lstChild.Add(new Child { ParentId = 4, ChildId = 11, ChildName = "Accountant" });
            lstChild.Add(new Child { ParentId = 4, ChildId = 12, ChildName = "Advertising Sales Agent" });
            lstChild.Add(new Child { ParentId = 4, ChildId = 13, ChildName = "Auditor" });
            lstChild.Add(new Child { ParentId = 4, ChildId = 14, ChildName = "Budget Analyst" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 15, ChildName = "Accounting" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 16, ChildName = "Bill and Account Collector" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 17, ChildName = "Credit Analyst" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 18, ChildName = "Financial Analyst" });
            lstChild.Add(new Child { ParentId = 5, ChildId = 19, ChildName = "Investment Fund Manager" });
            lstChild.Add(new Child { ParentId = 6, ChildId = 20, ChildName = "Air Crew Member" });
            lstChild.Add(new Child { ParentId = 6, ChildId = 21, ChildName = "Economist" });
            lstChild.Add(new Child { ParentId = 6, ChildId = 22, ChildName = "Financial Examiner" });
            lstChild.Add(new Child { ParentId = 6, ChildId = 23, ChildName = "Meter Reader" });
            lstChild.Add(new Child { ParentId = 6, ChildId = 24, ChildName = "Postal Service Clerk" });
            lstChild.Add(new Child { ParentId = 6, ChildId = 25, ChildName = "Postal Service Mail Carrier" });
            lstChild.Add(new Child { ParentId = 7, ChildId = 26, ChildName = "Acupuncturist" });
            lstChild.Add(new Child { ParentId = 7, ChildId = 27, ChildName = "Anesthesiologist" });
            lstChild.Add(new Child { ParentId = 7, ChildId = 28, ChildName = "Biologist" });
            lstChild.Add(new Child { ParentId = 7, ChildId = 29, ChildName = "Chiropractor" });
            lstChild.Add(new Child { ParentId = 7, ChildId = 30, ChildName = "Dental Assisstant" });
            lstChild.Add(new Child { ParentId = 7, ChildId = 31, ChildName = "Dentist" });
            lstChild.Add(new Child { ParentId = 8, ChildId = 32, ChildName = "Business Intelligence Analyst" });

            return lstChild;
        }

        public String Accept()
        {
            //MatchDBContext dbMatch = new MatchDBContext();
            //CultureInfo provider = CultureInfo.InvariantCulture;
            //DateTime jobPublishdate = DateTime.ParseExact(PublishDate, "MM/dd/yyyy HHH:mm:ss", provider);
            //Match applicant = dbMatch.Matches.Find(User.Identity.Name, JobTitle, jobPublishdate.ToString(), ApplicantEmailAddress);
            //applicant.acceptedByEmployer = true;
            //dbMatch.Entry(applicant).State = EntityState.Modified;
            //dbMatch.SaveChanges();
            return "dfdsf";
            //return RedirectToAction("eligibleAppliacants,Job");
        }

    }
}
