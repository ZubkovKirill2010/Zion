namespace Zion.STP.Dynamic
{
    public abstract class TextDocument<Pointer> : TextSource where Pointer : TextPointer<Pointer>
    {
        public event Action<Pointer, int>? Changed;

        public abstract TextSource BeginFrom(Pointer Position);
    }
}