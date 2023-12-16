﻿using System.Runtime.CompilerServices;

namespace LitEcs;

public enum FilterType { Has, Not, Any }

public readonly record struct Filter(StorageType StorageType, FilterType FilterType);
public readonly record struct Set(StorageType StorageType, dynamic Data);

public static class Ecs
{
    public static World World = new();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Filter Has<T>() => new(StorageType.Create<T>(), FilterType.Has);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Filter Not<T>() => new(StorageType.Create<T>(), FilterType.Not);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Filter Any<T>() => new(StorageType.Create<T>(), FilterType.Any);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Set Set<T>(T data = default) where T: struct => new(StorageType.Create<T>(), data);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Spawn(params Set[] types)
    {
        var entity = World.Spawn();
        foreach (var set in types)
        {
            World.AddComponent(set.StorageType, entity, set.Data);
        }
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Despawn(Entity entity)
    {
        World.Despawn(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DespawnAllWith<T>() where T : struct
    {
        var query = Query<Entity, T>();
        
        query.Run((count, entities, ts) =>
        {
            for (var i = 0; i < count; i++)
            {
                Despawn(entities[i]);
            }
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlive(Entity entity)
    {
        return World.IsAlive(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetComponent<T>(Entity entity) where T : struct
    {
        return ref World.GetComponent<T>(entity, Entity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasComponent<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Entity.None);
        return World.HasComponent(type, entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddComponent<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Entity.None);
        World.AddComponent(type, entity, new T());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddComponent<T>(Entity entity, T component) where T : struct
    {
        var type = StorageType.Create<T>(Entity.None);
        World.AddComponent(type, entity, component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveComponent<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Entity.None);
        World.RemoveComponent(type, entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<(StorageType, object)> GetComponents(Entity entity)
    {
        return World.GetComponents(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetComponent<T>(Entity entity, Entity target) where T : struct
    {
        return ref World.GetComponent<T>(entity, target);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasComponent<T>(Entity entity, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(target);
        return World.HasComponent(type, entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddComponent<T>(Entity entity, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(entity);
        World.AddComponent(type, entity, new T());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddComponent<T>(Entity entity, T component, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(entity);
        World.AddComponent(type, entity, component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveComponent<T>(Entity entity, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(entity);
        World.RemoveComponent(type, entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity GetTarget<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(entity);
        return World.GetTarget(type, entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<Entity> GetTargets<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Entity.None);
        return World.GetTargets(type, entity);
    }

    public static Query<Entity> Query()
    {
        return Query<Entity>();
    }
    
    public static Query<C> Query<C>(Filter[]? filters = null) where C : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C>());
        return (Query<C>)World.GetQuery(mask, 
            (w, m, t) => new Query<C>(w, m, t));
    }
    
    public static Query<C1, C2> Query<C1, C2>(Filter[]? filters = null) where C1 : struct where C2 : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C1>());
        mask.Has(StorageType.Create<C2>());
        return (Query<C1, C2>)World.GetQuery(mask, 
            (w, m, t) => new Query<C1, C2>(w, m, t));
    }

    public static Query<C1, C2, C3> Query<C1, C2, C3>(Filter[]? filters = null) where C1 : struct where C2 : struct where C3 : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C1>());
        mask.Has(StorageType.Create<C2>());
        mask.Has(StorageType.Create<C3>());
        return (Query<C1, C2, C3>)World.GetQuery(mask, 
            (w, m, t) => new Query<C1, C2, C3>(w, m, t));
    }

    public static Query<C1, C2, C3, C4> Query<C1, C2, C3, C4>(Filter[]? filters = null) where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C1>());
        mask.Has(StorageType.Create<C2>());
        mask.Has(StorageType.Create<C3>());
        mask.Has(StorageType.Create<C4>());
        return (Query<C1, C2, C3, C4>)World.GetQuery(mask, 
            (w, m, t) => new Query<C1, C2, C3, C4>(w, m, t));
    }

    public static Query<C1, C2, C3, C4, C5> Query<C1, C2, C3, C4, C5>(Filter[]? filters = null) where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C1>());
        mask.Has(StorageType.Create<C2>());
        mask.Has(StorageType.Create<C3>());
        mask.Has(StorageType.Create<C4>());
        mask.Has(StorageType.Create<C5>());
        return (Query<C1, C2, C3, C4, C5>)World.GetQuery(mask, 
            (w, m, t) => new Query<C1, C2, C3, C4, C5>(w, m, t));
    }

    public static Query<C1, C2, C3, C4, C5, C6> Query<C1, C2, C3, C4, C5, C6>(Filter[]? filters = null) where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C1>());
        mask.Has(StorageType.Create<C2>());
        mask.Has(StorageType.Create<C3>());
        mask.Has(StorageType.Create<C4>());
        mask.Has(StorageType.Create<C5>());
        mask.Has(StorageType.Create<C6>());
        return (Query<C1, C2, C3, C4, C5, C6>)World.GetQuery(mask, 
            (w, m, t) => new Query<C1, C2, C3, C4, C5, C6>(w, m, t));
    }

    public static Query<C1, C2, C3, C4, C5, C6, C7> Query<C1, C2, C3, C4, C5, C6, C7>(Filter[]? filters = null) where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C1>());
        mask.Has(StorageType.Create<C2>());
        mask.Has(StorageType.Create<C3>());
        mask.Has(StorageType.Create<C4>());
        mask.Has(StorageType.Create<C5>());
        mask.Has(StorageType.Create<C6>());
        mask.Has(StorageType.Create<C7>());
        return (Query<C1, C2, C3, C4, C5, C6, C7>)World.GetQuery(mask, 
            (w, m, t) => new Query<C1, C2, C3, C4, C5, C6, C7>(w, m, t));
    }

    public static Query<C1, C2, C3, C4, C5, C6, C7, C8> Query<C1, C2, C3, C4, C5, C6, C7, C8>(Filter[]? filters = null)
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
        where C8 : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C1>());
        mask.Has(StorageType.Create<C2>());
        mask.Has(StorageType.Create<C3>());
        mask.Has(StorageType.Create<C4>());
        mask.Has(StorageType.Create<C5>());
        mask.Has(StorageType.Create<C6>());
        mask.Has(StorageType.Create<C7>());
        mask.Has(StorageType.Create<C8>());
        return (Query<C1, C2, C3, C4, C5, C6, C7, C8>)World.GetQuery(mask, 
            (w, m, t) => new Query<C1, C2, C3, C4, C5, C6, C7, C8>(w, m, t));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Entity GetTypeEntity(Type type)
    {
        return World.GetTypeEntity(type);
    }
    
    private static Mask GetMask(Filter[]? filters)
    {
        var mask = MaskPool.Get();
        
        if (filters is null) return mask;
        
        foreach (var filter in filters)
        {
            switch (filter.FilterType)
            {
                case FilterType.Has: mask.HasTypes.Add(filter.StorageType);
                    break;
                case FilterType.Not: mask.NotTypes.Add(filter.StorageType);
                    break;
                case FilterType.Any: mask.AnyTypes.Add(filter.StorageType);
                    break;
            }
        }

        return mask;
    }
}