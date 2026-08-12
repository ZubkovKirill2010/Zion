using System.Collections;

namespace Zion.Serialization.TDC
{
    //Type -> Id
    public sealed class TypeIdRegistry : IEnumerable<TypeData>
    {
        private readonly Dictionary<Type, TypeInfo> Data;
        private ushort LastId;


        public TypeIdRegistry() : this(32) { }

        public TypeIdRegistry(int Capacity)
        {
            Data = new(Capacity);
        }


        public bool TryGetInfo(Type Type, out TypeInfo Info)
        {
            return Data.TryGetValue(Type, out Info);
        }

        public TypeInfo Add(Type Type, IFormat Format)
        {
            TypeInfo Info = new TypeInfo(++LastId, Format);
            Data.Add(Type, Info);
            return Info;
        }


        public void Write(BinaryWriter Writer)
        {

        }

        public void Read(BinaryReader Reader)
        {
            int Count = Reader.ReadInt32();

            for (int i = 0; i < Count; i++)
            {
                ushort TypeId = Reader.ReadUInt16();
                Type Type   = TypeRegistry.ReadType(Reader);
                IFormat Format = Reader.Read(IFormat.Serializer);

                //TODO
            }
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TypeData> GetEnumerator()
        {
            return Data.Select(static Pair => new TypeData(Pair)).GetEnumerator();
        }
    }
}