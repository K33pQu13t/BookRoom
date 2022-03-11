using BookingAudience.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class RegisterViewModel
    {
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public Role UserRole { get; set; }
        public string Password { get; set; }
    }
}
