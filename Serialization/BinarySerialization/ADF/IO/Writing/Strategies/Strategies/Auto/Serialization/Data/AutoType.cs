namespace Zion.Serialization.ADF
{
    public readonly record struct AutoType<T>(AutoWriter<T> Writer, TypeSchema<T> Schema);
}