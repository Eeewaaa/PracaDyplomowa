using RealEstate3.Models;

namespace RealEstate3.Data.Services
{
    public interface IReservationsService
    {
        Task StoreReservationsAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
        Task<List<Reservation>> GetReservationsByUserIdAndRoleAsync(string userId, string userRole);
    }
}
