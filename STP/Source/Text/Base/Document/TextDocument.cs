namespace Zion.STP.Dynamic
{
    public abstract class TextDocument<TPointer> : TextSource where TPointer : TextPointer<TPointer>
    {
        public event Action<TPointer, int>? Changed;

        public abstract TextSource BeginFrom(TPointer Position);
    }
}