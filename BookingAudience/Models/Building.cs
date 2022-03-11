using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Models
{
    public class Building
    {
        public int Id { get; set; }
        /// <summary>
        /// наименование корпуса
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// адрес здания
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// кодовая буква, с помощью которой формируется номер кабинета (в А корпусе кабинеты аля А123)
        /// </summary>
        public char CodeLetter { get; set; }
    }
}
