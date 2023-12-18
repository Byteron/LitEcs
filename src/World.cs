using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace LitEcs;

public sealed class World
{
    internal EntityRecord[] Meta = new EntityRecord[512];
    internal readonly Queue<int> UnusedIds = new();
    internal readonly List<Table> Tables = new();
    internal readonly Dictionary<int, Query> Queries = new();

    internal int EntityCount;

    readonly List<TableOperation> _tableOperations = new();
    readonly Dictionary<Type, Entity> _typeEntities = new();
    internal readonly Dictionary<StorageType, List<Table>> TablesByType = new();
    readonly Dictionary<Entity, HashSet<StorageType>> _typesByRelationTarget = new();
    readonly Dictionary<int, HashSet<StorageType>> _relationsByTypes = new();

    int _lockCount;
    bool _isLocked;

    public World()
    {
        AddTable(ImmutableSortedSet.Create(StorageType.Create<Entity>(Entity.None)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Spawn()
    {
        var id = UnusedIds.Count > 0 ? UnusedIds.Dequeue() : ++EntityCount;
        if (Meta.Length == EntityCount) Array.Resize(ref Meta, EntityCount << 1);

        var table = Tables[0];
        
        ref var meta = ref Meta[id];
        meta.Gen = (short) (-meta.Gen + 1);

        var entity = new Entity(id, meta.Gen);
        var row = table.Add(entity);
        meta.Table = 0;
        meta.Row = row;
        
        var entityStorage = (Entity[])table.Storages[0];
        entityStorage[row] = entity;

        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Despawn(Entity entity)
    {
        if (!IsAlive(entity)) return;

        if (_isLocked)
        {
            _tableOperations.Add(new TableOperation { Despawn = true, Entity = entity });
            return;
        }

        ref var meta = ref Meta[entity.Id];

        var table = Tables[meta.Table];

        Remove(table, meta.Row);

        meta.Row = 0;
        meta.Gen = (short)-meta.Gen;

        UnusedIds.Enqueue(entity.Id);

        if (!_typesByRelationTarget.TryGetValue(entity, out var list))
        {
            return;
        }

        foreach (var type in list)
        {
            var tablesWithType = TablesByType[type];

            foreach (var tableWithType in tablesWithType)
            {
                for (var i = 0; i < tableWithType.Count; i++)
                {
                    RemoveComponent(type, tableWithType.Entities[i]);
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddComponent(StorageType type, Entity entity, object data)
    {
        ref var meta = ref Meta[entity.Id];
        var oldTable = Tables[meta.Table];

        if (oldTable.Types.Contains(type))
        {
            throw new Exception($"Entity {entity} already has component of type {type}");
        }

        if (_isLocked)
        {
            _tableOperations.Add(new TableOperation { Add = true, Entity = entity, Type = type, Data = data });
            return;
        }

        var oldEdge = oldTable.GetTableEdge(type);

        var newTable = oldEdge.Add;

        if (newTable == null)
        {
            var newTypes = oldTable.Types.Add(type);
            newTable = AddTable(newTypes);
            oldEdge.Add = newTable;

            var newEdge = newTable.GetTableEdge(type);
            newEdge.Remove = oldTable;
        }

        var newRow = MoveEntry(oldTable, newTable, entity, meta.Row);

        meta.Row = newRow;
        meta.Table = newTable.Id;

        var storage = newTable.GetStorage(type);
        storage.SetValue(data, newRow);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetComponent<T>(Entity entity, Entity target)
    {
        var type = StorageType.Create<T>(target);
        var meta = Meta[entity.Id];
        var table = Tables[meta.Table];
        var storage = (T[])table.GetStorage(type);
        return ref storage[meta.Row];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent(StorageType type, Entity entity)
    {
        var meta = Meta[entity.Id];
        return meta.Gen > 0 && Tables[meta.Table].Types.Contains(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveComponent(StorageType type, Entity entity)
    {
        ref var meta = ref Meta[entity.Id];
        var oldTable = Tables[meta.Table];

        if (!oldTable.Types.Contains(type))
        {
            throw new Exception($"cannot remove non-existent component {type.Type.Name} from entity {entity}");
        }

        if (_isLocked)
        {
            _tableOperations.Add(new TableOperation { Add = false, Entity = entity, Type = type });
            return;
        }

        var oldEdge = oldTable.GetTableEdge(type);

        var newTable = oldEdge.Remove;

        if (newTable == null)
        {
            var newTypes = oldTable.Types.Remove(type);
            newTable = AddTable(newTypes);
            oldEdge.Remove = newTable;

            var newEdge = newTable.GetTableEdge(type);
            newEdge.Add = oldTable;

            Tables.Add(newTable);
        }

        var newRow = MoveEntry(oldTable, newTable, entity, meta.Row);

        meta.Row = newRow;
        meta.Table = newTable.Id;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int MoveEntry(Table oldTable, Table newTable, Entity entity, int oldRow)
    {
        var newRow = newTable.Add(entity);

        var oldIndex = 0;
        foreach (var type in oldTable.Types)
        {
            var newIndex = newTable.Types.IndexOf(type);
            if (newIndex < 0) continue;

            var oldStorage = oldTable.Storages[oldIndex];
            var newStorage = newTable.Storages[newIndex];

            Array.Copy(oldStorage, oldRow, newStorage, newRow, 1);
            oldIndex++;
        }

        Remove(oldTable, oldRow);

        return newRow;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove(Table table, int row)
    {
        if (row >= table.Count)
            throw new ArgumentOutOfRangeException(nameof(row), "row cannot be greater or equal to count");

        table.Count--;

        if (row < table.Count)
        {
            table.Entities[row] = table.Entities[table.Count];

            foreach (var storage in table.Storages)
            {
                Array.Copy(storage, table.Count, storage, row, 1);
            }

            GetEntityMeta(table.Entities[row].Id).Row = row;
        }

        table.Entities[table.Count] = default;

        foreach (var storage in table.Storages)
        {
            Array.Clear(storage, table.Count, 1);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Query GetQuery(Mask mask, Func<World, Mask, List<Table>, Query> createQuery)
    {
        var hash = mask.GetHashCode();

        if (Queries.TryGetValue(hash, out var query))
        {
            MaskPool.Add(mask);
            return query;
        }

        var matchingTables = new List<Table>();

        var type = mask.HasTypes[0];
        if (!TablesByType.TryGetValue(type, out var typeTables))
        {
            typeTables = new List<Table>();
            TablesByType[type] = typeTables;
        }

        foreach (var table in typeTables)
        {
            if (!IsMaskCompatibleWith(mask, table)) continue;

            matchingTables.Add(table);
        }

        query = createQuery(this, mask, matchingTables);
        Queries.Add(hash, query);

        return query;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool IsMaskCompatibleWith(Mask mask, Table table)
    {
        var has = ListPool<StorageType>.Get();
        var not = ListPool<StorageType>.Get();
        var any = ListPool<StorageType>.Get();

        var hasAnyTarget = ListPool<StorageType>.Get();
        var notAnyTarget = ListPool<StorageType>.Get();
        var anyAnyTarget = ListPool<StorageType>.Get();

        foreach (var type in mask.HasTypes)
        {
            if (type.Entity == Entity.Any) hasAnyTarget.Add(type);
            else has.Add(type);
        }

        foreach (var type in mask.NotTypes)
        {
            if (type.Entity == Entity.Any) notAnyTarget.Add(type);
            else not.Add(type);
        }

        foreach (var type in mask.AnyTypes)
        {
            if (type.Entity == Entity.Any) anyAnyTarget.Add(type);
            else any.Add(type);
        }

        var matchesComponents = table.Types.IsSupersetOf(has);
        matchesComponents &= !table.Types.Overlaps(not);
        matchesComponents &= mask.AnyTypes.Count == 0 || table.Types.Overlaps(any);

        var matchesRelation = true;

        foreach (var type in hasAnyTarget)
        {
            if (!_relationsByTypes.TryGetValue(type.Id, out var list))
            {
                matchesRelation = false;
                continue;
            }

            matchesRelation &= table.Types.Overlaps(list);
        }

        ListPool<StorageType>.Add(has);
        ListPool<StorageType>.Add(not);
        ListPool<StorageType>.Add(any);
        ListPool<StorageType>.Add(hasAnyTarget);
        ListPool<StorageType>.Add(notAnyTarget);
        ListPool<StorageType>.Add(anyAnyTarget);

        return matchesComponents && matchesRelation;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool IsAlive(Entity entity)
    {
        return Meta[entity.Id].Gen == entity.Gen;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref EntityRecord GetEntityMeta(int entityId)
    {
        return ref Meta[entityId];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Table GetTable(int tableId)
    {
        return Tables[tableId];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Entity GetTarget(StorageType type, Entity entity)
    {
        var meta = Meta[entity.Id];
        var table = Tables[meta.Table];

        foreach (var storageType in table.Types)
        {
            if (!storageType.IsRelation || storageType.Id != type.Id) continue;
            return storageType.Entity;
        }

        return Entity.None;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Entity[] GetTargets(StorageType type, Entity entity)
    {
        var meta = Meta[entity.Id];
        var table = Tables[meta.Table];

        var list = ListPool<Entity>.Get();

        foreach (var storageType in table.Types)
        {
            if (!storageType.IsRelation || storageType.Id != type.Id) continue;
            list.Add(storageType.Entity);
        }

        var targetEntities = list.ToArray();
        ListPool<Entity>.Add(list);

        return targetEntities;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal (StorageType, object)[] GetComponents(Entity entity)
    {
        var meta = Meta[entity.Id];
        var table = Tables[meta.Table];

        var list = ListPool<(StorageType, object)>.Get();

        foreach (var type in table.Types)
        {
            var storage = table.GetStorage(type);
            list.Add((type, storage.GetValue(meta.Row)!));
        }

        var array = list.ToArray();
        ListPool<(StorageType, object)>.Add(list);
        return array;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Table AddTable(ImmutableSortedSet<StorageType> types)
    {
        var table = new Table(Tables.Count, types);
        Tables.Add(table);

        foreach (var type in types)
        {
            if (!TablesByType.TryGetValue(type, out var tableList))
            {
                tableList = new List<Table>();
                TablesByType[type] = tableList;
            }

            tableList.Add(table);

            if (!type.IsRelation) continue;

            if (!_typesByRelationTarget.TryGetValue(type.Entity, out var typeList))
            {
                typeList = new HashSet<StorageType>();
                _typesByRelationTarget[type.Entity] = typeList;
            }

            typeList.Add(type);

            if (!_relationsByTypes.TryGetValue(type.Id, out var relationTypeSet))
            {
                relationTypeSet = new HashSet<StorageType>();
                _relationsByTypes[type.Id] = relationTypeSet;
            }

            relationTypeSet.Add(type);
        }

        foreach (var query in Queries.Values.Where(query => IsMaskCompatibleWith(query.Mask, table)))
        {
            query.AddTable(table);
        }

        return table;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Entity GetTypeEntity(Type type)
    {
        if (!_typeEntities.TryGetValue(type, out var entity))
        {
            entity = Spawn();
            _typeEntities.Add(type, entity);
        }

        ;

        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ApplyTableOperations()
    {
        foreach (var op in _tableOperations)
        {
            if (!IsAlive(op.Entity)) continue;

            if (op.Despawn) Despawn(op.Entity);
            else if (op.Add) AddComponent(op.Type, op.Entity, op.Data);
            else RemoveComponent(op.Type, op.Entity);
        }

        _tableOperations.Clear();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Lock()
    {
        _lockCount++;
        _isLocked = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Unlock()
    {
        _lockCount--;
        if (_lockCount != 0) return;
        _isLocked = false;

        ApplyTableOperations();
    }

    struct TableOperation
    {
        public bool Despawn;
        public bool Add;
        public StorageType Type;
        public Entity Entity;
        public object Data;
    }
}