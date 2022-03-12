using BookingAudience.Enums;
using BookingAudience.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class AudienceViewModel
    {
        public List<SelectListItem> BuildingOptions { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// номер этажа
        /// </summary>
        public int Floor { get; set; }
        public AudienceType Type { get; set; }
        /// <summary>
        /// строение
        /// </summary>
        private Building building;
        public Building Building 
        {
            get
            {
                return building;
            } 
            set 
            {
                building = value;
                BuildingId = building.Id;
            } 
        }
        public int BuildingId { get; set; }
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
