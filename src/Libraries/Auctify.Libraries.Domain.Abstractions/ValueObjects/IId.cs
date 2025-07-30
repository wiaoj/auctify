namespace Auctify.Libraries.Domain.Abstractions.ValueObjects;
public interface IId<out TSelf, TValue> : IValueObject<TSelf> {
    TValue Value { get; }
    static abstract TSelf From(TValue value);
}

public interface IId<TSelf> : IId<TSelf, string>;


public interface IValueObject<out TSelf> {
    static abstract TSelf New();
}
public interface IValueObject<out TSelf, in TValue> {
    static abstract TSelf New(TValue value);
}