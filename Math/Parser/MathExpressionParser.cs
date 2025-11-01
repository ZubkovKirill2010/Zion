using System.Text;

namespace Zion.MathExpressions
{
    public sealed class MathExpressionParser
    {
        private static readonly Dictionary<char, char> CharsAssociations = new()
        {
            { '{', '(' },
            { '}', ')' },
            { '[', '(' },
            { ']', ')' },
            { '.', ','}
        };

        private readonly string String;

        public MathExpressionParser(string String)
        {
            this.String = NormalizeString(String, 0, String.Length);
        }
        public MathExpressionParser(string String, int Start, int End)
        {
            this.String = NormalizeString(String, Start, End);
        }

        public IFraction Parse()
        {
            return null;
        }

        public bool TryParse(out IFraction Value)
        {
            try
            {
                Value = Parse();
                return true;
            }
            catch
            {
                Value = default;
                return false;
            }
        }


        internal static string NormalizeString(string String, int Start, int End)
        {
            if (Start < 0 || Start >= End)
            {
                throw new ArgumentOutOfRangeException($"Start(={Start}) < 0 || >= End(={End})");
            }
            if (End >= String.Length)
            {
                throw new ArgumentOutOfRangeException($"End(={End}) >= String.Length(={String.Length})");
            }

            StringBuilder Builder = new StringBuilder(End - Start);

            for (int i = Start; i < End; i++)
            {
                char Char = String[i];

                if (char.IsWhiteSpace(Char) || Char == '_')
                {
                    continue;
                }
                Builder.Append
                (
                    CharsAssociations.TryGetValue
                    (
                        Char,
                        out char Association
                    ) ? Association : Char
                );
            }

            return Builder.ToString();
        }
    }
}