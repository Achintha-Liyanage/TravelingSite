
using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity); // Typically synchronous
    void Delete(T entity); // Typically synchronous
    Task<int> SaveChangesAsync(); // To commit transactions
}
