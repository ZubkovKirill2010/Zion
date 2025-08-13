using Zion.Vectors;

namespace Zion
{
    public static class DirectionExtensions
    {
        private static readonly Direction[] Directions =
        [
            Direction.Up,
            Direction.Right,
            Direction.Down,
            Direction.Left
        ];

        public static Direction Abs(this Direction Direction)
        {
            switch (Direction)
            {
                case Direction.Left: return Direction.Right;
                case Direction.Down: return Direction.Up;
            }
            return Direction;
        }

        public static Direction GetByIndex(int Index)
        {
            return Directions[Index];
        }
        public static Direction GetByIndex(this Direction Direction, int Index)
        {
            return GetByIndex(Index);
        }

        public static Direction[] GetDirections() => Directions;
        public static Direction[] GetDirections(this Direction Direction) => Directions;

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
        public static Vector2Int ToVector2Int(this Direction Direction)
        {
            switch (Direction)
            {
                case Direction.Up: return Vector2Int.Up;
                case Direction.Down: return Vector2Int.Down;
                case Direction.Left: return Vector2Int.Left;
                case Direction.Right: return Vector2Int.Right;
            }
            return Vector2Int.Zero;
        }
    }
}