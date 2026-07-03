namespace Zion.Serialization.NSD
{
    public interface INSDReadProvider
    {
        public bool TryReadContainer<T>(string Key, out T Value) where T : INSDContainer<T>;

        public bool TryReadPrimitive<T>(string Key, out T Value) where T : IBinaryReadable<T>;

        public bool TryRead<T>(string Key, out T Value, IBinaryReader<T>? ObjectReader = null);


        public bool TryReadContainer<T>(string Key, Action<T> Setter) where T : INSDContainer<T>
        {
            if (TryReadContainer(Key, out T Value))
            {
                Setter(Value);
                return true;
            }
            return false;
        }

        public bool TryReadPrimitive<T>(string Key, Action<T> Setter) where T : IBinarySerializable<T>
        {
            if (TryReadPrimitive(Key, out T Value))
            {
                Setter(Value);
                return true;
            }
            return false;
        }

        public bool TryRead<T>(string Key, Action<T> Setter, IBinaryReader<T>? ObjectReader = null)
        {
            if (TryRead(Key, out T Value, ObjectReader))
            {
                Setter(Value);
                return true;
            }
            return false;
        }


        public T ReadContainer<T>(string Key) where T : INSDContainer<T>
        {
            return TryReadContainer(Key, out T Value) ? Value : throw new InvalidOperationException($"Error reading the '{Key}' container");
        }

        public T ReadPrimitive<T>(string Key) where T : IBinarySerializable<T>
        {
            return TryReadPrimitive(Key, out T Value) ? Value : throw new InvalidOperationException($"Error reading the '{Key}' primitive");
        }

        public T Read<T>(string Key, IBinaryReader<T>? ObjectReader = null)
        {
            return TryRead(Key, out T Value, ObjectReader) ? Value : throw new InvalidOperationException($"Error reading the '{Key}' object");
        }
    }
}