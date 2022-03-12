using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class BuildingViewModel
    {
        /// <summary>
        /// наименование корпуса
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// адрес здания
        /// </summary>
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// кодовая буква, с помощью которой формируется номер кабинета (в А корпусе кабинеты аля А123)
        /// </summary>
        [Required]
        public char CodeLetter { get; set; }
    }
}
