using Microsoft.AspNetCore.Mvc;
using webquanlydouong.Data;
using webquanlydouong.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace webquanlydouong.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Xem danh sách đơn hàng
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.User)  // Đảm bảo thông tin người dùng được tải cùng với đơn hàng
                .ToList();
            return View(orders);  // Trả về View "Views/Order/Index.cshtml"
        }

        // Xem chi tiết đơn hàng
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Products)
                .Include(o => o.User)
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            return View(order);  // Trả về View "Views/Order/Details.cshtml"
        }

        // Cập nhật trạng thái đơn hàng thành "Hoàn thành"
        [HttpPost]
        public IActionResult CompleteOrder(int id)
        {
            var order = _context.Orders
                .Where(o => o.Id == id)
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu trạng thái đã là "Hoàn thành", không cho phép cập nhật nữa
            if (order.Status == "Completed")
            {
                TempData["ErrorMessage"] = "Đơn hàng này đã hoàn thành!";
                return RedirectToAction("Details", new { id = id });
            }

            // Cập nhật trạng thái của đơn hàng thành "Hoàn thành"
            order.Status = "Completed";
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Đơn hàng đã được hoàn thành!";
            return RedirectToAction("Index");  // Trở về trang danh sách đơn hàng
        }
    }
}
