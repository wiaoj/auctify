using Auctify.WalletService.Domain.Aggregates.WalletAggregate;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;

namespace Auctify.WalletService.Application.Abstractions.Persistence;
public interface IWalletRepository {
    Task AddAsync(Wallet wallet, CancellationToken cancellationToken);
    Task<Wallet> GetById(WalletId id, CancellationToken cancellationToken);
}