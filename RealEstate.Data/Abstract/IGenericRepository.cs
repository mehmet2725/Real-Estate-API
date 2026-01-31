using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RealEstate.Entity.Abstract;

namespace RealEstate.Data.Abstract;

public interface IGenericRepository<T> where T : BaseClass
{
    // Retrieve by ID
    Task<T?> GetByIdAsync(int id);

    // Bring all
    // Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllAsync(params string[] includes);

    // Retrieve based on filter.
    // We return an IQueryable so that the query doesn't run immediately, but we can add to it later.
    IQueryable<T> Where(Expression<Func<T, bool>> expression);

    // Is it there or not? Checking
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

    // Adding
    Task AddAsync(T Entity);

    // Removing (We don't physically delete it from the database; we perform an `IsDeleted` operation in the service.)
    void Remove(T Entity);

    // Update
    void Update(T Entity);

    IQueryable<T> GetQuery(params string[] includes);
}
