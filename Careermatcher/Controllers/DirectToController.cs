using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Careermatcher.Controllers
{
    public class DirectToController : Controller
    {
        // GET: DirectTo
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Homepage()
        //{
        //    return View();
        //}
        public ActionResult Employer()
        {
            Session["userValue"] = "Employer";
            return RedirectToAction("LogIn", "Account", new { area = "" });
        }
        public ActionResult Applicant()
        {
            Session["userValue"] = "Applicant";
            return RedirectToAction("LogIn", "Account", new { area = "" });
        }
        public ActionResult Decider()
        {
            //User.Identity.Name;
            if (User.IsInRole("Employer")==true)
                return RedirectToAction("HomePage", "Employer", new { area = "" });
            return RedirectToAction("Index", "Applicant", new { area = "" });
        }
    }
}