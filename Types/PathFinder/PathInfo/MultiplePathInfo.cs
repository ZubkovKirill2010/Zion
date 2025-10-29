namespace Zion
{
    public readonly struct MultiplePathInfo<TPoint>
    {
        public readonly TPoint Start;
        public readonly TPoint End;
        public readonly Func<TPoint, TPoint, bool> EqualsPoints;
        public readonly Func<TPoint, TPoint?, IEnumerable<TPoint>> GetAllPath; //(Current, Last, AvailablePaths)

        public int MaxDistance { get; init; } = -1;
        public bool ExcludeVisited { get; init; }

        public MultiplePathInfo(TPoint Start, TPoint End, Func<TPoint, TPoint?, IEnumerable<TPoint>> GetAllPath, Func<TPoint, TPoint, bool> Equals)
        {
            this.Start = Start;
            this.End = End;
            this.GetAllPath = GetAllPath;
            EqualsPoints = Equals;
        }
        public MultiplePathInfo(TPoint Start, TPoint End, Func<TPoint, TPoint?, IEnumerable<TPoint>> GetAllPath)
        {
            this.Start = Start;
            this.End = End;
            this.GetAllPath = GetAllPath;
            EqualsPoints = EqualityComparer<TPoint>.Default.Equals;
        }
    }
}