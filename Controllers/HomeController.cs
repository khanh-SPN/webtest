using Microsoft.AspNetCore.Mvc;

namespace webquanlydouong.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index
        public IActionResult Index()
        {
            // Trang chủ hiển thị nội dung giới thiệu hoặc các thông tin cần thiết
            return View();
        }
    }
}
