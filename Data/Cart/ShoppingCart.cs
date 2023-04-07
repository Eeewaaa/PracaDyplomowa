using Microsoft.EntityFrameworkCore;
using RealEstate3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace RealEstate3.Data.Cart
{
    public class ShoppingCart
    {
        public REDbContext _context { get; set; } //Bo SCItems are stored in Db

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public double Price { get; set; }

        public ShoppingCart(REDbContext context)
        {
            _context = context;
            Price = 20.00;
        }

        //Metoda używana w start-up 
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<REDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        //Dodaj do koszyka
        public void AddItemToCart(Estate estate)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Estate.Id == estate.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null) 
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Estate = estate,

                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
                _context.SaveChanges();
            }

        }

        public void RemoveItemFromCart(Estate estate)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Estate.Id == estate.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                _context.ShoppingCartItems.Remove(shoppingCartItem);
                _context.SaveChanges();
            }
        }

        //Zwraca rzeczy w koszyku
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(n=> n.ShoppingCartId == ShoppingCartId).Include(n => n.Estate).Include(n => n.Estate.Category).ToList());
        }

        //Zwraca cene wszystkich rzeczy w koszyku
        public double GetShoppingCartTotal()
        {
            var amount = _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Count();
            var total = amount * Price;

            return total;
        }

        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
