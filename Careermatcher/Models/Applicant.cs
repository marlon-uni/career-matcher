using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class Applicant
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        [Key]
        public string email { get; set; }//needs to check for exsisting email address, dbs may actually throw smothing if there is a duplicate key
        public int phoneNumber { get; set; }
        public int MyProperty { get; set; }
    }
    public class ApplicantDBContext : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }
    }
}