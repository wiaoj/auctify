using Auctify.Libraries.Domain.Abstractions.DomainEvents;
using Auctify.Libraries.Domain.ValueObjects;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;

namespace Auctify.WalletService.Domain.Aggregates.WalletAggregate.DomainEvents;
public sealed record FundsDepositedDomainEvent : DomainEvent {
    private FundsDepositedDomainEvent() { }
    private FundsDepositedDomainEvent(DateTimeOffset occurredAt, DomainEventVersion version) : base(occurredAt, version) { }
    public WalletId WalletId { get; private set; } = null!;
    public Money Amount { get; private set; } = null!;

    internal static FundsDepositedDomainEvent From(WalletId walletId, Money amount, TimeProvider timeProvider) {
        return new FundsDepositedDomainEvent(timeProvider.GetUtcNow(), DomainEventVersion.New(1)) {
            WalletId = walletId,
            Amount = amount,
        };
    }
}