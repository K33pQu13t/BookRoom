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
        public List<SelectListItem> BuildingOptions { get; set; }
        public List<SelectListItem> FloorOptions { get; set; }
        public List<SelectListItem> AudienceTypeOptions { get; internal set; }

        public int SelectedBuildingId { get; set; }
        public int SelectedFloor{ get; set; }
        public int SelectedType{ get; set; }
    }
}
