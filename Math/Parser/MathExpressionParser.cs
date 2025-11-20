using System.Text;

namespace Zion.MathExpressions
{
    public sealed partial class MathExpressionParser
    {
        public MathExpressionParser(string String)
        {
            this.String = NormalizeString(String, 0, String.Length);
        }
        public MathExpressionParser(string String, int Start, int End)
        {
            this.String = NormalizeString(String, Start, End);
        }
        internal MathExpressionParser(string String, Dictionary<string, Fraction> Variables, Dictionary<string, MathFunction> Functions)
        {
            this.String = String;
            this.Variables = Variables;
            this.Functions = Functions;
        }

        public bool TryParse(out IMathTerm Value)
        {
            return ((Func<IMathTerm>)Parse).Try(out Value);
        }

        internal static string NormalizeString(string String, int Start, int End)
        {
            if (Start < 0 || Start >= End)
            {
                throw new ArgumentOutOfRangeException($"Start(={Start}) < 0 || >= End(={End})");
            }
            if (End > String.Length)
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


        public void AddFunction(string Name, MathFunction Function)
        {
            Functions.Add(Name, Function);
        }
        public void AddVariable(string Name, Fraction Value)
        {
            Variables.Add(Name, Value);
        }

        private IMathTerm Throw(string Message)
        {
            throw new ParsingException(String, Index, Message);
        }
        private IMathTerm Throw(int Index, string Message)
        {
            throw new ParsingException(String, Index, Message);
        }

        private IMathTerm InvalidChar()
        {
            throw new ParsingException(String, Index, $"Invalid char '{Current}'");
        }
        private IMathTerm Unfinished()
        {
            throw new ParsingException(String, String.Length - 1, "An unfinished expression");
        }

        private bool ExistsFunction(string Identifier) => Functions.ContainsKey(Identifier);
        private bool ExistsVariable(string Identifier) => Variables.ContainsKey(Identifier);

        private FunctionHandler GetHandler(int Start) => new FunctionHandler(this, String, Start);

        private static bool IsDigit(char Char) => char.IsDigit(Char) || Char == ',';
        private static bool IsLetter(char Char) => Translater.IsEnglish(Char);
    }
}