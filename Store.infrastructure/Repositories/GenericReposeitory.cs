using Microsoft.EntityFrameworkCore;
using Store.Core.Interfaces;
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
    public async Task AddAsync(T entity)
    {
      await _context.Set<T>().AddAsync(entity);
      await _context.SaveChangesAsync();
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

    public async Task<bool> UpdateAsync(T entity)
    {
      _context.Set<T>().Update(entity);
      var affectedRows = await _context.SaveChangesAsync();

      return affectedRows > 0; 
    }

  }
}
