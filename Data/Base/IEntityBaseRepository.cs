using Microsoft.AspNetCore.Hosting;
using RealEstate3.Data.Services;
using RealEstate3.Models;
using System.Linq.Expressions;

namespace RealEstate3.Data.Base
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdAsync(int id);
        //Task<Estate> GetEstateByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);

    }
}
