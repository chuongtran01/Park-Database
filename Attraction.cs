using System;
using System.ComponentModel.DataAnnotations;

namespace amusement_park
{
    public class Attraction
    {
        [Key]
        public int attraction_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int location { get; set; }
        public double min_height { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public int breakdown_nums { get; set; }
    }
}

