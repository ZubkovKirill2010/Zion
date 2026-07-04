using System.Collections.Concurrent;

namespace Zion.Serialization.NSD
{
    public sealed class NSDReadHandler
    {
        private readonly ConcurrentDictionary<string, Action<Stream>> Setters = new();

        public bool AddPrimitive<T>(string Key, Action<T> Setter) where T : IBinaryReadable<T>
        {
            return Add
            (
                Key,
                Stream => Setter(T.Read(new BinaryReader(Stream)))
            );
        }

        public bool AddContainer<T>(string Key, Action<T> Setter) where T : INSDContainer<T>
        {
            return Add
            (
                Key,
                Stream => Setter(T.Read(new NSDSequentialReadContext(Stream)))
            );
        }

        public bool Add<T>(string Key, Action<T> Setter, IBinaryReader<T>? ObjectReader = null)
        {
            ObjectReader ??= BinarySerializer.GetReader<T>();
            BinarySerializer.ReaderNotFound(ObjectReader);
            return Add(Key, Stream => Setter(ObjectReader.Read(new BinaryReader(Stream))));
        }


        public bool Contains(string Key)
        {
            return Setters.ContainsKey(Key);
        }

        public bool Remove(string Key)
        {
            return Setters.TryRemove(Key, out _);
        }


        public bool TryRead(string Key, Stream Stream)
        {
            if (Setters.TryGetValue(Key.NotNull(), out Action<Stream> Setter))
            {
                Setter(Stream.NotNull());
                return true;
            }
            return false;
        }

        public bool TryGetSetter(string Key, out Action<Stream> Setter)
        {
            return Setters.TryGetValue(Key, out Setter);
        }


        private bool Add(string Key, Action<Stream> Setter)
        {
            ArgumentException.ThrowIfNullOrEmpty(Key);
            return Setters.TryAdd(Key, Setter.NotNull());
        }
    }
}