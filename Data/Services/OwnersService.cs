using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using RealEstate3.Data.Base;
using RealEstate3.Models;

namespace RealEstate3.Data.Services
{
    public class OwnersService : EntityBaseRepository<Owner>, IOwnersService
    {
        private readonly REDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public OwnersService(REDbContext context, IWebHostEnvironment webHostEnvironment) : base(context) 
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<Owner> GetOwnerByUserId(string userId)
        {
         
                var ownerDetails = await _context.Set<Owner>()
                    .Include(d => d.EstateList)
                    .FirstOrDefaultAsync(n => n.UserId == userId);

            //var Owner = await _context.Owners.FirstOrDefaultAsync(n => n.UserId == userId);
            return ownerDetails;
        }

        public async Task<bool> IsOwnerAssigned(string userId)
        {
            var checkedOwner = await _context.Set<Owner>().FirstOrDefaultAsync(n => n.UserId == userId);
            if (checkedOwner == null) return true;
            else return false;
        }

        public async Task<string> UploadImage(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
        }

        public bool ValidateOwner(string userId, Owner owner)
        {
            if (userId == owner.UserId) return true;
            else return false;
        }


    }
}
