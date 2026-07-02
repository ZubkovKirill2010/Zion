namespace Zion.Vectors
{
    public static class DirectionExtensions
    {
        extension(HorizontalDirection Direction)
        {
            public HorizontalDirection Abs()
            {
                return Direction == HorizontalDirection.None ? HorizontalDirection.None : HorizontalDirection.Right;
            }

            public HorizontalDirection ToNegative()
            {
                return Direction == HorizontalDirection.None ? HorizontalDirection.None : HorizontalDirection.Left;
            }


            public Vector2 ToVector2()
            {
                switch (Direction)
                {
                    case HorizontalDirection.Left: return Vector2.Left;
                    case HorizontalDirection.Right: return Vector2.Right;
                    default: return Vector2.Zero;
                }
            }

            public Vector3 ToVector3()
            {
                switch (Direction)
                {
                    case HorizontalDirection.Left: return Vector3.Left;
                    case HorizontalDirection.Right: return Vector3.Right;
                    default: return Vector3.Zero;
                }
            }

            public Vector2Int ToVector2Int()
            {
                switch (Direction)
                {
                    case HorizontalDirection.Left: return Vector2Int.Left;
                    case HorizontalDirection.Right: return Vector2Int.Right;
                    default: return Vector2Int.Zero;
                }
            }

            public Vector3Int ToVector3Int()
            {
                switch (Direction)
                {
                    case HorizontalDirection.Left: return Vector3Int.Left;
                    case HorizontalDirection.Right: return Vector3Int.Right;
                    default: return Vector3Int.Zero;
                }
            }
        }

        extension(VerticalDirection Direction)
        {
            public VerticalDirection Abs()
            {
                return Direction == VerticalDirection.None ? VerticalDirection.None : VerticalDirection.Up;
            }

            public VerticalDirection ToNegative()
            {
                return Direction == VerticalDirection.None ? VerticalDirection.None : VerticalDirection.Down;
            }


            public Vector2 ToVector2()
            {
                switch (Direction)
                {
                    case VerticalDirection.Down: return Vector2.Down;
                    case VerticalDirection.Up: return Vector2.Up;
                    default: return Vector2.Zero;
                }
            }

            public Vector3 ToVector3()
            {
                switch (Direction)
                {
                    case VerticalDirection.Down: return Vector3.Down;
                    case VerticalDirection.Up: return Vector3.Up;
                    default: return Vector3.Zero;
                }
            }

            public Vector2Int ToVector2Int()
            {
                switch (Direction)
                {
                    case VerticalDirection.Down: return Vector2Int.Down;
                    case VerticalDirection.Up: return Vector2Int.Up;
                    default: return Vector2Int.Zero;
                }
            }

            public Vector3Int ToVector3Int()
            {
                switch (Direction)
                {
                    case VerticalDirection.Down: return Vector3Int.Down;
                    case VerticalDirection.Up: return Vector3Int.Up;
                    default: return Vector3Int.Zero;
                }
            }
        }

        extension(DepthDirection Direction)
        {
            public DepthDirection Abs()
            {
                return Direction == DepthDirection.None ? DepthDirection.None : DepthDirection.Forward;
            }

            public DepthDirection ToNegative()
            {
                return Direction == DepthDirection.None ? DepthDirection.None : DepthDirection.Back;
            }


            public Vector3 ToVector3()
            {
                switch (Direction)
                {
                    case DepthDirection.Back: return Vector3.Back;
                    case DepthDirection.Forward: return Vector3.Forward;
                    default: return Vector3.Zero;
                }
            }

            public Vector3Int ToVector3Int()
            {
                switch (Direction)
                {
                    case DepthDirection.Back: return Vector3Int.Back;
                    case DepthDirection.Forward: return Vector3Int.Forward;
                    default: return Vector3Int.Zero;
                }
            }
        }

        extension(Direction Direction)
        {
            public int X => GetAxis((int)Direction, 0);
            public int Y => GetAxis((int)Direction, 2);


            public Direction Abs()
            {
                return (Direction)((int)Direction | 0b_01_01);
            }

            public Direction ToNegative()
            {
                return (Direction)((int)Direction & 0b00001010);
            }


            public Vector2 ToVector2()
            {
                int Code = (int)Direction;
                return new Vector2(GetAxis(Code, 0), GetAxis(Code, 2));
            }

            public Vector3 ToVector3()
            {
                int Code = (int)Direction;
                return new Vector3(GetAxis(Code, 0), GetAxis(Code, 2), 0F);
            }

            public Vector2Int ToVector2Int()
            {
                int Code = (int)Direction;
                return new Vector2Int(GetAxis(Code, 0), GetAxis(Code, 2));
            }

            public Vector3Int ToVector3Int()
            {
                int Code = (int)Direction;
                return new Vector3Int(GetAxis(Code, 0), GetAxis(Code, 2), 0);
            }
        }

        extension(Direction3 Direction)
        {
            public int X => GetAxis((int)Direction, 0);
            public int Y => GetAxis((int)Direction, 2);
            public int Z => GetAxis((int)Direction, 4);


            public Direction3 Abs()
            {
                return (Direction3)((int)Direction | 0b_01_01_01);
            }

            public Direction3 ToNegative()
            {
                return (Direction3)((int)Direction & 0b00101010);
            }


            public Vector3 ToVector3()
            {
                int Code = (int)Direction;
                return new Vector3(GetAxis(Code, 0), GetAxis(Code, 2), GetAxis(Code, 4));
            }

            public Vector3Int ToVector3Int()
            {
                int Code = (int)Direction;
                return new Vector3Int(GetAxis(Code, 0), GetAxis(Code, 2), GetAxis(Code, 4));
            }
        }

        private static int GetAxis(int Direction, int Index)
        {
            switch ((Direction >> Index) & 0b11)
            {
                case 0b10: return -1;
                case 0b11: return 1;
                default:   return 0;
            }
        }
    }
}