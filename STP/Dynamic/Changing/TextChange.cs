namespace Zion.STP.Dynamic
{
    public readonly struct TextChange<TPointer> where TPointer : TextPointer<TPointer>
    {
        public readonly TPointer Position;
        public readonly int Length;

        public TextChange(TPointer Position, int Length)
        {
            this.Position = Position.NotNull();
            this.Length = Length;
        }
    }
}