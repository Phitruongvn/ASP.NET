namespace NguyenPhiTruong_2122110563.Model
{
    public class ProductCreateRequest
    {
        public string ProductName { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
        public double Price { get; set; }
        public double PriceSale { get; set; }
        public string? Content { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
