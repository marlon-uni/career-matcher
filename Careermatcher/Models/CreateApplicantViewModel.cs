using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class CreateApplicantViewModel
    {
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name required")]
        public string firstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        public string lastName { get; set; }
        [Key]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        [EmailAddress]
        public string email { get; set; }//needs to check for exsisting email address, dbs may actually throw smothing if there is a duplicate key
        [DisplayName("Phone Number")]
        public string phoneNumber { get; set; }
        [DisplayName("Your Education")]
        public String Education { get; set; }
        [DisplayName("Job tags")]
        public String IntrestedJobs { get; set; }
        [DisplayName("Please upload Resume(pdf ONLY)")]
        //[DataType(DataType.Upload)]
        //[NotMapped]
        //[RegularExpression(@"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.txt|.TXT|.xls|.XLS)$",ErrorMessage ="Please upload a pdf version of your resume")]

        
        public HttpPostedFileBase Resume { get; set; }
        //[NotMapped]
        //[DisplayName("Please upload a Profile picture")]
        //[DataType(DataType.Upload)]
        //[RegularExpression(@"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.txt|.TXT|.xls|.XLS)$",ErrorMessage ="jpegs only")]

        public HttpPostedFileBase Photo { get; set; }
    }
}