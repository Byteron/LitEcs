namespace LitEcs;

public readonly record struct Entity(int Id, short Gen)
{
    public static Entity Any = new(int.MaxValue, 0);
    public static Entity None = default;
}

public record struct EntityRecord(int Table, int Row, short Gen);