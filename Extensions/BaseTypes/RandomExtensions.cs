using Zion.Vectors;

namespace Zion
{
    public static class RandomExtensions
    {
        public static Vector2 NextVector2(this Random Random, Vector2 Min, Vector2 Max)
        {
            return Vector2.Lerp(Min, Max, Random.NextSingle());
        }
        public static Vector3 NextVector3(this Random Random, Vector3 Min, Vector3 Max)
        {
            return Vector3.Lerp(Min, Max, Random.NextSingle());
        }

        public static Vector2Int NextVector2Int(this Random Random, Vector2Int Min, Vector2Int Max)
        {
            return new Vector2Int
            (
                Random.Next(Min.x, Max.x),
                Random.Next(Min.y, Max.y)
            );
        }
        public static Vector3Int NextVector3Int(this Random Random, Vector3Int Min, Vector3Int Max)
        {
            return new Vector3Int
            (
                Random.Next(Min.x, Max.x),
                Random.Next(Min.y, Max.y),
                Random.Next(Min.z, Max.z)
            );
        }
    }
}