using RealEstate3.Data.Base;
using RealEstate3.Data.ViewModels;
using RealEstate3.Models;
using System.Threading.Tasks;

namespace RealEstate3.Data.Services
{
    public interface IEstatesService: IEntityBaseRepository<Estate>
    {

        Task<Estate?> GetEstateByIdAsync(int id);
        Task<NewEstateDropdownsVM> GetNewEstateDropdownsValues();
        Task AddNewEstateAsync(NewEstateVM newEstate);

        Task UpdateEstateAsync(NewEstateVM newEstate);
        Task<string> UploadImage(string folderPath, IFormFile file);



    }
}
