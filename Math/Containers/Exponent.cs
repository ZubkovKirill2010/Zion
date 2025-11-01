namespace Zion.MathExpressions
{
    public sealed class Exponent : IMathTerm
    {
        private readonly IMathTerm Value;
        private readonly IMathTerm Power;


        public Exponent(IMathTerm Value, IMathTerm Power)
        {
            this.Value = Value;
            this.Power = Power;
        }


        public Fraction GetValue(int Accuracy)
        {
            return Fraction.Pow(Value.GetValue(Accuracy), Power.GetValue(Accuracy), Accuracy);
        }
    }
}