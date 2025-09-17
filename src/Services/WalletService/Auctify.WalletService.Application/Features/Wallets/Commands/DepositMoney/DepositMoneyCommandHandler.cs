using Auctify.WalletService.Domain.Aggregates.WalletAggregate;

namespace Auctify.WalletService.Application.Features.Wallets.Commands.DepositMoney;
internal sealed class DepositMoneyCommandHandler(IWalletRepository walletRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    : IRequestHandler<DepositMoneyCommand, object?> {

    public async Task<object?> HandleAsync(IRequestContext<DepositMoneyCommand> context, CancellationToken cancellationToken) {
        DepositMoneyCommand command = context.Request;

        Wallet wallet = await walletRepository.GetById(command.WalletId, cancellationToken);

        Money money = Money.New(Amount.New(command.Amount), wallet.Balance.Currency);  // maybe convert another currency?

        wallet.Deposit(money, timeProvider);

        await context.PublishAsync(wallet.DomainEvents);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return default;
    }
}