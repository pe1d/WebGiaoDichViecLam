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

        public async Task<IActionResult> MyApplyJob()
        {
            var accountId = User.FindFirstValue(ClaimTypes.Sid);
            if (string.IsNullOrEmpty(accountId))
            {
                return View(); // Hoặc xử lý phù hợp nếu người dùng không đăng nhập
            }
            var profile = await _context.tblProfileUser
                .FirstOrDefaultAsync(p => p.iAccountID.ToString() == accountId);

            if (profile == null)
            {
                return View(); // Trả về lỗi hoặc redirect nếu không tìm thấy profile
            }

            ViewBag.jobUserApply = await _context.tblJobUser
                    .Include(j => j.TblProfileUser) // Include bảng tblProfileUser
                    .Include(j => j.TblJob)        // Include bảng tblJob
                        .ThenInclude(job => job.TblCategory) // Include thêm bảng tblCategory từ TblJob
                    .Include(j => j.TblJob.TblCompany) // Include thêm bảng tblCompany từ TblJob
                    .Where(j => j.iProfileID == profile.iProfileID)
                    .ToListAsync();

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
