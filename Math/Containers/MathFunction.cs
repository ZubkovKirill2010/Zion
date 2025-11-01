namespace Zion.MathExpressions
{
    public readonly struct MathFunction : IFraction
    {
        public readonly IFraction Value;
        public readonly Func<IFraction, int, Fraction> Function;

        public MathFunction(IFraction Value, Func<IFraction, int, Fraction> Function)
        {
            this.Value = Value;
            this.Function = Function;
        }

        public Fraction GetValue(int Accuracy)
        {
            return Function(Value, Accuracy);
        }
    }
}