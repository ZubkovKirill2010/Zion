using Zion.MathExpressions;

namespace Zion
{
    public readonly struct ProductNumber
    {
        public readonly IExpression Value;
        public readonly bool IsProduct;

        public ProductNumber(IExpression Value, bool IsProduct = true)
        {
            this.Value = Value;
            this.IsProduct = IsProduct;
        }

        public override string ToString()
        {
            string Result = Value.ToString() ?? "null";
            if (IsProduct)
            {
                return "* " + Result;
            }
            return "/ " + Result;
        }
    }
}