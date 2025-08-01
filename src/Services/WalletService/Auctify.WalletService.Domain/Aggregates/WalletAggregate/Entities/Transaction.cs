using Auctify.Libraries.Domain.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.Enums;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;

namespace Auctify.WalletService.Domain.Aggregates.WalletAggregate.Entities;
public sealed class Transaction : Entity<TransactionId> {
    public WalletId WalletId { get; private set; }
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }

#pragma warning disable CS8618 
    private Transaction() { }
    private Transaction(TransactionId id) : base(id) { }
#pragma warning restore CS8618 

    public static Transaction New(TransactionId id, WalletId walletId, TransactionType type, Money amount, TimeProvider timeProvider) {
        return new Transaction(id) {
            WalletId = walletId,
            Type = type,
            Amount = amount,
            Timestamp = timeProvider.GetUtcNow()
        };
    }

    public static Transaction New(WalletId walletId, TransactionType type, Money amount, TimeProvider timeProvider) {
        return New(TransactionId.New(), walletId, type, amount, timeProvider);
    }
}