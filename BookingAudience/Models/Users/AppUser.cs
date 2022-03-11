using BookingAudience.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Models.Users
{
    public class AppUser : IdentityUser<int>
    {
        public Role UserRole {get; set;}
        /// <summary>
        /// имя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// фамилия
        /// </summary>
        public string SecondName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Password { get; set; }
    }
}
