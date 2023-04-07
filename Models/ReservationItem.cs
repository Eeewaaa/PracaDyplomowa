using RealEstate3.Data.Base;
using RealEstate3.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate3.Models
{
    public class ReservationItem
    {

        public int? Id { get; set; }
        public double? Price { get; set; }

        //RELATIONSHIPS
        public int? EstateId { get; set; }

        public virtual Estate? Estate { get; set; }

        public int? ReservationId { get; set; }
     
        public virtual Reservation? Reservation { get; set; }
    }
}
