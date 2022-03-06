using ETickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETickets.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _db;
        public OrdersService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<Order>> GetOrderByUserIdAsync(string userId)
        {
            var orders = await _db.Orders.Include(n => n.OrderItem).ThenInclude(n => n.Movie).Where(n => n.UserId == userId).ToListAsync();
            return orders;
        }

        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress
            };

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            foreach(var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    MovieId = item.Movie.Id,
                    OrderId = order.Id,
                    Price = item.Movie.Price
                };
                await _db.OrderItems.AddAsync(orderItem);
            }
            await _db.SaveChangesAsync();
        }
    }
}
