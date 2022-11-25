using System;
using System.ComponentModel.DataAnnotations;

namespace amusement_park
{
    public class EmployeeUpdate
    {
        public string employee_id { get; set; }
        public string newUsername { get; set; }
        public string newFirstName { get; set; }
        public string newLastName { get; set; }
        public string newDOB { get; set; }
        public string newJobTitle { get; set; }
        public string newSupervisorID { get; set; }
    }
}

