using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RealEstate3.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace RealEstate3.Data.Services
{
    public class ReservationsService: IReservationsService
    {
        private readonly REDbContext _context;

        public ReservationsService(REDbContext context)
        {
            _context = context;
        }

        public Task<List<Reservation>> GetReservationsByUserIdAndRoleAsync(string userId, string userRole)
        {
            var reservations = _context.Reservations.Include(n => n.ReservationItems).ThenInclude(n => n.Estate).Where(n => n.UserId == userId).ToListAsync();

            if(userRole == "Admin")
            {
                reservations = _context.Reservations.Include(n => n.ReservationItems).ThenInclude(n => n.Estate).Include(n => n.User).ToListAsync();
            }   
            return reservations;
        }

        public async Task StoreReservationsAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var reservation = new Reservation()
            {
                UserId = userId,
                Email = userEmailAddress,
            };
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            foreach(var item in items)
            {
                var reservationItem = new ReservationItem()
                {
                    EstateId = item.Estate.Id,
                    Price = 20.00,
                    ReservationId = reservation.Id,
                };
               await  _context.ReservationItems.AddAsync(reservationItem);
            }
            await _context.SaveChangesAsync();
        }
    }
}
