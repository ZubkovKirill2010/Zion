namespace Zion.Serialization.ADF
{
    public sealed class ADFCheckingObjectWriter : ADFObjectWriter
    {
        #region Types
        private record struct PostponedParameter(int Index, StreamGroup StreamGroup);

        #endregion

        #region Constants
        private static readonly IComparer<PostponedParameter> PostponedParameterComparer
            = Comparer<PostponedParameter>.Create(static (A, B) => B.Index.CompareTo(A.Index));

        #endregion

        #region Data
        private readonly SortedList<PostponedParameter> PostponedItems;
        private readonly DataFormat Format;
        private int Current;

        #endregion

        #region Constructors
        public ADFCheckingObjectWriter(ADFWritingContext Context, ArenaStream Stream, DataFormat Format)
            : base(Context, Stream)
        {
            this.PostponedItems = new(0, PostponedParameterComparer);
            this.Format = Format;
        }

        #endregion

        #region OverrideMethods
        protected override StreamGroup GetStreamGroup(string Name, in uint NameId, in uint FormatId)
        {
            CheckRanges(Name, in NameId, out int ParameterIndex);

            var Target = Format[ParameterIndex];

            if (!FormatRegistry.IsAssignableFrom(Target.FormatId, FormatId))
            {
                throw new ADFFormatMismatchException(FormatId, Target.FormatId);
            }

            if (ParameterIndex == Current)
            {
                return base.GetStreamGroup(Name, in NameId, in FormatId);
            }

            var PostponedGroup = new StreamGroup(GetNewStream(32));
            PostponedItems.Add(new(ParameterIndex, PostponedGroup));

            return PostponedGroup;
        }

        protected override ArenaStream GetStreamForNull(string Name, in uint NameId)
        {
            CheckRanges(Name, in NameId, out int ParameterIndex);

            if (ParameterIndex == Current)
            {
                return GetBaseStream();
            }

            var PostponedStream = GetNewStream(32);
            PostponedItems.Add(new(ParameterIndex, new(PostponedStream)));

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

                var PostponedStream = Postponed.StreamGroup.BaseStream;

                BaseStream.Write(Postponed.StreamGroup.BaseStream);
                PostponedStream.Dispose();

                PostponedItems.RemoveAt(LastItem);

                Index++;
            }

            Current = Index;
        }

        #endregion

        #region PrivateMethods
        private void CheckRanges(string Name, in uint NameId, out int ParameterIndex)
        {
            var Format = this.Format;

            if (Current >= Format.ParametersCount)
            {
                throw new ADFTooManyParametersException(Current, Format.ParametersCount);
            }

            ParameterIndex = Format.IndexOf(NameId, Current);

            if (ParameterIndex == -1)
            {
                throw new ADFParameterNotExistsException(StringRegistry.GetString(in NameId));
            }

            if (ParameterIndex == Current)
            {
                var Parameter = Format[Current];

                if (NameId != Parameter.NameId || !Format.Contains(NameId))
                {
                    throw new ADFNameMismatchException(Name, StringRegistry.GetString(in NameId));
                }
            }
        }

        #endregion
    }
}