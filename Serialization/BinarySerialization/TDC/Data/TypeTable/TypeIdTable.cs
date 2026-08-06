namespace Zion.Serialization.TDC
{
    //Type -> Id
    public sealed class TypeIdTable
    {
        private readonly Dictionary<Type, ushort> Data;
        private ushort LastId;

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
    }
}