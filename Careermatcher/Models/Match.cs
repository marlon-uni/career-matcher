using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class Match
    {
        [Key, Column(Order = 1)]
        public string EmployerEmailAddress { get; set; }
        [Key, Column(Order = 2)]
        public string JobTitle { get; set; }
        [Key, Column(Order = 3)]
        public String PublishDate { get; set; }
        [Key, Column(Order = 4)]
        public String ApplicantEmailAddress { get; set; }
        public int indifferenceInEducationRequirent { get; set; }
        public int indiffernceInIntrestedJobsRequirent { get; set; }
        public bool acceptedByApplicant { get; set; }
        public bool acceptedByEmployer { get; set; }
        public bool rejectedByApplicant { get; set; }
        public bool rejectedByEmployer { get; set; }
    }
    public class MatchDBContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }
    }
}