using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class Job
    {
        [Key]
        public string MyProperty { get; set; }
    }
}
