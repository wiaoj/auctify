using Auctify.WalletService.Domain.Aggregates.WalletAggregate;

namespace Auctify.WalletService.Application.Abstractions.Persistence;
public interface IWalletRepository {
    Task AddAsync(Wallet wallet, CancellationToken cancellationToken);
}