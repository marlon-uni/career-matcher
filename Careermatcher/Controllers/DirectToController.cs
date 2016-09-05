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
        public ActionResult Employee()
        {
            Session["userValue"] = "Employee";
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
            if (User.IsInRole("Employee")==true)
                return RedirectToAction("HomePage", "Employee", new { area = "" });
            return RedirectToAction("Index", "Applicant", new { area = "" });
        }
    }
}