using BookingAudience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.DTO.Corpus
{
    public class AudienceDTO
    {
        public AudienceDTO(Audience audience)
        {
            Id = audience.Id;
            Building = audience.Building;
            Description = audience.Description;
            Floor = audience.Floor;
            HasAudio = audience.HasAudio;
            HasProjector = audience.HasProjector;
            IsBlockedByAdmin = audience.IsBlockedByAdmin;
            Number = audience.Number;
            Title = audience.Title;
            SeatPlaces = audience.SeatPlaces;
            TablesCount = audience.TablesCount;
            WorkComputersCount = audience.WorkComputersCount;
        }

        public int Id { get; }
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
        public string Title { get; set; }
        /// <summary>
        /// true если админ убрал возможность арендовать эту аудиторию (закрыта на ремонт или ещё что)
        /// </summary>
        public bool IsBlockedByAdmin { get; set; }

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
