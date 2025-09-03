using Microsoft.EntityFrameworkCore;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.infrastructure.Data;
using System.Linq.Expressions;

namespace Store.infrastructure.Repositories
{
  public class GenericReposeitory<T> : IGenericReposeitry<T> where T : class
  {
    private readonly AppDbContext _context;
    public GenericReposeitory(AppDbContext context)
    {
      _context = context;
    }
    public async Task<T?> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? entity : null;
     
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var entity= await _context.Set<T>().FindAsync(id);
      if (entity != null){
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return true;
      }
      return false;
    }

    public IQueryable<T>? GetAll()
   =>  _context.Set<T>().AsNoTracking();

    public  IQueryable<T>? GetAll(params Expression<Func<T, object>>[] includes)
    {
      var query = _context.Set<T>().AsQueryable();
      foreach (var include in includes)
      {
        query = query.Include(include);
      }
      return  query.AsNoTracking();
    }

    public async Task<T> GetByIdAsync(int id)
    {
      var entity= await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
      return entity!;
    }

    public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
      var query = _context.Set<T>().AsQueryable();
      foreach (var include in includes)
      {
        query = query.Include(include);
      }
      var entity= await query.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
      return entity!;
    }

    public async Task<T> UpdateAsync(T entity)
    {
      _context.Set<T>().Update(entity);
      await _context.SaveChangesAsync();

      return entity; 
    }

  }
}
