using System.Collections;

namespace Zion.Serialization.TDC
{
    //Id -> Type
    public sealed class TypeRegistry : IEnumerable<TypeData>
    {
        private readonly ResizableArray<Type> Data;
        private int Writed;

        public int Count { get; private set; }


        public TypeRegistry() : this(32) { }

        public TypeRegistry(int Capacity)
        {
            Data = new(Capacity);
        }





        public void Write(BinaryWriter Writer)
        {
            ResizableArray<Type> Data = this.Data;
            int Count = this.Count;

            Writer.Write(Count - Writed);

            for (int i = Writed; i < Count; i++)
            {

            }

            Writed = Count;
        }

        public void Read(BinaryReader Reader)
        {
            int Count = Reader.ReadInt32();

            for (int i = 0; i < Count; i++)
            {
                ushort TypeId = Reader.ReadUInt16();
                Type Type   = ReadType(Reader);
                IFormat Format = Reader.Read(IFormat.Serializer);

                //TODO
            }
        }


        internal static Type ReadType(BinaryReader Reader)
        {
            string TypeName = Reader.ReadString();
            Type? Item = Type.GetType(TypeName);

            return Item ?? throw new ArgumentNullException($"Type '{TypeName}' not found"); ;
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TypeData> GetEnumerator()
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}