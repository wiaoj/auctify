using Auctify.WalletService.Domain.Aggregates.WalletAggregate;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;

namespace Auctify.WalletService.Application.Features.Wallets.Commands.CreateWallet;
internal sealed class CreateWalletCommandHandler(IWalletRepository walletRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    : IRequestHandler<CreateWalletCommand, CreateWalletResult> {
    public async Task<CreateWalletResult> HandleAsync(IRequestContext<CreateWalletCommand> context, CancellationToken cancellationToken) {
        CreateWalletCommand command = context.Request;

        Wallet wallet = Wallet.New(
            WalletId.New(),
            command.UserId,
            Money.New(command.InitialAmount, command.Currency),
            timeProvider
        );
         
        await walletRepository.AddAsync(wallet, cancellationToken);
        await context.PublishAsync(wallet.DomainEvents);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateWalletResult(wallet.Id.Value);
    }
}