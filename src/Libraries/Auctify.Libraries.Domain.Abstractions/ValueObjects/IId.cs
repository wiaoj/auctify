namespace Auctify.Libraries.Domain.Abstractions.ValueObjects;
public interface IId;
public interface IId<out TSelf, TValue> : IId, IValueObject<TSelf> {
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
public interface IValueObject<out TSelf, in TValue1, in TValue2> {
    static abstract TSelf New(TValue1 value1, TValue2 value2);
}
public interface IValueObject<out TSelf, T1, T2, T3> {
    static abstract TSelf New(T1 value1, T2 value2, T3 value3);
}