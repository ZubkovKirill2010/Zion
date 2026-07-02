namespace Zion
{
    public static class GapBufferExtensions
    {
        public static void Add(this GapBuffer<char> Buffer, string String)
        {
            ArgumentNullException.ThrowIfNull(Buffer);
            ArgumentNullException.ThrowIfNull(String);

            Buffer.Add(String, String.Length);
        }

        public static void Insert(this GapBuffer<char> Buffer, int Index, string String)
        {
            ArgumentNullException.ThrowIfNull(Buffer);
            ArgumentNullException.ThrowIfNull(String);

            Buffer.Insert(Index, String, String.Length);
        }
    }
}