using System.Diagnostics;

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
            Debug.WriteLine("ME.TrySolve");
            if (new MathExpressionParser(String).TryParse(out IExpression? Result))
            {
                Debug.WriteLine("P_True");
                Value = Result.GetValue();
                return true;
            }
            Debug.WriteLine("P_False");
            Value = Fraction.Zero;
            return false;
        }
    }
}