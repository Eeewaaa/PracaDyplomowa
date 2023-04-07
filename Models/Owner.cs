using RealEstate3.Data.Base;
using RealEstate3.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate3.Models
{
    public class Owner : IEntityBase
    {
            public int Id { get; set; }
            [Required]
            [Display(Name = "Imię")]
            [StringLength(20, MinimumLength = 2, ErrorMessage = "Imię musi mieć minimum 2 znaki, maksymalnie 20")]
            public string? Name { get; set; }
            [Required]
            [Display(Name ="Nazwisko")]
            [StringLength(40, MinimumLength = 2, ErrorMessage = "Nazwisko musi mieć minimum 2 znaki, maksymalnie 40")]
            public string? Surname { get; set; }

            [Display(Name = "Płeć")]
            [Required]
            public Gender Gender { get; set; }

            [MaxLength(500)]
            public string? ProfilePictureUrl { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Data urodzenia")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? Birthday { get; set; }

            [MaxLength(100)]
            public string? Comment { get; set; }
            [Required]
            [RegularExpression(@"^\d{9}$")]
            [Display(Name = "Numer telefronu")]
            public string? ContactNumber { get; set; }

            [NotMapped]
            [Display(Name = "Zdjęcie profilowe")]
            public IFormFile? ProfilePic { get; set; }

            //Relationships
            public virtual List<Estate>? EstateList { get; set; }

            public string? UserId { get; set; }

            [ForeignKey(nameof(UserId))]
            public ApplicationUser? User { get; set; }



    }
}
