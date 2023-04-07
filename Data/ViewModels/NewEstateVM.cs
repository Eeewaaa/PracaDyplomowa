using Microsoft.EntityFrameworkCore;
using RealEstate3.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate3.Models
{
    [Keyless]
    public class NewEstateVM
    {
        public int? Id { get; set; }


        [Required]
        [MaxLength(100, ErrorMessage = "Podany tytuł jest za długi")]
        public string? Title { get; set; }

        [MaxLength(2000, ErrorMessage = "Opis jest za długi")]
        [Display(Name = "Opis")]
        public string? Description { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Dostępne od")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AvaiableFrom { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Street { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Number { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Podaj kod w formacie XX-XXX")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Display(Name = "Powierzchnia")]
        [Range(6, 500)]
        public double Size { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Range(50, 50000, ErrorMessage = "Czynsz musi się zawierać w przedziale 50,00-50.000PLN")]
        [Display(Name = "Czynsz")]
        public double Rent { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Display(Name = "Ogród")]
        public bool Garden { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Display(Name = "Balkon")]
        public bool Balcony { get; set; }

        [Required(ErrorMessage ="Pole jest wymagane")]
        [Display(Name = "Przyjazne zwierzętom")]
        public bool PetFriendly { get; set; }

        [Display(Name = "Wybierz kategorię")]
        [Required(ErrorMessage = "Należy wybrać kategorię")]
        public int CategoryId { get; set; }

        [Display(Name = "Dodaj zdjęcia")]

        [NotMapped]
        public IFormFileCollection? PictureFiles { get; set; }

        public List<Picture>? PicturesList { get; set; }
        [Required]
        public int OwnerId { get; set; }




    }
}
