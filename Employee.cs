using System;
using System.ComponentModel.DataAnnotations;

namespace amusement_park
{
    public class Employee
    {
        [Key]
        public int employee_id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public DateTime DOB { get; set; }
        public int supervisor_id { get; set; }
        public string job_title { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}

