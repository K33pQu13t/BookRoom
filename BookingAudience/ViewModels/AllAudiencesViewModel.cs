using BookingAudience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class AllAudiencesViewModel
    {
        /// <summary>
        /// список списков аудиторий. Каждый список аудиторий содержит аудитории только одного билдинга, и аудитории рассортированы там по этажам
        /// </summary>
        public List<List<Audience>> ListOfListsOfAudiences { get; set; }
    }
}
