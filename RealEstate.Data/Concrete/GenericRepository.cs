using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Abstract;

namespace RealEstate.Data.Concrete;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseClass
{
    // We need DbContext
    protected readonly RealEstateDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(RealEstateDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public async Task<List<T>> GetAllAsync()
    {
        // AsNoTracking: It only improves performance when we are reading.
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }
}
