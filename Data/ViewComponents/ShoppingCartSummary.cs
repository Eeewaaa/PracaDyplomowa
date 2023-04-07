using Microsoft.AspNetCore.Mvc;
using RealEstate3.Data.Cart;

namespace RealEstate3.Data.ViewComponents
{
    public class ShoppingCartSummary: ViewComponent
    {
        private readonly ShoppingCart _shoppingCart;
        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            var items = _shoppingCart.GetShoppingCartItems();

            return View(items.Count());   
        }

    }
}
