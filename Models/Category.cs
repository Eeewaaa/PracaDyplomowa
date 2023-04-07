using Microsoft.EntityFrameworkCore.Query;
using RealEstate3.Data.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate3.Models
{
    public class Category : IEntityBase
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa kategorii")]
        public string? Name { get; set; }

        [ForeignKey("UpperCategoryId")]
        public int? UpperCategoryId { get; set; }

        [Display(Name = "Nazwa nadrzędnej kategorii")]
        public virtual Category? UpperCategory { get; set; }

        [Display(Name="Lista podkategorii")]
        public virtual List<Category>? SubCategories { get; set; }
        public virtual List<Estate>? Estates { get; set; }
    }
}
