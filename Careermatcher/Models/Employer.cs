using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class Employer
    {
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name required")]
        public string firstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        public string lastName { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        [Key]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        [EmailAddress]
        public string email { get; set; }
        //[Required(ErrorMessage = "Mobile no. is required")]
        //[RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public int MyProperty { get; set; }
        public string phoneNumber { get; set; }
    }

    public class EmployerDBContext : DbContext
    {
        public DbSet<Employer> Employers { get; set; }
    }
}