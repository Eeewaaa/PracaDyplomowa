using Microsoft.AspNetCore.Identity;
using RealEstate3.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace RealEstate3.Models
{
    public class ApplicationUser: IdentityUser
    {

        [Required]
        [Display(Name = "Profil")]
        public virtual Owner? Owner { get; set; }

    }
}
