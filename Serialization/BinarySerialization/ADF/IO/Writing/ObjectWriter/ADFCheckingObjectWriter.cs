namespace Zion.Serialization.ADF
{
    public sealed class ADFCheckingObjectWriter : ADFObjectWriter
    {
        private record struct PostponedParameter(int Index, ArenaStream Stream);

        private static readonly IComparer<PostponedParameter> PostponedParameterComparer
            = Comparer<PostponedParameter>.Create(static (A, B) => B.Index.CompareTo(A.Index));

        private readonly SortedList<PostponedParameter> PostponedItems;
        private readonly DataFormat Format;
        private int Current;


        public ADFCheckingObjectWriter(BaseADFWriter Base, ArenaStream Stream, DataFormat Format)
            : base(Base, Stream)
        {
            this.PostponedItems = new(0, PostponedParameterComparer);
            this.Format = Format;
        }


        protected override ArenaStream GetStream(string Name, in uint NameId, in uint FormatId)
        {
            var Format = this.Format;

            if (Current >= Format.ParametersCount)
            {
                throw new ADFTooManyParametersException(Current, Format.ParametersCount);
            }

            int Index = Format.IndexOf(NameId, Current);

            if (Index == -1)
            {
                throw new ADFParameterNotExistsException(StringRegistry.GetString(in NameId));
            }

            if (Index == Current)
            {
                var Parameter = Format[Current];

                if (NameId != Parameter.NameId || !Format.Contains(NameId))
                {
                    throw new ADFNameMismatchException(Name, StringRegistry.GetString(in NameId));
                }

                if (!FormatRegistry.IsAssignableFrom(Parameter.FormatId, FormatId))
                {
                    throw new ADFFormatMismatchException(FormatId, Parameter.FormatId);
                }

                return GetBaseStream();
            }

            var PostponedStream = GetNewStream(32);
            PostponedItems.Add(new(Index, PostponedStream));

            return PostponedStream;
        }

        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            var PostponedItems = this.PostponedItems;
            int Count = Format.ParametersCount;
            int Index = Current + 1;

            var BaseStream = GetBaseStream();

            while (Index < Count && PostponedItems.Count > 0)
            {
                var LastItem = PostponedItems.Count - 1;
                var Postponed = PostponedItems[LastItem];

                if (Index != Postponed.Index) { break; }

                BaseStream.Write(Postponed.Stream);
                Postponed.Stream.Dispose();

                PostponedItems.RemoveAt(LastItem);

                Index++;
            }

            Current = Index;
        }
    }
}