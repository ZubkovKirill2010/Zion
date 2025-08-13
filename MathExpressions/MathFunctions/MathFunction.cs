namespace Zion.MathExpressions
{
    public sealed class MathFunction : IExpression, IMathFunction
    {
        private readonly Func<Fraction, Fraction> Function;
        public readonly IExpression Expression;

        public MathFunction(MathFunctionSample Function, IExpression Expression)
        {
            this.Function = Function.Function;
            this.Expression = Expression;
        }
        public MathFunction(Func<Fraction, Fraction> Function, IExpression Expression)
        {
            this.Function = Function;
            this.Expression = Expression;
        }

        public Fraction GetValue() => Function(Expression.GetValue());
    }
}