using Auctify.Libraries.Domain.Abstractions.Exceptions;
using Auctify.Libraries.Domain.Abstractions.ValueObjects;
using Wiaoj;

namespace Auctify.Libraries.Domain.Abstractions.DomainEvents;
public abstract record DomainEvent : IDomainEvent {
    public DomainEventId Id { get; }
    public DateTimeOffset OccurredAt { get; }
    public DomainEventVersion Version { get; }

#pragma warning disable CS8618  
    protected DomainEvent() { }
#pragma warning restore CS8618  
    protected DomainEvent(DateTimeOffset occurredAt, DomainEventVersion version) {
        this.Id = DomainEventId.New(occurredAt);
        this.OccurredAt = occurredAt;
        this.Version = version;
    }

    public override int GetHashCode() {
        return HashCode.Combine(this.Id, this.OccurredAt, this.Version);
    }
}
public sealed record DomainEventId : IValueObject<DomainEventId, DateTimeOffset> {
    public string Value { get; }

    private DomainEventId() { }
    private DomainEventId(Guid id) {
        this.Value = id.ToString();
    }

    public static DomainEventId New(DateTimeOffset value) {
        return new(Guid.CreateVersion7(value));
    }

    public static implicit operator string(DomainEventId id) {
        return id.Value;
    }
}

public sealed record DomainEventVersion : IValueObject<DomainEventVersion, int> {
    public int Value { get; }

    private DomainEventVersion() { }
    private DomainEventVersion(int value) {
        this.Value = value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="DomainEventVersionCannotBeNegativeException">Thrown when the value is negative.</exception>
    public static DomainEventVersion New(int value) {
        Preca.ThrowIfNegative<int, DomainEventVersionCannotBeNegativeException>(value);
        return new(value);
    }

    public static implicit operator int(DomainEventVersion version) {
        return version.Value;
    }
}
public sealed class DomainEventVersionCannotBeNegativeException() : DomainException($"Domain event version cannot be negative.");

public interface IDomainEvent {
    DomainEventId Id { get; }
    DateTimeOffset OccurredAt { get; }
    DomainEventVersion Version { get; }
}
public interface IHasDomainEvent {
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent @event);
    void ClearDomainEvents();
}