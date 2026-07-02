using System.Diagnostics.CodeAnalysis;

namespace Zion.Serialization
{
    public static class BinaryReaderExtensions
    {
        extension(BinaryReader Reader)
        {
            public T Read<T>() where T : IBinarySerializable<T>
            {
                return T.Read(Reader);
            }

            public T Read<T>(IBinaryReader<T>? ObjectReader = null)
            {
                ObjectReader ??= BinarySerializer.GetReader<T>();

                if (ObjectReader is null)
                {
                    throw new ArgumentException($"The basic Reader for {typeof(T).FullName} was not found");
                }

                return ObjectReader.Read(Reader);
            }


            public TCollection ReadCollection<TCollection, T>() where TCollection : ICollection<T>, new() where T : IBinaryReadable<T>
            {
                return ReadCollection<TCollection, T>(Reader, static Count => new TCollection());
            }

            public TCollection ReadCollection<TCollection, T>(Func<int, TCollection> NewCollection) where TCollection : ICollection<T> where T : IBinaryReadable<T>
            {
                int Count = Reader.ReadInt32();

                TCollection Collection = NewCollection(Count);

                for (int i = 0; i < Count; i++)
                {
                    Collection.Add(T.Read(Reader));
                }

                return Collection;
            }


            public TCollection ReadCollection<TCollection, T>(IBinaryReader<T>? ObjectReader = null) where TCollection : ICollection<T>, new()
            {
                return ReadCollection(Reader, static Count => new TCollection(), ObjectReader);
            }

            public TCollection ReadCollection<TCollection, T>(Func<int, TCollection> NewCollection, IBinaryReader<T>? ObjectReader = null) where TCollection : ICollection<T>
            {
                FindReader(ref ObjectReader);

                int Count = Reader.ReadInt32();

                TCollection Collection = NewCollection(Count);

                for (int i = 0; i < Count; i++)
                {
                    Collection.Add(ObjectReader.Read(Reader));
                }

                return Collection;
            }


            public List<T> ReadList<T>() where T : IBinaryReadable<T>
            {
                return ReadCollection<List<T>, T>(Reader, static Count => new List<T>(Count));
            }

            public List<T> ReadList<T>(IBinaryReader<T>? ObjectReader = null)
            {
                return ReadCollection(Reader, static Count => new List<T>(Count), ObjectReader);
            }


            public T[] ReadArray<T>() where T : IBinaryReadable<T>
            {
                int Count = Reader.ReadInt32();

                T[] Array = new T[Count];

                for (int i = 0; i < Count; i++)
                {
                    Array[i] = T.Read(Reader);
                }

                return Array;
            }

            public T[] ReadArray<T>(IBinaryReader<T>? ObjectReader = null)
            {
                FindReader(ref ObjectReader);

                int Count = Reader.ReadInt32();

                T[] Array = new T[Count];

                for (int i = 0; i < Count; i++)
                {
                    Array[i] = ObjectReader.Read(Reader);
                }

                return Array;
            }


            private static void FindReader<T>([NotNull] ref IBinaryReader<T>? ObjectReader)
            {
                ObjectReader ??= BinarySerializer.GetReader<T>() ?? throw new ArgumentException($"Reader for {typeof(T).FullName} not found");
            }
        }
    }
}