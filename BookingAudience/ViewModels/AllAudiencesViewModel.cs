using BookingAudience.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public List<SelectListItem> Buildings { get; set; }
        public List<SelectListItem> Floors { get; set; }
        public List<SelectListItem> AudienceTypes { get; internal set; }
    }
}
