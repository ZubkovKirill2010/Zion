namespace Zion.MathExpressions
{
    public readonly struct MathFunction : IMathTerm
    {
        public readonly IMathTerm Value;
        public readonly Func<IMathTerm, int, Fraction> Function;

        public MathFunction(IMathTerm Value, Func<IMathTerm, int, Fraction> Function)
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