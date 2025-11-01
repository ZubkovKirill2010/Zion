namespace Zion.MathExpressions
{
    public readonly struct Inversed : IFraction
    {
        public readonly IFraction Value;

        public Inversed(IFraction Value)
        {
            this.Value = Value;
        }

        public Fraction GetValue(int Accuracy)
        {
            return Value.GetValue(Accuracy).Inversed;
        }
    }
}