using Function = System.Func<Zion.MathExpressions.IMathTerm, int, Zion.MathExpressions.Fraction>;

namespace Zion.MathExpressions
{
    public static class MathFunctions
    {
        public static readonly Function Sqrt
            = (Value, Accuracy) => Fraction.Sqrt(Value.GetValue(Accuracy), 2, Accuracy);

        public static readonly Function Factorial
            = (Value, Accuracy) => Fraction.Factorial(Value.GetValue(Accuracy));

        public static readonly Function DoubleFactorial
            = (Value, Accuracy) => Fraction.DoubleFactorial(Value.GetValue(Accuracy));
    }
}