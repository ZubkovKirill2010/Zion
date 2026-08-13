using System.Collections;
using System.ComponentModel;

namespace Zion.Serialization.TDC
{
    //Id -> Type
    public sealed class TypeRegistry : IEnumerable<TypeData>
    {
        private readonly Dictionary<ushort, (Type, IFormat)> Data;

        public int Count => Data.Count;


        public TypeRegistry() : this(32) { }

        public TypeRegistry(int Capacity)
        {
            Data = new(Capacity);
        }


        public void Add(TypeData Item)
        {
            //TODO
        }


        public void Read(BinaryReader Reader)
        {
            var Data = this.Data;
            int Count = Reader.ReadInt32();

            Data.EnsureCapacity(Data.Count + Count);

            for (int i = 0; i < Count; i++)
            {
                TypeData Item = Reader.Read<TypeData>();
                Add(Item);
            }
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TypeData> GetEnumerator()
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}