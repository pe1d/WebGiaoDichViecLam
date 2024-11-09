using Microsoft.AspNetCore.Mvc;

namespace WebGiaoDichViecLam.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CategoryJob()
        {
            return View();

        }


    }
}
