using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LitEcs;

public struct StorageType : IComparable<StorageType>
{
    public Type Type { get; private set; }
    public ulong Value { get; private set; }
    public bool IsRelation { get; private set; }

    public ushort TypeId
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TypeIdConverter.Type(Value);
    }

    public Entity Entity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TypeIdConverter.Entity(Value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StorageType Create<T>(Entity entity = default)
    {
        return new StorageType()
        {
            Value = TypeIdConverter.Value<T>(entity),
            Type = typeof(T),
            IsRelation = entity.Id > 0,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(StorageType other)
    {
        return Value.CompareTo(other.Value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return (obj is StorageType other) && Value == other.Value;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(StorageType other)
    {
        return Value == other.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return IsRelation ? $"{GetHashCode()} {Type.Name}::{Entity}" : $"{GetHashCode()} {Type.Name}";
    }

    public static bool operator ==(StorageType left, StorageType right) => left.Equals(right);
    public static bool operator !=(StorageType left, StorageType right) => !left.Equals(right);
}
    
public static class TypeIdConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Value<T>(Entity entity)
    {
        return TypeIdAssigner<T>.Id | (ulong)entity.Gen << 16 | (ulong)entity.Id << 32;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Entity(ulong value)
    {
        return new((int)(value >> 32), (short)(value >> 16));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Type(ulong value)
    {
        return (ushort)value;
    }

class TypeIdAssigner
{
    protected static ushort Counter;
}

class TypeIdAssigner<T> : TypeIdAssigner
{
    public static readonly ushort Id;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TypeIdAssigner()
    {
        Id = ++Counter;
    }
}
}