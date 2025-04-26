using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenPhiTruong_2122110563.Data;
using NguyenPhiTruong_2122110563.Model;
using NguyenPhiTruong_2122110563.DTO;
using Microsoft.AspNetCore.Authorization;


namespace NguyenPhiTruong_2122110563.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // ✅ GET: api/Orders/5 (lấy chi tiết đơn hàng theo id)
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Where(o => o.OrderId == id)
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            var result = new
            {
                order.OrderId,
                order.OrderDate,
                order.TotalAmount,
                order.UserId,
                OrderDetails = order.OrderDetails.Select(od => new
                {
                    od.OrderDetailId,
                    od.OrderId,
                    od.ProductId,
                    ProductName = od.Product?.ProductName,
                    ProductImage = od.Product?.Image,
                    od.Quantity,
                    od.UnitPrice,
                    Total = od.Quantity * od.UnitPrice
                })
            };

            return Ok(result);
        }

        // ✅ GET: api/Orders/user/3 (lấy tất cả đơn hàng của 1 user)
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetOrdersByUser(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var result = orders.Select(order => new
            {
                order.OrderId,
                order.OrderDate,
                order.TotalAmount,
                order.UserId,
                OrderDetails = order.OrderDetails.Select(od => new
                {
                    od.OrderDetailId,
                    od.ProductId,
                    ProductName = od.Product?.ProductName,
                    ProductImage = od.Product?.Image,
                    od.Quantity,
                    od.UnitPrice,
                    Total = od.Quantity * od.UnitPrice
                })
            });

            return Ok(result);
        }

        // ✅ POST: api/Orders/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO dto)
        {
            var order = new Order
            {
                OrderDate = dto.OrderDate,
                TotalAmount = dto.TotalAmount,
                UserId = dto.UserId,
                Address = dto.Address, // ✅ Lấy địa chỉ từ DTO
                OrderDetails = dto.OrderDetails.Select(od => new OrderDetail
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đặt hàng thành công!",
                orderId = order.OrderId
            });
        }


        // ✅ PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // ✅ DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }

    // ✅ DTOs
    public class OrderCreateDTO
    {
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; } = string.Empty; // ✅ Thêm dòng này
        public List<OrderDetailCreateDTO> OrderDetails { get; set; } = new();
    }

    public class OrderDetailCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
