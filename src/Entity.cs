using System;
using System.Runtime.CompilerServices;

namespace LitEcs;

public readonly struct Entity
{
    public static readonly Entity None = new(Identity.None);
    public static readonly Entity Any = new(Identity.Any);

    public bool IsAny => Identity == Identity.Any;
    public bool IsNone => Identity == Identity.None;

    public Identity Identity { get; }

    public Entity(Identity identity)
    {
        Identity = identity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Entity entity && Identity.Equals(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Identity.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return Identity.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Entity left, Entity right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Entity left, Entity right) => !left.Equals(right);
}