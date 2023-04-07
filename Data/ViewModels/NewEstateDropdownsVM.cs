using RealEstate3.Models;

namespace RealEstate3.Data.ViewModels
{
    public class NewEstateDropdownsVM
    {
        public NewEstateDropdownsVM()
        {
            Owners = new List<Owner>();
            Categories = new List<Category>();
        }
        public List<Category> Categories { get; set; }
        public List<Owner> Owners { get; set;  }
    }
}
