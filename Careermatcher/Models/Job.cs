using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class Job
    {
        [Key]
        public string ID { get; set; }
        public string Education { get; set; }
        public string Intrest { get; set; }
    }

    public class JobDBContext : DbContext
    {
        public DbSet<Job> Applicants { get; set; }
    }
}
