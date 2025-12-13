using System.Collections;

namespace Zion.STP
{
    public readonly struct Template : IGroupTemplate, IEnumerable<ITemplate>
    {
        public readonly ITemplate[] Templates;

        public int Count => Templates.Length;

        public Template(ITemplate[] Templates)
        {
            ArgumentNullException.ThrowIfNull(Templates);
            if (Templates.Any(Template => Template is null))
            {
                throw new ArgumentNullException("Template is null");
            }
            this.Templates = Templates;
        }

        public ITemplate this[int Index] => Templates[Index];


        public bool IsMatch(StringView String, int Start, out Block Block)
        {
            Group Group = new Group(String, 0, this);
            Block = Group;

            foreach (ITokenTemplate Template in Templates)
            {
                if (Template.IsMatch(String, Start, out Block Current))
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

        public void Add(ITemplate Template)
        {
            Templates.Add(Template);
        }


        public IEnumerator<ITemplate> GetEnumerator()
        {
            return ((IEnumerable<ITemplate>)Templates).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Templates.GetEnumerator();
        }
    }
}