using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Product;
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
      try
      {
        await _context.Set<T>().AddAsync(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? entity : null;
      }
      catch
      {
        return null;
      }
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

    public async Task<IReadOnlyList<T>> GetAllAsync()
   => await _context.Set<T>().AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
      var query = _context.Set<T>().AsQueryable();
      foreach (var include in includes)
      {
        query = query.Include(include);
      }
      return await query.AsNoTracking().ToListAsync();
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
