using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Models
{
    public class Audience
    {
        public int Id { get; set; }
        private int floor;
        /// <summary>
        /// номер этажа
        /// </summary>
        public int Floor {
            get 
            {
                return floor;
            } 
            set
            {
                if (value <= -2 || value == 0)
                {
                    //todo ошибку оформить через кастомный класс
                    throw new Exception("этаж не может быть меньше чем -2 или не может быть равен 0");
                }
                floor = value;
            }
        }
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
