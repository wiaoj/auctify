using Auctify.Libraries.Domain.Abstractions.DomainEvents;
using Auctify.Libraries.Domain.ValueObjects;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;

namespace Auctify.WalletService.Domain.Aggregates.WalletAggregate.DomainEvents;
public sealed record FundsWithdrawnDomainEvent : DomainEvent {
    private FundsWithdrawnDomainEvent() { }
    private FundsWithdrawnDomainEvent(DateTimeOffset occurredAt, DomainEventVersion version) : base(occurredAt, version) { }
    public WalletId WalletId { get; private set; } = null!;
    public Money Amount { get; private set; } = null!;

    internal static FundsWithdrawnDomainEvent From(WalletId walletId, Money amount, TimeProvider timeProvider) {
        return new FundsWithdrawnDomainEvent(timeProvider.GetUtcNow(), DomainEventVersion.New(1)) {
            WalletId = walletId,
            Amount = amount,
        };
    }
}