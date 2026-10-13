namespace Zion.Serialization.ADF
{
    internal readonly record struct AutoType<T>(AutoWriter<T> Writer, TypeSchema Schema)
        : ILayerWriteInfo { }
}