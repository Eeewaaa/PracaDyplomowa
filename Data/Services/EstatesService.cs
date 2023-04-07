using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using RealEstate3.Data.Base;
using RealEstate3.Data.ViewModels;
using RealEstate3.Models;
using System.Linq.Expressions;

namespace RealEstate3.Data.Services
{
    public class EstatesService : EntityBaseRepository<Estate>, IEstatesService
    {
        private readonly REDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EstatesService(REDbContext context, IWebHostEnvironment webHostEnvironment ) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task<string> UploadImage(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
        }

        public async Task AddNewEstateAsync(NewEstateVM data)
        {

            var Address = new Address()
            {
                Street = data.Street,
                Number = data.Number,
                PostalCode = data.PostalCode
            };

            var newEstate = new Estate()
                {
                Title = data.Title,
                Description = data.Description,
                Size = data.Size,
                Rent = data.Rent,
                AvaiableFrom = data.AvaiableFrom,
                Balcony = data.Balcony,
                PetFriendly = data.PetFriendly,
                Address = Address,
                CategoryId = data.CategoryId,
                OwnerId = data.OwnerId,
                PicturesList = data.PicturesList
                };

            await _context.Estates.AddAsync(newEstate);
            await _context.SaveChangesAsync();
        }

        public async Task<Estate?> GetEstateByIdAsync(int id)
        {
            var estateDetails = await _context.Set<Estate>()
                .Include(o => o.Owner)
                .Include(a => a.Address)
                .Include(d => d.PicturesList)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(n => n.Id == id);
  
            return estateDetails;
        }

        public async Task<NewEstateDropdownsVM> GetNewEstateDropdownsValues()
        {
            var response = new NewEstateDropdownsVM()
            {
                Categories = await _context.Categories.OrderBy(n => n.UpperCategoryId).ToListAsync(),
                
                Owners = await _context.Owners.OrderBy(n => n.Surname).ToListAsync(),
            };
            return response;
        }

        public async Task UpdateEstateAsync(NewEstateVM data)
        {
            var dbEstate = await _context.Estates.FirstOrDefaultAsync(n => n.Id == data.Id);
            if(dbEstate != null)
            {
                var Address = new Address()
                {
                    Street = data.Street,
                    Number = data.Number,
                    PostalCode = data.PostalCode
                };

                dbEstate.Title = data.Title;
                dbEstate.Description = data.Description;
                dbEstate.Size = data.Size;
                dbEstate.Rent = data.Rent;
                dbEstate.AvaiableFrom = data.AvaiableFrom;
                dbEstate.Balcony = data.Balcony;
                dbEstate.PetFriendly = data.PetFriendly;
                dbEstate.Address = Address;
                dbEstate.CategoryId = data.CategoryId;
                dbEstate.OwnerId = data.OwnerId;
                dbEstate.PicturesList = data.PicturesList;
                
                await _context.SaveChangesAsync();

                //Remove Owner from DB if no estates left? 
            }
        }

        //public async Task<string> GetUserIdByEstateId(int id)
        //{   
        //    var estateDetails =await GetEstateByIdAsync(id);
        //    var OwnerUserId = estateDetails.Owner.UserId;
            
        //    return OwnerUserId;
        //}


    }
}
