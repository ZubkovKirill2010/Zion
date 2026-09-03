namespace Zion.Serialization.ADF
{
    public readonly struct AutoWriter<T> : IADFWriter<T>
    {
        private readonly TypeSchema Schema;

        public DataFormat Format => Schema.Format;


        public AutoWriter(TypeSchema Schema)
        {
            if (Schema.Type != typeof(T))
            {
                throw new ArgumentException($"TypeSchema type mismatch: expected {Schema.Type}, got {typeof(T)}");
            }

            this.Schema = Schema;
        }

        public void Write(ADFObjectWriter Writer, T Value)
        {

        }
    }
}