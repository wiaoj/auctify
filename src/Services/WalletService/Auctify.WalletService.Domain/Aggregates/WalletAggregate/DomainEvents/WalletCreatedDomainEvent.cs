using Auctify.Libraries.Domain.Abstractions.DomainEvents;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;

namespace Auctify.WalletService.Domain.Aggregates.WalletAggregate.DomainEvents;
public sealed record WalletCreatedDomainEvent : DomainEvent {
    private WalletCreatedDomainEvent() { }
    private WalletCreatedDomainEvent(DateTimeOffset occurredAt, DomainEventVersion version) : base(occurredAt, version) { }
    public WalletId WalletId { get; private set; } = null!;
    public Guid UserId { get; private set; }

    internal static WalletCreatedDomainEvent From(Wallet wallet, TimeProvider timeProvider) {
        return new WalletCreatedDomainEvent(timeProvider.GetUtcNow(), DomainEventVersion.New(1)) {
            WalletId = wallet.Id,
            UserId = wallet.UserId,
        };
    }
}