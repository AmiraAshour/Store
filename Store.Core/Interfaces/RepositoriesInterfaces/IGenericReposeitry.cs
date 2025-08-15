using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces.RepositoriesInterFaces
{
  public interface IGenericReposeitry<T> where T : class
  {
   IQueryable<T>?GetAll();
    IQueryable<T>? GetAll ( params Expression< Func<T, object>>[] includes);
    Task<T> GetByIdAsync(int id);
    Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task <T?>  AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
  }
}
