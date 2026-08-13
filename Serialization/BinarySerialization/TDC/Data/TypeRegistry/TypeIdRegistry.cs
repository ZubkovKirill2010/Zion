using System.Collections;

namespace Zion.Serialization.TDC
{
    //Type -> Id
    public sealed class TypeIdRegistry : IEnumerable<TypeData>
    {
        private readonly Dictionary<Type, TypeInfo> Data;
        private readonly List<TypeData> Page;

        private ushort LastId;

        public int Count => Data.Count;


        public TypeIdRegistry() : this(32) { }

        public TypeIdRegistry(int Capacity)
        {
            Data = new(Capacity);
            Page = new(Capacity);
        }


        public bool TryGetInfo(Type Type, out TypeInfo Info)
        {
            return Data.TryGetValue(Type, out Info);
        }

        public TypeInfo Add(Type Type, IFormat Format)
        {
            TypeInfo Info = new TypeInfo(++LastId, Format);
            Data.Add(Type, Info);
            Page.Add(new TypeData(Type, Info));
            return Info;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.WriteCollection(Page);
            Page.Clear();
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TypeData> GetEnumerator()
        {
            return Data.Select(static Pair => new TypeData(Pair)).GetEnumerator();
        }
    }
}