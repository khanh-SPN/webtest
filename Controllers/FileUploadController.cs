using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace webquanlydouong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly string _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public FileUploadController()
        {
            // Kiểm tra nếu thư mục chưa tồn tại thì tạo mới
            if (!Directory.Exists(_imageFolder))
            {
                Directory.CreateDirectory(_imageFolder);
            }
        }

        // POST: /api/fileupload/upload
        [HttpPost("upload")]
        public IActionResult UploadImage(IFormFile imgFile)
        {
            // Kiểm tra nếu không có ảnh được tải lên
            if (imgFile == null || imgFile.Length == 0)
            {
                return BadRequest(new { success = false, message = "Ảnh sản phẩm là bắt buộc." });
            }

            try
            {
                // Tạo tên file mới để tránh trùng lặp
                var fileName = Path.GetFileName(imgFile.FileName);
                var filePath = Path.Combine(_imageFolder, fileName);

                // Lưu ảnh vào thư mục wwwroot/images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imgFile.CopyTo(stream);
                }

                // Trả về đường dẫn ảnh
                var imageUrl = "/images/" + fileName;
                return Ok(new { success = true, imageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi tải ảnh: " + ex.Message });
            }
        }
    }
}
