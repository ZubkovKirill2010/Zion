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


        public override string ToString()
        {
            return $"[{Position}]";
        }

        public override int GetHashCode()
        {
            return Position;
        }


        public override bool Equals(IntPointer? Other)
        {
            return Other is not null && Position == Other.Position;
        }

        public override int CompareTo(IntPointer? Pointer)
        {
            return Position.CompareTo(Pointer.NotNull().Position);
        }
    }
}