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