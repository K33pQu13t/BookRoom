using BookingAudience.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Models
{
    public class Report
    {
        public Audience Audience;
        public AppUser Creator;
        public string Description { get; set; }
    }
}
