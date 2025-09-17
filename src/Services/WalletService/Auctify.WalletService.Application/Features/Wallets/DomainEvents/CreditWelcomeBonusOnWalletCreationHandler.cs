using Auctify.WalletService.Domain.Aggregates.WalletAggregate;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.DomainEvents;

namespace Auctify.WalletService.Application.Features.Wallets.DomainEvents;
internal sealed class CreditWelcomeBonusOnWalletCreationHandler(IWalletRepository walletRepository, TimeProvider timeProvider, IUnitOfWork unitOfWork) : IDomainEventHandler<WalletCreatedDomainEvent> {
    private const int WelcomeBonusAmount = 1000;
    public async Task HandleAsync(WalletCreatedDomainEvent domainEvent, CancellationToken cancellationToken) { 
        Wallet wallet = await walletRepository.GetById(domainEvent.WalletId, cancellationToken);

        Money money = Money.New(Amount.New(WelcomeBonusAmount), Currency.USD);

        wallet.Deposit(money, timeProvider);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}