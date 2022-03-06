using ETickets.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETickets.Data.Cart
{
    public class ShoppingCart
    {

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems{ get; set; }
        public AppDbContext _db { get; set; }

        public ShoppingCart(AppDbContext db)
        {
            _db = db;
        }


        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _db.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Movie).ToList());
        }

        public double GetShoppingCartTotal()
        {
            var total = _db.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Movie.Price * n.Amount).Sum();
            return total;
        }

        public void AddItem(Movie movie)
        {
            var shoppingCartItem = _db.ShoppingCartItems.FirstOrDefault(n => n.ShoppingCartId == ShoppingCartId && n.Movie.Id == movie.Id);

            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie, 
                    Amount = 1
                };
                _db.ShoppingCartItems.Add(shoppingCartItem);
            } else
            {
                shoppingCartItem.Amount++;
            }
            _db.SaveChanges();
        }

        public void RemoveItemFromCart(Movie movie)
        {
            var item = _db.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);
            if(item != null)
            {
                if(item.Amount > 1)
                {
                    item.Amount--;
                } else
                {
                    _db.ShoppingCartItems.Remove(item);
                }
            }
            _db.SaveChanges();
        }

        public async Task ClearShoppingCartAsync()
        {
            var items = await _db.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _db.ShoppingCartItems.RemoveRange(items);
            await _db.SaveChangesAsync();
        }

    }
}
