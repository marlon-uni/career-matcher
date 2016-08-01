using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string userName { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        [Key]
        public string email { get; set; }
        public int phoneNumber { get; set; }
    }

    public class EmployeeDBContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }
}