namespace Zion
{
    public readonly struct ProgressInfo
    {
        public readonly long Total;
        public readonly long Completed;
        public readonly double Percentage;

        public ProgressInfo(long Total, long Completed)
        {
            this.Total = Total;
            this.Completed = Completed;
            this.Percentage = ((double)Completed / (double)Total) * 100d;
        }
    }
}