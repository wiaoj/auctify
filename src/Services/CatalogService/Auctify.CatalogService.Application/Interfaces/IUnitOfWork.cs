namespace Auctify.CatalogService.Application.Interfaces;
public interface IUnitOfWork {
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}