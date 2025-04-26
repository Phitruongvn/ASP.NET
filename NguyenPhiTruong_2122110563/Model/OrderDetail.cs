namespace NguyenPhiTruong_2122110563.Model
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        // 🔹 Mối quan hệ với Order
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // 🔹 Mối quan hệ với Product
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        // ✅ Tổng tiền cho sản phẩm này (Quantity * UnitPrice)
        public double Total => Quantity * UnitPrice;
    }
}
