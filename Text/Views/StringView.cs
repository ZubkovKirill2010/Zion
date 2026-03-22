namespace Zion
{
    public sealed class StringView : TextView
    {
        private readonly string String;

        public StringView(string String)
        {
            ArgumentNullException.ThrowIfNull(String);

            this.String = String;
            Length = String.Length;
        }

        public override char this[int Index]
        {
            get => String[Index];
        }

        public static implicit operator StringView(string Value)
        {
            return new StringView(Value);
        }


        public override string ToString()
        {
            return String;
        }
    }
}