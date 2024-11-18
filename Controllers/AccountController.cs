using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebGiaoDichViecLam.Data;
using WebGiaoDichViecLam.Models;

namespace WebGiaoDichViecLam.Controllers
{
	public class AccountController : Controller
	{
		private const int MaxFailedAttempts = 3;
		private const int LockoutMinutes = 30;
		private readonly ApplicationDbContext _context;
		public AccountController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{

			return View();
		}

		public IActionResult Login()
		{
			if (HttpContext.Session.GetInt32("LockoutTime") != null)
			{
				DateTime lockoutTime = DateTime.Parse(HttpContext.Session.GetString("LockoutTime"));
				if (lockoutTime > DateTime.Now)
				{
					ModelState.AddModelError("", $"Tài khoản bị khóa. Vui lòng thử lại sau {lockoutTime - DateTime.Now:mm\\:ss} phút.");
					ViewData["IsLockedOut"] = true;
					return View();
				}
				else
				{
					// Xóa thời gian khóa nếu đã hết hạn
					HttpContext.Session.Remove("LockoutTime");
					HttpContext.Session.SetInt32("FailedAttempts", 0);
					ViewData["IsLockedOut"] = false;
				}
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(string username, string password)
		{
			if (HttpContext.Session.GetInt32("FailedAttempts") == null)
			{
				HttpContext.Session.SetInt32("FailedAttempts", 0);
			}
			// Giả sử phương thức ValidateUser để kiểm tra thông tin đăng nhập
			var user = ValidateUser(username, password);
			if (user != null)
			{
				// Đăng nhập thành công
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.sUserName.ToString()),
					new Claim(ClaimTypes.Role, user.iRoleID.ToString()) // Hoặc vai trò phù hợp
                };

				// Tạo ClaimsIdentity và Cookie Authentication
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var authProperties = new AuthenticationProperties
				{
					ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30), // Hết hạn sau 30 phút
					IsPersistent = true // Giữ đăng nhập sau khi đóng trình duyệt
				};
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

				HttpContext.Session.SetInt32("FailedAttempts", 0);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				// Đăng nhập thất bại, tăng số lần thử
				int failedAttempts = HttpContext.Session.GetInt32("FailedAttempts").Value;
				failedAttempts++;
				HttpContext.Session.SetInt32("FailedAttempts", failedAttempts);

				if (failedAttempts >= MaxFailedAttempts)
				{
					// Khóa tài khoản trong 30 phút
					DateTime lockoutTime = DateTime.Now.AddMinutes(LockoutMinutes);
					HttpContext.Session.SetString("LockoutTime", lockoutTime.ToString());
					ViewData["IsLockedOut"] = true;
					ModelState.AddModelError("", "Bạn đã đăng nhập sai quá 3 lần. Vui lòng thử lại sau 30 phút.");
				}
				else
				{
					ViewData["IsLockedOut"] = false;
					ModelState.AddModelError("", $"Đăng nhập thất bại. Bạn còn {MaxFailedAttempts - failedAttempts} lần thử.");
				}
			}
			return View();
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Account");
		}
		public IActionResult ForgotPassword()
		{
			return View();
		}
		public IActionResult ChangePassword()
		{
			return View();
		}

		public IActionResult SignUp()
		{
			return View();
		}

		public IActionResult SignUpUser()
		{
			return View();
		}

		public IActionResult SignUpEmployer()
		{
			return View();
		}
		private tblAccount ValidateUser(string username, string password)
		{
			// Kiểm tra tên đăng nhập và mật khẩu, trả về true nếu đúng
			// Thay bằng logic kiểm tra thực tế, ví dụ từ cơ sở dữ liệu
			var user = _context.tblAccount.FirstOrDefault(u => (u.sEmailAccount == username || u.sUserName == username) && u.sPassword == password);
			return user;
		}
	}
}
