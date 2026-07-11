namespace Zion.STP.Dynamic
{
    public abstract class TextDocument<TPointer> : TextSource where TPointer : TextPointer<TPointer>
    {
        public abstract TPointer CurrentPosition { get; }

        public event Action<TPointer, int>? Changed;


        public sealed override TextSource BeginNew() => BeginNewDocument();


        public abstract TextSource BeginFrom(TPointer Position);

        public abstract TextDocument<TPointer> BeginNewDocument();
    }
}