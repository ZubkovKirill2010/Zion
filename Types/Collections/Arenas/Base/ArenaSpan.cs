namespace Zion
{
    public readonly struct ArenaSpan<T>
    {
        public readonly Arena<T> Source;
        public readonly int Start;
        public readonly int Length;

        public ArenaSpan(Arena<T> Source, int Start, int Length)
        {
            this.Source = Source.NotNull();
            this.Start  = Start;
            this.Length = Length;
        }
    }
}