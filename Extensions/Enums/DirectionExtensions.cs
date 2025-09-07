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


        public static Direction GetDirection(int Index) => Directions[Index % Directions.Length];

        public static Direction Abs(this Direction Direction)
        {
            switch (Direction)
            {
                case Direction.Left: return Direction.Right;
                case Direction.Down: return Direction.Up;
            }
            return Direction;
        }
        public static HorizontalDirection Abs(this HorizontalDirection Direction)
        {
            if (Direction == HorizontalDirection.Left)
            {
                return HorizontalDirection.Right;
            }
            return Direction;
        }
        public static VerticalDirection Abs(this VerticalDirection Direction)
        {
            if (Direction == VerticalDirection.Down)
            {
                return VerticalDirection.Up;
            }
            return Direction;
        }

        public static Direction ToDirection(this HorizontalDirection HoriaontalDirection)
        {
            return HoriaontalDirection switch
            {
                HorizontalDirection.Left => Direction.Left,
                HorizontalDirection.Right => Direction.Right,
                _ => Direction.None
            };
        }
        public static Direction ToDirection(this VerticalDirection HoriaontalDirection)
        {
            return HoriaontalDirection switch
            {
                VerticalDirection.Down => Direction.Down,
                VerticalDirection.Up => Direction.Up,
                _ => Direction.None
            };
        }

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
        public static Vector2 ToVector2(this HorizontalDirection HoriaontalDirection)
        {
            return HoriaontalDirection switch
            {
                HorizontalDirection.Left => Vector2.Left,
                HorizontalDirection.Right => Vector2.Right,
                _ => Vector2.Zero
            };
        }
        public static Vector2 ToVector2(this VerticalDirection HoriaontalDirection)
        {
            return HoriaontalDirection switch
            {
                VerticalDirection.Down => Vector2.Down,
                VerticalDirection.Up => Vector2.Up,
                _ => Vector2.Zero
            };
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
        public static Vector2Int ToVector2Int(this HorizontalDirection HoriaontalDirection)
        {
            return HoriaontalDirection switch
            {
                HorizontalDirection.Left => Vector2Int.Left,
                HorizontalDirection.Right => Vector2Int.Right,
                _ => Vector2Int.Zero
            };
        }
        public static Vector2Int ToVector2Int(this VerticalDirection HoriaontalDirection)
        {
            return HoriaontalDirection switch
            {
                VerticalDirection.Down => Vector2Int.Down,
                VerticalDirection.Up => Vector2Int.Up,
                _ => Vector2Int.Zero
            };
        }
    }
}