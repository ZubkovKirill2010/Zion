namespace Zion
{
    public sealed class ADFWritingOptions
    {
        public static readonly ADFWritingOptions Default = new();

        public int  MinPageSize { get; init; } = 2048;
        public bool WriteHeader { get; init; } = true;
    }
}