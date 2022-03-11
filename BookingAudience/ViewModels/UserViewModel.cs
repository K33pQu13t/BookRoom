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
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
