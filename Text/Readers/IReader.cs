namespace Zion
{
    public interface IReader<T>
    {
        abstract bool TryRead(TextView View, out T Value, out int Length);
    }
}