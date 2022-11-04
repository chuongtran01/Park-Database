using System;
using System.ComponentModel.DataAnnotations;

namespace amusement_park
{
    public class Entry
    {
        [Key]
        public int entry_id { get; set; }
        public int customer_id { get; set; }
        public int ticket_id { get; set; }

    }
}

