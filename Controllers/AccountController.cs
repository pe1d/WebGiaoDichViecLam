﻿using Microsoft.AspNetCore.Mvc;

namespace WebGiaoDichViecLam.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Login()
		{
			return View();
		}

		public IActionResult ForgotPassword()
		{
			return View();
		}
	}
}
