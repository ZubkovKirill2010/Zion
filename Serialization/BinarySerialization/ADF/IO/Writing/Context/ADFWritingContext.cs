namespace Zion.Serialization.ADF
{
    public sealed class ADFWritingContext
    {
        public readonly Arena<byte>        Arena;
        public readonly ADFWritingOptions  Options;
        public readonly TypeAssociation    TypeAssociation;
        public readonly WritableRegistries Registries;
        public readonly WriteStrategies    WriteStrategies;

        public uint CurrentPage;
        public long CurrentPosition;//TODO: Сделать обновление позиции

        public ADFWritingContext(ADFWritingOptions? WritingOptions)
        {
            Arena           = new();
            Registries      = new();
            TypeAssociation = new();
            WriteStrategies = new();
            Options = WritingOptions ?? ADFWritingOptions.Default;
        }
    }
}