namespace Zion.STP.Dynamic
{
    public sealed class IntPointer : TextPointer<IntPointer>
    {
        #region Data
        public int Position;

        #endregion

        #region Properties
        public override bool IsValid => Position >= 0;

        #endregion

        #region Constructors
        public IntPointer(int Position)
        {
            this.Position = Position;
        }

        #endregion

        #region OverrideMethods
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


        public override IntPointer Sum(IntPointer Other)
        {
            return new IntPointer(Position + Other.Position);
        }

        public override IntPointer Subtract(IntPointer Other)
        {
            return new IntPointer(Position - Other.Position);
        }

        #endregion
    }
}