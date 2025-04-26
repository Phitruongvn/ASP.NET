namespace NguyenPhiTruong_2122110563.Model
{
    public class ProductUpdateRequest
    {
        public string ProductName { get; set; }
        public IFormFile? ImageFile { get; set; } // Có thể null nếu không upload ảnh mới
        public double Price { get; set; }
        public double PriceSale { get; set; }
        public string Content { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
