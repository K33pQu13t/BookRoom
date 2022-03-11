using BookingAudience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class AllAudiencesViewModel
    {
        public List<List<Audience>> Audiences { get; set; }
    }
}
