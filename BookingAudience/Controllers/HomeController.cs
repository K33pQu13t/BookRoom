using BookingAudience.DAL.Repositories;
using BookingAudience.Models;
using BookingAudience.Services.Audiences;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IServiceProvider provider)
        {
            _logger = logger;

            var _audiencesRepository = (IGenericRepository<Audience>)provider.GetService(typeof(IGenericRepository<Audience>));
            var _buildingsRepository = (IGenericRepository<Building>)provider.GetService(typeof(IGenericRepository<Building>));
            var a = new AudiencesManagementService(_audiencesRepository, _buildingsRepository);
        }

        public IActionResult Index()
        {
            //todo отладка
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Success()
        {
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
