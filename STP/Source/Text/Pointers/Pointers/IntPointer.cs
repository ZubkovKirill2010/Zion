namespace Zion.STP.Dynamic
{
    public sealed class IntPointer : TextPointer<IntPointer>
    {
        public int Position
        {
            get;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                field = value;
            }
        }


        public IntPointer(int Position)
        {
            this.Position = Position;
        }


        public static bool operator ==(IntPointer A, IntPointer B)
        {
            return A.Position == B.Position;
        }

        public static bool operator !=(IntPointer A, IntPointer B)
        {
            return A.Position != B.Position;
        }


        public override string ToString()
        {
            return $"[{Position}]";
        }

        public override bool Equals(object? Object)
        {
            return Object is IntPointer Pointer && this == Pointer;
        }

        public override int GetHashCode()
        {
            return Position;
        }


        public override int CompareTo(IntPointer? Pointer)
        {
            return Position.CompareTo(Pointer.NotNull().Position);
        }
    }
}