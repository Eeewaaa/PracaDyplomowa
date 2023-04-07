using RealEstate3.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace RealEstate3.Data.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Adres email")]

        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} musi mieć minimum {2} znaków", MinimumLength = 8)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Hasło musi mieć minimum 8 znaków oraz zawierać dużą literę (A-Z), małą literę (a-z) oraz cyfrę (0-9) lub znak specjalny (np. #,$,%)")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Potwierdź hasło")]

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła się nie zgadzają")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Tworzę konto jako:")]
        public Role Role { get; set; }
    }
}
