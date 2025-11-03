using System.Text;

namespace Zion.MathExpressions
{
    public sealed partial class MathExpressionParser
    {
        private static readonly Dictionary<char, char> CharsAssociations = new()
        {
            { '{', '(' },
            { '}', ')' },
            { '[', '(' },
            { ']', ')' },
            { '.', ','}
        };

        public Dictionary<string, MathFunction> Functions { internal get; init; } = new Dictionary<string, MathFunction>(StringComparer.OrdinalIgnoreCase)
        {
            { "sqrt",    MathFunctions.Sqrt                        },
            { "abs",     MathFunctions.Convert(Fraction.Abs)  },
            { "sign",    MathFunctions.Convert(Fraction.Sign) },
            { "round",   MathFunctions.Convert(Fraction.Round)     },
            { "floor",   MathFunctions.Convert(Fraction.Floor)     },
            { "ceiling", MathFunctions.Convert(Fraction.Ceiling)   },
            { "trunc",   MathFunctions.Convert(Fraction.Truncate)  },

            { "min",  MathFunctions.ConvertGrouped(Fraction.Min) },
            { "max",  MathFunctions.ConvertGrouped(Fraction.Max) }, 
            { "sum",  MathFunctions.ConvertGrouped(Values => Fraction.Sum(Values))     },
            { "aver", MathFunctions.ConvertGrouped(Values => Fraction.Average(Values)) }
            
            //... sin, cos, tan, cot, log  {  ln (_e), log (_10), log(A|B)  }
        };
        public Dictionary<string, Fraction> Variables { internal get; init; } = new Dictionary<string, Fraction>(StringComparer.OrdinalIgnoreCase)
        {
            { "pi", Fraction.Pi },
            { "e", Fraction.E },
        };

        private readonly Amount Result = new Amount();

        private readonly string String;
        private char Current => String[Index];
        private int Index;
        private bool Finished => Index >= String.Length || Current is '|' or '\\' or ')';

        public int Accuracy
        {
            private get => field;
            init => field = Math.Max(1, value);
        } = 14;

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

        private void AddMember(IMathTerm Member)
        {
            Result.Add(Member);
        }

        private MathExpressionParser SubParser(int Start, int End)
        {
            return new MathExpressionParser(String[Start..End], Variables, Functions);
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

        private bool ExistsFunction(string Identifier)
        {
            return Functions.ContainsKey(Identifier);
        }
        private bool ExistsVariable(string Identifier)
        {
            return Variables.ContainsKey(Identifier);
        }

        private MathFunctionHandler GetHandler(int Start)
        {
            return new MathFunctionHandler(this, String, Start);
        }

        private static bool IsDigit(char Char) => char.IsDigit(Char) || Char == ',';
        private static bool IsLetter(char Char) => Translater.IsEnglish(Char);
    }
}