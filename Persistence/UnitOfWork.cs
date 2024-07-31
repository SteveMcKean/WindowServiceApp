using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class UnitOfWork: IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}