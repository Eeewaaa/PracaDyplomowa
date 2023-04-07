using RealEstate3.Data.Base;
using RealEstate3.Models;

namespace RealEstate3.Data.Services
{
    public interface IOwnersService : IEntityBaseRepository<Owner>
    {
       Task<string> UploadImage(string folderPath, IFormFile file);

      Task<bool> IsOwnerAssigned(string userId);

       Task<Owner> GetOwnerByUserId(string userId);

        bool ValidateOwner(string userId, Owner owner);
    }
}
