using Auctify.Libraries.Domain.Abstractions.DomainEvents;
using Auctify.Libraries.Domain.Abstractions.Exceptions;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Wiaoj;
using Wiaoj.Extensions;

namespace Auctify.Libraries.Domain.Abstractions;
public abstract class Aggregate<TId> : Entity<TId>, IAggregate, IHasDomainEvent
    where TId : class, IId<TId> {
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsDeleted => this.DeletedAt.HasValue;

    private readonly List<IDomainEvent> domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

    protected Aggregate() { }
    protected Aggregate(TId id) : base(id) { }

    public void Delete(DateTimeOffset deletedAt) {
        Preca.ThrowIf<EntityAlreadyDeletedException>(this.IsDeleted);
        this.DeletedAt = deletedAt;
    }

    public void Restore() {
        Preca.ThrowIfFalse<EntityNotDeletedException>(this.IsDeleted);
        this.DeletedAt = null;
    }

    public void SetCreatedAt(DateTimeOffset createdAt) {
        Preca.ThrowIf<CreatedAtAlreadySetException>(this.CreatedAt != default);
        this.CreatedAt = createdAt;
    }

    public void SetUpdatedAt(DateTimeOffset updatedAt) {
        Preca.ThrowIfDefault(updatedAt);
        this.UpdatedAt = updatedAt;
    }

    public void AddDomainEvent(IDomainEvent @event) {
        Preca.ThrowIfNull(@event);
        if(this.domainEvents.Exists(x => x == @event))
            return;

        this.domainEvents.Add(@event);
    }

    public void ClearDomainEvents() {
        this.domainEvents.Clear();
    }
}

public abstract class Entity<TId> where TId : class, IId<TId> {
    public TId Id { get; private set; }

#pragma warning disable CS8618
    protected Entity() { }
#pragma warning restore CS8618

    protected Entity(TId id) {
        this.Id = Preca.Extensions.ThrowIfNull(id);
    }

    // Eşitlik kontrolü, sadece ve sadece ID'ye göre yapılır. 
    public override Boolean Equals(Object? obj) {
        if(obj is not Entity<TId> other)
            return false;

        if(ReferenceEquals(this, other))
            return true;

        return this.Id != default && other.Id != default && this.Id.Equals(other.Id);
    }

    public override int GetHashCode() {
        // GetType()'ı dahil etmek, farklı Entity tiplerinin (ama aynı ID değerine sahip)
        // yanlışlıkla eşit sayılmasını engeller.
        return (GetType().ToString() + this.Id).GetHashCode();
    }
}