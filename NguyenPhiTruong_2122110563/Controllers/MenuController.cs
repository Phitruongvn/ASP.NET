using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenPhiTruong_2122110563.Data;

using NguyenPhiTruong_2122110563.DTO;
using NguyenPhiTruong_2122110563.Model;

namespace NguyenPhiTruong_2122110563.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenuController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/menu/tree
        [HttpGet("tree")]
        public async Task<IActionResult> GetMenuTree()
        {
            var rootMenus = await _context.Menus
                .Where(m => m.ParentId == null)
                .Include(m => m.Children)
                .OrderBy(m => m.SortOrder)
                .ToListAsync();

            var result = rootMenus.Select(MapMenuToDto).ToList();

            return Ok(result);
        }

        // GET: api/menu
        // GET: api/menu
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var menus = await _context.Menus
                .Include(m => m.Children) // nếu cần children
                .OrderBy(m => m.SortOrder)
                .ToListAsync();

            var result = menus.Select(MapMenuToDto).ToList();

            return Ok(result);
        }


        // POST: api/menu
        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return Ok(menu);
        }

        // PUT: api/menu/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] Menu menu)
        {
            if (id != menu.Id) return BadRequest();

            _context.Entry(menu).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/menu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null) return NotFound();

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper: Map entity -> DTO (đệ quy)
        private MenuDto MapMenuToDto(Menu menu)
        {
            return new MenuDto
            {
                Id = menu.Id,
                Name = menu.Name,
                Link = menu.Link,
                Position = menu.Position,
                SortOrder = menu.SortOrder,
                Children = menu.Children?
                    .OrderBy(c => c.SortOrder)
                    .Select(MapMenuToDto)
                    .ToList() ?? new List<MenuDto>()
            };
        }
    }
}
