using RealEstate3.Data.Base;
using RealEstate3.Models;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace RealEstate3.Data.Services
{
    public class CategoryService : EntityBaseRepository<Category>, ICategoryService
    {
      
        private readonly REDbContext _context;
        public CategoryService( REDbContext context) : base(context)
        {
            _context = context;
        }

        async Task<IEnumerable<Category>> ICategoryService.SetUpperCategory()
        {

            var listOfCategories = await GetAllAsync();
            //foreach (var categoryItem in listOfCategories)
            //{
            //   categoryItem = await GetByIdAsync((int)categoryItem.UpperCategoryId);
            //}
            return listOfCategories;
        }

        //DLACZEGO NIE MOZE BYC ASYNC?
        async Task<IEnumerable<Category>> ICategoryService.GetSubCategories(int id)
        {           
            var subCategories = _context.Categories.Where(c => c.UpperCategoryId == id);
            return  subCategories;
        }


    }
}
