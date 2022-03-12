using BookingAudience.Enums;
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
        public AudienceType Type { get; set; }
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

        private string title;
        public string Title 
        {
            get
            {
                if (Number != -1)
                {
                    return $"{Building.CodeLetter}{Number}";
                }
                return title;
            }
            set
            {
                //если нет номера то можно назвать
                if (Number == -1)
                    title = value;
            } 
        }
        /// <summary>
        /// true если админ убрал возможность арендовать эту аудиторию (закрыта на ремонт или ещё что)
        /// </summary>
        public bool IsBlockedByAdmin { get; set; }
        //todo булевое поле IsCanBook - если false то нельзя букнуть, но не потому что её букнули, просто менеджер решил что нельзя, например там ремонт

        public string Description { get; set; }

        /// <summary>
        /// true если в аудитории есть проектор
        /// </summary>
        public bool HasProjector { get; set; }

        /// <summary>
        /// true если аудитория имеет аудио-оснащение
        /// </summary>
        public bool HasAudio { get; set; }

        public int TablesCount { get; set; }

        /// <summary>
        /// количество сидячих мест
        /// </summary>
        public int SeatPlaces { get; set; }

        /// <summary>
        /// количество компьютеров, за которые можно рассадить посетителей
        /// </summary>
        public int WorkComputersCount { get; set; }

    }
}
