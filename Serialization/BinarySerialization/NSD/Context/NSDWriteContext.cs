namespace Zion.Serialization.NSD
{
    public sealed class NSDWriteContext
    {
        private readonly INSDWriteProvider Provider;
        private readonly HashSet<string> UsingKeys;

        public NSDWriteContext(INSDWriteProvider Provider)
        {
            this.Provider = Provider.NotNull();
            UsingKeys = new HashSet<string>();
        }


        public void AddContainer<T>(string Key, T Value) where T : INSDContainer<T>
        {
            CheckKeyValue(Key, Value);
            Provider.AddContainer(Key, Value);
        }

        public void AddPrimitive<T>(string Key, T Value) where T : IBinaryWritable
        {
            CheckKeyValue(Key, Value);
            Provider.AddPrimitive(Key, Value);
        }

        public void AddSizable<T>(string Key, T Value) where T : INSDSizable<T>
        {
            CheckKeyValue(Key, Value);
            Provider.AddSizable(Key, Value);
        }


        private void CheckKeyValue<T>(string Key, T Value)
        {
            ArgumentNullException.ThrowIfNull(Value);
            CheckKey(Key);
        }

        private void CheckKey(string Key)
        {
            if (UsingKeys.Contains(Key.NotNull()))
            {
                throw new ArgumentException($"Key '{Key}' already using");
            }
            UsingKeys.Add(Key);
        }
    }
}