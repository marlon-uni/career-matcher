using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class Applicant
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
    }
    public class ApplicantDBContext : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }
    }
}