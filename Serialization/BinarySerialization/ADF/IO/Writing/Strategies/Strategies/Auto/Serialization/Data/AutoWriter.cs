namespace Zion.Serialization.ADF
{
    internal delegate void AutoWriter<in T>(ADFWritingContext Context, StreamGroup Target, T Value);
}