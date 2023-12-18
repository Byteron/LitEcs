using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace LitEcs;

public sealed class TableEdge
{
    public Table? Add;
    public Table? Remove;
}

public sealed class Table
{
    const int StartCapacity = 4;

    public readonly int Id;

    public readonly ImmutableSortedSet<StorageType> Types;

    public int Count { get; internal set; }
    public bool IsEmpty => Count == 0;

    internal Entity[] Entities;
    internal readonly Array[] Storages;

    readonly Dictionary<StorageType, TableEdge> _edges = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Table(int id, ImmutableSortedSet<StorageType> types)
    {
        Id = id;
        Types = types;

        Entities = new Entity[StartCapacity];
        Storages = new Array[types.Count];

        var i = 0;
        foreach (var type in types)
        {
            Storages[i] = Array.CreateInstance(type.Type, StartCapacity);
            i++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Add(Entity entity)
    {
        EnsureCapacity(Count + 1);
        Entities[Count] = entity;
        return Count++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TableEdge GetTableEdge(StorageType type)
    {
        if (_edges.TryGetValue(type, out var edge)) return edge;

        edge = new TableEdge();
        _edges[type] = edge;

        return edge;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] GetStorage<T>(Entity target)
    {
        var type = StorageType.Create<T>(target);
        return (T[])GetStorage(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Array GetStorage(StorageType type)
    {
        return Storages[Types.IndexOf(type)];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void EnsureCapacity(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "minCapacity must be positive");
        if (capacity <= Entities.Length) return;

        Resize(Math.Max(capacity, StartCapacity) << 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Resize(int length)
    {
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "length cannot be negative");
        if (length < Count)
            throw new ArgumentOutOfRangeException(nameof(length), "length cannot be smaller than Count");

        Array.Resize(ref Entities, length);

        for (var i = 0; i < Storages.Length; i++)
        {
            var elementType = Storages[i].GetType().GetElementType()!;
            var newStorage = Array.CreateInstance(elementType, length);
            Array.Copy(Storages[i], newStorage, Math.Min(Storages[i].Length, length));
            Storages[i] = newStorage;
        }
    }

    public override string ToString()
    {
        var s = $"Table {Id} ";
        foreach (var type in Types)
        {
            s += $"{type} ";
        }
        return s;
    }
}