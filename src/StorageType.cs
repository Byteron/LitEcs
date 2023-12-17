using System.Runtime.CompilerServices;

namespace LitEcs;

public record struct StorageType(Type Type, ushort Id, Entity Entity, int Size, bool IsRelation) : IComparable<StorageType>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StorageType Create<T>(Entity entity = default)
    {
        return new StorageType
        {
            Type = typeof(T),
            Id = TypeIdAssigner<T>.Id,
            Entity = entity,
            Size = Unsafe.SizeOf<T>(),
            IsRelation = entity.Id > 0,
        };
    }
    
    public ulong Value
    {
     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     get => Id | (ulong)Entity.Gen << 16 | (ulong)Entity.Id << 32;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(StorageType other)
    {
        return Value.CompareTo(other.Value);
    }
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