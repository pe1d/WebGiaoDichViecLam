using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebGiaoDichViecLam.Models;

namespace WebGiaoDichViecLam.Controllers
{
    public class JobClientController : Controller
    {
        private readonly ILogger<JobClientController> _logger;

        public JobClientController(ILogger<JobClientController> logger)
        {
            _logger = logger;
        }
        public IActionResult SearchJob()
        {
            return View();
        }
        public IActionResult DetailJob()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
