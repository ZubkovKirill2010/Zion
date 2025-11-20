using System.Collections;

namespace Zion.STP
{
    public readonly struct Template : ITokenTemplate, IEnumerable<ITokenTemplate>
    {
        public readonly ITokenTemplate[] Templates;

        public int Count => Templates.Length;

        public Template(ITokenTemplate[] Templates)
        {
            ArgumentNullException.ThrowIfNull(Templates);
            if (Templates.Any(Template => Template is null))
            {
                throw new ArgumentNullException("Template is null");
            }
            this.Templates = Templates;
        }


        public bool IsMatch(StringView String, int Start, out Token Block)
        {
            Group Group = new Group(String, this);
            Block = Group;

            foreach (ITokenTemplate Template in Templates)
            {
                if (Template.IsMatch(String, Start, out Token Current))
                {
                    Group.Add(Current);
                    Start += Current.Length;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public void Add(ITokenTemplate Template)
        {
            Templates.Add(Template);
        }


        public IEnumerator<ITokenTemplate> GetEnumerator()
        {
            return ((IEnumerable<ITokenTemplate>)Templates).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Templates.GetEnumerator();
        }
    }
}