namespace Auctify.WalletService.Application.Features.Wallets.Commands.CreateWallet;
public sealed record CreateWalletCommand : IRequest<CreateWalletResult> {
    public required Guid UserId { get; init; }
    public required Amount InitialAmount { get; init; }
    public required Currency Currency { get; init; }
}

public sealed record CreateWalletResult(Guid WalletId);