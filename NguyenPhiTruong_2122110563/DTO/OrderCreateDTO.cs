namespace NguyenPhiTruong_2122110563.DTO
{
    public class OrderCreateDTO
    {
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; } = string.Empty; // ✅ Thêm dòng này
        public List<OrderDetailCreateDTO> OrderDetails { get; set; } = new();
    }
}
