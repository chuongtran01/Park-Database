using System;
using System.ComponentModel.DataAnnotations;

namespace amusement_park
{
    public class EmployeeAdd
    {
        public string fname { get; set; }
        public string lname { get; set; }
        public string DOB { get; set; }
        public int supervisor_id { get; set; }
        public string job_title { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}

