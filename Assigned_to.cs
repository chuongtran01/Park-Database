using System;
using System.ComponentModel.DataAnnotations;

namespace amusement_park
{
    public class Assigned_to
    {
        [Key]
        public string assigned_id { get; set; }
        public string employee_id { get; set; }
        public string attraction_id { get; set; }
    }
}

