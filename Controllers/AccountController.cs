using Microsoft.AspNetCore.Mvc;
using webquanlydouong.Models;
using webquanlydouong.Data;
using Microsoft.AspNetCore.Http;  // Thêm namespace để sử dụng session

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Trang đăng nhập
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user != null && user.Password == model.Password)  // So sánh mật khẩu thủ công
            {
                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);  // Lưu role vào session

                // Sử dụng TempData để truyền thông tin người dùng vào các view
                TempData["UserEmail"] = user.Email;
                TempData["UserRole"] = user.Role;

                // Điều hướng theo role
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Product");
                }
                else if (user.Role == "User")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // Thêm thông báo lỗi khi đăng nhập không thành công
                TempData["LoginError"] = "Email hoặc mật khẩu không đúng.";
            }
        }

        return View(model);
    }

    // Trang đăng ký
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra nếu mật khẩu và mật khẩu xác nhận trùng khớp
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu và xác nhận mật khẩu không khớp.");
                return View(model);
            }

            var user = new User
            {
                Username = model.Email, // Gán email cho username
                Email = model.Email,    // Gán email
                Role = "User",          // Giả sử role mặc định là "User"
                Password = model.Password  // Gán mật khẩu từ RegisterViewModel vào
            };

            _context.Users.Add(user);
            _context.SaveChanges(); // Lưu user vào cơ sở dữ liệu

            return RedirectToAction("Login");  // Chuyển hướng tới trang đăng nhập
        }

        // Trả về view với thông báo lỗi nếu có
        ModelState.AddModelError(string.Empty, "Có lỗi xảy ra trong quá trình đăng ký.");
        return View(model);
    }

    // Đăng xuất
    public IActionResult Logout()
    {
        // Xóa thông tin người dùng khỏi session khi đăng xuất
        HttpContext.Session.Remove("UserEmail");
        HttpContext.Session.Remove("UserRole");

        return RedirectToAction("Index", "Home");
    }
}

