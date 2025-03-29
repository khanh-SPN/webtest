using Microsoft.AspNetCore.Mvc;
using webquanlydouong.Data;
using webquanlydouong.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;  // Đảm bảo bạn đã thêm thư viện này
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            
          
            // Lấy danh sách sản phẩm từ database
            var products = _context.Products.ToList();

            // Lấy giỏ hàng từ session
            var cart = GetCart();

            // Truyền giỏ hàng vào ViewData để hiển thị trong view
            ViewData["CartQuantity"] = cart.Sum(item => item.Quantity);  // Tổng số lượng sản phẩm trong giỏ hàng

            return View(products);
        
        }

        // Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int productId, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null || quantity <= 0) return BadRequest();

            // Lấy giỏ hàng từ session
            var cart = GetCart();

            // Kiểm tra nếu sản phẩm đã có trong giỏ hàng
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;  // Tăng số lượng nếu đã có trong giỏ hàng
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            // Lưu giỏ hàng vào session
            SetCart(cart);

            // Trở lại trang menu và hiển thị thông báo thành công
            TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng!";
            return RedirectToAction("Index");
        }


        // Lấy giỏ hàng từ session
        public List<CartItem> GetCart()
        {
            var cartBytes = HttpContext.Session.Get("Cart");  // Lấy dữ liệu byte[]
            if (cartBytes == null) return new List<CartItem>();  // Nếu không có giỏ hàng, trả về danh sách rỗng

            var cartJson = Encoding.UTF8.GetString(cartBytes);  // Chuyển byte[] thành chuỗi
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);  // Chuyển chuỗi thành List<CartItem>
            return cart;
        }

        // Lưu giỏ hàng vào session
        public void SetCart(List<CartItem> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            var cartBytes = Encoding.UTF8.GetBytes(cartJson);  // Chuyển sang byte[]
            HttpContext.Session.Set("Cart", cartBytes);  // Lưu vào Session
        }

        // Hiển thị giỏ hàng
        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Thanh toán và tạo đơn hàng
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            var userId = 1;  // Giả sử user đang đăng nhập có ID là 1

            // Tạo đơn hàng
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                Products = cart.Select(c => _context.Products.First(p => p.Id == c.ProductId)).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Xóa giỏ hàng sau khi thanh toán
            HttpContext.Session.Remove("Cart");

            TempData["SuccessMessage"] = "Thanh toán thành công!";
            return RedirectToAction("OrderDetails", new { id = order.Id });
        }

        // Xem chi tiết đơn hàng
        public IActionResult OrderDetails(int id)
        {
            var order = _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Products)
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

    }
}
