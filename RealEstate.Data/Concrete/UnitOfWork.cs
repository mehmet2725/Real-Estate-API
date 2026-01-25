using System.Threading.Tasks;
using RealEstate.Data.Abstract;

namespace RealEstate.Data.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly RealEstateDbContext _context;

    public UnitOfWork(RealEstateDbContext context)
    {
        _context = context;
    }
    public void Commit()
    {
        _context.SaveChanges();
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
