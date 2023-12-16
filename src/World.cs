using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LitEcs;
    
public sealed class World
{
    static int worldCount;

    readonly Archetypes _archetypes = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Spawn()
    {
        return new EntityBuilder(this, _archetypes.Spawn());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder On(Entity entity)
    {
        return new EntityBuilder(this, entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Despawn(Entity entity)
    {
        _archetypes.Despawn(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DespawnAllWith<T>() where T : struct
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
    public bool IsAlive(Entity entity)
    {
        return _archetypes.IsAlive(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetComponent<T>(Entity entity) where T : struct
    {
        return ref _archetypes.GetComponent<T>(entity.Identity, Identity.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        return _archetypes.HasComponent(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddComponent<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        _archetypes.AddComponent(type, entity.Identity, new T());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddComponent<T>(Entity entity, T component) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        _archetypes.AddComponent(type, entity.Identity, component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveComponent<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        _archetypes.RemoveComponent(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<(StorageType, object)> GetComponents(Entity entity)
    {
        return _archetypes.GetComponents(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Ref<T> GetComponent<T>(Entity entity, Entity target) where T : struct
    {
        return new Ref<T>(ref _archetypes.GetComponent<T>(entity.Identity, target.Identity));
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetComponent<T>(Entity entity, out Ref<T> component) where T : struct
    {
        if (!HasComponent<T>(entity))
        {
            component = default;
            return false;
        }

        component = new Ref<T>(ref _archetypes.GetComponent<T>(entity.Identity, Identity.None));
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Entity entity, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(target.Identity);
        return _archetypes.HasComponent(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddComponent<T>(Entity entity, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(target.Identity);
        _archetypes.AddComponent(type, entity.Identity, new T());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddComponent<T>(Entity entity, T component, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(target.Identity);
        _archetypes.AddComponent(type, entity.Identity, component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveComponent<T>(Entity entity, Entity target) where T : struct
    {
        var type = StorageType.Create<T>(target.Identity);
        _archetypes.RemoveComponent(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity GetTarget<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        return _archetypes.GetTarget(type, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<Entity> GetTargets<T>(Entity entity) where T : struct
    {
        var type = StorageType.Create<T>(Identity.None);
        return _archetypes.GetTargets(type, entity.Identity);
    }

    public QueryBuilder<Entity> Query()
    {
        return new QueryBuilder<Entity>(_archetypes);
    }

    public QueryBuilder<C> Query<C>() where C : struct
    {
        return new QueryBuilder<C>(_archetypes);
    }

    public QueryBuilder<C1, C2> Query<C1, C2>() where C1 : struct where C2 : struct
    {
        return new QueryBuilder<C1, C2>(_archetypes);
    }

    public QueryBuilder<C1, C2, C3> Query<C1, C2, C3>() where C1 : struct where C2 : struct where C3 : struct
    {
        return new QueryBuilder<C1, C2, C3>(_archetypes);
    }

    public QueryBuilder<C1, C2, C3, C4> Query<C1, C2, C3, C4>() where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4>(_archetypes);
    }

    public QueryBuilder<C1, C2, C3, C4, C5> Query<C1, C2, C3, C4, C5>() where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4, C5>(_archetypes);
    }

    public QueryBuilder<C1, C2, C3, C4, C5, C6> Query<C1, C2, C3, C4, C5, C6>() where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4, C5, C6>(_archetypes);
    }

    public QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Query<C1, C2, C3, C4, C5, C6, C7>() where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4, C5, C6, C7>(_archetypes);
    }

    public QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Query<C1, C2, C3, C4, C5, C6, C7, C8>()
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
        where C8 : struct
    {
        return new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>(_archetypes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Entity GetTypeEntity(Type type)
    {
        return _archetypes.GetTypeEntity(type);
    }
}