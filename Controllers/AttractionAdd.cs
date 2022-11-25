using System;
namespace amusement_park.Controllers
{
    public class AttractionAdd
    {
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string min_height { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public int breakdown_num { get; set; }
    }
}

