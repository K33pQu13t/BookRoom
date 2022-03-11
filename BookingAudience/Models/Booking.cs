using BookingAudience.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Models
{
    public class Booking
    {
        public int Id { get; set; }
        /// <summary>
        /// автор брони
        /// </summary>
        public AppUser Creator { get; set; }
        public DateTime BookingTime { get; set; }
        /// <summary>
        /// длительность бронирования в минутах
        /// </summary>
        public int DurationInMinutes { get; set; }
        public Audience BookedAudience { get; set; }
    }
}
