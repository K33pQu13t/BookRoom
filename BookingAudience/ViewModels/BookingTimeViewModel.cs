using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class BookingTimeViewModel
    {
        public DateTime DateOfBooking { get; set; }
        /// <summary>
        /// промежутки времени которые заняли
        /// </summary>
        public List<TimeRange> TimeRangeBooked { get; set; }
        /// <summary>
        /// время на которое можно заказать с интервалом 30 минут
        /// </summary>
        public TimeBooking TimeElements { get; set; }
    }

    /// <summary>
    /// диапазон времени
    /// </summary>
    public struct TimeRange
    {
        public TimeSpan start;
        public TimeSpan end;
    }

    /// <summary>
    /// время и можно ли его занять
    /// </summary>
    public struct TimeBooking
    {
        public TimeSpan time;
        public bool isFree;

        public void Add(DateTime date, bool canBook)
        {
            time = date.TimeOfDay;
            isFree = canBook;
        }
    }
}
