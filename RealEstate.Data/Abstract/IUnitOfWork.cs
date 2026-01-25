using System.Threading.Tasks;

namespace RealEstate.Data.Abstract;

public interface IUnitOfWork
{
    Task CommitAsync(); // SaveChangesAsync
    void Commit();  //Save the Senkron
}
