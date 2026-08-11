namespace Zion.Serialization.TDC
{
    //Type -> Id
    public sealed class TypeIdRegistry
    {
        private readonly Dictionary<Type, ushort> Data;
        private ushort LastId;


        public TypeIdRegistry() : this(32) { }

        public TypeIdRegistry(int Capacity)
        {
            Data = new(Capacity);
        }


        public ushort GetOrAdd<T>()
        {
            return GetOrAdd(typeof(T));
        }

        public ushort GetOrAdd(Type Type)
        {
            if (Data.TryGetValue(Type, out ushort Result))
            {
                return Result;
            }
            ushort Id = ++LastId;
            Data[Type] = Id;
            return Id;
        }


        public void Write(BinaryWriter Writer)
        {

        }

        public void Read(BinaryReader Reader)
        {

        }
    }
}