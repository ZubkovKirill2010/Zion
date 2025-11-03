namespace Zion.MathExpressions
{
    public sealed partial class MathExpressionParser
    {
        public IMathTerm Parse()
        {
            IMathTerm Expression = ParseAdditive();

            if (!(IsInsideFunction ? (Index < String.Length && (Current is '|' or ')')) : (Index >= String.Length)))
            {
                Throw(Index, $"Unexpected character '{Current}'");
            }

            return Expression;
        }

        internal IMathTerm ParseBeforeSeporator(out int End)
        {
            IMathTerm Result = Parse();
            End = Index;
            return Result;
        }


        private IMathTerm ParseAdditive()
        {
            IMathTerm Left = ParseMultiplicative();

            while (!Finished && (Current == '+' || Current == '-'))
            {
                char Operator = Current;
                Index++;

                IMathTerm Right = ParseMultiplicative();

                if (Operator == '+')
                {
                    Left = new Amount(Left, Right);
                }
                else // '-'
                {
                    Left = new Amount(Left, new Negative(Right));
                }

                if (IsInsideFunction && !Finished && (Current == '|' || Current == ')'))
                {
                    break;
                }
            }

            return Left;
        }

        private IMathTerm ParseMultiplicative()
        {
            IMathTerm Left = ParseExponential();

            while (!Finished && (Current == '*' || Current == '/' || Current == '%'))
            {
                char Operator = Current;
                Index++;

                IMathTerm Right = ParseExponential();

                if (Operator == '*')
                {
                    Left = new Product(Left, Right);
                }
                else if (Operator == '/')
                {
                    Left = new Division(Left, Right);
                }
                else // '%'
                {
                    Left = new Remainder(Left, Right);
                }

                if (IsInsideFunction && !Finished && (Current == '|' || Current == ')'))
                {
                    break;
                }
            }

            return Left;
        }

        private IMathTerm ParseExponential()
        {
            IMathTerm Left = ParseFactorial();

            while (!Finished && Current == '^')
            {
                Index++;
                IMathTerm Right = ParseExponential();
                Left = new Exponent(Left, Right);

                if (IsInsideFunction && !Finished && (Current == '|' || Current == ')'))
                {
                    break;
                }
            }

            return Left;
        }

        private IMathTerm ParseFactorial()
        {
            IMathTerm Term = ParsePrimary();

            while (!Finished && Current == '!')
            {
                Index++;
                if (!Finished && Current == '!')
                {
                    Index++;
                    Term = new SimpleFunction(Term, MathFunctions.DoubleFactorial);
                }
                else
                {
                    Term = new SimpleFunction(Term, MathFunctions.Factorial);
                }

                if (IsInsideFunction && !Finished && (Current == '|' || Current == ')'))
                {
                    break;
                }
            }

            return Term;
        }

        private IMathTerm ParsePrimary()
        {
            if (Finished)
            {
                return Unfinished();
            }

            bool IsNegative = false;
            if (Current == '-')
            {
                IsNegative = true;
                Index++;
            }
            else if (Current == '+')
            {
                Index++;
            }

            IMathTerm Term;

            if (IsDigit(Current))
            {
                Term = ParseNumber();
            }
            else if (Current == '(')
            {
                Term = ParseBrackets();
            }
            else if (IsLetter(Current))
            {   
                Term = ParseIdentifier();
            }
            else
            {
                return InvalidChar();
            }

            if (IsNegative)
            {
                Term = new Negative(Term);
            }

            return Term;
        }

        private IMathTerm ParseNumber()
        {
            int Start = Index;
            Index = String.Skip(Index, IsDigit);

            if (Index == Start)
            {
                InvalidChar();
            }

            string NumberString = String[Start..Index];
            return Fraction.ParseDecimal(NumberString);
        }

        private IMathTerm ParseBrackets()
        {
            if (Current != '(')
            {
                return Throw("Expected '('");
            }

            Index++;

            IMathTerm Value = ParseAdditive();

            if (Finished || Current != ')')
            {
                return Throw("Expected ')'");
            }

            Index++;
            return Value;
        }

        private IMathTerm ParseIdentifier()
        {
            int Start = Index;
            Index = String.Skip(Index, IsLetter);
            string Identifier = String[Start..Index];

            if (!Finished && Current == '(')
            {
                if (ExistsFunction(Identifier))
                {
                    return ParseFunctionCall(Identifier);
                }
                else if (ExistsVariable(Identifier))
                {
                    return new Product(Variables[Identifier], ParseBrackets());
                }
                else
                {
                    return Throw($"Unknown identifier '{Identifier}'");
                }
            }

            if (Variables.TryGetValue(Identifier, out Fraction value))
            {
                return value;
            }

            return Throw($"Unknown identifier '{Identifier}'");
        }

        private IMathTerm ParseFunctionCall(string Identifier)
        {
            MathFunctionHandler Handler = GetHandler(Index + 1);
            IMathTerm Result = Functions[Identifier](Handler);
            Index = Handler.Index - 1;

            if (Index >= String.Length || Current != ')')
            {
                Throw($"No closing bracket ')' in function \"{Identifier}\"");
            }

            Index++;
            return Result;
        }
    }
}