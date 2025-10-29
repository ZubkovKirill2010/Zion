namespace Zion.MathExpressions
{
    public sealed class MathExpressionParser : StreamingParser<IExpression>
    {
        private static readonly Exception ParseException = new Exception("Error in parsing a mathematical expression");

        private static readonly Dictionary<string, MathFunctionSample> DefaultFunctions = new()
        {
            { "sqrt", new MathFunctionSample(Fraction.Sqrt, true)                      },
            { "sin" , new MathFunctionSample(Fraction.Sin, true)                       },
            { "cos" , new MathFunctionSample(Fraction.Cos, true)                       },
            { "tg"  , new MathFunctionSample(Fraction.Tg, true)                        },
            { "ctg" , new MathFunctionSample(Fraction.Ctg, true)                       },
            { "pi"  , new MathFunctionSample((Fraction Value) => Fraction.Pi, false)  }
        };

        private readonly Amount Result;
        private readonly Dictionary<string, MathFunctionSample> Functions;

        public MathExpressionParser() : base(string.Empty)
        {
            Result = new Amount();
            Functions = DefaultFunctions;
        }
        public MathExpressionParser(string String, int Start = 0)
            : base
            (
                String.RemoveChars(Char => char.IsWhiteSpace(Char) || Char == '_').Replace('.', ',')
            )
        {
            Result = new Amount();
            Functions = DefaultFunctions;
        }
        public MathExpressionParser(string String, (string, MathFunctionSample)[] Functions) : base
            (
                String.RemoveChars(Char => char.IsWhiteSpace(Char) || Char == '_').Replace('.', ',')
            )
        {
            Result = new Amount();
            this.Functions = DefaultFunctions;
            foreach ((string, MathFunctionSample) Function in Functions)
            {
                this.Functions.Add(Function.Item1, Function.Item2);
            }
        }
        public MathExpressionParser(string String, Dictionary<string, MathFunctionSample> Functions) : base
            (
                String.RemoveChars(Char => char.IsWhiteSpace(Char) || Char == '_').Replace('.', ',')
            )
        {
            Result = new Amount();
            this.Functions = Functions;
        }


        public override string ToString() => Result.ToString();


        public override IExpression Parse()
        {
            while (Index < String.Length)
            {
                if (String[Index] == '-')
                {
                    Index++;
                    Result.Add(ParseMember(false));
                }
                else if (String[Index] == '+')
                {
                    Index++;
                    Result.Add(ParseMember(true));
                }
                else if (Index == 0)
                {
                    Result.Add(ParseMember(true));
                }
                else
                {
                    throw ParseException;
                }
            }

            if (Result.Count == 1)
            {
                return Result[0];
            }

            return Result;
        }


        private IExpression ParseMember(bool IsPositive)
        {
            Product Result = new Product();

            if (!IsPositive)
            {
                Result.Add(new Fraction(-1));
            }

            Result.Add(GetExpression());

            while (Index < String.Length)
            {
                switch (String[Index])
                {
                    case '+' or '-':
                        return Result.GetExpression();

                    case '*':
                        ParseProduct(Result, true);
                        break;

                    case '/':
                        ParseProduct(Result, false);
                        break;

                    default:
                        Index++;
                        throw new Exception($"Unexpected character: '{String[Index - 1]}' at position {Index - 1}");
                }
            }

            return Result.GetExpression();
        }

        private void ParseProduct(Product Result, bool IsProduct)
        {
            Index++;
            if (Index >= String.Length)
            {
                throw new Exception("Unexpected end of expression after '*' or '/'");
            }

            if (String[Index] == '-')
            {
                Result.Add(new Fraction(-1));
                Index++;
            }
            Result.Add(GetExpression(), IsProduct);
        }

        private IExpression GetExpression()
        {
            if (String[Index] == '-')
            {
                Index++;
            }

            if (char.IsDigit(String[Index]))
            {
                Start = Index;
                Index = String.Skip(Index, IsDigit);

                return new Fraction(double.Parse(String[Start..Index]));
            }
            else if (String[Index] == '|')
            {
                Start = Index + 1;
                Index = String.Skip(Start, (char Char) => Char != '|');

                if (Index >= String.Length || String[Index] != '|')
                {
                    throw new Exception("Unclosed absolute value expression");
                }

                return new MathFunction
                (
                    Fraction.Abs,
                    new MathExpressionParser
                    (
                        String[Start..Index++],
                        Functions
                    ).Parse() ?? throw new Exception("|Absolute| parsing error")
                );
            }
            else if (String[Index] == '(')
            {
                return new MathExpressionParser
                (
                    String.GetExpression(Index, out Index),
                    Functions
                ).Parse() ?? throw new Exception();
            }
            else if (char.IsLetter(String[Index]))
            {
                MathFunctionSample Function = FindFunction();
                IExpression Content =
                    Function.IsExpression ? new MathExpressionParser
                    (
                        String.GetExpression(Index, out Index), Functions
                    ).Parse()
                    ?? throw new Exception($"{Function}.Content parsing error")
                    : Fraction.Zero;

                return new MathFunction(Function, Content);
            }

            throw new Exception($"Unexpected character: '{String[Index]}' at position {Index}");
        }


        private MathFunctionSample FindFunction()
        {
            foreach (KeyValuePair<string, MathFunctionSample> Function in Functions)
            {
                if (BeginsFunction(Function))
                {
                    return Function.Value;
                }
            }
            throw new Exception($"Function not found at position {Index}");
        }

        private bool BeginsFunction(KeyValuePair<string, MathFunctionSample> Function)
        {
            if (String.Begins(Index, Function.Key))
            {
                Index += Function.Key.Length;
                return true;
            }
            return false;
        }


        private static bool IsDigit(char Char) => char.IsDigit(Char) || Char == ',';
    }
}