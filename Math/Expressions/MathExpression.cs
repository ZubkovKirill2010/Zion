﻿namespace Zion.MathExpressions
{
    public static class MathExpression
    {
        public static IFraction Parse(string String)
        {
            return Parse(String, 0, String.Length);
        }
        public static IFraction Parse(string String, int Start, int End)
        {
            return new MathExpressionParser(String, Start, End).Parse();
        }

        public static bool TryParse(string String, out IFraction Value)
        {
            return TryParse(String, 0, String.Length, out Value);
        }
        public static bool TryParse(string String, int Start, int End, out IFraction Value)
        {
            return new MathExpressionParser(String, Start, End).TryParse(out Value);
        }

        public static Fraction Solve(string String, int Accuracy = 10)
        {
            return Parse(String).GetValue(Accuracy);
        }
        public static bool TrySolve(string String, out Fraction Value, int Accuracy = 10)
        {
            if (TryParse(String, out IFraction Result))
            {
                Value = Result.GetValue(Accuracy);
                return true;
            }
            Value = default;
            return false;
        }
    }
}