using BookingAudience.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.ViewModels
{
    public class AdminPanelViewModel
    {
        public List<Building> Buildings { get; set; }
        public List<SelectListItem> BuildingsOptions { get; set; }
        public List<SelectListItem> TypesOptions { get; set; }
    }
}
