namespace NguyenPhiTruong_2122110563.Model
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        public int? ParentId { get; set; }

        // Quan hệ tự ánh xạ (Self-referencing)
        public Menu? Parent { get; set; }
        public List<Menu> Children { get; set; } = new List<Menu>();
    }
}
