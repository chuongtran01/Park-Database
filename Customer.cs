using System;
using System.ComponentModel.DataAnnotations;

namespace amusement_park
{
    public class Customer
    {
        [Key]
        public int customer_id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public double height { get; set; }
        public DateTime DOB { get; set; }
        public int tickets_bought { get; set; }
    }
}

