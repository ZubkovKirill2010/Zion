namespace Zion.Serialization.TDC
{
    public sealed class TDCWritingOptions
    {
        public static readonly TDCWritingOptions Default = new();

        public bool WriteHeader { get; init; } = true;
        public long MinPageSize { get; init; } = 1024L;
    }
}