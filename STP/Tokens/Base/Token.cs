namespace Zion.STP
{
    public abstract class Token
    {
        #region Constants
        private static readonly short[] EmptyErrorCodes = Array.Empty<short>();

        #endregion

        #region ReadonlyProperties
        public int Length
        {
            get;
            init
            {
                field = value > 0 ? value : throw new ArgumentOutOfRangeException($"Token.Length(={value}) <= 0");
            }
        }

        #endregion

        #region Properies
        public short[] ErrorCodes
        {
            get;
            set
            {
                field = value.NotNull();
                Status = value.Length == 0 ? Validation.Valid : Validation.Invalid;
            }
        } = EmptyErrorCodes;

        public Validation Status
        {
            get;
            private set
            {
                if (value == field) { return; }
                field = value;
                StatusChanged?.Invoke(value);
            }
        }

        public uint Format
        {
            get;
            set
            {
                if (value == field) { return; }
                field = value;
                FormatChanged?.Invoke(value);
            }
        }

        #endregion

        #region Actions
        public Action<Validation>? StatusChanged;
        public Action<uint>?       FormatChanged;

        #endregion

        #region Methods
        public override string ToString()
        {
            return $"[{GetType().Name.RemoveSuffix("Token")}]";
        }


        public void AddError(short ErrorCode)
        {
            ErrorCodes = ZArray.Add(ErrorCodes, ErrorCode);
        }

        public void ClearErrors()
        {
            ErrorCodes = EmptyErrorCodes;
        }

        #endregion
    }
}