namespace Zion.STP
{
    public sealed class ErrorGroup : Group
    {
        public ErrorGroup(StringView String, int Length, IGroupTemplate Template)
            : base(String, Length, Template) { }
    }
}