using BookingAudience.Enums;
using System;

namespace BookingAudience.DTO.Users
{
    public class RegisterDTO
    {
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public Role UserRole { get; set; }
        public string Password { get; set; }
    }
}
