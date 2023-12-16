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
        var query = Query<Entity>(Has<T>());
        
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

    public static Query<Entity> Query()
    {
        return Query<Entity>();
    }

    internal static Mask GetMask(Filter[] filters)
    {
        var mask = MaskPool.Get();
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
    
    public static Query<C> Query<C>(params Filter[] filters) where C : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C>());
        return (Query<C>)World.GetQuery(mask, 
            (archetypes, mask, matchingTables) => new Query<C>(archetypes, mask, matchingTables));
    }

    public static Query<C1, C2> Query<C1, C2>(params Filter[] filters) where C1 : struct where C2 : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C1>());
        mask.Has(StorageType.Create<C2>());
        return (Query<C1, C2>)World.GetQuery(mask, 
            (archetypes, mask, matchingTables) => new Query<C1, C2>(archetypes, mask, matchingTables));
    }

    public static Query<C1, C2, C3> Query<C1, C2, C3>(params Filter[] filters) where C1 : struct where C2 : struct where C3 : struct
    {
        var mask = GetMask(filters);
        mask.Has(StorageType.Create<C1>());
        mask.Has(StorageType.Create<C2>());
        mask.Has(StorageType.Create<C3>());
        return (Query<C1, C2, C3>)World.GetQuery(mask, 
            (archetypes, mask, matchingTables) => new Query<C1, C2, C3>(archetypes, mask, matchingTables));
    }

    public static Query<C1, C2, C3, C4> Query<C1, C2, C3, C4>(params Filter[] filters) where C1 : struct
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
            (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4>(archetypes, mask, matchingTables));
    }

    public static Query<C1, C2, C3, C4, C5> Query<C1, C2, C3, C4, C5>(params Filter[] filters) where C1 : struct
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
            (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5>(archetypes, mask, matchingTables));
    }

    public static Query<C1, C2, C3, C4, C5, C6> Query<C1, C2, C3, C4, C5, C6>(params Filter[] filters) where C1 : struct
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
            (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5, C6>(archetypes, mask, matchingTables));
    }

    public static Query<C1, C2, C3, C4, C5, C6, C7> Query<C1, C2, C3, C4, C5, C6, C7>(params Filter[] filters) where C1 : struct
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
            (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5, C6, C7>(archetypes, mask, matchingTables));
    }

    public static Query<C1, C2, C3, C4, C5, C6, C7, C8> Query<C1, C2, C3, C4, C5, C6, C7, C8>(params Filter[] filters)
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
            (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5, C6, C7, C8>(archetypes, mask, matchingTables));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Entity GetTypeEntity(Type type)
    {
        return World.GetTypeEntity(type);
    }
    
    public static Filter Has<T>() => new(StorageType.Create<T>(), FilterType.Has);
    public static Filter Not<T>() => new(StorageType.Create<T>(), FilterType.Not);
    public static Filter Any<T>() => new(StorageType.Create<T>(), FilterType.Any);
}