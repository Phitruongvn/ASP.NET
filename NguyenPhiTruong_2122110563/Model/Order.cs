namespace NguyenPhiTruong_2122110563.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public double TotalAmount { get; set; }
        public string Address { get; set; } = string.Empty; // ✅ Thêm dòng này


        // 🔹 Mối quan hệ với User
        public int UserId { get; set; }
        public User? User { get; set; }

        // 🔹 Mỗi Order có nhiều OrderDetails
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
