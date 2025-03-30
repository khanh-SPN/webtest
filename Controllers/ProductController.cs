using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using webquanlydouong.Data;
using webquanlydouong.Models;

namespace webquanlydouong.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Khai báo thư mục lưu ảnh
        private readonly string _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Product/Index
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {

            string ImageUrl = product.ImageUrl;
            // Gán đường dẫn ảnh vào trường ImageUrl
           

            // Đảm bảo Orders không null (Khởi tạo danh sách trống nếu không có đơn hàng liên kết)
            if (product.Orders == null)
            {
                product.Orders = new List<Order>();
            }

            if (ModelState.IsValid)
            {
                // Lưu sản phẩm vào cơ sở dữ liệu
                _context.Products.Add(product);
                _context.SaveChanges();

                // Thông báo thành công
                TempData["SuccessMessage"] = "Sản phẩm đã được thêm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                // Hiển thị chi tiết lỗi validation
                var errorMessages = ModelState.Values
                                              .SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();

                // Ghi lỗi vào TempData để hiển thị trên giao diện
                TempData["ErrorMessage"] = string.Join("<br />", errorMessages);
                return View(product);
            }
        }


        // GET: /Product/Edit/{id}
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Product/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product, IFormFile imgFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Nếu có ảnh mới, cập nhật ImageUrl
                if (imgFile != null && imgFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imgFile.FileName);
                    var filePath = Path.Combine(_imageFolder, fileName);

                    // Kiểm tra thư mục tồn tại, nếu không có thì tạo mới
                    if (!Directory.Exists(_imageFolder))
                    {
                        Directory.CreateDirectory(_imageFolder);
                    }

                    // Lưu ảnh mới vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imgFile.CopyTo(stream);
                    }

                    // Cập nhật đường dẫn ảnh mới
                    product.ImageUrl = "/images/" + fileName;
                }

                // Cập nhật sản phẩm trong cơ sở dữ liệu
                _context.Products.Update(product);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Sản phẩm đã được cập nhật thành công!";
                return RedirectToAction("Index");
            }

            return View(product);
        }


        // GET: /Product/Delete/{id}
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Product/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Sản phẩm đã được xóa thành công!";
            return RedirectToAction("Index");
        }
    }
}
