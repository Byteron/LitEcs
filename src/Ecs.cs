using System;
using System.Collections.Immutable;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace LitEcs;

public enum FilterType { Has, Not, Any }

public readonly record struct Filter(StorageType StorageType, FilterType FilterType);

public static class Ecs
{
    public static World World = new();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EntityBuilder Spawn()
    {
        return new EntityBuilder(World, World.Spawn());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EntityBuilder On(Entity entity)
    {
        return new EntityBuilder(World, entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Despawn(Entity entity)
    {
        World.Despawn(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DespawnAllWith<T>() where T : struct
    {
        var query = Query<Entity>().Has<T>().Build();
        
        query.Run((count, entities) =>
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
        return World.IsAlive(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetComponent<T>(Entity entity) where T : struct
    {
        return ref World.GetComponent<T>(entity.Identity, Identity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasComponent<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        return World.HasComponent(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddComponent<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        World.AddComponent(type, entity.Identity, new T());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddComponent<T>(Entity entity, T component) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        World.AddComponent(type, entity.Identity, component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveComponent<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        World.RemoveComponent(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<(StorageType, object)> GetComponents(Entity entity)
    {
        return World.GetComponents(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Ref<T> GetComponent<T>(Entity entity, Entity target) where T : struct
    {
        return new Ref<T>(ref World.GetComponent<T>(entity.Identity, target.Identity));
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetComponent<T>(Entity entity, out Ref<T> component) where T : struct
    {
        if (!HasComponent<T>(entity))
        {
            component = default;
            return false;
        }

        component = new Ref<T>(ref World.GetComponent<T>(entity.Identity, Identity.None));
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasComponent<T>(Entity entity, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(target.Identity);
        return World.HasComponent(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddComponent<T>(Entity entity, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(target.Identity);
        World.AddComponent(type, entity.Identity, new T());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddComponent<T>(Entity entity, T component, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(target.Identity);
        World.AddComponent(type, entity.Identity, component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveComponent<T>(Entity entity, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(target.Identity);
        World.RemoveComponent(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity GetTarget<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        return World.GetTarget(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<Entity> GetTargets<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        return World.GetTargets(type, entity.Identity);
    }

    public static QueryBuilder<Entity> Query()
    {
        return new QueryBuilder<Entity>(World);
    }

    public static QueryBuilder<C> Query<C>() where C : struct
    {
        return new QueryBuilder<C>(World);
    }

    public static QueryBuilder<C1, C2> Query<C1, C2>() where C1 : struct where C2 : struct
    {
        return new QueryBuilder<C1, C2>(World);
    }

    public static QueryBuilder<C1, C2, C3> Query<C1, C2, C3>() where C1 : struct where C2 : struct where C3 : struct
    {
        return new QueryBuilder<C1, C2, C3>(World);
    }

    public static QueryBuilder<C1, C2, C3, C4> Query<C1, C2, C3, C4>() where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4>(World);
    }

    public static QueryBuilder<C1, C2, C3, C4, C5> Query<C1, C2, C3, C4, C5>() where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4, C5>(World);
    }

    public static QueryBuilder<C1, C2, C3, C4, C5, C6> Query<C1, C2, C3, C4, C5, C6>() where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4, C5, C6>(World);
    }

    public static QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Query<C1, C2, C3, C4, C5, C6, C7>() where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4, C5, C6, C7>(World);
    }

    public static QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Query<C1, C2, C3, C4, C5, C6, C7, C8>()
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
        where C8 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>(World);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Entity GetTypeEntity(Type type)
    {
        return World.GetTypeEntity(type);
    }

    // public static Query Query(params Filter[] filters)
    // {
    //     foreach (var filter in filters)
    //     {
    //         switch (filter.FilterType)
    //         {
    //             case FilterType.Has: Console.WriteLine("Has: " + filter.StorageType.Type);
    //                 break;
    //             case FilterType.Not: Console.WriteLine("Not: " + filter.StorageType.Type);
    //                 break;
    //             case FilterType.Any: Console.WriteLine("Any: " + filter.StorageType.Type);
    //                 break;
    //         }
    //     }
    //     return new();
    // }
    //
    // public static void Example()
    // {
    //     var query = Query<Position, Velocity>(Has<Tag>(), Not<Enemy>());
    //     foreach (var (pos, vel) in query)
    //     {
    //         
    //     }
    // }
    //
    // public static Filter Has<T>() => new(StorageType.Create<T>(), FilterType.Has);
    // public static Filter Not<T>() => new(StorageType.Create<T>(), FilterType.Not);
    // public static Filter Any<T>() => new(StorageType.Create<T>(), FilterType.Any);

}