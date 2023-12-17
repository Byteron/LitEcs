using System.Runtime.CompilerServices;

namespace LitEcs;

public class Query
{
    public readonly List<Table> Tables;

    internal readonly World World;
    internal readonly Mask Mask;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Query(World world, Mask mask, List<Table> tables)
    {
        Tables = tables;
        World = world;
        Mask = mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Id);
        var table = World.GetTable(meta.Table);
        return Tables.Contains(table);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void AddTable(Table table)
    {
        Tables.Add(table);
    }
}

public class Query<C> : Query
    where C : struct
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref C Get(Entity entity)
    {
        var meta = World.GetEntityMeta(entity.Id);
        var table = World.GetTable(meta.Table);
        var storage = table.GetStorage<C>(Entity.None);
        return ref storage[meta.Row];
    }

    public void Run(Action<int, C[]> action)
    {
        World.Lock();
        
        for (var t = 0; t < Tables.Count; t++)
        {
            var table = Tables[t];

            if (table.IsEmpty) continue;

            var s = table.GetStorage<C>(Entity.None);

            action(table.Count, s);
        }
        
        World.Unlock();
    }
    
    public void RunParallel(Action<int, C[]> action)
    {
        World.Lock();

        Parallel.For(0, Tables.Count, t =>
        {
            var table = Tables[t];
            
            if (table.IsEmpty) return;

            var s = table.GetStorage<C>(Entity.None);
            
            action(table.Count, s);
        });
        
        World.Unlock();
    }
    
    public Enumerator<C> GetEnumerator()
    {
        return new Enumerator<C>(World, Tables);
    }
}

public class Query<C1, C2> : Query
    where C1 : struct
    where C2 : struct
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    public void Run(Action<int, C1[], C2[]> action)
    {
        World.Lock();
        
        for (var t = 0; t < Tables.Count; t++)
        {
            var table = Tables[t];

            if (table.IsEmpty) continue;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);

            action(table.Count, s1, s2);
        }
        
        World.Unlock();
    }
    
    public void RunParallel(Action<int, C1[], C2[]> action)
    {
        World.Lock();

        Parallel.For(0, Tables.Count, t =>
        {
            var table = Tables[t];

            if (table.IsEmpty) return;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);

            action(table.Count, s1, s2);
        });
        
        World.Unlock();
    }
    
    public Enumerator<C1, C2> GetEnumerator()
    {
        return new Enumerator<C1, C2>(World, Tables);
    }
}

public class Query<C1, C2, C3> : Query
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    public void Run(Action<int, C1[], C2[], C3[]> action)
    {
        World.Lock();
        
        for (var t = 0; t < Tables.Count; t++)
        {
            var table = Tables[t];

            if (table.IsEmpty) continue;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);

            action(table.Count, s1, s2, s3);
        }
        
        World.Unlock();
    }
    
    public void RunParallel(Action<int, C1[], C2[], C3[]> action)
    {
        World.Lock();

        Parallel.For(0, Tables.Count, t =>
        {
            var table = Tables[t];
            
            if (table.IsEmpty) return;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            
            action(table.Count, s1, s2, s3);
        });
        
        World.Unlock();
    }
    
    public Enumerator<C1, C2, C3> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4> : Query
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    public void Run(Action<int, C1[], C2[], C3[], C4[]> action)
    {
        World.Lock();
        
        for (var t = 0; t < Tables.Count; t++)
        {
            var table = Tables[t];

            if (table.IsEmpty) continue;
            
            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);

            action(table.Count, s1, s2, s3, s4);
        }
        
        World.Unlock();
    }
    
    public void RunParallel(Action<int, C1[], C2[], C3[], C4[]> action)
    {
        World.Lock();

        Parallel.For(0, Tables.Count, t =>
        {
            var table = Tables[t];
            
            if (table.IsEmpty) return;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            
            action(table.Count, s1, s2, s3, s4);
        });
        
        World.Unlock();
    }
    
    public Enumerator<C1, C2, C3, C4> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5> : Query
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    public void Run(Action<int, C1[], C2[], C3[], C4[], C5[]> action)
    {
        World.Lock();
        
        for (var t = 0; t < Tables.Count; t++)
        {
            var table = Tables[t];

            if (table.IsEmpty) continue;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);

            action(table.Count, s1, s2, s3, s4, s5);
        }
        
        World.Unlock();
    }
    
    public void RunParallel(Action<int, C1[], C2[], C3[], C4[], C5[]> action)
    {
        World.Lock();

        Parallel.For(0, Tables.Count, t =>
        {
            var table = Tables[t];
            
            if (table.IsEmpty) return;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);
            
            action(table.Count, s1, s2, s3, s4, s5);
        });
        
        World.Unlock();
    }
    
    public Enumerator<C1, C2, C3, C4, C5> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5, C6> : Query
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    public void Run(Action<int, C1[], C2[], C3[], C4[], C5[], C6[]> action)
    {
        World.Lock();
        
        for (var t = 0; t < Tables.Count; t++)
        {
            var table = Tables[t];

            if (table.IsEmpty) continue;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);
            var s6 = table.GetStorage<C6>(Entity.None);

            action(table.Count, s1, s2, s3, s4, s5, s6);
        }
        
        World.Unlock();
    }
    
    public void RunParallel(Action<int, C1[], C2[], C3[], C4[], C5[], C6[]> action)
    {
        World.Lock();

        Parallel.For(0, Tables.Count, t =>
        {
            var table = Tables[t];
            
            if (table.IsEmpty) return;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);
            var s6 = table.GetStorage<C6>(Entity.None);
            
            action(table.Count, s1, s2, s3, s4, s5, s6);
        });
        
        World.Unlock();
    }
    
    public Enumerator<C1, C2, C3, C4, C5, C6> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5, C6, C7> : Query
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    public void Run(Action<int, C1[], C2[], C3[], C4[], C5[], C6[], C7[]> action)
    {
        World.Lock();
        
        for (var t = 0; t < Tables.Count; t++)
        {
            var table = Tables[t];

            if (table.IsEmpty) continue;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);
            var s6 = table.GetStorage<C6>(Entity.None);
            var s7 = table.GetStorage<C7>(Entity.None);

            action(table.Count, s1, s2, s3, s4, s5, s6, s7);
        }
        
        World.Unlock();
    }
    
    public void RunParallel(Action<int, C1[], C2[], C3[], C4[], C5[], C6[], C7[]> action)
    {
        World.Lock();

        Parallel.For(0, Tables.Count, t =>
        {
            var table = Tables[t];
            
            if (table.IsEmpty) return;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);
            var s6 = table.GetStorage<C6>(Entity.None);
            var s7 = table.GetStorage<C7>(Entity.None);
            
            action(table.Count, s1, s2, s3, s4, s5, s6, s7);
        });
        
        World.Unlock();
    }
    
    public Enumerator<C1, C2, C3, C4, C5, C6, C7> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5, C6, C7, C8> : Query
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    public void Run(Action<int, C1[], C2[], C3[], C4[], C5[], C6[], C7[], C8[]> action)
    {
        World.Lock();
        
        for (var t = 0; t < Tables.Count; t++)
        {
            var table = Tables[t];

            if (table.IsEmpty) continue;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);
            var s6 = table.GetStorage<C6>(Entity.None);
            var s7 = table.GetStorage<C7>(Entity.None);
            var s8 = table.GetStorage<C8>(Entity.None);

            action(table.Count, s1, s2, s3, s4, s5, s6, s7, s8);
        }
        
        World.Unlock();
    }
    
    public void RunParallel(Action<int, C1[], C2[], C3[], C4[], C5[], C6[], C7[], C8[]> action)
    {
        World.Lock();

        Parallel.For(0, Tables.Count, t =>
        {
            var table = Tables[t];
            
            if (table.IsEmpty) return;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);
            var s6 = table.GetStorage<C6>(Entity.None);
            var s7 = table.GetStorage<C7>(Entity.None);
            var s8 = table.GetStorage<C8>(Entity.None);
            
            action(table.Count, s1, s2, s3, s4, s5, s6, s7, s8);
        });
        
        World.Unlock();
    }
    
    public Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8>(World, Tables);
    }
}

public class Query<C1, C2, C3, C4, C5, C6, C7, C8, C9> : Query
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
    where C9 : struct
{
    public Query(World world, Mask mask, List<Table> tables) : base(world, mask, tables)
    {
    }

    public void Run(Action<int, C1[], C2[], C3[], C4[], C5[], C6[], C7[], C8[], C9[]> action)
    {
        World.Lock();
        
        for (var t = 0; t < Tables.Count; t++)
        {
            var table = Tables[t];
            
            if (table.IsEmpty) continue;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);
            var s6 = table.GetStorage<C6>(Entity.None);
            var s7 = table.GetStorage<C7>(Entity.None);
            var s8 = table.GetStorage<C8>(Entity.None);
            var s9 = table.GetStorage<C9>(Entity.None);
            
            action(table.Count, s1, s2, s3, s4, s5, s6, s7, s8, s9);
        }
        
        World.Unlock();
    }
    
    public void RunParallel(Action<int, C1[], C2[], C3[], C4[], C5[], C6[], C7[], C8[], C9[]> action)
    {
        World.Lock();

        Parallel.For(0, Tables.Count, t =>
        {
            var table = Tables[t];
            
            if (table.IsEmpty) return;

            var s1 = table.GetStorage<C1>(Entity.None);
            var s2 = table.GetStorage<C2>(Entity.None);
            var s3 = table.GetStorage<C3>(Entity.None);
            var s4 = table.GetStorage<C4>(Entity.None);
            var s5 = table.GetStorage<C5>(Entity.None);
            var s6 = table.GetStorage<C6>(Entity.None);
            var s7 = table.GetStorage<C7>(Entity.None);
            var s8 = table.GetStorage<C8>(Entity.None);
            var s9 = table.GetStorage<C9>(Entity.None);
            
            action(table.Count, s1, s2, s3, s4, s5, s6, s7, s8, s9);
        });
        
        World.Unlock();
    }
    
    public Enumerator<C1, C2, C3, C4, C5, C6, C7, C8, C9> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8, C9>(World, Tables);
    }
}


public struct Enumerator<C> : IDisposable
{
    readonly World world;
    private readonly List<Table> tables;

    private int tableIndex;
    private int entityIndex;

    private C[] storage;
    
    public Enumerator(World world, List<Table> tables)
    {
        this.world = world;
        this.tables = tables;
        
        tableIndex = 0;
        entityIndex = 0;
        
        storage = tableIndex == tables.Count ? null! : tables[tableIndex].GetStorage<C>(Entity.None);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (tableIndex == tables.Count) return false;

        if (++entityIndex < tables[tableIndex].Count) return true;

        entityIndex = 0;
        tableIndex++;

        while (tableIndex < tables.Count && tables[tableIndex].IsEmpty)
        {
            tableIndex++;
        }

        if (tableIndex < tables.Count) return false;
        
        storage = tables[tableIndex].GetStorage<C>(Entity.None);

        return entityIndex < tables[tableIndex].Count;
    }

    public Ref<C> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(ref storage[entityIndex]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        world.Unlock();
    }
}

public struct Enumerator<C1, C2> : IDisposable
{
    readonly World world;
    private readonly List<Table> tables;

    private int tableIndex;
    private int entityIndex;

    C1[] storage1;
    C2[] storage2;
    
    public Enumerator(World world, List<Table> tables)
    {
        this.world = world;
        this.tables = tables;
        
        tableIndex = 0;
        entityIndex = 0;

        UpdateStorage();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (tableIndex == tables.Count) return false;

        if (++entityIndex < tables[tableIndex].Count) return true;

        entityIndex = 0;
        tableIndex++;

        while (tableIndex < tables.Count && tables[tableIndex].IsEmpty)
        {
            tableIndex++;
        }

        if (tableIndex < tables.Count) return false;

        UpdateStorage();

        return tableIndex < tables.Count && entityIndex < tables[tableIndex].Count;
    }

    public RefValueTuple<C1, C2> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(ref storage1[entityIndex], ref storage2[entityIndex]);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateStorage()
    {
        if (tableIndex == tables.Count) return;
        storage1 = tables[tableIndex].GetStorage<C1>(Entity.None);
        storage2 = tables[tableIndex].GetStorage<C2>(Entity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        world.Unlock();
    }
}

public struct Enumerator<C1, C2, C3> : IDisposable
{
    readonly World world;
    private readonly List<Table> tables;

    private int tableIndex;
    private int entityIndex;

    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    
    public Enumerator(World world, List<Table> tables)
    {
        this.world = world;
        this.tables = tables;
        
        tableIndex = 0;
        entityIndex = 0;

        UpdateStorage();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (tableIndex == tables.Count) return false;

        if (++entityIndex < tables[tableIndex].Count) return true;

        entityIndex = 0;
        tableIndex++;

        while (tableIndex < tables.Count && tables[tableIndex].IsEmpty)
        {
            tableIndex++;
        }

        if (tableIndex < tables.Count) return false;

        UpdateStorage();

        return tableIndex < tables.Count && entityIndex < tables[tableIndex].Count;
    }

    public RefValueTuple<C1, C2, C3> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(ref storage1[entityIndex], ref storage2[entityIndex], ref storage3[entityIndex]);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateStorage()
    {
        if (tableIndex == tables.Count) return;
        storage1 = tables[tableIndex].GetStorage<C1>(Entity.None);
        storage2 = tables[tableIndex].GetStorage<C2>(Entity.None);
        storage3 = tables[tableIndex].GetStorage<C3>(Entity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        world.Unlock();
    }
}

public struct Enumerator<C1, C2, C3, C4> : IDisposable
{
    readonly World world;
    private readonly List<Table> tables;

    private int tableIndex;
    private int entityIndex;

    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    
    public Enumerator(World world, List<Table> tables)
    {
        this.world = world;
        this.tables = tables;
        
        tableIndex = 0;
        entityIndex = 0;

        UpdateStorage();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (tableIndex == tables.Count) return false;

        if (++entityIndex < tables[tableIndex].Count) return true;

        entityIndex = 0;
        tableIndex++;

        while (tableIndex < tables.Count && tables[tableIndex].IsEmpty)
        {
            tableIndex++;
        }

        if (tableIndex < tables.Count) return false;

        UpdateStorage();

        return tableIndex < tables.Count && entityIndex < tables[tableIndex].Count;
    }

    public RefValueTuple<C1, C2, C3, C4> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(ref storage1[entityIndex], ref storage2[entityIndex], ref storage3[entityIndex],
            ref storage4[entityIndex]);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateStorage()
    {
        if (tableIndex == tables.Count) return;
        storage1 = tables[tableIndex].GetStorage<C1>(Entity.None);
        storage2 = tables[tableIndex].GetStorage<C2>(Entity.None);
        storage3 = tables[tableIndex].GetStorage<C3>(Entity.None);
        storage4 = tables[tableIndex].GetStorage<C4>(Entity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        world.Unlock();
    }
}

public struct Enumerator<C1, C2, C3, C4, C5> : IDisposable
{
    readonly World world;
    private readonly List<Table> tables;

    private int tableIndex;
    private int entityIndex;

    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    
    public Enumerator(World world, List<Table> tables)
    {
        this.world = world;
        this.tables = tables;
        
        tableIndex = 0;
        entityIndex = 0;

        UpdateStorage();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (tableIndex == tables.Count) return false;

        if (++entityIndex < tables[tableIndex].Count) return true;

        entityIndex = 0;
        tableIndex++;

        while (tableIndex < tables.Count && tables[tableIndex].IsEmpty)
        {
            tableIndex++;
        }

        if (tableIndex < tables.Count) return false;

        UpdateStorage();

        return tableIndex < tables.Count && entityIndex < tables[tableIndex].Count;
    }

    public RefValueTuple<C1, C2, C3, C4, C5> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(ref storage1[entityIndex], ref storage2[entityIndex], ref storage3[entityIndex],
            ref storage4[entityIndex], ref storage5[entityIndex]);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateStorage()
    {
        if (tableIndex == tables.Count) return;
        storage1 = tables[tableIndex].GetStorage<C1>(Entity.None);
        storage2 = tables[tableIndex].GetStorage<C2>(Entity.None);
        storage3 = tables[tableIndex].GetStorage<C3>(Entity.None);
        storage4 = tables[tableIndex].GetStorage<C4>(Entity.None);
        storage5 = tables[tableIndex].GetStorage<C5>(Entity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        world.Unlock();
    }
}

public struct Enumerator<C1, C2, C3, C4, C5, C6> : IDisposable
{
    readonly World world;
    private readonly List<Table> tables;

    private int tableIndex;
    private int entityIndex;

    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    C6[] storage6;
    
    public Enumerator(World world, List<Table> tables)
    {
        this.world = world;
        this.tables = tables;
        
        tableIndex = 0;
        entityIndex = 0;

        UpdateStorage();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (tableIndex == tables.Count) return false;

        if (++entityIndex < tables[tableIndex].Count) return true;

        entityIndex = 0;
        tableIndex++;

        while (tableIndex < tables.Count && tables[tableIndex].IsEmpty)
        {
            tableIndex++;
        }

        if (tableIndex < tables.Count) return false;

        UpdateStorage();

        return tableIndex < tables.Count && entityIndex < tables[tableIndex].Count;
    }

    public RefValueTuple<C1, C2, C3, C4, C5, C6> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(ref storage1[entityIndex], ref storage2[entityIndex], ref storage3[entityIndex],
            ref storage4[entityIndex], ref storage5[entityIndex], ref storage6[entityIndex]);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateStorage()
    {
        if (tableIndex == tables.Count) return;
        storage1 = tables[tableIndex].GetStorage<C1>(Entity.None);
        storage2 = tables[tableIndex].GetStorage<C2>(Entity.None);
        storage3 = tables[tableIndex].GetStorage<C3>(Entity.None);
        storage4 = tables[tableIndex].GetStorage<C4>(Entity.None);
        storage5 = tables[tableIndex].GetStorage<C5>(Entity.None);
        storage6 = tables[tableIndex].GetStorage<C6>(Entity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        world.Unlock();
    }
}

public struct Enumerator<C1, C2, C3, C4, C5, C6, C7> : IDisposable
{
    readonly World world;
    private readonly List<Table> tables;

    private int tableIndex;
    private int entityIndex;

    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    C6[] storage6;
    C7[] storage7;
    
    public Enumerator(World world, List<Table> tables)
    {
        this.world = world;
        this.tables = tables;
        
        tableIndex = 0;
        entityIndex = 0;

        UpdateStorage();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (tableIndex == tables.Count) return false;

        if (++entityIndex < tables[tableIndex].Count) return true;

        entityIndex = 0;
        tableIndex++;

        while (tableIndex < tables.Count && tables[tableIndex].IsEmpty)
        {
            tableIndex++;
        }

        if (tableIndex < tables.Count) return false;

        UpdateStorage();

        return tableIndex < tables.Count && entityIndex < tables[tableIndex].Count;
    }

    public RefValueTuple<C1, C2, C3, C4, C5, C6, C7> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(ref storage1[entityIndex], ref storage2[entityIndex], ref storage3[entityIndex],
            ref storage4[entityIndex], ref storage5[entityIndex], ref storage6[entityIndex],
            ref storage7[entityIndex]);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateStorage()
    {
        if (tableIndex == tables.Count) return;
        storage1 = tables[tableIndex].GetStorage<C1>(Entity.None);
        storage2 = tables[tableIndex].GetStorage<C2>(Entity.None);
        storage3 = tables[tableIndex].GetStorage<C3>(Entity.None);
        storage4 = tables[tableIndex].GetStorage<C4>(Entity.None);
        storage5 = tables[tableIndex].GetStorage<C5>(Entity.None);
        storage6 = tables[tableIndex].GetStorage<C6>(Entity.None);
        storage7 = tables[tableIndex].GetStorage<C7>(Entity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        world.Unlock();
    }
}

public struct Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> : IDisposable
{
    readonly World world;
    private readonly List<Table> tables;

    private int tableIndex;
    private int entityIndex;

    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    C6[] storage6;
    C7[] storage7;
    C8[] storage8;
    
    public Enumerator(World world, List<Table> tables)
    {
        this.world = world;
        this.tables = tables;
        
        tableIndex = 0;
        entityIndex = 0;

        UpdateStorage();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (tableIndex == tables.Count) return false;

        if (++entityIndex < tables[tableIndex].Count) return true;

        entityIndex = 0;
        tableIndex++;

        while (tableIndex < tables.Count && tables[tableIndex].IsEmpty)
        {
            tableIndex++;
        }

        if (tableIndex < tables.Count) return false;

        UpdateStorage();

        return tableIndex < tables.Count && entityIndex < tables[tableIndex].Count;
    }

    public RefValueTuple<C1, C2, C3, C4, C5, C6, C7, C8> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(ref storage1[entityIndex], ref storage2[entityIndex], ref storage3[entityIndex],
            ref storage4[entityIndex], ref storage5[entityIndex], ref storage6[entityIndex],
            ref storage7[entityIndex], ref storage8[entityIndex]);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateStorage()
    {
        if (tableIndex == tables.Count) return;
        storage1 = tables[tableIndex].GetStorage<C1>(Entity.None);
        storage2 = tables[tableIndex].GetStorage<C2>(Entity.None);
        storage3 = tables[tableIndex].GetStorage<C3>(Entity.None);
        storage4 = tables[tableIndex].GetStorage<C4>(Entity.None);
        storage5 = tables[tableIndex].GetStorage<C5>(Entity.None);
        storage6 = tables[tableIndex].GetStorage<C6>(Entity.None);
        storage7 = tables[tableIndex].GetStorage<C7>(Entity.None);
        storage8 = tables[tableIndex].GetStorage<C8>(Entity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        world.Unlock();
    }
}

public struct Enumerator<C1, C2, C3, C4, C5, C6, C7, C8, C9> : IDisposable
{
    readonly World world;
    private readonly List<Table> tables;

    private int tableIndex;
    private int entityIndex;

    C1[] storage1;
    C2[] storage2;
    C3[] storage3;
    C4[] storage4;
    C5[] storage5;
    C6[] storage6;
    C7[] storage7;
    C8[] storage8;
    C9[] storage9;
    
    public Enumerator(World world, List<Table> tables)
    {
        this.world = world;
        this.tables = tables;
        
        tableIndex = 0;
        entityIndex = 0;

        UpdateStorage();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (tableIndex == tables.Count) return false;

        if (++entityIndex < tables[tableIndex].Count) return true;

        entityIndex = 0;
        tableIndex++;

        while (tableIndex < tables.Count && tables[tableIndex].IsEmpty)
        {
            tableIndex++;
        }

        if (tableIndex < tables.Count) return false;

        UpdateStorage();

        return tableIndex < tables.Count && entityIndex < tables[tableIndex].Count;
    }

    public RefValueTuple<C1, C2, C3, C4, C5, C6, C7, C8, C9> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(ref storage1[entityIndex], ref storage2[entityIndex], ref storage3[entityIndex],
            ref storage4[entityIndex], ref storage5[entityIndex], ref storage6[entityIndex],
            ref storage7[entityIndex], ref storage8[entityIndex], ref storage9[entityIndex]);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateStorage()
    {
        if (tableIndex == tables.Count) return;
        storage1 = tables[tableIndex].GetStorage<C1>(Entity.None);
        storage2 = tables[tableIndex].GetStorage<C2>(Entity.None);
        storage3 = tables[tableIndex].GetStorage<C3>(Entity.None);
        storage4 = tables[tableIndex].GetStorage<C4>(Entity.None);
        storage5 = tables[tableIndex].GetStorage<C5>(Entity.None);
        storage6 = tables[tableIndex].GetStorage<C6>(Entity.None);
        storage7 = tables[tableIndex].GetStorage<C7>(Entity.None);
        storage8 = tables[tableIndex].GetStorage<C8>(Entity.None);
        storage9 = tables[tableIndex].GetStorage<C9>(Entity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        world.Unlock();
    }
}