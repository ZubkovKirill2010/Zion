namespace Zion.Serialization.ADF
{
    public sealed class ADFWritingContext
    {
        public readonly Arena<byte> Arena;
        public readonly ADFWritingOptions Options;
        public readonly WritableRegistries Registries;

        public ADFWritingContext(ADFWritingOptions? WritingOptions)
        {
            Arena = new();
            Registries = new();
            Options = WritingOptions ?? ADFWritingOptions.Default;
        }
    }
}