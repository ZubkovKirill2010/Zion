namespace Zion
{
    [Flags]
    public enum ProgrammingLanguage : int
    {
        C = 1,
        CPlusPlus = 2,
        CSharp = 4,
        Java = 8,
        Python = 16,
        JavaScript = 32,
        TypeScript = 64,
        PHP = 128,
        Go = 256,
        Swift = 512,
        Kotlin = 1024,
        Ruby = 2048,
        Rust = 4096,
        Dart = 8192,

        SQL = 16384,
        HTML = 32768,
        CSS = 65536,
        Bash = 131072,
        PowerShell = 262144,

        R = 524288,
        Assembly = 1048576,
        Lua = 2097152,
        Scala = 4194304,
        FSharp = 8388608,
        ObjectiveC = 16777216,
        Xaml = 33554432,
        XML = 67108864,
        YAML = 134217728,
        Markdown = 268435456,
        Dockerfile = 536870912,
        GraphQL = 1073741824,
        Pascal = int.MaxValue,
    }
}
