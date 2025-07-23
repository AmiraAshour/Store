using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces
{
  public interface IGenericReposeitry<T> where T : class
  {
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllAsync ( params Expression< Func<T, object>>[] includes);
    Task<T> GetByIdAsync(int id);
    Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task   AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
  }
}
