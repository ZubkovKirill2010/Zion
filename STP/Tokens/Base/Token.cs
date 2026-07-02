using System.Diagnostics.CodeAnalysis;

namespace Zion.STP
{
    public abstract class Token : IEqualityComparer<Token>
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
                value ??= EmptyErrorCodes;
                field = value;
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

        public virtual int LineBreakCount => 0;

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

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType(), Length);
        }

        public override bool Equals(object? Object)
        {
            return Object is Token Token && Equals(this, Token);
        }


        public int GetHashCode([DisallowNull] Token Object)
        {
            return Object.GetHashCode();
        }


        public void AddError(short ErrorCode)
        {
            ErrorCodes = ZArray.Concat(ErrorCodes, ErrorCode);
        }

        public void ClearErrors()
        {
            ErrorCodes = EmptyErrorCodes;
        }

        #endregion

        #region EqualityComparing
        public bool Equals(Token? A, Token? B)
        {
            if (ReferenceEquals(A, B))  { return true;  }
            if (A is null || B is null) { return false; }

            return A.GetType() == B.GetType() && A.Length == B.Length && A.DataEquals(B);
        }

        public bool Equals(Token? Other)
        {
            return Equals(this, Other);
        }

        protected virtual bool DataEquals(Token Other)
        {
            return true;
        }

        #endregion
    }
}