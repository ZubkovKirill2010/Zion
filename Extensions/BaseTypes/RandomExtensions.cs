using Zion.Vectors;

namespace Zion
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Generates a random <see cref="Vector2"/> within the specified range.
        /// </summary>
        /// <param name="Random">The random number generator.</param>
        /// <param name="Min">The inclusive minimum vector values.</param>
        /// <param name="Max">The exclusive maximum vector values.</param>
        /// <returns>A random vector with components between Min and Max.</returns>
        public static Vector2 NextVector2(this Random Random, Vector2 Min, Vector2 Max)
        {
            return new Vector2(Random.Next((int)Min.x, (int)Max.x), Random.Next((int)Min.y, (int)Max.y));
        }

        /// <summary>
        /// Generates a random <see cref="Vector3"/> within the specified range.
        /// </summary>
        /// <param name="Random">The random number generator.</param>
        /// <param name="Min">The inclusive minimum vector values.</param>
        /// <param name="Max">The exclusive maximum vector values.</param>
        /// <returns>A random vector with components between Min and Max.</returns>
        public static Vector3 NextVector3(this Random Random, Vector3 Min, Vector3 Max)
        {
            return new Vector3(Random.Next((int)Min.x, (int)Max.x), Random.Next((int)Min.y, (int)Max.y), Random.Next((int)Min.z, (int)Max.z));
        }

        /// <summary>
        /// Generates a random <see cref="Vector2Int"/> within the specified range.
        /// </summary>
        /// <param name="Random">The random number generator.</param>
        /// <param name="Min">The inclusive minimum vector values.</param>
        /// <param name="Max">The exclusive maximum vector values.</param>
        /// <returns>A random integer vector with components between Min and Max.</returns>
        public static Vector2Int NextVector2Int(this Random Random, Vector2Int Min, Vector2Int Max)
        {
            return new Vector2Int
            (
                Random.Next(Min.x, Max.x),
                Random.Next(Min.y, Max.y)
            );
        }

        /// <summary>
        /// Generates a random <see cref="Vector3Int"/> within the specified range.
        /// </summary>
        /// <param name="Random">The random number generator.</param>
        /// <param name="Min">The inclusive minimum vector values.</param>
        /// <param name="Max">The exclusive maximum vector values.</param>
        /// <returns>A random integer vector with components between Min and Max.</returns>
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