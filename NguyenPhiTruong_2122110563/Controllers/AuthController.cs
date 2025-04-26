using Microsoft.AspNetCore.Mvc;
using NguyenPhiTruong_2122110563.Data;
using NguyenPhiTruong_2122110563.Model;
using NguyenPhiTruong_2122110563.Services;
using System.Linq;

namespace NguyenPhiTruong_2122110563.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegister request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("Email đã tồn tại.");
            }

            var newUser = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password  // ⚠️ Hash password sau này nha
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("Đăng ký thành công.");
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Sai email hoặc mật khẩu.");
            }

            var token = _jwtService.GenerateToken(user.UserName);
            return Ok(new
            {
                token,
                userId = user.UserId, // ✅ Thêm dòng này
                userName = user.UserName
            });

        }
    }
}
