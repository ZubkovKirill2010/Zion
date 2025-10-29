namespace Zion
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random Random, float Min, float Max)
        {
            return Random.Next((int)Min, (int)Max) + Random.NextSingle();
        }
        public static double NextDouble(this Random Random, double Min, double Max)
        {
            return Random.Next((int)Min, (int)Max) + Random.NextDouble();
        }


        public static T NextOf<T>(this Random Random, T Min, T Max) where T : IRandomizable<T>
        {
            return T.GetRandom(Random, Min, Max);
        }
    }
}