using BookingAudience.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class UserViewModel
    {
        /// <summary>
        /// имя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// фамилия
        /// </summary>
        public string SecondName { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
