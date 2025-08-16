namespace Zion.MathExpressions
{
    public static class MathExpression
    {
        public static Fraction Solve(string String)
        {
            return new MathExpressionParser(String).Parse().GetValue();
        }

        public static bool TrySolve(string String, out Fraction Value)
        {
            if (new MathExpressionParser(String).TryParse(out var Result))
            {
                Value = Result.GetValue();
                return true;
            }
            Value = Fraction.Zero;
            return false;
        }
    }
}