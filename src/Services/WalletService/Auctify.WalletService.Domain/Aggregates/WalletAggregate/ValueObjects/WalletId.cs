using Auctify.Libraries.Domain.Abstractions.ValueObjects;

namespace Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;
public sealed record WalletId : IId<WalletId, Guid> {
    public Guid Value { get; private set; }

    private WalletId() { }
    public static WalletId From(Guid value) {
        return new WalletId {
            Value = value
        };
    }

    public static WalletId New() {
        return new WalletId {
            Value = Guid.CreateVersion7()
        };
    }
}