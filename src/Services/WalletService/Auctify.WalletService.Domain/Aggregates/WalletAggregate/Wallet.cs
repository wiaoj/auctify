using Auctify.Libraries.Domain.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.DomainEvents;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.Entities;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.Enums;
using Auctify.WalletService.Domain.Aggregates.WalletAggregate.ValueObjects;

namespace Auctify.WalletService.Domain.Aggregates.WalletAggregate;
public sealed class Wallet : Aggregate<WalletId> {
    public Guid UserId { get; private set; }
    public Money Balance { get; private set; }

    private readonly List<Transaction> transactions = [];
    public IReadOnlyCollection<Transaction> Transactions => this.transactions.AsReadOnly();

#pragma warning disable CS8618 
    private Wallet() { }
    private Wallet(WalletId id) : base(id) { }
#pragma warning restore CS8618

    public static Wallet New(WalletId id, Guid userId, Money startingBalance, TimeProvider timeProvider) {
        if(startingBalance.Amount < Amount.Zero) throw new InvalidOperationException("Wallet cannot be created with a negative balance.");

        Wallet wallet = new(id) {
            UserId = userId,
            Balance = startingBalance
        };

        wallet.AddDomainEvent(WalletCreatedDomainEvent.From(wallet, timeProvider));

        return wallet;
    }

    public void Deposit(Money amount, TimeProvider timeProvider) {
        if(amount.Currency != this.Balance.Currency) throw new InvalidOperationException("Cannot deposit money of a different currency.");

        this.Balance += amount;
        Transaction transaction = Transaction.New(this.Id, TransactionType.Deposit, amount, timeProvider);
        this.transactions.Add(transaction);

        AddDomainEvent(FundsDepositedDomainEvent.From(this.Id, amount, timeProvider));
    }

    public void Withdraw(Money amount, TimeProvider timeProvider) {
        if(amount.Currency != this.Balance.Currency) throw new InvalidOperationException("Cannot withdraw money of a different currency.");
        if(this.Balance < amount) throw new InvalidOperationException("Insufficient funds.");

        this.Balance -= amount;
        Transaction transaction = Transaction.New(this.Id, TransactionType.Withdrawal, amount, timeProvider);
        this.transactions.Add(transaction);


        AddDomainEvent(FundsWithdrawnDomainEvent.From(this.Id, amount, timeProvider));
    }
}