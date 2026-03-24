namespace Zion
{
    public sealed partial class ObjectReader //_Special
    {
        public bool TryReadIdentifier(out string Value)
        {
            Value = null!;
            int Index = this.Index;

            if (IsEnd) { return false; }

            bool IsNormal(char Char)
            {
                return Char.IsEnglish() || Char == '_' || (Char >= '0'&& Char <= '9');
            }

            int Start = Index;
            char FirstChar = Text[Start];

            if (!FirstChar.IsEnglish() && FirstChar != '_')
            {
                return false;
            }

            while (Index < Length && IsNormal(Text[Index]))
            {
                Index++;
            }

            if (Index == Start)
            {
                return false;
            }

            Value = Text.Substring(Start, Index - Start);
            this.Index = Index;
            return true;
        }

        public string ReadIdentifier()
        {
            return Unsafe<string>(TryReadIdentifier);
        }
    }
}