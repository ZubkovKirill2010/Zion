using Zion.Vectors;

namespace Zion.STP.Dynamic
{
    public sealed class RowColumnPointer : TextPointer<RowColumnPointer>
    {
        public int Row
        {
            get;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                field = value;
            }
        }
        public int Column
        {
            get;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                field = value;
            }
        }


        public RowColumnPointer(int Row, int Column)
        {
            this.Row = Row;
            this.Column = Column;
        }
        public RowColumnPointer(Vector2Int Position)
            : this(Position.Y, Position.X) { }


        public static bool operator ==(RowColumnPointer A, RowColumnPointer B)
        {
            return A.Row == B.Row && A.Column == B.Column;
        }

        public static bool operator !=(RowColumnPointer A, RowColumnPointer B)
        {
            return A.Row != B.Row || A.Column != B.Column;
        }


        public override string ToString()
        {
            return $"[{Row}; {Column}]";
        }

        public override bool Equals(object? Object)
        {
            return Object is RowColumnPointer Pointer && this == Pointer;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
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
    }
}