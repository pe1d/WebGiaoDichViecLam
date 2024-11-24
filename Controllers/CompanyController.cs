using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebGiaoDichViecLam.Data;
using WebGiaoDichViecLam.Models;

namespace WebGiaoDichViecLam.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ApplicationDbContext _context;

        public CompanyController(ILogger<CompanyController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> ListCompany()
        {
            ViewBag.companies = await _context.tblCompany.ToArrayAsync();
            return View();
        }
        public async Task<IActionResult> DetailCompany(int id)
        {
            ViewBag.company = await _context.tblCompany.FirstOrDefaultAsync(item => item.iCompanyID == id);
            ViewBag.countJob = await _context.tblJob.Where(j => j.iCompanyID == id).CountAsync();
            ViewBag.jobs = await _context.tblJob.Include(item => item.TblCategory).Where(j => j.iCompanyID == id).ToListAsync();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
