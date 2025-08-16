using Zion.Vectors;

namespace Zion
{
    public static class DirectionExtensions
    {
        /// <summary>
        /// Converts direction to absolute value (Left->Right, Down->Up).
        /// </summary>
        public static Direction Abs(this Direction Direction)
        {
            switch (Direction)
            {
                case Direction.Left: return Direction.Right;
                case Direction.Down: return Direction.Up;
            }
            return Direction;
        }

        /// <summary>
        /// Converts direction to 2D vector representation.
        /// </summary>
        public static Vector2 ToVector2(this Direction Direction)
        {
            switch (Direction)
            {
                case Direction.Up: return Vector2.Up;
                case Direction.Down: return Vector2.Down;
                case Direction.Left: return Vector2.Left;
                case Direction.Right: return Vector2.Right;
            }
            return Vector2.Zero;
        }

        // Other methods remain the same...
    }
}