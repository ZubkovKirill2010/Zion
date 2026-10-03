namespace Zion.Serialization.ADF
{
    public delegate void AutoWriter<in T>(ADFWritingContext Context, StreamGroup Target, T Value);
}