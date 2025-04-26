namespace NguyenPhiTruong_2122110563.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Một User có thể có nhiều Product (nếu bạn muốn quản lý sản phẩm theo user)
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
