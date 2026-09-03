namespace Zion.Serialization.ADF
{
    [Flags]
    public enum FormatFlags : ushort
    {
        None = 0,

        IsDynamic   = 1 << 0,
        IsNullable  = 1 << 1,
        IsAbstract  = 1 << 2,
        IsReference = 1 << 3
    }
}