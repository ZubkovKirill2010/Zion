using System.Text;

namespace Zion
{
    [Serializable]
    public sealed class MassColorText
    {
        public readonly ColorText[] Text;
        public readonly int Length;

        public MassColorText(params ColorText[] Blocks)
        {
            Text = Blocks;

            foreach (ColorText Block in Blocks)
            {
                Length += Block.Length;
            }
        }

        public override string ToString()
        {
            StringBuilder Result = new StringBuilder();
            foreach (ColorText Item in Text)
            {
                Result.Append(Item.ToString());
            }
            return Result.ToString();
        }
        public string ToWhiteString()
        {
            StringBuilder Result = new StringBuilder();
            foreach (ColorText Item in Text)
            {
                Result.Append(Item.Text);
            }
            return Result.ToString();
        }
    }
}