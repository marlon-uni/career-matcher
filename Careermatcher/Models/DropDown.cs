using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Careermatcher.Models
{
    public class DropDown
    {
        public List<Parent> DropdownDataModel { get; set; }
        public List<Parent> ParentDataModel { get; set; }
        public List<Child> ClildDataModel { get; set; }
        public int SelectedValue { get; set; }
        public String Value { get; set; }
    }
    public class Parent
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
    }

    public class Child
    {
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string ChildName { get; set; }
    }
    public class Education
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }

}