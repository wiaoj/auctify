using Auctify.Libraries.Domain.Abstractions.ValueObjects;

namespace Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;
public sealed record TransactionId : IId<TransactionId, Guid> {
    public Guid Value { get; private set; }

    private TransactionId() { }
    public static TransactionId From(Guid value) {
        return new TransactionId {
            Value = value
        };
    }

    public static TransactionId New() {
        return new TransactionId {
            Value = Guid.CreateVersion7()
        };
    }
}