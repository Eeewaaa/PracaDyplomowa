using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RealEstate3.Data.Base;

namespace RealEstate3.Models
{
    public class Picture: IEntityBase
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? OriginalName { get; set; }
        [MaxLength(100)]
        public string? PictureURL { get; set; }
        //[NotMapped]
        //public IFormFile ImageFile { get; set; }

        public int EstateId { get; set; }
        public virtual Estate? Estate { get; set; }

    }
}
