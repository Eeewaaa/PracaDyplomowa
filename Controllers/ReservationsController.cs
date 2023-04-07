using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate3.Data.Cart;
using RealEstate3.Data.Services;
using RealEstate3.Data.ViewModels;
using System.Security.Claims;

namespace RealEstate3.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {

        private readonly IEstatesService _estatesService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IReservationsService _reservationsService;

        public ReservationsController(IEstatesService estatesService, ShoppingCart shoppingCart, IReservationsService reservationsService)
        {
            _estatesService = estatesService;
            _shoppingCart = shoppingCart;
            _reservationsService = reservationsService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role); 
            var reservations = await _reservationsService.GetReservationsByUserIdAndRoleAsync(userId, userRole);
            return View(reservations);
        }
        public IActionResult ShoppingCart()
        {
            ViewBag.Price = _shoppingCart.Price;
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
            };
            return View(response);
        }

        public async Task<RedirectToActionResult> AddToShoppingCart(int id)
        {
            var item = await _estatesService.GetEstateByIdAsync(id);
            
            if (item!= null)
            {
                if (User.IsInRole("Owner"))
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var OwnerUserId = item.Owner.UserId;
                    if (OwnerUserId == userId)
                    {
                        return RedirectToAction("Index", "Estates");
                    }
                }

                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));

        }

        public async Task<RedirectToActionResult> RemoveFromShoppingCart(int id)
        {
            var item = await _estatesService.GetEstateByIdAsync(id);
            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));

        }

        public async Task<IActionResult> CompleteReservation()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _reservationsService.StoreReservationsAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();

            return View("ReservationCompleted");
        }

    }
}
