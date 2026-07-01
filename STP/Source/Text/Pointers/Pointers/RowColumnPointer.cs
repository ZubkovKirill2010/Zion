using Zion.Vectors;

namespace Zion.STP.Dynamic
{
    public sealed class RowColumnPointer : TextPointer<RowColumnPointer>
    {
        #region Data
        public int Row;
        public int Column;

        #endregion

        #region Properties
        public override bool IsValid => Row >= 0 && Column >= 0;

        #endregion

        #region Constructors
        public RowColumnPointer(int Row, int Column)
        {
            this.Row = Row;
            this.Column = Column;
        }

        public RowColumnPointer(Vector2Int Position)
            : this(Position.Y, Position.X) { }

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            return $"[{Row}; {Column}]";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }


        public override bool Equals(RowColumnPointer? Other)
        {
            return Other is not null && Row == Other.Row && Column == Other.Column;
        }

        public override int CompareTo(RowColumnPointer? Pointer)
        {
            ArgumentNullException.ThrowIfNull(Pointer);

            int RowComparing = Row.CompareTo(Pointer.Row);

            if (RowComparing != 0)
            {
                return RowComparing;
            }

            return Column.CompareTo(Pointer.Column);
        }


        public override RowColumnPointer Sum(RowColumnPointer Other)
        {
            return new RowColumnPointer(Row + Other.Row, Column + Other.Column);
        }

        public override RowColumnPointer Subtract(RowColumnPointer Other)
        {
            return new RowColumnPointer(Row - Other.Row, Column - Other.Column);
        }

        #endregion
    }
}