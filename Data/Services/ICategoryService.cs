using RealEstate3.Data.Base;
using RealEstate3.Models;
using System.Threading.Tasks;

namespace RealEstate3.Data.Services
{
    public interface ICategoryService : IEntityBaseRepository<Category>
    {
       Task<IEnumerable<Category>> SetUpperCategory();
       Task<IEnumerable<Category>> GetSubCategories(int id);
    }
}
