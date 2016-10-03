using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class Match
    {
        [DisplayName("Employer's Email Address")]
        [Key, Column(Order = 1)]
        public string EmployerEmailAddress { get; set; }
        [DisplayName("Job Title")]
        [Key, Column(Order = 2)]
        public string JobTitle { get; set; }
        [DisplayName("Job publish date")]
        [Key, Column(Order = 3)]
        public String PublishDate { get; set; }
        [DisplayName("Applicant's Email address")]
        [Key, Column(Order = 4)]
        public String ApplicantEmailAddress { get; set; }
        [DisplayName("Applicant's name")]
        public String ApplicantName { get; set; }
        [DisplayName("Education in-similarity score")]
        public int indifferenceInEducationRequirment { get; set; }
        [DisplayName("Job intrest in-similarity score")]
        public int indiffernceInIntrestedJobsRequirment { get; set; }
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