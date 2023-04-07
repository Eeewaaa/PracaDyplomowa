using RealEstate3.Data.Base;
using RealEstate3.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RealEstate3.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public List<ReservationItem>? ReservationItems { get; set; }
    }
}
