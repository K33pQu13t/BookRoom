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
        private int durationInMinutes;
        /// <summary>
        /// длительность бронирования в минутах
        /// </summary>
        public int DurationInMinutes 
        {
            get
            {
                return durationInMinutes;
            }
            set
            {
                if (value < 30)
                {
                    throw new Exception("Нельзя забронировать комнату менее чем на 30 минут");
                }
                durationInMinutes = value;
            }
        }
        public Audience BookedAudience { get; set; }

        public bool IsRangeInThisTimeRange(DateTime startTime, DateTime endTime)
        {
            if (startTime > BookingTime && startTime < BookingTime.AddMinutes(DurationInMinutes))
            {
                return true;
            }
            //todo тут может получаться тру если даже всё в порядке
            if (endTime > BookingTime && endTime < BookingTime.AddMinutes(DurationInMinutes))
            {
                return true;
            }
            return false;
        }
    }
}
