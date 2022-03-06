using ETickets.Data;
using ETickets.Data.Cart;
using ETickets.Data.Services;
using ETickets.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ETickets.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;
        public OrderController(IMoviesService moviesService, ShoppingCart cart, IOrdersService ordersService)
        {
            _moviesService = moviesService;
            _shoppingCart = cart;
            _ordersService = ordersService;
        }
        public IActionResult ShoppingCart()
        {
            var data = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = data;
            var response = new ShoppingCartViewModel()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(response);
        }

        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            var item = await _moviesService.GetByIdAsync(id);
            if(item != null)
            {
                _shoppingCart.AddItem(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }


        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _moviesService.GetByIdAsync(id);
            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }



        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = "";
            string userEmailAddress = "";

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();
            return View("OrderComplete");

        }

        public async Task<IActionResult> Index()
        {
            string userId = "";
            var orders = await _ordersService.GetOrderByUserIdAsync(userId);
            return View(orders);

        }
    }
}
