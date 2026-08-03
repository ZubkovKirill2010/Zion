namespace Zion.Serialization.NSD
{
    public sealed class NSDBatchReader : IDisposable
    {
        private readonly NSDReadContext Context;
        private readonly NSDReadHandler Handler;

        public NSDBatchReader(NSDReadContext Context)
        {
            this.Context = Context.NotNull();
            this.Handler = new();
        }


        public bool ReadPrimitive<T>(string Key, Action<T> Setter) where T : IBinarySerializable<T>
        {
            return Handler.AddPrimitive(Key, Setter);
        }

        public bool ReadContainer<T>(string Key, Action<T> Setter) where T : INSDContainer<T>
        {
            return Handler.AddContainer(Key, Setter);
        }

        public bool Read<T>(string Key, Action<T> Setter, IBinaryReader<T>? ObjectReader = null)
        {
            return Handler.Add(Key, Setter, ObjectReader);
        }


        public bool Read(string Key, Action<bool> Setter)
        {
            return Handler.Add(Key, Setter, bool.Serializer);
        }

        public bool Read(string Key, Action<byte> Setter)
        {
            return Handler.Add(Key, Setter, byte.Serializer);
        }

        public bool Read(string Key, Action<sbyte> Setter)
        {
            return Handler.Add(Key, Setter, sbyte.Serializer);
        }

        public bool Read(string Key, Action<char> Setter)
        {
            return Handler.Add(Key, Setter, char.Serializer);
        }

        public bool Read(string Key, Action<decimal> Setter)
        {
            return Handler.Add(Key, Setter, decimal.Serializer);
        }

        public bool Read(string Key, Action<double> Setter)
        {
            return Handler.Add(Key, Setter, double.Serializer);
        }

        public bool Read(string Key, Action<float> Setter)
        {
            return Handler.Add(Key, Setter, float.Serializer);
        }

        public bool Read(string Key, Action<int> Setter)
        {
            return Handler.Add(Key, Setter, int.Serializer);
        }

        public bool Read(string Key, Action<uint> Setter)
        {
            return Handler.Add(Key, Setter, uint.Serializer);
        }

        public bool Read(string Key, Action<long> Setter)
        {
            return Handler.Add(Key, Setter, long.Serializer);
        }

        public bool Read(string Key, Action<ulong> Setter)
        {
            return Handler.Add(Key, Setter, ulong.Serializer);
        }

        public bool Read(string Key, Action<short> Setter)
        {
            return Handler.Add(Key, Setter, short.Serializer);
        }

        public bool Read(string Key, Action<ushort> Setter)
        {
            return Handler.Add(Key, Setter, ushort.Serializer);
        }

        public bool Read(string Key, Action<string> Setter)
        {
            return Handler.Add(Key, Setter, string.Serializer);
        }


        public bool ReadArray<T>(string Key, Action<T[]> Setter) where T : IBinaryReadable<T>
        {
            return Handler.Add
            (
                Key,
                Setter,
                new ArrayReader<T>(T.Read)
            );
        }

        public bool ReadArray<T>(string Key, Action<T[]> Setter, IBinaryReader<T>? ItemReader = null)
        {
            ItemReader ??= BinarySerializer.GetReader<T>();
            BinarySerializer.ReaderNotFound(ItemReader);

            return Handler.Add
            (
                Key,
                Setter,
                new ArrayReader<T>(ItemReader.Read)
            );
        }

        public bool ReadBooleanArray(string Key, Action<bool[]> Setter)
        {
            return ReadArray(Key, Setter, bool.Serializer);
        }

        public bool ReadByteArray(string Key, Action<byte[]> Setter)
        {
            return ReadArray(Key, Setter, byte.Serializer);
        }

        public bool ReadSByteArray(string Key, Action<sbyte[]> Setter)
        {
            return ReadArray(Key, Setter, sbyte.Serializer);
        }

        public bool ReadCharArray(string Key, Action<char[]> Setter)
        {
            return ReadArray(Key, Setter, char.Serializer);
        }

        public bool ReadDecimalArray(string Key, Action<decimal[]> Setter)
        {
            return ReadArray(Key, Setter, decimal.Serializer);
        }

        public bool ReadDoubleArray(string Key, Action<double[]> Setter)
        {
            return ReadArray(Key, Setter, double.Serializer);
        }

        public bool ReadSingleArray(string Key, Action<float[]> Setter)
        {
            return ReadArray(Key, Setter, float.Serializer);
        }

        public bool ReadInt32Array(string Key, Action<int[]> Setter)
        {
            return ReadArray(Key, Setter, int.Serializer);
        }

        public bool ReadUInt32Array(string Key, Action<uint[]> Setter)
        {
            return ReadArray(Key, Setter, uint.Serializer);
        }

        public bool ReadInt64Array(string Key, Action<long[]> Setter)
        {
            return ReadArray(Key, Setter, long.Serializer);
        }

        public bool ReadUInt64Array(string Key, Action<ulong[]> Setter)
        {
            return ReadArray(Key, Setter, ulong.Serializer);
        }

        public bool ReadInt16Array(string Key, Action<short[]> Setter)
        {
            return ReadArray(Key, Setter, short.Serializer);
        }

        public bool ReadUInt16Array(string Key, Action<ushort[]> Setter)
        {
            return ReadArray(Key, Setter, ushort.Serializer);
        }

        public bool ReadStringArray(string Key, Action<string[]> Setter)
        {
            return ReadArray(Key, Setter, string.Serializer);
        }


        public bool ReadCollection<TCollection, T>(string Key, Action<TCollection> Setter) where TCollection : ICollection<T>, new() where T : IBinaryReadable<T>
        {
            return ReadCollection<TCollection, T>(Key, Setter, static Count => new());
        }

        public bool ReadCollection<TCollection, T>(string Key, Action<TCollection> Setter, IBinaryReader<T>? ItemReader = null) where TCollection : ICollection<T>, new()
        {
            return ReadCollection(Key, Setter, static Count => new(), ItemReader);
        }

        public bool ReadBooleanCollection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<bool>, new()
        {
            return ReadCollection(Key, Setter, bool.Serializer);
        }

        public bool ReadByteCollection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<byte>, new()
        {
            return ReadCollection(Key, Setter, byte.Serializer);
        }

        public bool ReadSByteCollection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<sbyte>, new()
        {
            return ReadCollection(Key, Setter, sbyte.Serializer);
        }

        public bool ReadCharCollection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<char>, new()
        {
            return ReadCollection(Key, Setter, char.Serializer);
        }

        public bool ReadDecimalCollection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<decimal>, new()
        {
            return ReadCollection(Key, Setter, decimal.Serializer);
        }

        public bool ReadDoubleCollection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<double>, new()
        {
            return ReadCollection(Key, Setter, double.Serializer);
        }

        public bool ReadSingleCollection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<float>, new()
        {
            return ReadCollection(Key, Setter, float.Serializer);
        }

        public bool ReadInt32Collection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<int>, new()
        {
            return ReadCollection(Key, Setter, int.Serializer);
        }

        public bool ReadUInt32Collection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<uint>, new()
        {
            return ReadCollection(Key, Setter, uint.Serializer);
        }

        public bool ReadInt64Collection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<long>, new()
        {
            return ReadCollection(Key, Setter, long.Serializer);
        }

        public bool ReadUInt64Collection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<ulong>, new()
        {
            return ReadCollection(Key, Setter, ulong.Serializer);
        }

        public bool ReadInt16Collection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<short>, new()
        {
            return ReadCollection(Key, Setter, short.Serializer);
        }

        public bool ReadUInt16Collection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<ushort>, new()
        {
            return ReadCollection(Key, Setter, ushort.Serializer);
        }

        public bool ReadStringCollection<TCollection>(string Key, Action<TCollection> Setter) where TCollection : ICollection<string>, new()
        {
            return ReadCollection(Key, Setter, string.Serializer);
        }


        public bool ReadCollection<TCollection, T>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<T> where T : IBinaryReadable<T>
        {
            return Handler.Add
            (
                Key,
                Setter,
                new CollectionReader<TCollection, T>
                (
                    T.Read, Factory
                )
            );
        }

        public bool ReadCollection<TCollection, T>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory, IBinaryReader<T>? ItemReader = null) where TCollection : ICollection<T>
        {
            ItemReader ??= BinarySerializer.GetReader<T>();
            BinarySerializer.ReaderNotFound(ItemReader);

            return Handler.Add
            (
                Key,
                Setter,
                new CollectionReader<TCollection, T>
                (
                    ItemReader.Read,
                    Factory
                )
            );
        }

        public bool ReadBooleanCollection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<bool>
        {
            return ReadCollection(Key, Setter, Factory, bool.Serializer);
        }

        public bool ReadByteCollection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<byte>
        {
            return ReadCollection(Key, Setter, Factory, byte.Serializer);
        }

        public bool ReadSByteCollection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<sbyte>
        {
            return ReadCollection(Key, Setter, Factory, sbyte.Serializer);
        }

        public bool ReadCharCollection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<char>
        {
            return ReadCollection(Key, Setter, Factory, char.Serializer);
        }

        public bool ReadDecimalCollection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<decimal>
        {
            return ReadCollection(Key, Setter, Factory, decimal.Serializer);
        }

        public bool ReadDoubleCollection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<double>
        {
            return ReadCollection(Key, Setter, Factory, double.Serializer);
        }

        public bool ReadSingleCollection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<float>
        {
            return ReadCollection(Key, Setter, Factory, float.Serializer);
        }

        public bool ReadInt32Collection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<int>
        {
            return ReadCollection(Key, Setter, Factory, int.Serializer);
        }

        public bool ReadUInt32Collection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<uint>
        {
            return ReadCollection(Key, Setter, Factory, uint.Serializer);
        }

        public bool ReadInt64Collection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<long>
        {
            return ReadCollection(Key, Setter, Factory, long.Serializer);
        }

        public bool ReadUInt64Collection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<ulong>
        {
            return ReadCollection(Key, Setter, Factory, ulong.Serializer);
        }

        public bool ReadInt16Collection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<short>
        {
            return ReadCollection(Key, Setter, Factory, short.Serializer);
        }

        public bool ReadUInt16Collection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<ushort>
        {
            return ReadCollection(Key, Setter, Factory, ushort.Serializer);
        }

        public bool ReadStringCollection<TCollection>(string Key, Action<TCollection> Setter, Func<int, TCollection> Factory) where TCollection : ICollection<string>
        {
            return ReadCollection(Key, Setter, Factory, string.Serializer);
        }        


        public bool TryRead(string Key, Stream Stream)
        {
            return Handler.TryRead(Key, Stream);
        }

        public bool TryGetSetter(string Key, out Action<Stream> Setter)
        {
            return Handler.TryGetSetter(Key, out Setter);
        }


        public void Dispose()
        {
            Context.ReadAll(this);
        }
    }
}