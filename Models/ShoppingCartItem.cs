using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate3.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public Estate Estate { get; set; }
        public string ShoppingCartId { get; set; }

    }
}
