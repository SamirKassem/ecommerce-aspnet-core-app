using ETickets.Data.Cart;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Data.ViewComponents
{
    public class ShoppingCartSummary:ViewComponent
    {
        private readonly ShoppingCart _cart;
        public ShoppingCartSummary(ShoppingCart cart)
        {
            _cart = cart;
        }

        //Default method needed to show a view component.
        public IViewComponentResult Invoke()
        {
            var items = _cart.GetShoppingCartItems();
            return View(items.Count);
        }
    }
}
