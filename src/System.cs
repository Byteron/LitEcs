using System.Runtime.CompilerServices;

namespace LitEcs;

public interface ISystem
{
    void Run(World world);
}

public sealed class SystemGroup
{
    readonly List<ISystem> systems = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SystemGroup Add(ISystem aSystem)
    {
        systems.Add(aSystem);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Run(World world)
    {
        foreach (var system in systems)
        {
            system.Run(world);
        }
    }
}