using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebGiaoDichViecLam.Data;
using WebGiaoDichViecLam.Models;
//Diep
namespace WebGiaoDichViecLam.Controllers
{
    public class JobClientController : Controller
    {
        private readonly ILogger<JobClientController> _logger;
        private readonly ApplicationDbContext _context;

        public JobClientController(ILogger<JobClientController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> SearchJob()
        {
            try
            {
                var jobs = await _context.tblJob.Include(j => j.TblCategory).Include(j => j.TblCompany).ToListAsync();
                var categories = await _context.tblCategory.ToArrayAsync();
                ViewBag.Jobs = jobs;
                ViewBag.Categories = categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi truy vấn dữ liệu từ tblJob.");
                throw;
            }
            return View();
        }
        public async Task<IActionResult> DetailJob(int id)
        {
            var job = await _context.tblJob.Include(j => j.TblCategory).Include(j => j.TblCompany).FirstOrDefaultAsync(item => item.iJobID == id);
            ViewBag.job = job;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
