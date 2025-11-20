namespace Zion.STP
{
    public readonly struct Optional : ITokenTemplate
    {
        public readonly ITokenTemplate Template;

        public Optional(ITokenTemplate Template)
        {
            ArgumentNullException.ThrowIfNull(Template);
            this.Template = Template;
        }


        public bool IsMatch(StringView String, int Start, out Token Block)
        {
            if (!Template.IsMatch(String, Start, out Block))
            {
                Block = new EmptyBlock(String);
            }
            return true;
        }
    }
}