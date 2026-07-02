namespace Zion
{
    public interface IRandomizable<T> where T : IRandomizable<T>
    {
        static abstract T GetRandom(Random Random, T Min, T Max);
    }
}