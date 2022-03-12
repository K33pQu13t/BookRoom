using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// true если дата между указанным промежутком
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool IsInRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck <= endDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate1"></param>
        /// <param name="endDate1"></param>
        /// <param name="startDate2"></param>
        /// <param name="endDate2"></param>
        /// <returns>true если два промежутка пересекаются</returns>
        public static bool HasIntersection(DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            return (startDate1 <= startDate2 && endDate1 >= startDate2) ||
                    (startDate1 >= startDate2 && endDate2 >= startDate1);
        }
    }
}
