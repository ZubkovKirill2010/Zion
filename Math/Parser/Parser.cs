namespace Zion.MathExpressions
{
    public sealed partial class MathExpressionParser
    {
        public IMathTerm Parse()
        {
            IMathTerm expression = ParseAdditive();

            if (!Finished)
            {
                Throw($"Unexpected character '{Current}' at position {Index}");
            }

            return expression;
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
                    Term = new MathFunction(Term, MathFunctions.DoubleFactorial);
                }
                else
                {
                    Term = new MathFunction(Term, MathFunctions.Factorial);
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
                Term = ParseParentheses();
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
                return InvalidChar();
            }

            string NumberString = String[Start..Index];
            return Fraction.ParseDecimal(NumberString);
        }

        private IMathTerm ParseParentheses()
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

            if (!Finished && Current == '(')//Is function
            {
                return ParseFunctionCall(Identifier);
            }

            if (Variables is not null && Variables.TryGetValue(Identifier, out Fraction value))
            {
                return value;
            }

            return Throw($"Unknown identifier '{Identifier}'");
        }

        private IMathTerm ParseFunctionCall(string FunctionName)
        {
            if (Current != '(')
            {
                return Throw("Expected '(' after function name");
            }

            Index++;

            List<IMathTerm> Parameters = new List<IMathTerm>();

            if (Current != ')')
            {
                do
                {
                    Parameters.Add(ParseAdditive());

                    if (Finished)
                    {
                        return Throw("Expected ')' or ','");
                    }

                    if (Current == ')')
                    {
                        break;
                    }

                    if (Current != ',')
                    {
                        return Throw("Expected ','");
                    }

                    Index++;
                }
                while (!Finished);
            }

            if (Finished || Current != ')')
            {
                return Throw("Expected ')'");
            }

            Index++;

            //if (Functions is not null && Functions.TryGetValue(FunctionName, out var function))
            //{
            //    return new MathFunction(Functions[FunctionName], Parameters, function);
            //}

            return Throw($"Unknown function '{FunctionName}'");
        }
    }
}