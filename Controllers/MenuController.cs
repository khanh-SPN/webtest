using Microsoft.AspNetCore.Mvc;
using webquanlydouong.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;  // Thêm namespace để sử dụng session

namespace webquanlydouong.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Menu/Index
        public IActionResult Index()
        {
            // Kiểm tra session, nếu không có session thì chuyển về trang chủ
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                // Nếu không có session, điều hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }

            // Nếu có session, lấy danh sách tất cả các sản phẩm
            var products = _context.Products.ToList();

            // Trả về tất cả sản phẩm cho View
            return View(products);
        }
    }
}
