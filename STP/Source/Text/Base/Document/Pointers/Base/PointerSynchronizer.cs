namespace Zion.STP.Dynamic
{
    public abstract class PointerSynchronizer<Pointer> where Pointer : TextPointer<Pointer>
    {
        public abstract void Synchronize(Pointer Start, int Offset);


        public abstract void AddPointer(Pointer Pointer);

        public abstract void RemovePointer(Pointer Pointer);
    }
}