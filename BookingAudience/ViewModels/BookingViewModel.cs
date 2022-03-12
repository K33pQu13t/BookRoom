using BookingAudience.Models;
using BookingAudience.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class BookingViewModel
    {
        /// <summary>
        /// автор брони
        /// </summary>
        public AppUser Creator { get; set; }
        public DateTime BookingTime { get; set; }
        private int durationInMinutes;
        /// <summary>
        /// длительность бронирования в минутах
        /// </summary>
        public int DurationInMinutes { get; set; }
        public Audience BookedAudience { get; set; }
    }
}
