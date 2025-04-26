namespace NguyenPhiTruong_2122110563.DTO
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public int? ParentId { get; set; }
        public List<MenuDto> Children { get; set; } = new List<MenuDto>();
    }

}
