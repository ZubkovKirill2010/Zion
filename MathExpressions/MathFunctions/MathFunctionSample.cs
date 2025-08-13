namespace Zion.MathExpressions
{
    public readonly struct MathFunctionSample
    {
        public readonly Func<Fraction, Fraction> Function;
        public readonly bool IsExpression;

        public MathFunctionSample(Func<Fraction, Fraction> Function, bool IsExpression)
        {
            this.Function = Function;
            this.IsExpression = IsExpression;
        }

        public Fraction GetValue(IExpression Expression) => Function(Expression.GetValue());
    }
}