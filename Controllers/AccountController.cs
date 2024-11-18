using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebGiaoDichViecLam.Data;
using WebGiaoDichViecLam.Models;
using Microsoft.EntityFrameworkCore;

namespace WebGiaoDichViecLam.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext _context { get; set; }
        private readonly ILogger<AccountController> _logger;


        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            this._context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Kiểm tra tài khoản với thông tin username và password
            var account = _context.tblAccount.FirstOrDefault(a => a.sUserName == username && a.sPassword == password);
            if (account != null)
            {
                // Lưu thông tin tài khoản vào session
                HttpContext.Session.SetString("Username", account.sUserName);
                HttpContext.Session.SetString("Email", account.sEmailAccount);
            

                // Chuyển hướng đến trang Index
                return RedirectToAction("Index", "Home");
            }

            // Thông báo lỗi nếu đăng nhập thất bại
            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác.";
            return View();
        }



        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string current_password, string new_password, string confirm_new_password)
        {
            // Lấy thông tin tài khoản của người dùng hiện tại từ session hoặc cookie (ví dụ)
            var currentUser = _context.tblAccount.FirstOrDefault(a => a.sUserName == User.Identity.Name);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy tài khoản của bạn.";
                return View();
            }

            // Kiểm tra mật khẩu hiện tại có đúng không
            if (currentUser.sPassword != current_password)
            {
                ViewBag.ErrorMessage = "Mật khẩu hiện tại không đúng.";
                return View();
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu mới có khớp không
            if (new_password != confirm_new_password)
            {
                ViewBag.ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                return View();
            }


            // Cập nhật mật khẩu mới
            currentUser.sPassword = new_password;
            _context.tblAccount.Update(currentUser);
            await _context.SaveChangesAsync();

            // Thông báo thành công
            ViewBag.SuccessMessage = "Mật khẩu đã được thay đổi thành công!";
            return View();
        }



        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string username, string password, string email, string reg_role)
        {
            // Kiểm tra nếu username hoặc email đã tồn tại
            var existingAccount = _context.tblAccount.FirstOrDefault(a => a.sUserName == username || a.sEmailAccount == email);
            if (existingAccount != null)
            {
                ViewBag.ErrorMessage = "Tài khoản hoặc email đã tồn tại.";
                return View(); // Hiển thị lại trang đăng ký với thông báo lỗi
            }

            // Tạo tài khoản mới
            var roleID = reg_role == "candidate" ? 1 : reg_role == "employer" ? 2 : 0;
            if (roleID == 0)
            {
                ViewBag.ErrorMessage = "Vai trò không hợp lệ.";
                return View(); // Hiển thị lại trang đăng ký với thông báo lỗi
            }

            // Đăng ký tài khoản
            var newAccount = new tblAccount
            {
                sUserName = username,
                sPassword = password,
                sEmailAccount = email,
                iRoleID = roleID
            };

            _context.tblAccount.Add(newAccount);
            await _context.SaveChangesAsync();

            // Chuyển hướng đến form đăng ký ứng viên hoặc nhà tuyển dụng dựa trên vai trò
            if (roleID == 1)  // Vai trò Ứng viên
            {
                return RedirectToAction("SignUpUser", new { accountId = newAccount.iAccountID });

            }
            else if (roleID == 2)  // Vai trò Nhà tuyển dụng
            {
                return RedirectToAction("SignUpEmployer", new { accountId = newAccount.iAccountID });
            }
            else
            {
                ViewBag.ErrorMessage = "Không xác định được vai trò.";
                return View(); // Trả về thông báo lỗi nếu không thể xác định vai trò
            }
        }


        // Phương thức cho việc đăng ký ứng viên
        [HttpGet]
        public IActionResult SignUpUser(int accountId)
        {
            _logger.LogInformation($"Received accountId in SignUpUser: {accountId}");
            ViewBag.AccountId = accountId;
            return View();  
        }


        [HttpPost]
        public async Task<IActionResult> SignUpUser(int iAccountID, string fullname, string phone, string address, string skills, string experience, string education, IFormFile cv, IFormFile profile_picture)
        {


            // Log thông tin nhận được
            _logger.LogInformation($"Received data: AccountID = {iAccountID}, FullName = {fullname}, Phone = {phone}, Address = {address}");

            // Kiểm tra nếu accountID tồn tại trong bảng tblAccount
            var account = await _context.tblAccount.FindAsync(iAccountID);
            if (account == null)
            {
                ViewBag.ErrorMessage = "Tài khoản không tồn tại.";
                return View();
            }

            // Tạo profile cho ứng viên
            var profile = new tblProfileUser
            {
                iAccountID = iAccountID,
                sFullName = fullname,
                sPhoneNumber = phone,
                sAddress = address,
                sSkills = skills,
                sExperience = experience,
                sEducation = education,
                sCV = cv != null ? cv.FileName : string.Empty,
                sPhoto = profile_picture != null ? profile_picture.FileName : string.Empty
            };

            _context.tblProfileUser.Add(profile);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }


        // Phương thức cho việc đăng ký nhà tuyển dụng
        [HttpGet]
        public IActionResult SignUpEmployer(int accountId)
        {
            ViewBag.AccountId = accountId;
            return View();
        }


        [HttpPost]
      public async Task<IActionResult> SignUpEmployer(int iAccountID, string fullname, string phone, string address,
                                                  string companyName, string companyPhone, string companyAddress,
                                                  string companyDescription, int numberEmployees, IFormFile companyPhoto)
{
    // Kiểm tra nếu accountID tồn tại trong bảng tblAccount
    var account = await _context.tblAccount.FindAsync(iAccountID);
    if (account == null)
    {
        ViewBag.ErrorMessage = "Tài khoản không tồn tại.";
        return View();
    }

    // Xử lý ảnh công ty nếu có
    string photoPath = "default.png"; // Giá trị mặc định nếu không có ảnh

    if (companyPhoto != null && companyPhoto.Length > 0)
    {
        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/companies");

        // Kiểm tra thư mục tồn tại, nếu không thì tạo mới
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string fileName = Guid.NewGuid().ToString() + "_" + companyPhoto.FileName;
        string path = Path.Combine(directoryPath, fileName);

        // Lưu ảnh công ty vào thư mục
        try
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await companyPhoto.CopyToAsync(stream);
            }

            photoPath = "images/companies/" + fileName; // Cập nhật đường dẫn ảnh
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Không thể lưu ảnh công ty: " + ex.Message;
            return View();
        }
    }

    // Tạo bản ghi công ty mới và lưu vào database
    var company = new tblCompany
    {
        sNameCompany = companyName,
        iPhoneNumber = companyPhone,
        sAddressCompany = companyAddress,
        iNumberEmployees = numberEmployees,
        sDescriptionCompany = companyDescription,
        sPhotoCompany = photoPath
    };

    try
    {
        _context.tblCompany.Add(company);
        await _context.SaveChangesAsync(); // Lưu công ty vào database
    }
    catch (DbUpdateException dbEx)
    {
        // Log hoặc hiển thị thông tin chi tiết lỗi
        ViewBag.ErrorMessage = "Không thể lưu công ty: " + dbEx.Message;
        if (dbEx.InnerException != null)
        {
            ViewBag.ErrorMessage += " - " + dbEx.InnerException.Message;
        }
        return View();
    }
    catch (Exception ex)
    {
        ViewBag.ErrorMessage = "Lỗi xảy ra: " + ex.Message;
        return View();
    }

    // Sau khi công ty được lưu, lấy giá trị iCompanyID và tạo bản ghi nhà tuyển dụng
    var employer = new tblEmployer
    {
        iAccountID = iAccountID,
        sFullName = fullname,
        sPhoneNumber = phone,
        sAddress = address,
        iCompanyID = company.iCompanyID // Đảm bảo iCompanyID được gán giá trị hợp lệ
    };

    try
    {
        _context.tblEmployer.Add(employer);
        await _context.SaveChangesAsync(); // Lưu nhà tuyển dụng vào database
    }
    catch (DbUpdateException dbEx)
    {
        ViewBag.ErrorMessage = "Không thể lưu nhà tuyển dụng: " + dbEx.Message;
        if (dbEx.InnerException != null)
        {
            ViewBag.ErrorMessage += " - " + dbEx.InnerException.Message;
        }
        return View();
    }
    catch (Exception ex)
    {
        ViewBag.ErrorMessage = "Lỗi xảy ra: " + ex.Message;
        return View();
    }

    return RedirectToAction("Login", "Account"); // Chuyển hướng sau khi đăng ký thành công
}







        [HttpGet]
        public IActionResult ManagerAccount()
        {
            return View();
        }




    }


}

