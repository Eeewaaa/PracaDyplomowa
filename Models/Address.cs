using RealEstate3.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace RealEstate3.Models
{
    public class Address: IEntityBase
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string? Street { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Number { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Podaj kod w formacie XX-XXX")]
        public string? PostalCode { get; set; }

        //Relationships
        public virtual Estate? Estate { get; set; }

      
    }
}
