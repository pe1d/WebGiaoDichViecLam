using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebGiaoDichViecLam.Data;
using WebGiaoDichViecLam.Models;

namespace WebGiaoDichViecLam.Controllers
{
    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyJob([FromBody] JsonElement data)
        {
            int jobId = int.Parse(data.GetProperty("jobID").GetString());
            string message = data.GetProperty("message").GetString();
            // Kiểm tra xem job có tồn tại không
            var job = await _context.tblJob.FindAsync(jobId);
            if (job == null)
            {
                return NotFound("Công việc không tồn tại.");
            }
            // Lấy thông tin người dùng hiện tại
            var profile = await _context.tblProfileUser
                .FirstOrDefaultAsync(p => p.iAccountID.ToString() == User.FindFirstValue(ClaimTypes.Sid)); //User.FindFirstValue(ClaimTypes.Sid) là ID người dùng.
            if (profile == null)
            {
                return BadRequest("Không tìm thấy thông tin hồ sơ người dùng.");
            }
            // Tạo đối tượng tblJobUser
            var jobUser = new tblJobUser
            {
                iJobID = jobId,
                iProfileID = profile.iProfileID,
                sType = "Apply",
                sMessage = message
            };

            // Lưu vào cơ sở dữ liệu
            _context.tblJobUser.Add(jobUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ứng tuyển thành công!" });
        }
    }
}