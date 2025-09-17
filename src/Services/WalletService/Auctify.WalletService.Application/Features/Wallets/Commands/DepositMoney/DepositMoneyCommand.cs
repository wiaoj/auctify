using Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;

namespace Auctify.WalletService.Application.Features.Wallets.Commands.DepositMoney;
public sealed record DepositMoneyCommand : IRequest<object> {
    public required Guid UserId { get; set; }
    public required WalletId WalletId { get; set; }
    public decimal Amount { get; set; }
}