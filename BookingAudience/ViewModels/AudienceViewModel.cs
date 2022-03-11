using BookingAudience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class AudienceViewModel
    {
        /// <summary>
        /// номер этажа
        /// </summary>
        public int Floor { get; set; }
        /// <summary>
        /// строение
        /// </summary>
        //public string Building { get; set; }
        public Building Building { get; set; }
        /// <summary>
        /// номер кабинета. -1 если без номера
        /// </summary>
        public int Number { get; set; } = -1;
    }
}
