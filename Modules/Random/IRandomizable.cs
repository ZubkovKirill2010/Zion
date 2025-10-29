namespace Zion
{
    public interface IRandomizable<T> where T : IRandomizable<T>
    {
        abstract static T GetRandom(Random Random, T Min, T Max);
    }
}