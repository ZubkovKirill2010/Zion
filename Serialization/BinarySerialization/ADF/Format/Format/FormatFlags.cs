namespace Zion.Serialization.ADF
{
    [Flags]
    public enum FormatFlags : ushort
    {
        None = 0,

        IsArray     = 1 << 0,
        IsReference = 1 << 1,
        IsAbstract  = (1 << 1) | (1 << 2),
        IsNullable  = 1 << 3,
        IsEnum      = 1 << 4
    }
}