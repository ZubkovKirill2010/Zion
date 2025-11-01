using System.Text;

namespace Zion
{
    public sealed class ParsingException : Exception
    {
        public readonly string String;
        public readonly int Index;

        public ParsingException(string Message) : base(Message)
        {
            String = string.Empty;
            Index = -1;
        }
        public ParsingException(string String, int Index, string Message) : base(Message)
        {
            this.String = String;
            this.Index = Index;
        }

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine(String);
            if (Index >= 0)
            {
                Builder.Append(new string(' ', Index));
                Builder.AppendLine($"^{Index}");
            }
            else
            {
                Builder.AppendLine("?");
            }
            Builder.Append(Message);

            return Builder.ToString();
        }
    }
}