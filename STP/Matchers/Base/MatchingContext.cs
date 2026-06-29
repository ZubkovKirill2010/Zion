namespace Zion.STP
{
    public sealed class MatchingContext
    {
        public bool GoToNext = true;
        public ICorrection? Correction;

        public bool Corrected => Correction is not null;
    }
}