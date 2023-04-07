using RealEstate3.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate3.Models
{
    public class Estate: IEntityBase
    {
        
        public int Id { get; set; }

        //[MaxLength(100)]
        //public string PictureUrl { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Podany tytuł jest za długi")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Powierzchnia")]
        [Range(6, 500)]
        public double Size { get; set; }

        [Required]
        [Range(50, 50000, ErrorMessage = "Czynsz musi się zawierać w przedziale 50,00-50.000PLN")]
        [Display(Name = "Czynsz")]
        public double Rent { get; set; }

        [Required]
        [Display(Name = "Ogród")]
        public bool Garden { get; set; }

        [Required]
        [Display(Name = "Balkon")]
        public bool Balcony { get; set; }

        [Required]
        [Display(Name = "Przyjazne zwierzętom")]
        public bool PetFriendly { get; set; }

        [MaxLength(2000, ErrorMessage = "Opis jest za długi")]
        [Display(Name = "Opis")]
        public string? Description { get; set; }

        // [Required]
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Dostępne od")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AvaiableFrom { get; set; }

        //Relationships

        public int AddressId { get; set; }

        [Required]
        [Display(Name = "Adres")]
        public virtual Address Address { get; set; }

        public int OwnerId { get; set; }
        public virtual Owner? Owner { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Display(Name = "Kategoria")]
        public virtual Category? Category { get; set; }

        [NotMapped]
        public IFormFileCollection? PictureFiles { get; set; }
        public List<Picture>? PicturesList { get; set; }
    }
}
