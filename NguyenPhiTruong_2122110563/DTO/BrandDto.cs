namespace NguyenPhiTruong_2122110563.Dto
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }

    
}
