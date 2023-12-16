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
}