using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenPhiTruong_2122110563.Data;
using NguyenPhiTruong_2122110563.Model;
using Microsoft.AspNetCore.Authorization;


namespace NguyenPhiTruong_2122110563.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Nếu có ảnh mới upload
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var fileExtension = Path.GetExtension(request.ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Chỉ cho phép định dạng JPG, JPEG, PNG hoặc WEBP.");
                }

                var folderPath = Path.Combine("wwwroot", "uploads");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var safeProductName = string.Join("_", request.ProductName.Split(Path.GetInvalidFileNameChars()));
                var fileName = safeProductName + fileExtension;
                var filePath = Path.Combine(folderPath, fileName);

                int count = 1;
                while (System.IO.File.Exists(filePath))
                {
                    fileName = $"{safeProductName}_{count}{fileExtension}";
                    filePath = Path.Combine(folderPath, fileName);
                    count++;
                }

                // Lưu file mới
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                // Cập nhật lại đường dẫn ảnh mới
                product.Image =  fileName;
            }

            // Cập nhật các thông tin khác
            product.ProductName = request.ProductName;
            product.Price = request.Price;
            product.PriceSale = request.PriceSale;
            product.Content = request.Content;
            product.UpdatedAt = DateTime.UtcNow;
            product.BrandId = request.BrandId;
            product.CategoryId = request.CategoryId;
            product.UserId = request.UserId;

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductCreateRequest request)
        {
            string imageUrl = "";

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                // 🛡️ 1. Kiểm tra định dạng file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var fileExtension = Path.GetExtension(request.ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Chỉ cho phép định dạng JPG, JPEG, PNG hoặc WEBP.");
                }

                // 🛡️ 2. Tạo thư mục uploads nếu chưa có
                var folderPath = Path.Combine("wwwroot", "uploads");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // 🛡️ 3. Đổi tên file an toàn
                var safeProductName = string.Join("_", request.ProductName.Split(Path.GetInvalidFileNameChars()));
                var fileName = safeProductName + fileExtension;
                var filePath = Path.Combine(folderPath, fileName);

                int count = 1;
                while (System.IO.File.Exists(filePath))
                {
                    fileName = $"{safeProductName}_{count}{fileExtension}";
                    filePath = Path.Combine(folderPath, fileName);
                    count++;
                }

                // 🛡️ 4. Lưu file ảnh
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                // 🛡️ 5. Gán đường dẫn ảnh
                imageUrl =  fileName;
            }

            // 🛡️ 6. Tạo Product mới
            var product = new Product
            {
                ProductName = request.ProductName,
                Image = imageUrl,
                Price = request.Price,
                PriceSale = request.PriceSale,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                UserId = request.UserId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }



        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
