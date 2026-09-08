using System.Collections;

namespace Zion.Serialization.ADF
{
    public struct StreamGroup : IEnumerable<ArenaStream>
    {
        public  readonly ArenaStream BaseStream;
        private readonly List<StreamGroup> Groups;

        public readonly long Length;

        public int Count => Groups.Count;


        public StreamGroup(ArenaStream Stream)
        {
            BaseStream = Stream.NotNull();
            Groups = new(0);
        }

        private StreamGroup(ArenaStream BaseStream, List<StreamGroup> Groups, long Length)
        {
            this.BaseStream = BaseStream;
            this.Groups = Groups;
            this.Length = Length;
        }


        public StreamGroup Add(StreamGroup Group)
        {
            Groups.Add(Group);
            return new(BaseStream, Groups, Length + Group.Length);
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public IEnumerator<ArenaStream> GetEnumerator()
        {
            if (BaseStream is null) { yield break; }

            yield return BaseStream;

            foreach (var Group in Groups)
            {
                foreach (var Stream in Group)
                {
                    yield return Stream;
                }
            }
        }
    }
}