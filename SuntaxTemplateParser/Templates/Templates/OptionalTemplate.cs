namespace Zion.STP
{
    public sealed class OptionalTemplate : Template
    {
        private readonly Template Template;

        public OptionalTemplate(Template Template)
        {
            this.Template = Template;
        }

        public override bool Read(StringView String, int Start, out Block Block)
        {
            Block = Template.Read(String, Start, out Block) ? Block : new EmptyToken();
            return true;
        }
    }
}