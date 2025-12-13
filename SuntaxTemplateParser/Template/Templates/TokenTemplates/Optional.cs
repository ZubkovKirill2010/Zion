namespace Zion.STP
{
    public readonly struct Optional : ITokenTemplate
    {
        public readonly ITokenTemplate Template;

        public RGBColor Color { get; init; }

        public Optional(ITokenTemplate Template)
        {
            ArgumentNullException.ThrowIfNull(Template);
            this.Template = Template;
        }


        public bool IsMatch(StringView String, int Start, out Block Block)
        {
            if (!Template.IsMatch(String, Start, out Block))
            {
                Block = new EmptyBlock(String, this);
            }
            return true;
        }
    }
}